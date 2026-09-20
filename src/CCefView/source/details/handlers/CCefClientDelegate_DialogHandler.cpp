#include "CCefClientDelegate.h"

#include <CefBrowser.h>

#include <nlohmann/json.hpp>

#include "details/utils/ValueConvertor.h"

bool
CCefClientDelegate::onFileDialog(CefRefPtr<CefBrowser>& browser,
                                 CefBrowserHost::FileDialogMode mode,
                                 const CefString& title,
                                 const CefString& default_file_path,
                                 const std::vector<CefString>& accept_filters,
#if CEF_VERSION_MAJOR < 102
                                 int selected_accept_filter,
#endif
                                 CefRefPtr<CefFileDialogCallback>& callback)
{
  if (!IsValidBrowser(browser) || !pCefView_->callbackTable_.pfnOnFileDialog) {
    // No bridge: complete explicitly so a page cannot wait forever.
    if (callback)
      callback->Cancel();
    return true;
  }

  const int64_t requestId = pCefView_->reserveRequestId();
  // A synchronous managed handler may answer before returning.
  pCefView_->storeFileDialogCallback(requestId, callback);
  nlohmann::json filters = nlohmann::json::array();
  for (const auto& filter : accept_filters)
    filters.push_back(filter.ToString());
  const bool handled = pCefView_->callbackTable_.pfnOnFileDialog(pCefView_, browser->GetIdentifier(),
                                                                 requestId,
                                                                 static_cast<int>(mode),
                                                                 title.ToString().c_str(),
                                                                 default_file_path.ToString().c_str(),
                                                                 filters.dump().c_str());
  if (!handled)
    pCefView_->storeFileDialogCallback(requestId, nullptr);
  return handled;
}
