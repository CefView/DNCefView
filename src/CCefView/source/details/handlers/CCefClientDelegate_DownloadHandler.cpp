#include "CCefClientDelegate.h"

#include <CefBrowser.h>

#include "details/utils/ValueConvertor.h"

void
CCefClientDelegate::onBeforeDownload(CefRefPtr<CefBrowser>& browser,
                                     CefRefPtr<CefDownloadItem>& download_item,
                                     const CefString& suggested_name,
                                     CefRefPtr<CefBeforeDownloadCallback>& callback)
{
  if (!IsValidBrowser(browser) || !pCefView_->callbackTable_.pfnOnBeforeDownload) {
    // No bridge: deny explicitly instead of leaving the request ambiguous.
    if (callback)
      callback->Continue(CefString(), false);
    return;
  }

  if (!download_item || !callback)
    return;

  // requestId doubles as the download id for the answer channel; the item
  // callback is tracked separately by download id once updates start.
  const int64_t downloadId = pCefView_->reserveRequestId();
  pCefView_->storeBeforeDownloadCallback(downloadId, callback);
  const bool handled = pCefView_->callbackTable_.pfnOnBeforeDownload(
    browser->GetIdentifier(),
    downloadId,
    download_item->GetURL().ToString().c_str(),
    suggested_name.ToString().c_str(),
    download_item->GetMimeType().ToString().c_str(),
    download_item->GetTotalBytes());
  if (!handled)
    pCefView_->storeBeforeDownloadCallback(downloadId, nullptr);
}

void
CCefClientDelegate::onDownloadUpdated(CefRefPtr<CefBrowser>& browser,
                                      CefRefPtr<CefDownloadItem>& download_item,
                                      CefRefPtr<CefDownloadItemCallback>& callback)
{
  if (!IsValidBrowser(browser) || !download_item)
    return;

  const int64_t downloadId = download_item->GetId();
  // Track the item callback for pause/resume/cancel while the download runs;
  // terminal states drop it so the map cannot outlive the download.
  const bool terminal = download_item->IsComplete() || download_item->IsCanceled() || download_item->IsInterrupted();
  if (!terminal && callback)
    pCefView_->storeDownloadItemCallback(downloadId, callback);
  else
    pCefView_->dropDownloadItemCallback(downloadId);

  if (!pCefView_->callbackTable_.pfnOnDownloadUpdated)
    return;

  // CefDownloadItem exposes state via Is* predicates; CEF 127 has no public
  // cef_download_state_t, so cross the ABI with this stable host-side mapping:
  // 0 idle, 1 in progress, 2 complete, 3 canceled, 4 interrupted.
  int state;
  if (download_item->IsComplete())
    state = 2;
  else if (download_item->IsCanceled())
    state = 3;
  else if (download_item->IsInterrupted())
    state = 4;
  else if (download_item->IsInProgress())
    state = 1;
  else
    state = 0;

  pCefView_->callbackTable_.pfnOnDownloadUpdated(
    browser->GetIdentifier(),
    downloadId,
    state,
    download_item->GetPercentComplete(),
    download_item->GetCurrentSpeed(),
    download_item->GetReceivedBytes(),
    download_item->GetTotalBytes());
}
