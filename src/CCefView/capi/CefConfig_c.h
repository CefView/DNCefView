#ifndef CefConfig_H_
#define CefConfig_H_
#pragma once

#if defined _WIN32 || defined __CYGWIN__
#ifdef __GNUC__
#define CCEFVIEW_EXPORT __attribute__((dllexport))
#else
#define CCEFVIEW_EXPORT __declspec(dllexport)
#endif
#else
#if __GNUC__ >= 4
#define CCEFVIEW_EXPORT __attribute__((visibility("default")))
#else
#define CCEFVIEW_EXPORT
#endif
#endif

#include <stdint.h>

#include "CefTypes_c.h"

#if defined(__cplusplus)
extern "C"
{
#endif

  typedef struct CCefConfig ccefconfig_class;
  CCEFVIEW_EXPORT void CCefConfig_Delete(ccefconfig_class * thiz);
  CCEFVIEW_EXPORT ccefconfig_class * CCefConfig_new0();
  CCEFVIEW_EXPORT void CCefConfig_addCommandLineSwitch(ccefconfig_class * thiz, const char * smitch);
  CCEFVIEW_EXPORT void CCefConfig_addCommandLineSwitchWithValue(ccefconfig_class * thiz, const char * smitch, const char * v);
  CCEFVIEW_EXPORT void CCefConfig_setLogLevel(ccefconfig_class * thiz, cefviewloglevel_enum lvl);
  CCEFVIEW_EXPORT cefviewloglevel_enum CCefConfig_logLevel(ccefconfig_class * thiz);
  CCEFVIEW_EXPORT void CCefConfig_setLocale(ccefconfig_class * thiz, const char * locale);
  CCEFVIEW_EXPORT const char * CCefConfig_locale(ccefconfig_class * thiz);
  CCEFVIEW_EXPORT void CCefConfig_setUserAgent(ccefconfig_class * thiz, const char * agent);
  CCEFVIEW_EXPORT const char * CCefConfig_userAgent(ccefconfig_class * thiz);
  CCEFVIEW_EXPORT void CCefConfig_setCachePath(ccefconfig_class * thiz, const char * path);
  CCEFVIEW_EXPORT const char * CCefConfig_cachePath(ccefconfig_class * thiz);
  CCEFVIEW_EXPORT void CCefConfig_setUserDataPath(ccefconfig_class * thiz, const char * path);
  CCEFVIEW_EXPORT const char * CCefConfig_userDataPath(ccefconfig_class * thiz);
  CCEFVIEW_EXPORT void CCefConfig_setBridgeObjectName(ccefconfig_class * thiz, const char * name);
  CCEFVIEW_EXPORT const char * CCefConfig_bridgeObjectName(ccefconfig_class * thiz);
  CCEFVIEW_EXPORT void CCefConfig_setBackgroundColor(ccefconfig_class * thiz, uint32_t color);
  CCEFVIEW_EXPORT uint32_t CCefConfig_backgroundColor(ccefconfig_class * thiz);
  CCEFVIEW_EXPORT void CCefConfig_setAcceptLanguageList(ccefconfig_class * thiz, const char * languages);
  CCEFVIEW_EXPORT const char * CCefConfig_acceptLanguageList(ccefconfig_class * thiz);
  CCEFVIEW_EXPORT void CCefConfig_setPersistSessionCookies(ccefconfig_class * thiz, bool enabled);
  CCEFVIEW_EXPORT bool CCefConfig_persistSessionCookies(ccefconfig_class * thiz);
  CCEFVIEW_EXPORT void CCefConfig_setPersistUserPreferences(ccefconfig_class * thiz, bool enabled);
  CCEFVIEW_EXPORT bool CCefConfig_persistUserPreferences(ccefconfig_class * thiz);
  CCEFVIEW_EXPORT void CCefConfig_setMultiThreadedMessageLoop(ccefconfig_class * thiz, bool enable);
  CCEFVIEW_EXPORT bool CCefConfig_multiThreadedMessageLoop(ccefconfig_class * thiz);
  CCEFVIEW_EXPORT void CCefConfig_setRemoteDebuggingPort(ccefconfig_class * thiz, short port);
  CCEFVIEW_EXPORT short CCefConfig_remoteDebuggingPort(ccefconfig_class * thiz);

#if defined(__cplusplus)
}
#endif


#endif // CefConfig_H_