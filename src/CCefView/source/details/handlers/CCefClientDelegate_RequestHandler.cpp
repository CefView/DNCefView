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
  // Renderer crash observation point for the host (RUN-01); forwarding to the
  // callback table lands with the ABI 3 batch.
  (void)browser;
  (void)status;
#if CEF_VERSION_MAJOR >= 124
  (void)errorCode;
  (void)errorString;
#endif
}
