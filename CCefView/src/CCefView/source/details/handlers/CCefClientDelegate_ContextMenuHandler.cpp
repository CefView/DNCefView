#include "CCefClientDelegate.h"

#include <CefBrowser.h>

#include "details/utils/CommonUtils.h"
#include "details/utils/MenuBuilder.h"

void
CCefClientDelegate::onBeforeContextMenu(CefRefPtr<CefBrowser>& browser,
                                        CefRefPtr<CefFrame>& frame,
                                        CefRefPtr<CefContextMenuParams>& params,
                                        CefRefPtr<CefMenuModel>& model)
{
  FLog();

  if (!pCefView_)
    return;

  // popup browser doesn't involve off-screen rendering
  if (browser->IsPopup()) {
    return;
  }

  if (pCefView_->callbackTable_.pfnOnBeforeContextMenu) {
    auto menuData = MenuBuilder::CreateMenuDataFromCefMenu(model.get());
    auto allow = pCefView_->callbackTable_.pfnOnBeforeContextMenu(menuData.c_str());
    if (!allow) {
      model->Clear();
    }
  }
}

bool
CCefClientDelegate::onRunContextMenu(CefRefPtr<CefBrowser>& browser,
                                     CefRefPtr<CefFrame>& frame,
                                     CefRefPtr<CefContextMenuParams>& params,
                                     CefRefPtr<CefMenuModel>& model,
                                     CefRefPtr<CefRunContextMenuCallback>& callback)
{
  FLog();

  if (browser->IsPopup()) {
    return false;
  }

  pCefView_->contextMenuCallback_ = callback;

  if (pCefView_->callbackTable_.pfnOnRunCefContextMenu) {
    pCefView_->callbackTable_.pfnOnRunCefContextMenu(params->GetXCoord(), params->GetYCoord());
  }

  return true;
}

bool
CCefClientDelegate::onContextMenuCommand(CefRefPtr<CefBrowser>& browser,
                                         CefRefPtr<CefFrame>& frame,
                                         CefRefPtr<CefContextMenuParams>& params,
                                         int command_id,
                                         CefContextMenuHandler::EventFlags event_flags)
{
  FLog();

  return false;
}

void
CCefClientDelegate::onContextMenuDismissed(CefRefPtr<CefBrowser>& browser, CefRefPtr<CefFrame>& frame)
{
  FLog();

  if (pCefView_->callbackTable_.pfnOnCefContextMenuDismissed) {
    pCefView_->callbackTable_.pfnOnCefContextMenuDismissed();
  }

  pCefView_->contextMenuCallback_ = nullptr;
}
