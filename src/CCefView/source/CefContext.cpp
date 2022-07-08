#include "CefContext.h"

#include <list>

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
  return;
}
