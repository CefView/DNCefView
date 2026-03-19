#include "CefContext.h"

#include <chrono>
#include <condition_variable>
#include <cstdio>
#include <list>
#include <mutex>

#include <include/cef_cookie.h>
#include <include/internal/cef_time.h>
#include <include/cef_origin_whitelist.h>

namespace {

struct CookieVisitState {
  std::mutex mutex;
  std::condition_variable condition;
  std::vector<CefCookie> cookies;
  bool completed = false;
};

class CookieCollectorVisitor : public CefCookieVisitor {
public:
  explicit CookieCollectorVisitor(std::shared_ptr<CookieVisitState> state)
    : state_(std::move(state))
  {
  }

  ~CookieCollectorVisitor() override
  {
    if (!state_)
      return;

    std::lock_guard<std::mutex> lock(state_->mutex);
    state_->completed = true;
    state_->condition.notify_all();
  }

  bool Visit(const CefCookie& cookie, int count, int total, bool& deleteCookie) override
  {
    deleteCookie = false;

    if (!state_)
      return false;

    std::lock_guard<std::mutex> lock(state_->mutex);
    state_->cookies.push_back(cookie);
    return true;
  }

private:
  std::shared_ptr<CookieVisitState> state_;

  IMPLEMENT_REFCOUNTING(CookieCollectorVisitor);
};

static std::string
EscapeJson(const std::string& input)
{
  std::string output;
  output.reserve(input.size() + 8);

  for (unsigned char ch : input) {
    switch (ch) {
      case '"':
        output += "\\\"";
        break;
      case '\\':
        output += "\\\\";
        break;
      case '\b':
        output += "\\b";
        break;
      case '\f':
        output += "\\f";
        break;
      case '\n':
        output += "\\n";
        break;
      case '\r':
        output += "\\r";
        break;
      case '\t':
        output += "\\t";
        break;
      default:
        if (ch < 0x20) {
          char buffer[7] = { 0 };
          std::snprintf(buffer, sizeof(buffer), "\\u%04x", static_cast<unsigned int>(ch));
          output += buffer;
        } else {
          output += static_cast<char>(ch);
        }
        break;
    }
  }

  return output;
}

static std::string
CefTimeToIso8601(const cef_time_t& time)
{
  char buffer[32] = { 0 };
  std::snprintf(buffer,
                sizeof(buffer),
                "%04d-%02d-%02dT%02d:%02d:%02dZ",
                time.year,
                time.month,
                time.day_of_month,
                time.hour,
                time.minute,
                time.second);
  return std::string(buffer);
}

static std::string
CefBaseTimeToIso8601(const cef_basetime_t& baseTime)
{
  cef_time_t time = {};
  if (!cef_time_from_basetime(baseTime, &time))
    return std::string();

  return CefTimeToIso8601(time);
}

static void
AppendCookieJson(std::string& output, const CefCookie& cookie)
{
  output += "{";
  output += "\"name\":\"" + EscapeJson(CefString(&cookie.name).ToString()) + "\",";
  output += "\"value\":\"" + EscapeJson(CefString(&cookie.value).ToString()) + "\",";
  output += "\"domain\":\"" + EscapeJson(CefString(&cookie.domain).ToString()) + "\",";
  output += "\"path\":\"" + EscapeJson(CefString(&cookie.path).ToString()) + "\",";
  output += "\"secure\":" + std::string(cookie.secure ? "true" : "false") + ",";
  output += "\"httpOnly\":" + std::string(cookie.httponly ? "true" : "false") + ",";
  output += "\"hasExpires\":" + std::string(cookie.has_expires ? "true" : "false") + ",";
  output += "\"creation\":\"" + EscapeJson(CefBaseTimeToIso8601(cookie.creation)) + "\",";
  output += "\"lastAccess\":\"" + EscapeJson(CefBaseTimeToIso8601(cookie.last_access)) + "\",";
  output += "\"expires\":\"" + EscapeJson(CefBaseTimeToIso8601(cookie.expires)) + "\",";
  output += "\"sameSite\":" + std::to_string(static_cast<int>(cookie.same_site)) + ",";
  output += "\"priority\":" + std::to_string(static_cast<int>(cookie.priority));
  output += "}";
}

static std::string
BuildCookieSnapshotJson(const std::vector<CefCookie>& cookies, bool started, bool timedOut)
{
  std::string output;
  output.reserve(256 + cookies.size() * 128);

  output += "{";
  output += "\"started\":" + std::string(started ? "true" : "false") + ",";
  output += "\"timedOut\":" + std::string(timedOut ? "true" : "false") + ",";
  output += "\"cookies\":[";

  for (size_t i = 0; i < cookies.size(); ++i) {
    if (i > 0)
      output += ",";
    AppendCookieJson(output, cookies[i]);
  }

  output += "]}";
  return output;
}

static std::string
CollectCookiesJson(CefRefPtr<CefCookieManager> manager,
                   const std::string* url,
                   bool includeHttpOnly,
                   int timeoutMs)
{
  if (!manager)
    return BuildCookieSnapshotJson({}, false, false);

  if (timeoutMs <= 0)
    timeoutMs = 3000;

  auto state = std::make_shared<CookieVisitState>();
  CefRefPtr<CookieCollectorVisitor> visitor = new CookieCollectorVisitor(state);

  bool started = false;
  if (url) {
    started = manager->VisitUrlCookies(CefString(*url), includeHttpOnly, visitor);
  } else {
    started = manager->VisitAllCookies(visitor);
  }

  visitor = nullptr;

  if (!started)
    return BuildCookieSnapshotJson({}, false, false);

  std::vector<CefCookie> cookies;
  bool timedOut = false;

  {
    std::unique_lock<std::mutex> lock(state->mutex);
    bool completed = state->condition.wait_for(
      lock, std::chrono::milliseconds(timeoutMs), [&state]() { return state->completed; });

    if (!completed)
      timedOut = true;

    cookies = state->cookies;
  }

  return BuildCookieSnapshotJson(cookies, true, timedOut);
}

} // namespace

