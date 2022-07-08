#include "CefBrowser_c.h"
#include "CefBrowser.h"

void CCefBrowser_Delete(ccefbrowser_class * thiz) {
  return delete thiz;
}

ccefbrowser_class * CCefBrowser_new0(cefbrowsercallback_struct callback, const char * url, const ccefsetting_class * setting) {
  return new CCefBrowser(callback, url, setting);
}

void CCefBrowser_addLocalFolderResource(ccefbrowser_class * thiz, const char * path, const char * url, int priority) {
  thiz->addLocalFolderResource(path, url, priority);
}

void CCefBrowser_addArchiveResource(ccefbrowser_class * thiz, const char * path, const char * url, const char * password, int priority) {
  thiz->addArchiveResource(path, url, password, priority);
}

int CCefBrowser_browserId(ccefbrowser_class * thiz) {
  return thiz->browserId();
}

void CCefBrowser_navigateToString(ccefbrowser_class * thiz, const char * content) {
  thiz->navigateToString(content);
}

void CCefBrowser_navigateToUrl(ccefbrowser_class * thiz, const char * url) {
  thiz->navigateToUrl(url);
}

bool CCefBrowser_canGoBack(ccefbrowser_class * thiz) {
  return thiz->canGoBack();
}

bool CCefBrowser_canGoForward(ccefbrowser_class * thiz) {
  return thiz->canGoForward();
}

void CCefBrowser_goBack(ccefbrowser_class * thiz) {
  thiz->goBack();
}

void CCefBrowser_goForward(ccefbrowser_class * thiz) {
  thiz->goForward();
}

bool CCefBrowser_isLoading(ccefbrowser_class * thiz) {
  return thiz->isLoading();
}

void CCefBrowser_reload(ccefbrowser_class * thiz) {
  thiz->reload();
}

void CCefBrowser_stopLoad(ccefbrowser_class * thiz) {
  thiz->stopLoad();
}

bool CCefBrowser_triggerEventOnMainFrame(ccefbrowser_class * thiz, const char * evtName, const char * evtArgs) {
  return thiz->triggerEventOnMainFrame(evtName, evtArgs);
}

bool CCefBrowser_triggerEventOnFrame(ccefbrowser_class * thiz, const char * evtName, const char * evtArgs, int64_t frameId) {
  return thiz->triggerEventOnFrame(evtName, evtArgs, frameId);
}

bool CCefBrowser_broadcastEvent(ccefbrowser_class * thiz, const char * evtName, const char * evtArgs) {
  return thiz->broadcastEvent(evtName, evtArgs);
}

bool CCefBrowser_triggerEvent(ccefbrowser_class * thiz, const char * name, const char * args, int64_t frameId) {
  return thiz->triggerEvent(name, args, frameId);
}

bool CCefBrowser_responseQCefQuery(ccefbrowser_class * thiz, const ccefquery_class * query) {
  return thiz->responseQCefQuery(query);
}

bool CCefBrowser_executeJavascript(ccefbrowser_class * thiz, int64_t frameId, const char * code, const char * url) {
  return thiz->executeJavascript(frameId, code, url);
}

bool CCefBrowser_executeJavascriptWithResult(ccefbrowser_class * thiz, int64_t frameId, const char * code, const char * url, int64_t context) {
  return thiz->executeJavascriptWithResult(frameId, code, url, context);
}

bool CCefBrowser_setPreference(ccefbrowser_class * thiz, const char * name, const char * value) {
  return thiz->setPreference(name, value);
}

void CCefBrowser_setDisablePopupContextMenu(ccefbrowser_class * thiz, bool disable) {
  thiz->setDisablePopupContextMenu(disable);
}

bool CCefBrowser_isPopupContextMenuDisabled(ccefbrowser_class * thiz) {
  return thiz->isPopupContextMenuDisabled();
}

void CCefBrowser_ImeSetComposition(ccefbrowser_class * thiz, const char * text, cefviewcompositionunderline_struct * underlines, int count, cefviewrange_struct replacement_range, cefviewrange_struct selection_range) {
  thiz->ImeSetComposition(text, underlines, count, replacement_range, selection_range);
}

void CCefBrowser_ImeCommitText(ccefbrowser_class * thiz, const char * text, cefviewrange_struct replacement_range, int relative_cursor_pos) {
  thiz->ImeCommitText(text, replacement_range, relative_cursor_pos);
}

void CCefBrowser_ImeFinishComposingText(ccefbrowser_class * thiz, bool keep_selection) {
  thiz->ImeFinishComposingText(keep_selection);
}

void CCefBrowser_setFocus(ccefbrowser_class * thiz, bool focused) {
  thiz->setFocus(focused);
}

void CCefBrowser_wasResized(ccefbrowser_class * thiz) {
  thiz->wasResized();
}

void CCefBrowser_wasHidden(ccefbrowser_class * thiz, bool hidden) {
  thiz->wasHidden(hidden);
}

void CCefBrowser_sendMouseMoveEvent(ccefbrowser_class * thiz, int x, int y, uint32_t modifiers, bool leave) {
  thiz->sendMouseMoveEvent(x, y, modifiers, leave);
}

void CCefBrowser_sendMouseClickEvent(ccefbrowser_class * thiz, int x, int y, uint32_t modifiers, cefviewmousebuttontype_enum type, bool mouseUp, int clickCount) {
  thiz->sendMouseClickEvent(x, y, modifiers, (cef_mouse_button_type_t)type, mouseUp, clickCount);
}

void CCefBrowser_sendWheelEvent(ccefbrowser_class * thiz, int x, int y, uint32_t modifiers, int deltaX, int deltaY) {
  thiz->sendWheelEvent(x, y, modifiers, deltaX, deltaY);
}

void CCefBrowser_sendKeyEvent(ccefbrowser_class * thiz, cefviewkeyeventtype_enum type, uint32_t modifiers, int windowsKeyCode, int nativeKeyCode, bool isSysKey, uint16_t character, uint16_t umodifiedCharacter, bool isFocusOnEditableField) {
  thiz->sendKeyEvent((cef_key_event_type_t)type, modifiers, windowsKeyCode, nativeKeyCode, isSysKey, character, umodifiedCharacter, isFocusOnEditableField);
}

void CCefBrowser_notifyMoveOrResizeStarted(ccefbrowser_class * thiz) {
  thiz->notifyMoveOrResizeStarted();
}

void CCefBrowser_notifyScreenChanged(ccefbrowser_class * thiz) {
  thiz->notifyScreenChanged();
}

