#include "CCefClientDelegate.h"

#include <CefBrowser.h>

bool
CCefClientDelegate::onBeforeBrowse(CefRefPtr<CefBrowser>& browser,
                                   CefRefPtr<CefFrame>& frame,
                                   CefRefPtr<CefRequest>& request,
                                   bool user_gesture,
                                   bool is_redirect)
{
  // In-page navigation policy is owned by the host; false keeps the CEF default.
  return false;
}

void
CCefClientDelegate::onRenderProcessTerminated(CefRefPtr<CefBrowser>& browser,
                                              CefRequestHandler::TerminationStatus status
#if CEF_VERSION_MAJOR >= 124
                                              ,
                                              int errorCode,
                                              const CefString& errorString
#endif
)
{
  if (!IsValidBrowser(browser) || !pCefView_->callbackTable_.pfnOnRenderProcessTerminated)
    return;
  pCefView_->callbackTable_.pfnOnRenderProcessTerminated(pCefView_, 
    browser->GetIdentifier(), static_cast<int>(status)
#if CEF_VERSION_MAJOR >= 124
                                ,
                                errorCode,
                                errorString.ToString().c_str()
#else
                                ,
                                0,
                                ""
#endif
  );
}

void
CCefClientDelegate::onFindResult(CefRefPtr<CefBrowser>& browser,
                                 int identifier,
                                 int count,
                                 const CefRect& selectionRect,
                                 int activeMatchOrdinal,
                                 bool finalUpdate)
{
  if (!IsValidBrowser(browser) || !pCefView_->callbackTable_.pfnOnFindResult)
    return;
  const CefViewRect rect{ selectionRect.x, selectionRect.y, selectionRect.width, selectionRect.height };
  pCefView_->callbackTable_.pfnOnFindResult(pCefView_, 
    browser->GetIdentifier(), identifier, count, activeMatchOrdinal, finalUpdate, rect);
}

#if CEF_VERSION_MAJOR >= 106
bool
CCefClientDelegate::onShowPermissionPrompt(CefRefPtr<CefBrowser>& browser,
                                           uint64_t prompt_id,
                                           const CefString& requesting_origin,
                                           uint32_t requested_permissions,
                                           CefRefPtr<CefPermissionPromptCallback>& callback)
{
  if (!IsValidBrowser(browser) || !pCefView_->callbackTable_.pfnOnPermissionPrompt)
    return false; // default handling keeps the CEF dismissal behavior
  const uint64_t promptId = prompt_id;
  pCefView_->storePermissionPromptCallback(promptId, callback);
  const bool handled = pCefView_->callbackTable_.pfnOnPermissionPrompt(pCefView_, 
    browser->GetIdentifier(), promptId, requesting_origin.ToString().c_str(), requested_permissions);
  if (!handled)
    pCefView_->storePermissionPromptCallback(promptId, nullptr);
  return handled;
}
#endif
