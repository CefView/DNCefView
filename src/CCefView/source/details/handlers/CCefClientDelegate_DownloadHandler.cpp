#include "CCefClientDelegate.h"

#include <CefBrowser.h>

#include "details/utils/ValueConvertor.h"

void
CCefClientDelegate::onBeforeDownload(CefRefPtr<CefBrowser>& browser,
                                     CefRefPtr<CefDownloadItem>& download_item,
                                     const CefString& suggested_name,
                                     CefRefPtr<CefBeforeDownloadCallback>& callback)
{
}

void
CCefClientDelegate::onDownloadUpdated(CefRefPtr<CefBrowser>& browser,
                                      CefRefPtr<CefDownloadItem>& download_item,
                                      CefRefPtr<CefDownloadItemCallback>& callback)
{
}
