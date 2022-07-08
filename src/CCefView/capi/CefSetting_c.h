#ifndef CefSetting_H_
#define CefSetting_H_
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

  typedef struct CCefSetting ccefsetting_class;
  CCEFVIEW_EXPORT void CCefSetting_Delete(ccefsetting_class * thiz);
  CCEFVIEW_EXPORT ccefsetting_class * CCefSetting_new0();
  CCEFVIEW_EXPORT void CCefSetting_setStandardFontFamily(ccefsetting_class * thiz, const char * value);
  CCEFVIEW_EXPORT const char * CCefSetting_standardFontFamily(ccefsetting_class * thiz);
  CCEFVIEW_EXPORT void CCefSetting_setFixedFontFamily(ccefsetting_class * thiz, const char * value);
  CCEFVIEW_EXPORT const char * CCefSetting_fixedFontFamily(ccefsetting_class * thiz);
  CCEFVIEW_EXPORT void CCefSetting_setSerifFontFamily(ccefsetting_class * thiz, const char * value);
  CCEFVIEW_EXPORT const char * CCefSetting_serifFontFamily(ccefsetting_class * thiz);
  CCEFVIEW_EXPORT void CCefSetting_setSansSerifFontFamily(ccefsetting_class * thiz, const char * value);
  CCEFVIEW_EXPORT const char * CCefSetting_sansSerifFontFamily(ccefsetting_class * thiz);
  CCEFVIEW_EXPORT void CCefSetting_setCursiveFontFamily(ccefsetting_class * thiz, const char * value);
  CCEFVIEW_EXPORT const char * CCefSetting_cursiveFontFamily(ccefsetting_class * thiz);
  CCEFVIEW_EXPORT void CCefSetting_setFantasyFontFamily(ccefsetting_class * thiz, const char * value);
  CCEFVIEW_EXPORT const char * CCefSetting_fantasyFontFamily(ccefsetting_class * thiz);
  CCEFVIEW_EXPORT void CCefSetting_setDefaultEncoding(ccefsetting_class * thiz, const char * value);
  CCEFVIEW_EXPORT const char * CCefSetting_defaultEncoding(ccefsetting_class * thiz);
  CCEFVIEW_EXPORT void CCefSetting_setAcceptLanguageList(ccefsetting_class * thiz, const char * value);
  CCEFVIEW_EXPORT const char * CCefSetting_acceptLanguageList(ccefsetting_class * thiz);
  CCEFVIEW_EXPORT void CCefSetting_setWindowlessFrameRate(ccefsetting_class * thiz, const int value);
  CCEFVIEW_EXPORT int CCefSetting_windowlessFrameRate(ccefsetting_class * thiz);
  CCEFVIEW_EXPORT void CCefSetting_setDefaultFontSize(ccefsetting_class * thiz, const int value);
  CCEFVIEW_EXPORT int CCefSetting_defaultFontSize(ccefsetting_class * thiz);
  CCEFVIEW_EXPORT void CCefSetting_setDefaultFixedFontSize(ccefsetting_class * thiz, const int value);
  CCEFVIEW_EXPORT int CCefSetting_defaultFixedFontSize(ccefsetting_class * thiz);
  CCEFVIEW_EXPORT void CCefSetting_setMinimumFontSize(ccefsetting_class * thiz, const int value);
  CCEFVIEW_EXPORT int CCefSetting_minimumFontSize(ccefsetting_class * thiz);
  CCEFVIEW_EXPORT void CCefSetting_setMinimumLogicalFontSize(ccefsetting_class * thiz, const int value);
  CCEFVIEW_EXPORT int CCefSetting_minimumLogicalFontSize(ccefsetting_class * thiz);
  CCEFVIEW_EXPORT void CCefSetting_setRemoteFonts(ccefsetting_class * thiz, cefviewpluingstate_enum value);
  CCEFVIEW_EXPORT cefviewpluingstate_enum CCefSetting_remoteFonts(ccefsetting_class * thiz);
  CCEFVIEW_EXPORT void CCefSetting_setJavascript(ccefsetting_class * thiz, cefviewpluingstate_enum value);
  CCEFVIEW_EXPORT cefviewpluingstate_enum CCefSetting_javascript(ccefsetting_class * thiz);
  CCEFVIEW_EXPORT void CCefSetting_setJavascriptCloseWindows(ccefsetting_class * thiz, cefviewpluingstate_enum value);
  CCEFVIEW_EXPORT cefviewpluingstate_enum CCefSetting_javascriptCloseWindows(ccefsetting_class * thiz);
  CCEFVIEW_EXPORT void CCefSetting_setJavascriptAccessClipboard(ccefsetting_class * thiz, cefviewpluingstate_enum value);
  CCEFVIEW_EXPORT cefviewpluingstate_enum CCefSetting_javascriptAccessClipboard(ccefsetting_class * thiz);
  CCEFVIEW_EXPORT void CCefSetting_setJavascriptDomPaste(ccefsetting_class * thiz, cefviewpluingstate_enum value);
  CCEFVIEW_EXPORT cefviewpluingstate_enum CCefSetting_javascriptDomPaste(ccefsetting_class * thiz);
  CCEFVIEW_EXPORT void CCefSetting_setPlugins(ccefsetting_class * thiz, cefviewpluingstate_enum value);
  CCEFVIEW_EXPORT cefviewpluingstate_enum CCefSetting_plugins(ccefsetting_class * thiz);
  CCEFVIEW_EXPORT void CCefSetting_setImageLoading(ccefsetting_class * thiz, cefviewpluingstate_enum value);
  CCEFVIEW_EXPORT cefviewpluingstate_enum CCefSetting_imageLoading(ccefsetting_class * thiz);
  CCEFVIEW_EXPORT void CCefSetting_setImageShrinkStandaloneToFit(ccefsetting_class * thiz, cefviewpluingstate_enum value);
  CCEFVIEW_EXPORT cefviewpluingstate_enum CCefSetting_imageShrinkStandaloneToFit(ccefsetting_class * thiz);
  CCEFVIEW_EXPORT void CCefSetting_setTextAreaResize(ccefsetting_class * thiz, cefviewpluingstate_enum value);
  CCEFVIEW_EXPORT cefviewpluingstate_enum CCefSetting_textAreaResize(ccefsetting_class * thiz);
  CCEFVIEW_EXPORT void CCefSetting_setTabToLinks(ccefsetting_class * thiz, cefviewpluingstate_enum value);
  CCEFVIEW_EXPORT cefviewpluingstate_enum CCefSetting_tabToLinks(ccefsetting_class * thiz);
  CCEFVIEW_EXPORT void CCefSetting_setLocalStorage(ccefsetting_class * thiz, cefviewpluingstate_enum value);
  CCEFVIEW_EXPORT cefviewpluingstate_enum CCefSetting_localStorage(ccefsetting_class * thiz);
  CCEFVIEW_EXPORT void CCefSetting_setDatabases(ccefsetting_class * thiz, cefviewpluingstate_enum value);
  CCEFVIEW_EXPORT cefviewpluingstate_enum CCefSetting_databases(ccefsetting_class * thiz);
  CCEFVIEW_EXPORT void CCefSetting_setWebGL(ccefsetting_class * thiz, cefviewpluingstate_enum value);
  CCEFVIEW_EXPORT cefviewpluingstate_enum CCefSetting_webGL(ccefsetting_class * thiz);
  CCEFVIEW_EXPORT void CCefSetting_setBackgroundColor(ccefsetting_class * thiz, const uint32_t & value);
  CCEFVIEW_EXPORT uint32_t CCefSetting_backgroundColor(ccefsetting_class * thiz);

#if defined(__cplusplus)
}
#endif


#endif // CefSetting_H_