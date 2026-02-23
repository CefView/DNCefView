#include "CCefClientDelegate.h"

#include <CefBrowser.h>

void
CCefClientDelegate::takeFocus(CefRefPtr<CefBrowser>& browser, bool next)
{
  if (!IsValidBrowser(browser))
    return;

  if (pCefView_->callbackTable_.pfnFocusReleasedByTabKey) {
    pCefView_->callbackTable_.pfnFocusReleasedByTabKey(browser->GetIdentifier(), next);
    pCefView_->hasCefGotFocus = false;
  }
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

  if (pCefView_->callbackTable_.pfnGotFocus) {
    pCefView_->callbackTable_.pfnGotFocus(browser->GetIdentifier());
    pCefView_->hasCefGotFocus = true;
  }
}
