#include "CefSetting_c.h"
#include "CefSetting.h"

void CCefSetting_Delete(ccefsetting_class * thiz) {
  return delete thiz;
}

ccefsetting_class * CCefSetting_new0() {
  return new CCefSetting();
}

void CCefSetting_setStandardFontFamily(ccefsetting_class * thiz, const char * value) {
  thiz->setStandardFontFamily(value);
}

const char * CCefSetting_standardFontFamily(ccefsetting_class * thiz) {
  return thiz->standardFontFamily().c_str();
}

void CCefSetting_setFixedFontFamily(ccefsetting_class * thiz, const char * value) {
  thiz->setFixedFontFamily(value);
}

const char * CCefSetting_fixedFontFamily(ccefsetting_class * thiz) {
  return thiz->fixedFontFamily().c_str();
}

void CCefSetting_setSerifFontFamily(ccefsetting_class * thiz, const char * value) {
  thiz->setSerifFontFamily(value);
}

const char * CCefSetting_serifFontFamily(ccefsetting_class * thiz) {
  return thiz->serifFontFamily().c_str();
}

void CCefSetting_setSansSerifFontFamily(ccefsetting_class * thiz, const char * value) {
  thiz->setSansSerifFontFamily(value);
}

const char * CCefSetting_sansSerifFontFamily(ccefsetting_class * thiz) {
  return thiz->sansSerifFontFamily().c_str();
}

void CCefSetting_setCursiveFontFamily(ccefsetting_class * thiz, const char * value) {
  thiz->setCursiveFontFamily(value);
}

const char * CCefSetting_cursiveFontFamily(ccefsetting_class * thiz) {
  return thiz->cursiveFontFamily().c_str();
}

void CCefSetting_setFantasyFontFamily(ccefsetting_class * thiz, const char * value) {
  thiz->setFantasyFontFamily(value);
}

const char * CCefSetting_fantasyFontFamily(ccefsetting_class * thiz) {
  return thiz->fantasyFontFamily().c_str();
}

void CCefSetting_setDefaultEncoding(ccefsetting_class * thiz, const char * value) {
  thiz->setDefaultEncoding(value);
}

const char * CCefSetting_defaultEncoding(ccefsetting_class * thiz) {
  return thiz->defaultEncoding().c_str();
}

void CCefSetting_setAcceptLanguageList(ccefsetting_class * thiz, const char * value) {
  thiz->setAcceptLanguageList(value);
}

const char * CCefSetting_acceptLanguageList(ccefsetting_class * thiz) {
  return thiz->acceptLanguageList().c_str();
}

void CCefSetting_setWindowlessFrameRate(ccefsetting_class * thiz, const int value) {
  thiz->setWindowlessFrameRate(value);
}

int CCefSetting_windowlessFrameRate(ccefsetting_class * thiz) {
  return thiz->windowlessFrameRate();
}

void CCefSetting_setDefaultFontSize(ccefsetting_class * thiz, const int value) {
  thiz->setDefaultFontSize(value);
}

int CCefSetting_defaultFontSize(ccefsetting_class * thiz) {
  return thiz->defaultFontSize();
}

void CCefSetting_setDefaultFixedFontSize(ccefsetting_class * thiz, const int value) {
  thiz->setDefaultFixedFontSize(value);
}

int CCefSetting_defaultFixedFontSize(ccefsetting_class * thiz) {
  return thiz->defaultFixedFontSize();
}

void CCefSetting_setMinimumFontSize(ccefsetting_class * thiz, const int value) {
  thiz->setMinimumFontSize(value);
}

int CCefSetting_minimumFontSize(ccefsetting_class * thiz) {
  return thiz->minimumFontSize();
}

void CCefSetting_setMinimumLogicalFontSize(ccefsetting_class * thiz, const int value) {
  thiz->setMinimumLogicalFontSize(value);
}

int CCefSetting_minimumLogicalFontSize(ccefsetting_class * thiz) {
  return thiz->minimumLogicalFontSize();
}

void CCefSetting_setRemoteFonts(ccefsetting_class * thiz, cefviewpluingstate_enum value) {
  thiz->setRemoteFonts((cef_state_t)value);
}

cefviewpluingstate_enum CCefSetting_remoteFonts(ccefsetting_class * thiz) {
  return (cef_state_t)thiz->remoteFonts();
}

