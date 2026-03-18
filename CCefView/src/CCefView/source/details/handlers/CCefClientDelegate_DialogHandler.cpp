#include "CCefClientDelegate.h"

#include <CefBrowser.h>

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
  bool handled = false;
  if (!IsValidBrowser(browser))
    return handled;

  return handled;
}
