#ifndef CefBrowser_H_
#define CefBrowser_H_
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

#include "CefBrowserCallback_c.h"
#include "CefContext_c.h"
#include "CefSetting_c.h"

#if defined(__cplusplus)
extern "C"
{
#endif

  typedef struct CCefBrowser ccefbrowser_class;
  CCEFVIEW_EXPORT void CCefBrowser_Delete(ccefbrowser_class * thiz);
  CCEFVIEW_EXPORT ccefbrowser_class * CCefBrowser_new0(cefbrowsercallback_struct callback, const char * url, const ccefsetting_class * setting);
  CCEFVIEW_EXPORT void CCefBrowser_addLocalFolderResource(ccefbrowser_class * thiz, const char * path, const char * url, int priority);
  CCEFVIEW_EXPORT void CCefBrowser_addArchiveResource(ccefbrowser_class * thiz, const char * path, const char * url, const char * password, int priority);
  CCEFVIEW_EXPORT int CCefBrowser_browserId(ccefbrowser_class * thiz);
  CCEFVIEW_EXPORT void CCefBrowser_navigateToString(ccefbrowser_class * thiz, const char * content);
  CCEFVIEW_EXPORT void CCefBrowser_navigateToUrl(ccefbrowser_class * thiz, const char * url);
  CCEFVIEW_EXPORT bool CCefBrowser_canGoBack(ccefbrowser_class * thiz);
  CCEFVIEW_EXPORT bool CCefBrowser_canGoForward(ccefbrowser_class * thiz);
  CCEFVIEW_EXPORT void CCefBrowser_goBack(ccefbrowser_class * thiz);
  CCEFVIEW_EXPORT void CCefBrowser_goForward(ccefbrowser_class * thiz);
  CCEFVIEW_EXPORT bool CCefBrowser_isLoading(ccefbrowser_class * thiz);
  CCEFVIEW_EXPORT void CCefBrowser_reload(ccefbrowser_class * thiz);
  CCEFVIEW_EXPORT void CCefBrowser_stopLoad(ccefbrowser_class * thiz);
  CCEFVIEW_EXPORT bool CCefBrowser_triggerEventOnMainFrame(ccefbrowser_class * thiz, const char * evtName, const char * evtArgs);
  CCEFVIEW_EXPORT bool CCefBrowser_triggerEventOnFrame(ccefbrowser_class * thiz, const char * evtName, const char * evtArgs, int64_t frameId);
  CCEFVIEW_EXPORT bool CCefBrowser_broadcastEvent(ccefbrowser_class * thiz, const char * evtName, const char * evtArgs);
  CCEFVIEW_EXPORT bool CCefBrowser_triggerEvent(ccefbrowser_class * thiz, const char * name, const char * args, int64_t frameId);
  CCEFVIEW_EXPORT bool CCefBrowser_responseQCefQuery(ccefbrowser_class * thiz, const ccefquery_class * query);
  CCEFVIEW_EXPORT bool CCefBrowser_executeJavascript(ccefbrowser_class * thiz, int64_t frameId, const char * code, const char * url);
  CCEFVIEW_EXPORT bool CCefBrowser_executeJavascriptWithResult(ccefbrowser_class * thiz, int64_t frameId, const char * code, const char * url, int64_t context);
  CCEFVIEW_EXPORT bool CCefBrowser_setPreference(ccefbrowser_class * thiz, const char * name, const char * value);
  CCEFVIEW_EXPORT void CCefBrowser_setDisablePopupContextMenu(ccefbrowser_class * thiz, bool disable);
  CCEFVIEW_EXPORT bool CCefBrowser_isPopupContextMenuDisabled(ccefbrowser_class * thiz);
  CCEFVIEW_EXPORT void CCefBrowser_ImeSetComposition(ccefbrowser_class * thiz, const char * text, cefviewcompositionunderline_struct * underlines, int count, cefviewrange_struct replacement_range, cefviewrange_struct selection_range);
  CCEFVIEW_EXPORT void CCefBrowser_ImeCommitText(ccefbrowser_class * thiz, const char * text, cefviewrange_struct replacement_range, int relative_cursor_pos);
  CCEFVIEW_EXPORT void CCefBrowser_ImeFinishComposingText(ccefbrowser_class * thiz, bool keep_selection);
  CCEFVIEW_EXPORT void CCefBrowser_setFocus(ccefbrowser_class * thiz, bool focused);
  CCEFVIEW_EXPORT void CCefBrowser_wasResized(ccefbrowser_class * thiz);
  CCEFVIEW_EXPORT void CCefBrowser_wasHidden(ccefbrowser_class * thiz, bool hidden);
  CCEFVIEW_EXPORT void CCefBrowser_sendMouseMoveEvent(ccefbrowser_class * thiz, int x, int y, uint32_t modifiers, bool leave);
  CCEFVIEW_EXPORT void CCefBrowser_sendMouseClickEvent(ccefbrowser_class * thiz, int x, int y, uint32_t modifiers, cefviewmousebuttontype_enum type, bool mouseUp, int clickCount);
  CCEFVIEW_EXPORT void CCefBrowser_sendWheelEvent(ccefbrowser_class * thiz, int x, int y, uint32_t modifiers, int deltaX, int deltaY);
  CCEFVIEW_EXPORT void CCefBrowser_sendKeyEvent(ccefbrowser_class * thiz, cefviewkeyeventtype_enum type, uint32_t modifiers, int windowsKeyCode, int nativeKeyCode, bool isSysKey, uint16_t character, uint16_t umodifiedCharacter, bool isFocusOnEditableField);
  CCEFVIEW_EXPORT void CCefBrowser_notifyMoveOrResizeStarted(ccefbrowser_class * thiz);
  CCEFVIEW_EXPORT void CCefBrowser_notifyScreenChanged(ccefbrowser_class * thiz);

#if defined(__cplusplus)
}
#endif


#endif // CefBrowser_H_