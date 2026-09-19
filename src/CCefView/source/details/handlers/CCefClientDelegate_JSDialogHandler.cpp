#include "CCefClientDelegate.h"

#include <CefBrowser.h>


bool
CCefClientDelegate::onJSDialog(CefRefPtr<CefBrowser>& browser,
                               const CefString& origin_url,
                               CefJSDialogHandler::JSDialogType dialog_type,
                               const CefString& message_text,
                               const CefString& default_prompt_text,
                               CefRefPtr<CefJSDialogCallback>& callback,
                               bool& suppress_message)
{
  if (!IsValidBrowser(browser) || !pCefView_->callbackTable_.pfnOnJSDialog)
    return false;

  const int64_t requestId = pCefView_->reserveJSDialogRequestId();
  // A synchronous managed handler may continue the dialog before returning.
  pCefView_->storeJSDialogCallback(requestId, callback);
  const bool handled = pCefView_->callbackTable_.pfnOnJSDialog(browser->GetIdentifier(),
                                                               requestId,
                                                               origin_url.ToString().c_str(),
                                                               static_cast<int>(dialog_type),
                                                               message_text.ToString().c_str(),
                                                               default_prompt_text.ToString().c_str(),
                                                               suppress_message);

  if (!handled) {
    pCefView_->storeJSDialogCallback(requestId, nullptr);
  }

  return handled;
}

bool
CCefClientDelegate::onBeforeUnloadDialog(CefRefPtr<CefBrowser>& browser,
                                         const CefString& message_text,
                                         bool is_reload,
                                         CefRefPtr<CefJSDialogCallback>& callback)
{
  if (!IsValidBrowser(browser) || !pCefView_->callbackTable_.pfnOnBeforeUnloadDialog)
    return false;

  // Same reserve map and continueJSDialog answer channel as onJSDialog.
  const int64_t requestId = pCefView_->reserveJSDialogRequestId();
  pCefView_->storeJSDialogCallback(requestId, callback);
  const bool handled = pCefView_->callbackTable_.pfnOnBeforeUnloadDialog(
    browser->GetIdentifier(), requestId, message_text.ToString().c_str(), is_reload);
  if (!handled)
    pCefView_->storeJSDialogCallback(requestId, nullptr);
  return handled;
}

void
CCefClientDelegate::onResetDialogState(CefRefPtr<CefBrowser>& browser)
{
  if (!IsValidBrowser(browser))
    return;

  pCefView_->clearJSDialogCallbacks();
}

void
CCefClientDelegate::onDialogClosed(CefRefPtr<CefBrowser>& browser)
{
  if (!IsValidBrowser(browser))
    return;

  pCefView_->clearJSDialogCallbacks();
}
