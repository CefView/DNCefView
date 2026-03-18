#include "CCefClientDelegate.h"

#include <CefBrowser.h>

#include "details/utils/ValueConvertor.h"

bool
CCefClientDelegate::onJSDialog(CefRefPtr<CefBrowser>& browser,
                               const CefString& origin_url,
                               CefJSDialogHandler::JSDialogType dialog_type,
                               const CefString& message_text,
                               const CefString& default_prompt_text,
                               CefRefPtr<CefJSDialogCallback>& callback,
                               bool& suppress_message)
{
  return false;
}

bool
CCefClientDelegate::onBeforeUnloadDialog(CefRefPtr<CefBrowser>& browser,
                                         const CefString& message_text,
                                         bool is_reload,
                                         CefRefPtr<CefJSDialogCallback>& callback)
{
  return false;
}

void
CCefClientDelegate::onResetDialogState(CefRefPtr<CefBrowser>& browser)
{
}

void
CCefClientDelegate::onDialogClosed(CefRefPtr<CefBrowser>& browser)
{
}
