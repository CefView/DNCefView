// Maintained interop snapshot: preserve UI-thread dispatch when merging generated candidates.
// clang-format off
#include "CefBrowser_c.h"
#include "CefBrowser.h"
#include "UiDispatch.h"

void CCefBrowser_Delete(ccefbrowser_class * thiz) {
  OnCefUi([=]() { delete thiz; });
}

ccefbrowser_class * CCefBrowser_new0(cefbrowsercallback_struct callback, const char * url, const ccefsetting_class * setting) {
  return OnCefUi([=]() { return new CCefBrowser(callback, url, setting); });
}

void CCefBrowser_start(ccefbrowser_class * thiz) {
  OnCefUi([=]() { thiz->start(); });
}

void CCefBrowser_addLocalFolderResource(ccefbrowser_class * thiz, const char * path, const char * url, int priority) {
  OnCefUi([=]() { thiz->addLocalFolderResource(path, url, priority); });
}

void CCefBrowser_addArchiveResource(ccefbrowser_class * thiz, const char * path, const char * url, const char * password, int priority) {
  OnCefUi([=]() { thiz->addArchiveResource(path, url, password, priority); });
}

int CCefBrowser_browserId(ccefbrowser_class * thiz) {
  return OnCefUi([=]() { return thiz->browserId(); });
}

void CCefBrowser_navigateToString(ccefbrowser_class * thiz, const char * content) {
  OnCefUi([=]() { thiz->navigateToString(content); });
}

void CCefBrowser_navigateToUrl(ccefbrowser_class * thiz, const char * url) {
  OnCefUi([=]() { thiz->navigateToUrl(url); });
}

bool CCefBrowser_canGoBack(ccefbrowser_class * thiz) {
  return OnCefUi([=]() { return thiz->canGoBack(); });
}

bool CCefBrowser_canGoForward(ccefbrowser_class * thiz) {
  return OnCefUi([=]() { return thiz->canGoForward(); });
}

void CCefBrowser_goBack(ccefbrowser_class * thiz) {
  OnCefUi([=]() { thiz->goBack(); });
}

void CCefBrowser_goForward(ccefbrowser_class * thiz) {
  OnCefUi([=]() { thiz->goForward(); });
}

bool CCefBrowser_isLoading(ccefbrowser_class * thiz) {
  return OnCefUi([=]() { return thiz->isLoading(); });
}

void CCefBrowser_reload(ccefbrowser_class * thiz) {
  OnCefUi([=]() { thiz->reload(); });
}

void CCefBrowser_stopLoad(ccefbrowser_class * thiz) {
  OnCefUi([=]() { thiz->stopLoad(); });
}

bool CCefBrowser_triggerEventOnMainFrame(ccefbrowser_class * thiz, const char * evtName, const char * evtArgs) {
  return OnCefUi([=]() { return thiz->triggerEventOnMainFrame(evtName, evtArgs); });
}

bool CCefBrowser_triggerEventOnFrame(ccefbrowser_class * thiz, const char * evtName, const char * evtArgs, const char * frameId) {
  return OnCefUi([=]() { return thiz->triggerEventOnFrame(evtName, evtArgs, frameId); });
}

bool CCefBrowser_broadcastEvent(ccefbrowser_class * thiz, const char * evtName, const char * evtArgs) {
  return OnCefUi([=]() { return thiz->broadcastEvent(evtName, evtArgs); });
}

bool CCefBrowser_triggerEvent(ccefbrowser_class * thiz, const char * name, const char * args, const char * frameId) {
  return OnCefUi([=]() { return thiz->triggerEvent(name, args, frameId); });
}

bool CCefBrowser_responseQCefQuery(ccefbrowser_class * thiz, const ccefquery_class * query) {
  return OnCefUi([=]() { return thiz->responseQCefQuery(query); });
}

bool CCefBrowser_executeJavascript(ccefbrowser_class * thiz, const char * frameId, const char * code, const char * url) {
  return OnCefUi([=]() { return thiz->executeJavascript(frameId, code, url); });
}

bool CCefBrowser_executeJavascriptWithResult(ccefbrowser_class * thiz, const char * frameId, const char * code, const char * url, const char * context) {
  return OnCefUi([=]() { return thiz->executeJavascriptWithResult(frameId, code, url, context); });
}

bool CCefBrowser_setPreference(ccefbrowser_class * thiz, const char * name, const char * value) {
  return OnCefUi([=]() { return thiz->setPreference(name, value); });
}

void CCefBrowser_setDisablePopupContextMenu(ccefbrowser_class * thiz, bool disable) {
  OnCefUi([=]() { thiz->setDisablePopupContextMenu(disable); });
}

bool CCefBrowser_isPopupContextMenuDisabled(ccefbrowser_class * thiz) {
  return OnCefUi([=]() { return thiz->isPopupContextMenuDisabled(); });
}

void CCefBrowser_setWindowlessFrameRate(ccefbrowser_class * thiz, int rate) {
  OnCefUi([=]() { thiz->setWindowlessFrameRate(rate); });
}

void CCefBrowser_sendExternalBeginFrame(ccefbrowser_class * thiz) {
  OnCefUi([=]() { thiz->sendExternalBeginFrame(); });
}

