#include "CefContext.h"

#include <chrono>
#include <condition_variable>
#include <cstdio>
#include <list>
#include <mutex>

#include <include/cef_cookie.h>
#include <include/cef_origin_whitelist.h>
#include <include/internal/cef_time.h>

CCefContext* CCefContext::instance_ = nullptr;

CCefContext::CCefContext(const CCefConfig* config)
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

void
CCefContext::doCefMessageLoopWork()
{
  CefDoMessageLoopWork();
}

bool
CCefContext::isSafeToShutdown()
{
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
#if defined(OS_MACOS)
  if (delayMs < kCefWorkerIntervalMs) {
    // disaptch call to doCefMessageLoopWork
    dispatch_after(dispatch_time(DISPATCH_TIME_NOW, delayMs * NSEC_PER_MSEC), dispatch_get_main_queue(), ^{
      if (instance_) {
        instance_->doCefMessageLoopWork();
      }
    });
  }
#endif
}
