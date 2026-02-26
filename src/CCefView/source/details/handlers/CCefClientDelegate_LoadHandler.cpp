#include "CCefClientDelegate.h"

#include <CefBrowser.h>

#include "details/utils/CommonUtils.h"

void
CCefClientDelegate::loadingStateChanged(CefRefPtr<CefBrowser>& browser,
                                        bool isLoading,
                                        bool canGoBack,
                                        bool canGoForward)
{
  if (!IsValidBrowser(browser))
    return;

  if (!isLoading) {
    // loading complete
    if (auto focusedFrame = browser->GetFocusedFrame()) {
      browser->GetHost()->SetFocus(true);
    }
  }

  if (pCefView_->callbackTable_.pfnLoadingStateChanged) {
    pCefView_->callbackTable_.pfnLoadingStateChanged(browser->GetIdentifier(), isLoading, canGoBack, canGoForward);
  }
}

void
CCefClientDelegate::loadStart(CefRefPtr<CefBrowser>& browser, CefRefPtr<CefFrame>& frame, int transition_type)
{
  if (!IsValidBrowser(browser))
    return;

  if (pCefView_->callbackTable_.pfnLoadStart) {
    auto frameId = frame->GetIdentifier();
    pCefView_->callbackTable_.pfnLoadStart(
      browser->GetIdentifier(), FrameIdC2X(frameId).c_str(), frame->IsMain(), transition_type);
  }
}

void
CCefClientDelegate::loadEnd(CefRefPtr<CefBrowser>& browser, CefRefPtr<CefFrame>& frame, int httpStatusCode)
{
  if (!IsValidBrowser(browser))
    return;

  if (pCefView_->callbackTable_.pfnLoadEnd) {
    auto frameId = frame->GetIdentifier();
    pCefView_->callbackTable_.pfnLoadEnd(
      browser->GetIdentifier(), FrameIdC2X(frameId).c_str(), frame->IsMain(), httpStatusCode);
  }
}

void
CCefClientDelegate::loadError(CefRefPtr<CefBrowser>& browser,
                              CefRefPtr<CefFrame>& frame,
                              int errorCode,
                              const CefString& errorMsg,
                              const CefString& failedUrl,
                              bool& handled)
{
  if (!IsValidBrowser(browser))
    return;

  if (pCefView_->callbackTable_.pfnLoadError) {
    auto frameId = frame->GetIdentifier();
    handled = pCefView_->callbackTable_.pfnLoadError(browser->GetIdentifier(),
                                                     FrameIdC2X(frameId).c_str(),
                                                     errorCode,
                                                     frame->IsMain(),
                                                     errorMsg.ToString().c_str(),
                                                     failedUrl.ToString().c_str());
  }

  return;
}
