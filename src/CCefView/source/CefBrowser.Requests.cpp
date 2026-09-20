// ABI 3/4 request-answer channels: the reserve maps and their continuation
// exports. Split out of CefBrowser.cpp (1356 lines, triple threshold breach);
// the class declaration stays in CefBrowser.h, only the translation unit
// moved. Keep the destruction-order contract: the destructor calls the
// clear* helpers in the same order as CefBrowser.cpp documents.
#include "CefBrowser.h"

#include <include/cef_browser.h>
#include <include/cef_frame.h>
#include <include/cef_jsdialog_handler.h>
#include <mutex>

int64_t
CCefBrowser::reserveJSDialogRequestId()
{
  std::lock_guard<std::mutex> lock(jsDialogCallbacksMutex_);
  return nextJSDialogRequestId_++;
}

void
CCefBrowser::storeJSDialogCallback(int64_t requestId, CefRefPtr<CefJSDialogCallback> callback)
{
  if (requestId <= 0)
    return;

  std::lock_guard<std::mutex> lock(jsDialogCallbacksMutex_);
  if (callback)
    jsDialogCallbacks_[requestId] = callback;
  else
    jsDialogCallbacks_.erase(requestId);
}

bool
CCefBrowser::continueJSDialog(int64_t requestId, bool success, const std::string& userInput)
{
  CefRefPtr<CefJSDialogCallback> callback = nullptr;
  {
    std::lock_guard<std::mutex> lock(jsDialogCallbacksMutex_);
    const auto it = jsDialogCallbacks_.find(requestId);
    if (it == jsDialogCallbacks_.end())
      return false;

    callback = it->second;
    jsDialogCallbacks_.erase(it);
  }

  if (!callback)
    return false;

  callback->Continue(success, userInput);
  return true;
}

void
CCefBrowser::clearJSDialogCallbacks()
{
  std::lock_guard<std::mutex> lock(jsDialogCallbacksMutex_);
  jsDialogCallbacks_.clear();
}

// ---------------------------------------------------------------------------
// ABI 3 reserve maps
// ---------------------------------------------------------------------------

int64_t
CCefBrowser::reserveRequestId()
{
  std::lock_guard<std::mutex> lock(requestMapsMutex_);
  return nextRequestId_++;
}

void
CCefBrowser::storeFileDialogCallback(int64_t requestId, CefRefPtr<CefFileDialogCallback> callback)
{
  std::lock_guard<std::mutex> lock(requestMapsMutex_);
  if (callback)
    fileDialogCallbacks_[requestId] = callback;
  else
    fileDialogCallbacks_.erase(requestId);
}

bool
CCefBrowser::takeFileDialogCallback(int64_t requestId, CefRefPtr<CefFileDialogCallback>& callback)
{
  std::lock_guard<std::mutex> lock(requestMapsMutex_);
  auto it = fileDialogCallbacks_.find(requestId);
  if (it == fileDialogCallbacks_.end())
    return false;
  callback = it->second;
  fileDialogCallbacks_.erase(it);
  return true;
}

void
CCefBrowser::clearFileDialogCallbacks()
{
  std::lock_guard<std::mutex> lock(requestMapsMutex_);
  fileDialogCallbacks_.clear();
}

void
CCefBrowser::storeBeforeDownloadCallback(int64_t requestId, CefRefPtr<CefBeforeDownloadCallback> callback)
{
  std::lock_guard<std::mutex> lock(requestMapsMutex_);
  if (callback)
    beforeDownloadCallbacks_[requestId] = callback;
  else
    beforeDownloadCallbacks_.erase(requestId);
}

bool
CCefBrowser::takeBeforeDownloadCallback(int64_t requestId, CefRefPtr<CefBeforeDownloadCallback>& callback)
{
  std::lock_guard<std::mutex> lock(requestMapsMutex_);
  auto it = beforeDownloadCallbacks_.find(requestId);
  if (it == beforeDownloadCallbacks_.end())
    return false;
  callback = it->second;
  beforeDownloadCallbacks_.erase(it);
  return true;
}

void
CCefBrowser::clearBeforeDownloadCallbacks()
{
  std::lock_guard<std::mutex> lock(requestMapsMutex_);
  beforeDownloadCallbacks_.clear();
}

