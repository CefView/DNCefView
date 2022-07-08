#include "CCefClientDelegate.h"

#include "../CefBrowser.h"
#include "../CefBrowser_Impl.h"

void
CCefClientDelegate::loadingStateChanged(CefRefPtr<CefBrowser>& browser,
                                        bool isLoading,
                                        bool canGoBack,
                                        bool canGoForward)
{
  if (!IsValidBrowser(browser))
    return;

  if (pCefView_->callbackTable_.pfnLoadingStateChanged)
    pCefView_->callbackTable_.pfnLoadingStateChanged(browser->GetIdentifier(), isLoading, canGoBack, canGoForward);
}

void
CCefClientDelegate::loadStart(CefRefPtr<CefBrowser>& browser, CefRefPtr<CefFrame>& frame, int transition_type)
{
  if (!IsValidBrowser(browser))
    return;

  if (pCefView_->callbackTable_.pfnLoadStart)
    pCefView_->callbackTable_.pfnLoadStart(
      browser->GetIdentifier(), frame->GetIdentifier(), frame->IsMain(), transition_type);
}

void
CCefClientDelegate::loadEnd(CefRefPtr<CefBrowser>& browser, CefRefPtr<CefFrame>& frame, int httpStatusCode)
{
  if (!IsValidBrowser(browser))
    return;

  if (pCefView_->callbackTable_.pfnLoadEnd)
    pCefView_->callbackTable_.pfnLoadEnd(
      browser->GetIdentifier(), frame->GetIdentifier(), frame->IsMain(), httpStatusCode);
}

void
CCefClientDelegate::loadError(CefRefPtr<CefBrowser>& browser,
                              CefRefPtr<CefFrame>& frame,
                              int errorCode,
                              const std::string& errorMsg,
                              const std::string& failedUrl,
                              bool& handled)
{
  if (!IsValidBrowser(browser))
    return;

  if (pCefView_->callbackTable_.pfnLoadError)
    handled = pCefView_->callbackTable_.pfnLoadError(browser->GetIdentifier(),
                                                     frame->GetIdentifier(),
                                                     errorCode,
                                                     frame->IsMain(),
                                                     errorMsg.c_str(),
                                                     failedUrl.c_str());

  return;
}
