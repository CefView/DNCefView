#include "CCefClientDelegate.h"

#include <CefBrowser.h>

#include "details/utils/ValueConvertor.h"

void
CCefClientDelegate::onBeforeDownload(CefRefPtr<CefBrowser>& browser,
                                     CefRefPtr<CefDownloadItem>& download_item,
                                     const CefString& suggested_name,
                                     CefRefPtr<CefBeforeDownloadCallback>& callback)
{
  // Downloads are denied until the Unity path/permission bridge is installed.
  // Always complete the CEF callback; an empty handler leaves the request in
  // an ambiguous state and can retain browser resources indefinitely.
  if (callback)
    callback->Continue(CefString(), false);
}

void
CCefClientDelegate::onDownloadUpdated(CefRefPtr<CefBrowser>& browser,
                                      CefRefPtr<CefDownloadItem>& download_item,
                                      CefRefPtr<CefDownloadItemCallback>& callback)
{
  if (callback && download_item && !download_item->IsComplete() && !download_item->IsCanceled())
    callback->Cancel();
}