void
CCefBrowser::storeDownloadItemCallback(int64_t downloadId, CefRefPtr<CefDownloadItemCallback> callback)
{
  std::lock_guard<std::mutex> lock(requestMapsMutex_);
  if (callback)
    downloadItemCallbacks_[downloadId] = callback;
  else
    downloadItemCallbacks_.erase(downloadId);
}

bool
CCefBrowser::takeDownloadItemCallback(int64_t downloadId, CefRefPtr<CefDownloadItemCallback>& callback)
{
  std::lock_guard<std::mutex> lock(requestMapsMutex_);
  auto it = downloadItemCallbacks_.find(downloadId);
  if (it == downloadItemCallbacks_.end())
    return false;
  callback = it->second;
  downloadItemCallbacks_.erase(it);
  return true;
}

void
CCefBrowser::dropDownloadItemCallback(int64_t downloadId)
{
  std::lock_guard<std::mutex> lock(requestMapsMutex_);
  downloadItemCallbacks_.erase(downloadId);
}

void
CCefBrowser::clearDownloadItemCallbacks()
{
  std::lock_guard<std::mutex> lock(requestMapsMutex_);
  downloadItemCallbacks_.clear();
}

void
CCefBrowser::storeContextMenuCallback(int64_t requestId, CefRefPtr<CefRunContextMenuCallback> callback)
{
  std::lock_guard<std::mutex> lock(requestMapsMutex_);
  if (callback)
    contextMenuCallbacks_[requestId] = callback;
  else
    contextMenuCallbacks_.erase(requestId);
}

bool
CCefBrowser::takeContextMenuCallback(int64_t requestId, CefRefPtr<CefRunContextMenuCallback>& callback)
{
  std::lock_guard<std::mutex> lock(requestMapsMutex_);
  auto it = contextMenuCallbacks_.find(requestId);
  if (it == contextMenuCallbacks_.end())
    return false;
  callback = it->second;
  contextMenuCallbacks_.erase(it);
  return true;
}

void
CCefBrowser::clearContextMenuCallbacks()
{
  std::lock_guard<std::mutex> lock(requestMapsMutex_);
  contextMenuCallbacks_.clear();
}

void
CCefBrowser::storePermissionPromptCallback(uint64_t promptId, CefRefPtr<CefPermissionPromptCallback> callback)
{
  std::lock_guard<std::mutex> lock(requestMapsMutex_);
  if (callback)
    permissionPromptCallbacks_[promptId] = callback;
  else
    permissionPromptCallbacks_.erase(promptId);
}

bool
CCefBrowser::takePermissionPromptCallback(uint64_t promptId, CefRefPtr<CefPermissionPromptCallback>& callback)
{
  std::lock_guard<std::mutex> lock(requestMapsMutex_);
  auto it = permissionPromptCallbacks_.find(promptId);
  if (it == permissionPromptCallbacks_.end())
    return false;
  callback = it->second;
  permissionPromptCallbacks_.erase(it);
  return true;
}

void
CCefBrowser::clearPermissionPromptCallbacks()
{
  std::lock_guard<std::mutex> lock(requestMapsMutex_);
  permissionPromptCallbacks_.clear();
}

// ---------------------------------------------------------------------------
// ABI 3 editor commands / find
// ---------------------------------------------------------------------------

void
CCefBrowser::copy()
{
  if (pCefBrowser_)
    pCefBrowser_->GetFocusedFrame()->Copy();
}

void
CCefBrowser::cut()
{
  if (pCefBrowser_)
    pCefBrowser_->GetFocusedFrame()->Cut();
}

void
CCefBrowser::paste()
{
  if (pCefBrowser_)
    pCefBrowser_->GetFocusedFrame()->Paste();
}

void
CCefBrowser::selectAll()
{
  if (pCefBrowser_)
    pCefBrowser_->GetFocusedFrame()->SelectAll();
}

void
CCefBrowser::undo()
{
  if (pCefBrowser_)
    pCefBrowser_->GetFocusedFrame()->Undo();
}

void
CCefBrowser::redo()
{
  if (pCefBrowser_)
    pCefBrowser_->GetFocusedFrame()->Redo();
}

