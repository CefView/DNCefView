#include "CefConfig.h"

#include <CefViewCoreProtocol.h>

CCefConfig::CCefConfig()
{
  backgroundColor_ = 0xFFFFFFFF;
  logLevel_ = LOGSEVERITY_DEFAULT;
  userAgent_ = kCefViewDefaultUserAgent;
}

CCefConfig::~CCefConfig() {}

void
CCefConfig::addCommandLineSwitch(const std::string& smitch)
{
  commandLineArgs_[smitch] = std::string();
}

void
CCefConfig::addCommandLineSwitchWithValue(const std::string& smitch, const std::string& v)
{
  commandLineArgs_[smitch] = v;
}

void
CCefConfig::setLogLevel(CefViewLogLevel lvl)
{
  logLevel_ = lvl;
}

CefViewLogLevel
CCefConfig::logLevel() const
{
  return logLevel_;
}

void
CCefConfig::setLocale(const std::string& locale)
{
  locale_ = locale;
}

const std::string&
CCefConfig::locale() const
{
  return locale_;
}

void
CCefConfig::setUserAgent(const std::string& agent)
{
  userAgent_ = agent;
}

const std::string&
CCefConfig::userAgent() const
{
  return userAgent_;
}

void
CCefConfig::setCachePath(const std::string& path)
{
  cachePath_ = path;
}

const std::string&
CCefConfig::cachePath() const
{
  return cachePath_;
}

void
CCefConfig::setUserDataPath(const std::string& path)
{
  userDataPath_ = path;
}

const std::string&
CCefConfig::userDataPath() const
{
  return userDataPath_;
}

void
CCefConfig::setBridgeObjectName(const std::string& name)
{
  bridgeObjectName_ = name;
}

const std::string&
CCefConfig::bridgeObjectName() const
{
  return bridgeObjectName_;
}

void
CCefConfig::setBuiltinSchemaName(const std::string& name)
{
  builtinSchemaName_ = name;
}

const std::string&
CCefConfig::builtinSchemaName() const
{
  return builtinSchemaName_;
}

void
CCefConfig::setBackgroundColor(uint32_t color)
{
  backgroundColor_ = color;
}

uint32_t
CCefConfig::backgroundColor() const
{
  return backgroundColor_.value_or(-1);
}

void
CCefConfig::setAcceptLanguageList(const std::string& languages)
{
  acceptLanguageList_ = languages;
}

const std::string&
CCefConfig::acceptLanguageList() const
{
  return acceptLanguageList_;
}

void
CCefConfig::setPersistSessionCookies(bool enabled)
{
  persistSessionCookies_ = enabled;
}

bool
CCefConfig::persistSessionCookies() const
{
  return persistSessionCookies_.value_or(false);
}

void
CCefConfig::setPersistUserPreferences(bool enabled)
{
  persistUserPreferences_ = enabled;
}

bool
CCefConfig::persistUserPreferences() const
{
  return persistUserPreferences_.value_or(false);
}

void
CCefConfig::setMultiThreadedMessageLoop(bool enable)
{
  multiThreadedMessageLoop_ = enable;
}

bool
CCefConfig::multiThreadedMessageLoop() const
{
  return multiThreadedMessageLoop_.value_or(false);
}

void
CCefConfig::setRemoteDebuggingPort(short port)
{
  remoteDebuggingPort_ = port;
}

short
CCefConfig::remoteDebuggingPort() const
{
  return remoteDebuggingPort_.value_or(-1);
}

const CCefConfig::CCefConfig::ArgsMap&
CCefConfig::GetCommandLineArgs(const CCefConfig* config)
{
  // validate the input source config parameter
  if (!config) {
    // return an empty map
    static ArgsMap emptyMap;
    return emptyMap;
  }

  // return the command line arguments from the config
  return config->commandLineArgs_;
}

void
CCefConfig::CopyToCefSettings(const CCefConfig* config, CefSettings& settings)
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
