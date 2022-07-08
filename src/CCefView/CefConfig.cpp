#include "CefConfig.h"

#include <optional>

#include <include/cef_app.h>

#include <CefViewCoreProtocol.h>

class CCefConfig::Implementation
{
public:
  std::string locale_;
  std::string userAgent_;
  std::string cachePath_;
  std::string userDataPath_;
  std::string bridgeObjectName_;
  std::string acceptLanguageList_;

  CefViewLogLevel logLevel_;
  std::optional<uint32_t> backgroundColor_;
  std::optional<short> remoteDebuggingPort_;
  std::optional<bool> persistSessionCookies_;
  std::optional<bool> persistUserPreferences_;
  std::optional<bool> multiThreadedMessageLoop_;

  ArgsMap commandLineArgs_;
};

CCefConfig::CCefConfig()
  : pImpl_(std::make_unique<Implementation>())
{
  pImpl_->backgroundColor_ = 0xFFFFFFFF;
  pImpl_->logLevel_ = LOGSEVERITY_DEFAULT;
  pImpl_->userAgent_ = kCefViewDefaultUserAgent;
}

CCefConfig::~CCefConfig()
{
  pImpl_.reset();
}

void
CCefConfig::addCommandLineSwitch(const std::string& smitch)
{
  pImpl_->commandLineArgs_[smitch] = std::string();
}

void
CCefConfig::addCommandLineSwitchWithValue(const std::string& smitch, const std::string& v)
{
  pImpl_->commandLineArgs_[smitch] = v;
}

void
CCefConfig::setLogLevel(CefViewLogLevel lvl)
{
  pImpl_->logLevel_ = lvl;
}

CefViewLogLevel
CCefConfig::logLevel() const
{
  return pImpl_->logLevel_;
}

void
CCefConfig::setLocale(const std::string& locale)
{
  pImpl_->locale_ = locale;
}

const std::string&
CCefConfig::locale() const
{
  return pImpl_->locale_;
}

void
CCefConfig::setUserAgent(const std::string& agent)
{
  pImpl_->userAgent_ = agent;
}

const std::string&
CCefConfig::userAgent() const
{
  return pImpl_->userAgent_;
}

void
CCefConfig::setCachePath(const std::string& path)
{
  pImpl_->cachePath_ = path;
}

const std::string&
CCefConfig::cachePath() const
{
  return pImpl_->cachePath_;
}

void
CCefConfig::setUserDataPath(const std::string& path)
{
  pImpl_->userDataPath_ = path;
}

const std::string&
CCefConfig::userDataPath() const
{
  return pImpl_->userDataPath_;
}

void
CCefConfig::setBridgeObjectName(const std::string& name)
{
  pImpl_->bridgeObjectName_ = name;
}

const std::string&
CCefConfig::bridgeObjectName() const
{
  return pImpl_->bridgeObjectName_;
}

void
CCefConfig::setBackgroundColor(uint32_t color)
{
  pImpl_->backgroundColor_ = color;
}

uint32_t
CCefConfig::backgroundColor() const
{
  return pImpl_->backgroundColor_.value_or(-1);
}

void
CCefConfig::setAcceptLanguageList(const std::string& languages)
{
  pImpl_->acceptLanguageList_ = languages;
}

const std::string&
CCefConfig::acceptLanguageList() const
{
  return pImpl_->acceptLanguageList_;
}

void
CCefConfig::setPersistSessionCookies(bool enabled)
{
  pImpl_->persistSessionCookies_ = enabled;
}

bool
CCefConfig::persistSessionCookies() const
{
  return pImpl_->persistSessionCookies_.value_or(false);
}

void
CCefConfig::setPersistUserPreferences(bool enabled)
{
  pImpl_->persistUserPreferences_ = enabled;
}

bool
CCefConfig::persistUserPreferences() const
{
  return pImpl_->persistUserPreferences_.value_or(false);
}

void
CCefConfig::setMultiThreadedMessageLoop(bool enable)
{
  pImpl_->multiThreadedMessageLoop_ = enable;
}

bool
CCefConfig::multiThreadedMessageLoop() const
{
  return pImpl_->multiThreadedMessageLoop_.value_or(false);
}

void
CCefConfig::setRemoteDebuggingPort(short port)
{
  pImpl_->remoteDebuggingPort_ = port;
}

short
CCefConfig::remoteDebuggingPort() const
{
  return pImpl_->remoteDebuggingPort_.value_or(-1);
}

const CCefConfig::ArgsMap&
CCefConfig::getCommandLineArgs() const
{
  return pImpl_->commandLineArgs_;
}

void
CCefConfig::copyToCefSettings(const CCefConfig* config, CefSettings& settings)
{
  // validate the input source config parameter
  if (!config) {
    // just copy the mandatory fields
    CCefConfig cfg;

    settings.background_color = cfg.backgroundColor();
    CefString(&settings.user_agent) = cfg.userAgent();

    return;
  }

  if (!config->userAgent().empty())
    CefString(&settings.user_agent) = config->userAgent();

  if (!config->cachePath().empty())
    CefString(&settings.cache_path) = config->cachePath();

  if (!config->userDataPath().empty())
    CefString(&settings.user_data_path) = config->userDataPath();

  if (!config->locale().empty())
    CefString(&settings.locale) = config->locale();

  if (!config->acceptLanguageList().empty())
    CefString(&settings.accept_language_list) = config->acceptLanguageList();

  if (config->persistSessionCookies())
    settings.persist_session_cookies = config->persistSessionCookies();

  if (config->persistUserPreferences())
    settings.persist_user_preferences = config->persistUserPreferences();

  if (config->multiThreadedMessageLoop())
    settings.multi_threaded_message_loop = config->multiThreadedMessageLoop();

  if (config->backgroundColor())
    settings.background_color = config->backgroundColor();

  if (config->remoteDebuggingPort())
    settings.remote_debugging_port = config->remoteDebuggingPort();

  settings.log_severity = (cef_log_severity_t)config->logLevel();
}
