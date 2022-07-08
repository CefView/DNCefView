
#include "../CefContext.h"

#include <dlfcn.h>
#include <filesystem>

#include <include/cef_app.h>

#include <CefViewBrowserApp.h>
#include <CefViewCoreProtocol.h>

#include "details/CCefAppDelegate.h"

#include "CefContext_Impl.h"

static void
InitCefDirectorySettings(CefSettings& settings)
{
  // initialize path items
  Dl_info dl_info;
  dladdr((void*)InitCefDirectorySettings, &dl_info);
  std::filesystem::path currentModulePath(dl_info.dli_fname);
  currentModulePath = currentModulePath.parent_path();

  // build renderer process path
  auto rendererExePath = currentModulePath / kCefViewRenderProcessName;
  CefString(&settings.browser_subprocess_path) = rendererExePath.u8string();

  // build resource directory path
  auto resourceDirPath = currentModulePath / kCefViewResourceDirectoryName;
  CefString(&settings.resources_dir_path) = resourceDirPath.u8string();

  // build locales directory path
  auto localesDirPath = resourceDirPath / kCefViewLocalesDirectoryName;
  CefString(&settings.locales_dir_path) = localesDirPath.u8string();
}

bool
CCefContext::init(const CCefConfig* config)
{
  // Build CefSettings
  CefSettings cef_settings;
  CCefConfig::copyToCefSettings(config, cef_settings);

  // fixed values
  cef_settings.pack_loading_disabled = false;
  cef_settings.multi_threaded_message_loop = true;
  cef_settings.external_message_pump = false;

  InitCefDirectorySettings(cef_settings);

#if !defined(CEF_USE_SANDBOX)
  cef_settings.no_sandbox = true;
#endif

  // Initialize CEF.
  // 1. create app delegate
  CCefConfig::ArgsMap cmdArgs;
  copyCmdLineArgs(config, cmdArgs);
  auto appDelegate = std::make_shared<CCefAppDelegate>(this, cmdArgs);

  // 2. create browser app
  auto bridgeObjectName = config ? config->bridgeObjectName() : std::string();
  auto app = new CefViewBrowserApp(bridgeObjectName, appDelegate);

  // 3. startup CEF
  CefMainArgs main_args;
  if (!CefInitialize(main_args, cef_settings, app, nullptr)) {
    assert(0);
    return false;
  }

  pImpl_->pApp_ = app;
  pImpl_->pAppDelegate_ = appDelegate;

  return true;
}

void
CCefContext::uninit()
{
  if (!pImpl_->pApp_)
    return;

  pImpl_->pAppDelegate_ = nullptr;
  pImpl_->pApp_ = nullptr;

  // shutdown the cef
  CefShutdown();
}
