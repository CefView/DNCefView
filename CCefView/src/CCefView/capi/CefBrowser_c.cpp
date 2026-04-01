// Auto-generated file. Do not modify.
// clang-format off
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

bool CCefBrowser_triggerEventOnFrame(ccefbrowser_class * thiz, const char * evtName, const char * evtArgs, const char * frameId) {
  return thiz->triggerEventOnFrame(evtName, evtArgs, frameId);
}

bool CCefBrowser_broadcastEvent(ccefbrowser_class * thiz, const char * evtName, const char * evtArgs) {
  return thiz->broadcastEvent(evtName, evtArgs);
}

bool CCefBrowser_triggerEvent(ccefbrowser_class * thiz, const char * name, const char * args, const char * frameId) {
  return thiz->triggerEvent(name, args, frameId);
}

bool CCefBrowser_replyCefQuery(ccefbrowser_class * thiz, const ccefquery_class * query) {
  return thiz->replyCefQuery(query);
}

bool CCefBrowser_executeJavascript(ccefbrowser_class * thiz, const char * frameId, const char * code, const char * url) {
  return thiz->executeJavascript(frameId, code, url);
}

bool CCefBrowser_executeJavascriptWithResult(ccefbrowser_class * thiz, const char * frameId, const char * code, const char * url, const char * context) {
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

void CCefBrowser_setWindowlessFrameRate(ccefbrowser_class * thiz, int rate) {
  thiz->setWindowlessFrameRate(rate);
}

void CCefBrowser_sendExternalBeginFrame(ccefbrowser_class * thiz) {
  thiz->sendExternalBeginFrame();
}

void CCefBrowser_showDevTools(ccefbrowser_class * thiz) {
  thiz->showDevTools();
}

void CCefBrowser_closeDevTools(ccefbrowser_class * thiz) {
  thiz->closeDevTools();
}

bool CCefBrowser_hasDevTools(ccefbrowser_class * thiz) {
  return thiz->hasDevTools();
}

void CCefBrowser_closeBrowser(ccefbrowser_class * thiz, bool forceClose) {
  thiz->closeBrowser(forceClose);
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

void CCefBrowser_dragTargetDragEnterText(ccefbrowser_class * thiz, int x, int y, uint32_t modifiers, const char * text, const char * html, const char * baseUrl, cefviewdragoperation_enum allowedOps) {
  thiz->dragTargetDragEnterText(x, y, modifiers, text, html, baseUrl, (cef_drag_operations_mask_t)allowedOps);
}

void CCefBrowser_dragTargetDragEnterFiles(ccefbrowser_class * thiz, int x, int y, uint32_t modifiers, const char * filePaths, cefviewdragoperation_enum allowedOps) {
  thiz->dragTargetDragEnterFiles(x, y, modifiers, filePaths, (cef_drag_operations_mask_t)allowedOps);
}

void CCefBrowser_dragTargetDragOver(ccefbrowser_class * thiz, int x, int y, uint32_t modifiers, cefviewdragoperation_enum allowedOps) {
  thiz->dragTargetDragOver(x, y, modifiers, (cef_drag_operations_mask_t)allowedOps);
}

void CCefBrowser_dragTargetDragLeave(ccefbrowser_class * thiz) {
  thiz->dragTargetDragLeave();
}

void CCefBrowser_dragTargetDrop(ccefbrowser_class * thiz, int x, int y, uint32_t modifiers) {
  thiz->dragTargetDrop(x, y, modifiers);
}

void CCefBrowser_dragSourceEndedAt(ccefbrowser_class * thiz, int x, int y, cefviewdragoperation_enum operation) {
  thiz->dragSourceEndedAt(x, y, (cef_drag_operations_mask_t)operation);
}

void CCefBrowser_dragSourceSystemDragEnded(ccefbrowser_class * thiz) {
  thiz->dragSourceSystemDragEnded();
}

void CCefBrowser_sendTouchEvent(ccefbrowser_class * thiz, int touchId, float x, float y, float radiusX, float radiusY, float rotationAngle, float pressure, int touchEventType, uint32_t modifiers, int pointerType) {
  thiz->sendTouchEvent(touchId, x, y, radiusX, radiusY, rotationAngle, pressure, touchEventType, modifiers, pointerType);
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

void CCefBrowser_imeSetComposition(ccefbrowser_class * thiz, const char * text, cefviewcompositionunderline_struct underlines[], int count, cefviewrange_struct replacementRange, cefviewrange_struct selectionRange) {
  thiz->imeSetComposition(text, underlines, count, replacementRange, selectionRange);
}

void CCefBrowser_imeCommitText(ccefbrowser_class * thiz, const char * text, cefviewrange_struct replacementRange, int relativeCursorPos) {
  thiz->imeCommitText(text, replacementRange, relativeCursorPos);
}

void CCefBrowser_imeFinishComposingText(ccefbrowser_class * thiz, bool keepSelection) {
  thiz->imeFinishComposingText(keepSelection);
}

void CCefBrowser_imeCancelComposition(ccefbrowser_class * thiz) {
  thiz->imeCancelComposition();
}

bool CCefBrowser_continueJSDialog(ccefbrowser_class * thiz, void * dialogHandle, bool success, const char * userInput) {
  return thiz->continueJSDialog(dialogHandle, success, userInput);
}