void
CCefBrowser::del()
{
  if (pCefBrowser_)
    pCefBrowser_->GetFocusedFrame()->Delete();
}

void
CCefBrowser::startFinding(const std::string& searchText, bool forward, bool matchCase)
{
  if (!pCefBrowser_)
    return;
  // CEF 127: Find(searchText, forward, matchCase, findNext); the request id is
  // tracked by CEF internally and surfaced via OnFindResult(identifier, ...).
  pCefBrowser_->GetHost()->Find(searchText, forward, matchCase, true);
}

void
CCefBrowser::stopFinding(bool clearSelection)
{
  if (pCefBrowser_)
    pCefBrowser_->GetHost()->StopFinding(clearSelection);
}

// ---------------------------------------------------------------------------
// ABI 3 dialog / download / context menu / permission answers
// ---------------------------------------------------------------------------

bool
CCefBrowser::continueFileDialog(int64_t requestId, int filterIndex, const std::vector<std::string>& filePaths)
{
  CefRefPtr<CefFileDialogCallback> callback;
  if (!takeFileDialogCallback(requestId, callback) || !callback)
    return false;
  (void)filterIndex; // CEF 127 dropped the selected-filter output from Continue(paths).
  std::vector<CefString> paths;
  for (auto& path : filePaths)
    paths.push_back(path);
  callback->Continue(paths);
  return true;
}

void
CCefBrowser::cancelFileDialog(int64_t requestId)
{
  CefRefPtr<CefFileDialogCallback> callback;
  if (takeFileDialogCallback(requestId, callback) && callback)
    callback->Cancel();
}

bool
CCefBrowser::continueDownload(int64_t downloadId, const std::string& downloadPath, bool showDialog)
{
  CefRefPtr<CefBeforeDownloadCallback> callback;
  if (!takeBeforeDownloadCallback(downloadId, callback) || !callback)
    return false;
  callback->Continue(downloadPath, showDialog);
  return true;
}

void
CCefBrowser::cancelDownload(int64_t downloadId)
{
  // Before the first Continue the download is cancelled by dropping the callback;
  // afterwards the item callback owns Cancel/Pause/Resume.
  CefRefPtr<CefBeforeDownloadCallback> before;
  if (takeBeforeDownloadCallback(downloadId, before)) {
    if (before)
      before->Continue("", false);
    return;
  }
  CefRefPtr<CefDownloadItemCallback> item;
  if (takeDownloadItemCallback(downloadId, item) && item)
    item->Cancel();
}

void
CCefBrowser::pauseDownload(int64_t downloadId)
{
  CefRefPtr<CefDownloadItemCallback> callback;
  if (takeDownloadItemCallback(downloadId, callback) && callback)
    callback->Pause();
}

void
CCefBrowser::resumeDownload(int64_t downloadId)
{
  CefRefPtr<CefDownloadItemCallback> callback;
  if (takeDownloadItemCallback(downloadId, callback) && callback)
    callback->Resume();
}

bool
CCefBrowser::continueContextMenu(int64_t requestId, int commandId, int eventFlags)
{
  CefRefPtr<CefRunContextMenuCallback> callback;
  if (!takeContextMenuCallback(requestId, callback) || !callback)
    return false;
  callback->Continue(commandId, static_cast<CefContextMenuHandler::EventFlags>(eventFlags));
  return true;
}

void
CCefBrowser::cancelContextMenu(int64_t requestId)
{
  CefRefPtr<CefRunContextMenuCallback> callback;
  if (takeContextMenuCallback(requestId, callback) && callback)
    callback->Cancel();
}

bool
CCefBrowser::continuePermissionPrompt(uint64_t promptId, bool allow)
{
#if CEF_VERSION_MAJOR >= 106
  CefRefPtr<CefPermissionPromptCallback> callback;
  if (!takePermissionPromptCallback(promptId, callback) || !callback)
    return false;
  callback->Continue(allow ? CEF_PERMISSION_RESULT_ACCEPT : CEF_PERMISSION_RESULT_DENY);
  return true;
#else
  (void)promptId;
  (void)allow;
  return false;
#endif
}