void CCefBrowser_showDevTools(ccefbrowser_class * thiz) {
  OnCefUi([=]() { thiz->showDevTools(); });
}

void CCefBrowser_closeDevTools(ccefbrowser_class * thiz) {
  OnCefUi([=]() { thiz->closeDevTools(); });
}

bool CCefBrowser_hasDevTools(ccefbrowser_class * thiz) {
  return OnCefUi([=]() { return thiz->hasDevTools(); });
}

void CCefBrowser_closeBrowser(ccefbrowser_class * thiz, bool forceClose) {
  OnCefUi([=]() { thiz->closeBrowser(forceClose); });
}

bool CCefBrowser_continueJSDialog(ccefbrowser_class * thiz, int64_t requestId, bool success, const char * userInput) {
  const char * safeInput = userInput ? userInput : "";
  return OnCefUi([=]() { return thiz->continueJSDialog(requestId, success, safeInput); });
}

void CCefBrowser_setFocus(ccefbrowser_class * thiz, bool focused) {
  OnCefUi([=]() { thiz->setFocus(focused); });
}

void CCefBrowser_wasResized(ccefbrowser_class * thiz) {
  OnCefUi([=]() { thiz->wasResized(); });
}

void CCefBrowser_wasHidden(ccefbrowser_class * thiz, bool hidden) {
  OnCefUi([=]() { thiz->wasHidden(hidden); });
}

void CCefBrowser_sendMouseMoveEvent(ccefbrowser_class * thiz, int x, int y, uint32_t modifiers, bool leave) {
  OnCefUi([=]() { thiz->sendMouseMoveEvent(x, y, modifiers, leave); });
}

void CCefBrowser_sendMouseClickEvent(ccefbrowser_class * thiz, int x, int y, uint32_t modifiers, cefviewmousebuttontype_enum type, bool mouseUp, int clickCount) {
  OnCefUi([=]() { thiz->sendMouseClickEvent(x, y, modifiers, (cef_mouse_button_type_t)type, mouseUp, clickCount); });
}

void CCefBrowser_sendWheelEvent(ccefbrowser_class * thiz, int x, int y, uint32_t modifiers, int deltaX, int deltaY) {
  OnCefUi([=]() { thiz->sendWheelEvent(x, y, modifiers, deltaX, deltaY); });
}

void CCefBrowser_dragTargetDragEnterText(ccefbrowser_class * thiz, int x, int y, uint32_t modifiers, const char * text, const char * html, const char * baseUrl, cefviewdragoperation_enum allowedOps) {
  OnCefUi([=]() { thiz->dragTargetDragEnterText(x, y, modifiers, text, html, baseUrl, (cef_drag_operations_mask_t)allowedOps); });
}

void CCefBrowser_dragTargetDragEnterFiles(ccefbrowser_class * thiz, int x, int y, uint32_t modifiers, const char * filePaths, cefviewdragoperation_enum allowedOps) {
  OnCefUi([=]() { thiz->dragTargetDragEnterFiles(x, y, modifiers, filePaths, (cef_drag_operations_mask_t)allowedOps); });
}

void CCefBrowser_dragTargetDragOver(ccefbrowser_class * thiz, int x, int y, uint32_t modifiers, cefviewdragoperation_enum allowedOps) {
  OnCefUi([=]() { thiz->dragTargetDragOver(x, y, modifiers, (cef_drag_operations_mask_t)allowedOps); });
}

void CCefBrowser_dragTargetDragLeave(ccefbrowser_class * thiz) {
  OnCefUi([=]() { thiz->dragTargetDragLeave(); });
}

void CCefBrowser_dragTargetDrop(ccefbrowser_class * thiz, int x, int y, uint32_t modifiers) {
  OnCefUi([=]() { thiz->dragTargetDrop(x, y, modifiers); });
}

void CCefBrowser_dragSourceEndedAt(ccefbrowser_class * thiz, int x, int y, cefviewdragoperation_enum operation) {
  OnCefUi([=]() { thiz->dragSourceEndedAt(x, y, (cef_drag_operations_mask_t)operation); });
}

void CCefBrowser_dragSourceSystemDragEnded(ccefbrowser_class * thiz) {
  OnCefUi([=]() { thiz->dragSourceSystemDragEnded(); });
}
void CCefBrowser_sendTouchEvent(ccefbrowser_class * thiz, int touchId, float x, float y, float radiusX, float radiusY, float rotationAngle, float pressure, int touchEventType, uint32_t modifiers, int pointerType) {
  OnCefUi([=]() { thiz->sendTouchEvent(touchId, x, y, radiusX, radiusY, rotationAngle, pressure, touchEventType, modifiers, pointerType); });
}

void CCefBrowser_sendKeyEvent(ccefbrowser_class * thiz, cefviewkeyeventtype_enum type, uint32_t modifiers, int windowsKeyCode, int nativeKeyCode, bool isSysKey, uint16_t character, uint16_t umodifiedCharacter, bool isFocusOnEditableField) {
  OnCefUi([=]() { thiz->sendKeyEvent((cef_key_event_type_t)type, modifiers, windowsKeyCode, nativeKeyCode, isSysKey, character, umodifiedCharacter, isFocusOnEditableField); });
}

