#include "CefContext.h"

#include <list>

#include <CefViewBrowserClient.h>

#include "details/CCefAppDelegate.h"

#include "CefContext_Impl.h"

CCefContext* CCefContext::instance_ = nullptr;

CCefContext::CCefContext(const CCefConfig* config)
  : pImpl_(std::make_unique<Implementation>())
{
  instance_ = this;
  init(config);
}

CCefContext::~CCefContext()
{
  uninit();

  pImpl_.reset();

  instance_ = nullptr;
}

void
CCefContext::addFolderResource(const std::string& path, const std::string& url, int priority /*= 0*/)
{
  pImpl_->folderResourceMappingList_.push_back({ path, url, priority });
}

void
CCefContext::addArchiveResource(const std::string& path,
                                const std::string& url,
                                const std::string& password /*= ""*/,
                                int priority /*= 0*/)
{
  pImpl_->archiveResourceMappingList_.push_back({ path, url, password, priority });
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
  return pImpl_->pApp_->IsSafeToExit();
}

CCefContext*
CCefContext::instance()
{
  return instance_;
}

const CCefConfig*
CCefContext::cefConfig() const
{
  return pImpl_->config_;
}

void
CCefContext::scheduleCefLoopWork(int64_t delayMs)
{
  return;
}

void
CCefContext::copyCmdLineArgs(const CCefConfig* config, CCefConfig::ArgsMap& args)
{
  if (!config)
    return;

  args = config->getCommandLineArgs();
}
