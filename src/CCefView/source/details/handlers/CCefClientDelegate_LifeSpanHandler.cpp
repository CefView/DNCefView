#include "CCefClientDelegate.h"

#include <CefBrowser.h>

bool
CCefClientDelegate::onBeforePopup(CefRefPtr<CefBrowser>& browser,
                                  CefRefPtr<CefFrame>& frame,
                                  const CefString& targetUrl,
                                  const CefString& targetFrameName,
                                  CefLifeSpanHandler::WindowOpenDisposition targetDisposition,
                                  CefWindowInfo& windowInfo,
                                  CefBrowserSettings& settings,
                                  bool& DisableJavascriptAccess)
{
  bool cancel = true;

#if CEF_VERSION_MAJOR < 119
  auto CefNewPopupValue = CefLifeSpanHandler::WindowOpenDisposition::WOD_NEW_POPUP;
#else
  auto CefNewPopupValue = CefLifeSpanHandler::WindowOpenDisposition::CEF_WOD_NEW_POPUP;
#endif

  if (targetDisposition == CefNewPopupValue) {
    if (pCefView_->callbackTable_.pfnOnBeforeNewPopupCreate) {
      auto i = frame->GetIdentifier().ToString();
      auto u = targetUrl.ToString();
      auto n = targetFrameName.ToString();
      auto d = (CefViewWindowOpenDisposition)targetDisposition;
      auto r = windowInfo.bounds;
      auto j = DisableJavascriptAccess;
      CCefSetting s;
      CCefSetting::CopyFromCefBrowserSettings(settings, &s);

      cancel = pCefView_->callbackTable_.pfnOnBeforeNewPopupCreate(i.c_str(), u.c_str(), n.c_str(), d, &r, &s, &j);

      if (!cancel) {
        windowInfo.bounds = r;
        CCefSetting::CopyToCefBrowserSettings(&s, settings);
        DisableJavascriptAccess = j;
      }
    }
  } else {
    if (pCefView_->callbackTable_.pfnOnBeforeNewBrowserCreate) {
      auto i = frame->GetIdentifier().ToString();
      auto u = targetUrl.ToString();
      auto n = targetFrameName.ToString();
      auto d = (CefViewWindowOpenDisposition)targetDisposition;
      auto r = windowInfo.bounds;
      CCefSetting s;
      CCefSetting::CopyFromCefBrowserSettings(settings, &s);

      pCefView_->callbackTable_.pfnOnBeforeNewBrowserCreate(i.c_str(), u.c_str(), n.c_str(), d, r, &s);
      cancel = true;
    }
  }

  return cancel;
}

void
CCefClientDelegate::onAfterCreate(CefRefPtr<CefBrowser>& browser)
{
  if (!pCefView_)
    return;

  if (browser->IsPopup()) {
    // pop-up browser

  } else {
    // main browser
    pCefView_->pCefBrowser_ = browser;

    if (pCefView_->callbackTable_.pfnOnAfterCreated)
      pCefView_->callbackTable_.pfnOnAfterCreated();
  }
}

bool
CCefClientDelegate::doClose(CefRefPtr<CefBrowser>& browser)
{
  if (!pCefView_)
    return false;

  bool rt = false;
  if (pCefView_->callbackTable_.pfnDoClose) {
    rt = pCefView_->callbackTable_.pfnDoClose();
  }
  return rt;
}

bool
CCefClientDelegate::requestClose(CefRefPtr<CefBrowser>& browser)
{
  if (!pCefView_)
    return false;

  bool rt = true;
  if (pCefView_->callbackTable_.pfnRequestClose) {
    rt = pCefView_->callbackTable_.pfnRequestClose();
  }
  return rt;
}

void
CCefClientDelegate::onBeforeClose(CefRefPtr<CefBrowser>& browser)
{
  if (!pCefView_)
    return;

  if (pCefView_->callbackTable_.pfnOnBeforeClose) {
    pCefView_->callbackTable_.pfnOnBeforeClose();
  }
}
