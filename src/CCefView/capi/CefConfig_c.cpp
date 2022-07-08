#include "CefConfig_c.h"
#include "CefConfig.h"

void CCefConfig_Delete(ccefconfig_class * thiz) {
  return delete thiz;
}

ccefconfig_class * CCefConfig_new0() {
  return new CCefConfig();
}

void CCefConfig_addCommandLineSwitch(ccefconfig_class * thiz, const char * smitch) {
  thiz->addCommandLineSwitch(smitch);
}

void CCefConfig_addCommandLineSwitchWithValue(ccefconfig_class * thiz, const char * smitch, const char * v) {
  thiz->addCommandLineSwitchWithValue(smitch, v);
}

void CCefConfig_setLogLevel(ccefconfig_class * thiz, cefviewloglevel_enum lvl) {
  thiz->setLogLevel((cef_log_severity_t)lvl);
}

cefviewloglevel_enum CCefConfig_logLevel(ccefconfig_class * thiz) {
  return (cef_log_severity_t)thiz->logLevel();
}

void CCefConfig_setLocale(ccefconfig_class * thiz, const char * locale) {
  thiz->setLocale(locale);
}

const char * CCefConfig_locale(ccefconfig_class * thiz) {
  return thiz->locale().c_str();
}

void CCefConfig_setUserAgent(ccefconfig_class * thiz, const char * agent) {
  thiz->setUserAgent(agent);
}

const char * CCefConfig_userAgent(ccefconfig_class * thiz) {
  return thiz->userAgent().c_str();
}

void CCefConfig_setCachePath(ccefconfig_class * thiz, const char * path) {
  thiz->setCachePath(path);
}

const char * CCefConfig_cachePath(ccefconfig_class * thiz) {
  return thiz->cachePath().c_str();
}

void CCefConfig_setUserDataPath(ccefconfig_class * thiz, const char * path) {
  thiz->setUserDataPath(path);
}

const char * CCefConfig_userDataPath(ccefconfig_class * thiz) {
  return thiz->userDataPath().c_str();
}

void CCefConfig_setBridgeObjectName(ccefconfig_class * thiz, const char * name) {
  thiz->setBridgeObjectName(name);
}

const char * CCefConfig_bridgeObjectName(ccefconfig_class * thiz) {
  return thiz->bridgeObjectName().c_str();
}

void CCefConfig_setBackgroundColor(ccefconfig_class * thiz, uint32_t color) {
  thiz->setBackgroundColor(color);
}

uint32_t CCefConfig_backgroundColor(ccefconfig_class * thiz) {
  return thiz->backgroundColor();
}

void CCefConfig_setAcceptLanguageList(ccefconfig_class * thiz, const char * languages) {
  thiz->setAcceptLanguageList(languages);
}

const char * CCefConfig_acceptLanguageList(ccefconfig_class * thiz) {
  return thiz->acceptLanguageList().c_str();
}

void CCefConfig_setPersistSessionCookies(ccefconfig_class * thiz, bool enabled) {
  thiz->setPersistSessionCookies(enabled);
}

bool CCefConfig_persistSessionCookies(ccefconfig_class * thiz) {
  return thiz->persistSessionCookies();
}

void CCefConfig_setPersistUserPreferences(ccefconfig_class * thiz, bool enabled) {
  thiz->setPersistUserPreferences(enabled);
}

bool CCefConfig_persistUserPreferences(ccefconfig_class * thiz) {
  return thiz->persistUserPreferences();
}

void CCefConfig_setMultiThreadedMessageLoop(ccefconfig_class * thiz, bool enable) {
  thiz->setMultiThreadedMessageLoop(enable);
}

bool CCefConfig_multiThreadedMessageLoop(ccefconfig_class * thiz) {
  return thiz->multiThreadedMessageLoop();
}

void CCefConfig_setRemoteDebuggingPort(ccefconfig_class * thiz, short port) {
  thiz->setRemoteDebuggingPort(port);
}

short CCefConfig_remoteDebuggingPort(ccefconfig_class * thiz) {
  return thiz->remoteDebuggingPort();
}

