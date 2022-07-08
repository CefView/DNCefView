#include "CCefClientDelegate.h"

#include "../CefBrowser_Impl.h"

void
CCefClientDelegate::takeFocus(CefRefPtr<CefBrowser>& browser, bool next)
{
  if (!IsValidBrowser(browser))
    return;

  if (pCefView_->callbackTable_.pfnFocusReleasedByTabKey)
    pCefView_->callbackTable_.pfnFocusReleasedByTabKey(browser->GetIdentifier(), next);
}

bool
CCefClientDelegate::setFocus(CefRefPtr<CefBrowser>& browser)
{
  // allow the focus setting action
  return false;
}

void
CCefClientDelegate::gotFocus(CefRefPtr<CefBrowser>& browser)
{
  if (!IsValidBrowser(browser))
    return;

  if (pCefView_->callbackTable_.pfnGotFocus)
    pCefView_->callbackTable_.pfnGotFocus(browser->GetIdentifier());
}