void CCefBrowser_notifyMoveOrResizeStarted(ccefbrowser_class * thiz) {
  OnCefUi([=]() { thiz->notifyMoveOrResizeStarted(); });
}

void CCefBrowser_notifyScreenChanged(ccefbrowser_class * thiz) {
  OnCefUi([=]() { thiz->notifyScreenChanged(); });
}

void CCefBrowser_imeSetComposition(ccefbrowser_class * thiz, const char * text, cefviewcompositionunderline_struct underlines[], int count, cefviewrange_struct replacement_range, cefviewrange_struct selection_range) {
  OnCefUi([=]() { thiz->imeSetComposition(text, underlines, count, replacement_range, selection_range); });
}

void CCefBrowser_imeCommitText(ccefbrowser_class * thiz, const char * text, cefviewrange_struct replacement_range, int relative_cursor_pos) {
  OnCefUi([=]() { thiz->imeCommitText(text, replacement_range, relative_cursor_pos); });
}

void CCefBrowser_imeFinishComposingText(ccefbrowser_class * thiz, bool keep_selection) {
  OnCefUi([=]() { thiz->imeFinishComposingText(keep_selection); });
}

void CCefBrowser_imeCancelComposition(ccefbrowser_class * thiz) {
  OnCefUi([=]() { thiz->imeCancelComposition(); });
}

// ---------------------------------------------------------------------------
// ABI 3 additions (declared in CefBrowser_c.h alongside the generated surface)
// ---------------------------------------------------------------------------

void CCefBrowser_copy(ccefbrowser_class * thiz) {
  OnCefUi([=]() { thiz->copy(); });
}

void CCefBrowser_cut(ccefbrowser_class * thiz) {
  OnCefUi([=]() { thiz->cut(); });
}

void CCefBrowser_paste(ccefbrowser_class * thiz) {
  OnCefUi([=]() { thiz->paste(); });
}

void CCefBrowser_selectAll(ccefbrowser_class * thiz) {
  OnCefUi([=]() { thiz->selectAll(); });
}

void CCefBrowser_undo(ccefbrowser_class * thiz) {
  OnCefUi([=]() { thiz->undo(); });
}

void CCefBrowser_redo(ccefbrowser_class * thiz) {
  OnCefUi([=]() { thiz->redo(); });
}

void CCefBrowser_delete(ccefbrowser_class * thiz) {
  OnCefUi([=]() { thiz->del(); });
}

void CCefBrowser_startFinding(ccefbrowser_class * thiz, const char * searchText, bool forward, bool matchCase) {
  OnCefUi([=]() { thiz->startFinding(searchText ? searchText : "", forward, matchCase); });
}

void CCefBrowser_stopFinding(ccefbrowser_class * thiz, bool clearSelection) {
  OnCefUi([=]() { thiz->stopFinding(clearSelection); });
}

bool CCefBrowser_continueFileDialog(ccefbrowser_class * thiz, int64_t requestId, int filterIndex, const char * const * filePaths, int filePathCount) {
  std::vector<std::string> paths;
  if (filePaths)
    for (int i = 0; i < filePathCount; ++i)
      if (filePaths[i]) paths.push_back(filePaths[i]);
  return OnCefUi([=]() { return thiz->continueFileDialog(requestId, filterIndex, paths); });
}

void CCefBrowser_cancelFileDialog(ccefbrowser_class * thiz, int64_t requestId) {
  OnCefUi([=]() { thiz->cancelFileDialog(requestId); });
}

bool CCefBrowser_continueDownload(ccefbrowser_class * thiz, int64_t downloadId, const char * downloadPath, bool showDialog) {
  const char * safePath = downloadPath ? downloadPath : "";
  return OnCefUi([=]() { return thiz->continueDownload(downloadId, safePath, showDialog); });
}

void CCefBrowser_cancelDownload(ccefbrowser_class * thiz, int64_t downloadId) {
  OnCefUi([=]() { thiz->cancelDownload(downloadId); });
}

void CCefBrowser_pauseDownload(ccefbrowser_class * thiz, int64_t downloadId) {
  OnCefUi([=]() { thiz->pauseDownload(downloadId); });
}

void CCefBrowser_resumeDownload(ccefbrowser_class * thiz, int64_t downloadId) {
  OnCefUi([=]() { thiz->resumeDownload(downloadId); });
}

bool CCefBrowser_continueContextMenu(ccefbrowser_class * thiz, int64_t requestId, int commandId, int eventFlags) {
  return OnCefUi([=]() { return thiz->continueContextMenu(requestId, commandId, eventFlags); });
}

void CCefBrowser_cancelContextMenu(ccefbrowser_class * thiz, int64_t requestId) {
  OnCefUi([=]() { thiz->cancelContextMenu(requestId); });
}

bool CCefBrowser_continuePermissionPrompt(ccefbrowser_class * thiz, uint64_t promptId, bool allow) {
  return OnCefUi([=]() { return thiz->continuePermissionPrompt(promptId, allow); });
}
