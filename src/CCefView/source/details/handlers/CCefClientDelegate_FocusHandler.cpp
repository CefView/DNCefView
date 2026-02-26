#include "CCefClientDelegate.h"

#include <CefBrowser.h>

void
CCefClientDelegate::takeFocus(CefRefPtr<CefBrowser>& browser, bool next)
{
  if (!IsValidBrowser(browser))
    return;

  if (pCefView_->callbackTable_.pfnOnFocusReleasedByTabKey) {
    pCefView_->callbackTable_.pfnOnFocusReleasedByTabKey(browser->GetIdentifier(), next);
  }
}

bool
CCefClientDelegate::setFocus(CefRefPtr<CefBrowser>& browser)
{
  // allow the focus setting action
  if (pCefView_->callbackTable_.pfnOnRequestSetFocus) {
    return pCefView_->callbackTable_.pfnOnRequestSetFocus(browser->GetIdentifier());
  }

  return false;
}

void
CCefClientDelegate::gotFocus(CefRefPtr<CefBrowser>& browser)
{
  if (!IsValidBrowser(browser))
    return;

  if (pCefView_->callbackTable_.pfnOnGotFocus) {
    pCefView_->callbackTable_.pfnOnGotFocus(browser->GetIdentifier());
  }
}
