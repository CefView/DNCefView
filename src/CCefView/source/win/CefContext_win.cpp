#include <CefContext.h>

#undef OS_WINDOWS
#include <Shlwapi.h>

#include <filesystem>

#include <include/cef_app.h>

#include <CefViewBrowserApp.h>
#include <CefViewCoreProtocol.h>

#include "details/handlers/CCefAppDelegate.h"

bool
CCefContext::init(const CCefConfig* config)
{
  config_ = config;

  // get current dll handle
  HMODULE hCurrentModule = nullptr;
  ::GetModuleHandleEx(
    GET_MODULE_HANDLE_EX_FLAG_FROM_ADDRESS | GET_MODULE_HANDLE_EX_FLAG_UNCHANGED_REFCOUNT,
    reinterpret_cast<LPCTSTR>(&CCefContext::instance), &hCurrentModule);

  // set cef folder path
  std::vector<wchar_t> modPath(MAX_PATH * 4);
  ::GetModuleFileNameW(hCurrentModule, modPath.data(), static_cast<DWORD>(modPath.size()));
  ::PathRemoveFileSpecW(modPath.data());
  ::PathCombineW(modPath.data(), modPath.data(), L"CefView");
  ::SetDllDirectoryW(modPath.data());

#if CEF_VERSION_MAJOR < 112
  // Enable High-DPI support on Windows 7 or newer.
  CefEnableHighDPISupport();
#endif

  // Build CefSettings
  CefSettings cef_settings;
  CCefConfig::CopyToCefSettings(config, cef_settings);

#if CEF_VERSION_MAJOR >= 125 && CEF_VERSION_MAJOR <= 127
  //  https://github.com/chromiumembedded/cef/issues/3685
  cef_settings.chrome_runtime = true;
#endif

  // fixed values
  // CEF 128+ dropped the chrome_runtime workaround for
  // https://github.com/chromiumembedded/cef/issues/3685 (only valid for 125-127)
  // and removed pack_loading_disabled / persist_user_preferences.
#if CEF_VERSION_MAJOR < 128
  cef_settings.pack_loading_disabled = false;
#endif
#if CEF_VERSION_MAJOR >= 128
  cef_settings.no_sandbox = true; // no sandbox support in this host; see linux/mac twins
#endif

  // external message pump
  if (cef_settings.multi_threaded_message_loop) {
    cef_settings.external_message_pump = false;
  } else {
    cef_settings.external_message_pump = true;
  }

  // path values
  if (CefString(&cef_settings.browser_subprocess_path).empty()) {
    auto subprocessPath = (std::filesystem::path(modPath.data()) / kCefViewRenderProcessName);
    CefString(&cef_settings.browser_subprocess_path) = subprocessPath.u8string().c_str();
  }

  // create job object
  DWORD dwProcessId = ::GetProcessId(::GetCurrentProcess());
  windowsJobName_ = std::string("CefView-Job-{4aa7bf34-029a-4f95-9d23-9638593cc174}-") + std::to_string(dwProcessId);
  windowsJobHandle_ = ::CreateJobObjectA(nullptr, windowsJobName_.c_str());
  if (nullptr == windowsJobHandle_) {
    // qWarning() << "Failed to create windows job object:" << ::GetLastError();
  } else {
    JOBOBJECT_EXTENDED_LIMIT_INFORMATION info;
    ::memset(&info, 0, sizeof(info));
    info.BasicLimitInformation.LimitFlags = JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE | JOB_OBJECT_LIMIT_SILENT_BREAKAWAY_OK;
    if (!::SetInformationJobObject(windowsJobHandle_, JobObjectExtendedLimitInformation, &info, sizeof(info))) {
      // qWarning() << "Failed to set information for windows job object:" << GetLastError();
    }
    // CefViewWing joins this named job itself. Keep the host outside the
    // KILL_ON_JOB_CLOSE job so disposing the context cannot terminate Unity.
  }

  // Initialize CEF
  auto cmdArgs = CCefConfig::GetCommandLineArgs(config);
  cmdArgs[kCefViewWindowsJobNameKey] = windowsJobName_;
  auto appDelegate = std::make_shared<CCefAppDelegate>(this, cmdArgs);
  auto bridgeObjectName = config ? config->bridgeObjectName() : std::string();
  auto builtinSchemeName = config ? config->builtinSchemaName() : std::string();
  auto app = new CefViewBrowserApp(builtinSchemeName, bridgeObjectName, appDelegate);

  void* sandboxInfo = nullptr;
#if defined(CEF_USE_SANDBOX)
  // Manage the life span of the sandbox information object. This is necessary
  // for sandbox support on Windows. See cef_sandbox_win.h for complete details.
  static CefScopedSandboxInfo scopedSandbox;
  sandboxInfo = scopedSandbox.sandbox_info();
#endif

  CefMainArgs main_args(::GetModuleHandle(nullptr));
  if (!CefInitialize(main_args, cef_settings, app, sandboxInfo)) {
    if (windowsJobHandle_) { ::CloseHandle(windowsJobHandle_); windowsJobHandle_ = nullptr; }
    return false;
  }

  pApp_ = app;
  pAppDelegate_ = appDelegate;

  return true;
}

void
CCefContext::uninit()
{
  if (!pApp_) {
    config_ = nullptr;
    return;
  }

  pAppDelegate_ = nullptr;
  pApp_ = nullptr;

  // shutdown the cef
  CefShutdown();
  if (windowsJobHandle_) { ::CloseHandle(windowsJobHandle_); windowsJobHandle_ = nullptr; }
  config_ = nullptr;
}
