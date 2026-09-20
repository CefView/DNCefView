#include "CCefClientDelegate.h"

#include <CefBrowser.h>

#include <nlohmann/json.hpp>

#include "details/utils/CommonUtils.h"
#include "details/utils/MenuBuilder.h"

namespace {
nlohmann::json
SerializeMenuModel(CefRefPtr<CefMenuModel> model)
{
  nlohmann::json items = nlohmann::json::array();
  if (!model)
    return items;
  for (int i = 0; i < model->GetCount(); ++i) {
    nlohmann::json item;
    item["type"] = static_cast<int>(model->GetTypeAt(i));
    item["label"] = model->GetLabelAt(i).ToString();
    item["commandId"] = model->GetCommandIdAt(i);
    item["enable"] = model->IsEnabledAt(i);
    item["visible"] = model->IsVisibleAt(i);
    item["checked"] = model->IsCheckedAt(i);
    item["groupId"] = model->GetGroupIdAt(i);
    switch (model->GetTypeAt(i)) {
      case MENUITEMTYPE_SEPARATOR:
        break;
      case MENUITEMTYPE_SUBMENU:
        item["children"] = SerializeMenuModel(model->GetSubMenuAt(i));
        break;
      default:
        item["children"] = nlohmann::json::array();
        break;
    }
    items.push_back(item);
  }
  return items;
}
} // namespace

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
    if (pCefView_->disablePopuContextMenu_) {
      model->Clear();
    }

    return;
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

  if (!IsValidBrowser(browser) || !pCefView_->callbackTable_.pfnOnContextMenu) {
    if (callback)
      callback->Cancel();
    return true;
  }

  // The menu crosses the ABI as JSON; the command comes back as a CEF
  // built-in command id executed by CefRunContextMenuCallback::Continue.
  const int64_t requestId = pCefView_->reserveRequestId();
  pCefView_->storeContextMenuCallback(requestId, callback);
  nlohmann::json contextParams;
  contextParams["x"] = params->GetXCoord();
  contextParams["y"] = params->GetYCoord();
  contextParams["type"] = static_cast<int>(params->GetTypeFlags());
  const bool handled = pCefView_->callbackTable_.pfnOnContextMenu(pCefView_, 
    browser->GetIdentifier(), requestId, contextParams.dump().c_str(), SerializeMenuModel(model).dump().c_str());
  if (!handled)
    pCefView_->storeContextMenuCallback(requestId, nullptr);
  return handled;
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

  if (!IsValidBrowser(browser))
    return;
  // Drop any unanswered menu so a destroyed menu cannot fire later.
  pCefView_->clearContextMenuCallbacks();
  if (pCefView_->callbackTable_.pfnOnContextMenuDismissed)
    pCefView_->callbackTable_.pfnOnContextMenuDismissed(pCefView_, browser->GetIdentifier());
}
