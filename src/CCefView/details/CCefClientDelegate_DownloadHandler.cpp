#include "CCefClientDelegate.h"

#include "utils/ValueConvertor.h"

#include "../CefBrowser.h"
#include "../CefBrowser_Impl.h"

void
CCefClientDelegate::onBeforeDownload(CefRefPtr<CefBrowser> browser,
                                     CefRefPtr<CefDownloadItem> download_item,
                                     const CefString& suggested_name,
                                     CefRefPtr<CefBeforeDownloadCallback> callback)
{}

void
CCefClientDelegate::onDownloadUpdated(CefRefPtr<CefBrowser> browser,
                                      CefRefPtr<CefDownloadItem> download_item,
                                      CefRefPtr<CefDownloadItemCallback> callback)
{}