CCefContext* CCefContext::instance_ = nullptr;

CCefContext::CCefContext(const CCefConfig* config)
  : config_(config)
{
  instance_ = this;
  init(config);
}

CCefContext::~CCefContext()
{
  uninit();

  instance_ = nullptr;
}

void
CCefContext::addFolderResource(const std::string& path, const std::string& url, int priority /*= 0*/)
{
  pApp_->AddLocalFolderResource(path, url, priority);
}

void
CCefContext::addArchiveResource(const std::string& path,
                                const std::string& url,
                                const std::string& password /*= ""*/,
                                int priority /*= 0*/)
{
  pApp_->AddArchiveResource(path, url, password, priority);
}

bool
CCefContext::addCookie(const std::string& name,
                       const std::string& value,
                       const std::string& domain,
                       const std::string& url)
{
  CefCookie cookie;
  CefString(&cookie.name).FromString(name);
  CefString(&cookie.value).FromString(value);
  CefString(&cookie.domain).FromString(domain);
  return CefCookieManager::GetGlobalManager(nullptr)->SetCookie(CefString(url), cookie, nullptr);
}

bool
CCefContext::deleteCookie(const std::string& url, const std::string& name)
{
  auto manager = CefCookieManager::GetGlobalManager(nullptr);
  if (!manager)
    return false;

  return manager->DeleteCookies(CefString(url), CefString(name), nullptr);
}

bool
CCefContext::deleteAllCookies()
{
  auto manager = CefCookieManager::GetGlobalManager(nullptr);
  if (!manager)
    return false;

  return manager->DeleteCookies(CefString(), CefString(), nullptr);
}

bool
CCefContext::addCrossOriginWhitelistEntry(const std::string& sourceOrigin,
                                          const std::string& targetProtocol,
                                          const std::string& targetDomain,
                                          bool allowTargetSubdomains)
{
  return CefAddCrossOriginWhitelistEntry(
    CefString(sourceOrigin), CefString(targetProtocol), CefString(targetDomain), allowTargetSubdomains);
}

bool
CCefContext::removeCrossOriginWhitelistEntry(const std::string& sourceOrigin,
                                             const std::string& targetProtocol,
                                             const std::string& targetDomain,
                                             bool allowTargetSubdomains)
{
  return CefRemoveCrossOriginWhitelistEntry(
    CefString(sourceOrigin), CefString(targetProtocol), CefString(targetDomain), allowTargetSubdomains);
}

bool
CCefContext::clearCrossOriginWhitelist()
{
  return CefClearCrossOriginWhitelist();
}

std::string
CCefContext::visitAllCookiesJson(int timeoutMs)
{
  auto manager = CefCookieManager::GetGlobalManager(nullptr);
  return CollectCookiesJson(manager, nullptr, true, timeoutMs);
}

std::string
CCefContext::visitUrlCookiesJson(const std::string& url, bool includeHttpOnly, int timeoutMs)
{
  auto manager = CefCookieManager::GetGlobalManager(nullptr);
  return CollectCookiesJson(manager, &url, includeHttpOnly, timeoutMs);
}

void
CCefContext::doCefMessageLoopWork()
{
  CefDoMessageLoopWork();
}

bool
CCefContext::isSafeToShutdown()
{
  if (!pApp_)
    return true;

  return pApp_->IsSafeToExit();
}

CCefContext*
CCefContext::instance()
{
  return instance_;
}

const CCefConfig*
CCefContext::cefConfig() const
{
  return config_;
}

void
CCefContext::scheduleCefLoopWork(int64_t delayMs)
{
  return;
}
