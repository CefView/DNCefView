#include "../CefContext.h"

#include <filesystem>
#include <windows.h>

#include <include/cef_app.h>

#include <CefViewBrowserApp.h>
#include <CefViewCoreProtocol.h>

#include "details/CCefAppDelegate.h"

#include "CefContext_Impl.h"

static void
InitCefDirectorySettings(CefSettings& settings)
{
  // initialize path items
  HMODULE hCurrentModule = nullptr;
  ::GetModuleHandleEx(GET_MODULE_HANDLE_EX_FLAG_FROM_ADDRESS, (LPCTSTR)InitCefDirectorySettings, &hCurrentModule);
  WCHAR currentModulePathBuf[MAX_PATH] = { 0 };
  ::GetModuleFileNameW(hCurrentModule, currentModulePathBuf, _countof(currentModulePathBuf));
  std::filesystem::path currentModulePath(currentModulePathBuf);
  currentModulePath = currentModulePath.parent_path();

  // build renderer process path
  auto rendererExePath = currentModulePath / kCefViewRenderProcessName ".exe";
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
  // Enable High-DPI support on Windows 7 or newer.
  CefEnableHighDPISupport();

  // Build CefSettings
  CefSettings cef_settings;
  CCefConfig::copyToCefSettings(config, cef_settings);

  // fixed values
  cef_settings.pack_loading_disabled = false;
  cef_settings.multi_threaded_message_loop = true;
  cef_settings.external_message_pump = false;
  cef_settings.windowless_rendering_enabled = true;

  // initialize directory
  InitCefDirectorySettings(cef_settings);

#if !defined(CEF_USE_SANDBOX)
  cef_settings.no_sandbox = true;
#endif

  // Initialize CEF
  // 1. create app delegate
  CCefConfig::ArgsMap cmdArgs;
  copyCmdLineArgs(config, cmdArgs);
  auto appDelegate = std::make_shared<CCefAppDelegate>(this, cmdArgs);

  // 2. create browser app
  auto bridgeObjectName = config ? config->bridgeObjectName() : std::string();
  auto app = new CefViewBrowserApp(bridgeObjectName, appDelegate);

  // 3. initialize the sandbox
  void* sandboxInfo = nullptr;
#if defined(CEF_USE_SANDBOX)
  // Manage the life span of the sandbox information object. This is necessary
  // for sandbox support on Windows. See cef_sandbox_win.h for complete details.
  static CefScopedSandboxInfo scoped_sandbox;
  sandbox_info = scoped_sandbox.sandbox_info();
#endif

  // 4. startup CEF
  CefMainArgs main_args(::GetModuleHandle(nullptr));
  if (!CefInitialize(main_args, cef_settings, app, sandboxInfo)) {
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