void CCefSetting_setJavascript(ccefsetting_class * thiz, cefviewpluingstate_enum value) {
  thiz->setJavascript((cef_state_t)value);
}

cefviewpluingstate_enum CCefSetting_javascript(ccefsetting_class * thiz) {
  return (cef_state_t)thiz->javascript();
}

void CCefSetting_setJavascriptCloseWindows(ccefsetting_class * thiz, cefviewpluingstate_enum value) {
  thiz->setJavascriptCloseWindows((cef_state_t)value);
}

cefviewpluingstate_enum CCefSetting_javascriptCloseWindows(ccefsetting_class * thiz) {
  return (cef_state_t)thiz->javascriptCloseWindows();
}

void CCefSetting_setJavascriptAccessClipboard(ccefsetting_class * thiz, cefviewpluingstate_enum value) {
  thiz->setJavascriptAccessClipboard((cef_state_t)value);
}

cefviewpluingstate_enum CCefSetting_javascriptAccessClipboard(ccefsetting_class * thiz) {
  return (cef_state_t)thiz->javascriptAccessClipboard();
}

void CCefSetting_setJavascriptDomPaste(ccefsetting_class * thiz, cefviewpluingstate_enum value) {
  thiz->setJavascriptDomPaste((cef_state_t)value);
}

cefviewpluingstate_enum CCefSetting_javascriptDomPaste(ccefsetting_class * thiz) {
  return (cef_state_t)thiz->javascriptDomPaste();
}

void CCefSetting_setPlugins(ccefsetting_class * thiz, cefviewpluingstate_enum value) {
  thiz->setPlugins((cef_state_t)value);
}

cefviewpluingstate_enum CCefSetting_plugins(ccefsetting_class * thiz) {
  return (cef_state_t)thiz->plugins();
}

void CCefSetting_setImageLoading(ccefsetting_class * thiz, cefviewpluingstate_enum value) {
  thiz->setImageLoading((cef_state_t)value);
}

cefviewpluingstate_enum CCefSetting_imageLoading(ccefsetting_class * thiz) {
  return (cef_state_t)thiz->imageLoading();
}

void CCefSetting_setImageShrinkStandaloneToFit(ccefsetting_class * thiz, cefviewpluingstate_enum value) {
  thiz->setImageShrinkStandaloneToFit((cef_state_t)value);
}

cefviewpluingstate_enum CCefSetting_imageShrinkStandaloneToFit(ccefsetting_class * thiz) {
  return (cef_state_t)thiz->imageShrinkStandaloneToFit();
}

void CCefSetting_setTextAreaResize(ccefsetting_class * thiz, cefviewpluingstate_enum value) {
  thiz->setTextAreaResize((cef_state_t)value);
}

cefviewpluingstate_enum CCefSetting_textAreaResize(ccefsetting_class * thiz) {
  return (cef_state_t)thiz->textAreaResize();
}

void CCefSetting_setTabToLinks(ccefsetting_class * thiz, cefviewpluingstate_enum value) {
  thiz->setTabToLinks((cef_state_t)value);
}

cefviewpluingstate_enum CCefSetting_tabToLinks(ccefsetting_class * thiz) {
  return (cef_state_t)thiz->tabToLinks();
}

void CCefSetting_setLocalStorage(ccefsetting_class * thiz, cefviewpluingstate_enum value) {
  thiz->setLocalStorage((cef_state_t)value);
}

cefviewpluingstate_enum CCefSetting_localStorage(ccefsetting_class * thiz) {
  return (cef_state_t)thiz->localStorage();
}

void CCefSetting_setDatabases(ccefsetting_class * thiz, cefviewpluingstate_enum value) {
  thiz->setDatabases((cef_state_t)value);
}

cefviewpluingstate_enum CCefSetting_databases(ccefsetting_class * thiz) {
  return (cef_state_t)thiz->databases();
}

void CCefSetting_setWebGL(ccefsetting_class * thiz, cefviewpluingstate_enum value) {
  thiz->setWebGL((cef_state_t)value);
}

cefviewpluingstate_enum CCefSetting_webGL(ccefsetting_class * thiz) {
  return (cef_state_t)thiz->webGL();
}

void CCefSetting_setBackgroundColor(ccefsetting_class * thiz, const uint32_t & value) {
  thiz->setBackgroundColor(value);
}

uint32_t CCefSetting_backgroundColor(ccefsetting_class * thiz) {
  return thiz->backgroundColor();
}

