#include "CefBrowser.h"

#pragma region cef_headers
#include <include/cef_browser.h>
#include <include/cef_drag_data.h>
#include <include/cef_frame.h>
#include <include/cef_jsdialog_handler.h>
#include <include/cef_parser.h>
#pragma endregion cef_headers

#include <algorithm>
#include <ranges>
#include <sstream>
#include <vector>

#include <CefViewBrowserClient.h>
#include <CefViewCoreProtocol.h>

#include "CefContext.h"
#include "CefVersion.h"
#include "details/handlers/CCefClientDelegate.h"
#include "details/utils/CommonUtils.h"
#include "details/utils/ValueConvertor.h"

const std::string CCefBrowser::MainFrameID = "0";
const std::string CCefBrowser::AllFrameID = "-1";

namespace {
inline CefRefPtr<CefBrowserHost>
GetBrowserHost(const CefRefPtr<CefBrowser>& browser)
{
  if (!browser)
    return nullptr;

  auto host = browser->GetHost();
  if (!host || !host->IsWindowRenderingDisabled())
    return nullptr;

  return host;
}

inline CefMouseEvent
BuildMouseEvent(int x, int y, uint32_t modifiers)
{
  CefMouseEvent event;
  event.x = x;
  event.y = y;
  event.modifiers = modifiers;
  return event;
}

std::vector<std::string>
SplitFilePaths(const std::string& filePaths)
{
  std::vector<std::string> result;
  auto paths =
    filePaths                                                                                                  //
    | std::views::split('\n')                                                                                  //
    | std::views::transform([](auto&& rng) { return std::string(&*rng.begin(), std::ranges::distance(rng)); }) //
    | std::views::filter([](const std::string& s) { return !s.empty(); });
  std::ranges::copy(paths, std::back_inserter(result));

  return result;
}

std::string
ExtractFileName(const std::string& path)
{
  auto splitPos = path.find_last_of("\\/");
  if (splitPos == std::string::npos)
    return path;

  return path.substr(splitPos + 1);
}
} // namespace

CCefBrowser::CCefBrowser(CefBrowserCallback callback, const std::string& url, const CCefSetting* setting)
  : callbackTable_(callback)
{
  auto pContext = CCefContext::instance();

  // create browser client handler delegate
  auto pClientDelegate = std::make_shared<CCefClientDelegate>(this);

  // create browser client handler
  CefRefPtr<CefViewBrowserClient> pClient = new CefViewBrowserClient(pContext->pApp_, pClientDelegate);

  // create the browser settings
  CefBrowserSettings browserSettings;
  CCefSetting::CopyToCefBrowserSettings(setting, browserSettings);

  CefWindowInfo window_info;
  window_info.SetAsWindowless(nullptr);
  window_info.shared_texture_enabled = false;
  window_info.external_begin_frame_enabled = false;

  if (CefColorGetA(browserSettings.background_color) == 0)
    transparentPaintingEnabled_ = true;

  // create browser object
  bool success = CefBrowserHost::CreateBrowser(window_info,     // window info
                                               pClient,         // handler
                                               url,             // url
                                               browserSettings, // settings
                                               nullptr,
                                               CefRequestContext::GetGlobalContext());
  if (!success) {
    return;
  }

  pClient_ = pClient;
  pClientDelegate_ = pClientDelegate;
}

CCefBrowser::~CCefBrowser()
{
  resetSourceDragState();
  clearJSDialogCallbacks();

  // clean all browsers
  if (pClient_) {
    pClient_->CloseAllBrowsers();
  }

  pClient_ = nullptr;
  pCefBrowser_ = nullptr;
}

void
CCefBrowser::addLocalFolderResource(const std::string& path, const std::string& url, int priority /*= 0*/)
{
  if (pClient_) {
    pClient_->AddLocalDirectoryResourceProvider(path, url, priority);
  }
}

void
CCefBrowser::addArchiveResource(const std::string& path,
                                const std::string& url,
                                const std::string& password /*= ""*/,
                                int priority /*= 0*/)
{
  if (pClient_) {
    pClient_->AddArchiveResourceProvider(path, url, password, priority);
  }
}

int
CCefBrowser::browserId()
{
  if (pCefBrowser_)
    return pCefBrowser_->GetIdentifier();

  return -1;
}

void
CCefBrowser::navigateToString(const std::string& content)
{
  if (pCefBrowser_) {
    auto data = CefURIEncode(CefBase64Encode(content.c_str(), content.size()), false).ToString();
    data = "data:text/html;base64," + data;
    pCefBrowser_->GetMainFrame()->LoadURL(data);
  }
}

void
CCefBrowser::navigateToUrl(const std::string& url)
{
  if (pCefBrowser_) {
    CefString strUrl;
    strUrl.FromString(url);
    pCefBrowser_->GetMainFrame()->LoadURL(strUrl);
  }
}

bool
CCefBrowser::canGoBack()
{
  if (pCefBrowser_)
    return pCefBrowser_->CanGoBack();

  return false;
}

bool
CCefBrowser::canGoForward()
{
  if (pCefBrowser_)
    return pCefBrowser_->CanGoForward();

  return false;
}

void
CCefBrowser::goBack()
{
  if (pCefBrowser_)
    pCefBrowser_->GoBack();
}

void
CCefBrowser::goForward()
{
  if (pCefBrowser_)
    pCefBrowser_->GoForward();
}

bool
CCefBrowser::isLoading()
{
  if (pCefBrowser_)
    return pCefBrowser_->IsLoading();

  return false;
}

void
CCefBrowser::reload()
{
  if (pCefBrowser_)
    pCefBrowser_->Reload();
}

void
CCefBrowser::stopLoad()
{
  if (pCefBrowser_)
    pCefBrowser_->StopLoad();
}

bool
CCefBrowser::triggerEventOnMainFrame(const std::string& evtName, const std::string& evtArgs)
{
  return triggerEvent(evtName, evtArgs, CCefBrowser::MainFrameID);
}

bool
CCefBrowser::triggerEventOnFrame(const std::string& evtName, const std::string& evtArgs, const std::string& frameId)
{
  return triggerEvent(evtName, evtArgs, frameId);
}

bool
CCefBrowser::broadcastEvent(const std::string& evtName, const std::string& evtArgs)
{
  return triggerEvent(evtName, evtArgs, AllFrameID);
}

bool
CCefBrowser::triggerEvent(const std::string& name,
                          const std::string& args,
                          const std::string& frameId /*= CCefView::MainFrameID*/)
{
  if (!name.empty()) {
    return sendEventNotifyMessage(frameId, name, args);
  }

  return false;
}

bool
CCefBrowser::replyCefQuery(const CCefQuery* query)
{
  if (pClient_) {
    CefString res;
    res.FromString(query->getResponse());
    return pClient_->ResponseQuery(query->getId(), query->getResult(), res, query->getError());
  }
  return false;
}

bool
CCefBrowser::executeJavascript(const std::string& frameId, const std::string& code, const std::string& url)
{
  if (code.empty())
    return false;

  if (pCefBrowser_) {
    // get frame instance
    auto frame = (frameId == CCefBrowser::MainFrameID) ? //
                   pCefBrowser_->GetMainFrame()          // get main frame
                                                       : //
#if CEF_VERSION_MAJOR < 122                              // get frame by id
                   pCefBrowser_->GetFrame(frameId);
#else
                   pCefBrowser_->GetFrameByIdentifier(frameId);
#endif

    if (frame) {
      CefString c;
      c.FromString(code);

      CefString u;
      if (url.empty()) {
        u = frame->GetURL();
      } else {
        u.FromString(url);
      }

      frame->ExecuteJavaScript(c, u, 0);

      return true;
    }
  }

  return false;
}

bool
CCefBrowser::executeJavascriptWithResult(const std::string& frameId,
                                         const std::string& code,
                                         const std::string& url,
                                         const std::string& context)
{
  if (code.empty())
    return false;

  if (pClient_ && pCefBrowser_) {
    // get frame instance
    auto frame = (frameId == CCefBrowser::MainFrameID) ? //
                   pCefBrowser_->GetMainFrame()          // get main frame
                                                       : //
#if CEF_VERSION_MAJOR < 122                              // get frame by id
                   pCefBrowser_->GetFrame(frameId);
#else
                   pCefBrowser_->GetFrameByIdentifier(frameId);
#endif

    if (!frame)
      return false;

    CefString c;
    c.FromString(code);

    CefString u;
    if (url.empty()) {
      u = frame->GetURL();
    } else {
      u.FromString(url);
    }

    return pClient_->AsyncExecuteJSCode(pCefBrowser_, frame, c, u, context);
  }

  return false;
}

bool
CCefBrowser::setPreference(const std::string& name, const std::string& value /*, std::string& error*/)
{
  if (pCefBrowser_) {
    CefRefPtr<CefBrowserHost> host = pCefBrowser_->GetHost();
    if (host) {
      CefString n;
      n.FromString(name);

      auto v = CefValue::Create();
      if (!ValueConvertor::JsonStringToCefValue(v.get(), value)) {
        // TODO:

        return false;
      }

      CefString e;
      auto r = host->GetRequestContext()->SetPreference(n, v, e);
      // error = e.ToString();
      return r;
    }
  }

  return false;
}

void
CCefBrowser::setDisablePopupContextMenu(bool disable)
{
  disablePopuContextMenu_ = disable;
}

bool
CCefBrowser::isPopupContextMenuDisabled()
{
  return disablePopuContextMenu_;
}

void
CCefBrowser::executeContextMenuCommand(int id)
{
  if (contextMenuCallback_) {
    contextMenuCallback_->Continue(id, EVENTFLAG_NONE);
  }
}

void
CCefBrowser::cancelContextMenu()
{
  if (contextMenuCallback_) {
    contextMenuCallback_->Cancel();
  }
}

void
CCefBrowser::setWindowlessFrameRate(int rate)
{
  if (!pCefBrowser_)
    return;

  pCefBrowser_->GetHost()->SetWindowlessFrameRate(rate);
}

void
CCefBrowser::sendExternalBeginFrame()
{
  if (!pCefBrowser_)
    return;

  auto host = pCefBrowser_->GetHost();
  if (!host || !host->IsWindowRenderingDisabled())
    return;

  host->SendExternalBeginFrame();
}

void
CCefBrowser::showDevTools()
{
  if (!pCefBrowser_)
    return;

  auto host = pCefBrowser_->GetHost();
  if (!host || host->HasDevTools())
    return;

  CefWindowInfo windowInfo;
#if defined(OS_WIN)
  windowInfo.SetAsPopup(nullptr, "CefView DevTools");
#else
  CefString(&windowInfo.window_name) = "CefView DevTools";
  windowInfo.SetAsChild(nullptr, { 100, 100, 800, 600 });
#endif

  CefBrowserSettings browserSettings;
  host->ShowDevTools(windowInfo, nullptr, browserSettings, CefPoint());
}

void
CCefBrowser::closeDevTools()
{
  if (!pCefBrowser_)
    return;

  auto host = pCefBrowser_->GetHost();
  if (!host || !host->HasDevTools())
    return;

  host->CloseDevTools();
}

bool
CCefBrowser::hasDevTools()
{
  if (!pCefBrowser_)
    return false;

  auto host = pCefBrowser_->GetHost();
  if (!host)
    return false;

  return host->HasDevTools();
}

void
CCefBrowser::closeBrowser(bool forceClose)
{
  if (!pCefBrowser_)
    return;

  auto host = pCefBrowser_->GetHost();
  if (!host)
    return;

  host->CloseBrowser(forceClose);
}

void
CCefBrowser::setFocus(bool focused)
{
  if (!pCefBrowser_)
    return;

  pCefBrowser_->GetHost()->SetFocus(focused);
}

void
CCefBrowser::wasResized()
{
  if (!pCefBrowser_)
    return;

  pCefBrowser_->GetHost()->WasResized();
}

void
CCefBrowser::wasHidden(bool hidden)
{
  if (!pCefBrowser_)
    return;

  auto host = pCefBrowser_->GetHost();
  if (!host)
    return;

  host->WasHidden(hidden);
  if (hidden && sourceDragActive_)
    endSourceDrag(0, 0, 0, true);
}

void
CCefBrowser::sendMouseMoveEvent(int x, int y, uint32_t modifiers, bool leave)
{
  if (!pCefBrowser_)
    return;

  auto host = pCefBrowser_->GetHost();
  if (!host)
    return;

  CefMouseEvent event = BuildMouseEvent(x, y, modifiers);
  host->SendMouseMoveEvent(event, leave);

  if (sourceDragActive_) {
    if (leave) {
      endSourceDrag(x, y, modifiers, true);
    } else {
      updateSourceDragTarget(x, y, modifiers);
    }
  }
}

void
CCefBrowser::sendMouseClickEvent(int x,
                                 int y,
                                 uint32_t modifiers,
                                 CefViewMouseButtonType type,
                                 bool mouseUp,
                                 int clickCount)
{
  if (!pCefBrowser_)
    return;

  auto host = pCefBrowser_->GetHost();
  if (!host)
    return;

  CefMouseEvent event = BuildMouseEvent(x, y, modifiers);
  host->SendMouseClickEvent(event, (CefBrowserHost::MouseButtonType)type, mouseUp, clickCount);

  if (sourceDragActive_ && type == MBT_LEFT && mouseUp) {
    endSourceDrag(x, y, modifiers, false);
  }
}

void
CCefBrowser::sendWheelEvent(int x, int y, uint32_t modifiers, int deltaX, int deltaY)
{
  if (!pCefBrowser_)
    return;

  auto host = pCefBrowser_->GetHost();
  if (!host)
    return;

  CefMouseEvent event = BuildMouseEvent(x, y, modifiers);
  host->SendMouseWheelEvent(event, deltaX, deltaY);
}

void
CCefBrowser::dragTargetDragEnterText(int x,
                                     int y,
                                     uint32_t modifiers,
                                     const std::string& text,
                                     const std::string& html,
                                     const std::string& baseUrl,
                                     CefViewDragOperation allowedOps)
{
  auto host = GetBrowserHost(pCefBrowser_);
  if (!host)
    return;

  auto dragData = CefDragData::Create();
  if (!text.empty())
    dragData->SetFragmentText(text);
  if (!html.empty())
    dragData->SetFragmentHtml(html);
  if (!baseUrl.empty())
    dragData->SetFragmentBaseURL(baseUrl);

  host->DragTargetDragEnter(
    dragData, BuildMouseEvent(x, y, modifiers), static_cast<CefBrowserHost::DragOperationsMask>(allowedOps));
}

void
CCefBrowser::dragTargetDragEnterFiles(int x,
                                      int y,
                                      uint32_t modifiers,
                                      const std::string& filePaths,
                                      CefViewDragOperation allowedOps)
{
  auto host = GetBrowserHost(pCefBrowser_);
  if (!host)
    return;

  auto dragData = CefDragData::Create();
  auto paths = SplitFilePaths(filePaths);
  for (const auto& path : paths) {
    dragData->AddFile(path, ExtractFileName(path));
  }

  host->DragTargetDragEnter(
    dragData, BuildMouseEvent(x, y, modifiers), static_cast<CefBrowserHost::DragOperationsMask>(allowedOps));
}

void
CCefBrowser::dragTargetDragOver(int x, int y, uint32_t modifiers, CefViewDragOperation allowedOps)
{
  auto host = GetBrowserHost(pCefBrowser_);
  if (!host)
    return;

  host->DragTargetDragOver(BuildMouseEvent(x, y, modifiers),
                           static_cast<CefBrowserHost::DragOperationsMask>(allowedOps));
}

void
CCefBrowser::dragTargetDragLeave()
{
  auto host = GetBrowserHost(pCefBrowser_);
  if (!host)
    return;

  host->DragTargetDragLeave();
}

void
CCefBrowser::dragTargetDrop(int x, int y, uint32_t modifiers)
{
  auto host = GetBrowserHost(pCefBrowser_);
  if (!host)
    return;

  host->DragTargetDrop(BuildMouseEvent(x, y, modifiers));
}

void
CCefBrowser::dragSourceEndedAt(int x, int y, CefViewDragOperation operation)
{
  auto host = GetBrowserHost(pCefBrowser_);
  if (!host)
    return;

  host->DragSourceEndedAt(x, y, static_cast<CefBrowserHost::DragOperationsMask>(operation));
}

void
CCefBrowser::dragSourceSystemDragEnded()
{
  auto host = GetBrowserHost(pCefBrowser_);
  if (!host)
    return;

  host->DragSourceSystemDragEnded();
  resetSourceDragState();
}
void
CCefBrowser::sendTouchEvent(int touchId,
                            float x,
                            float y,
                            float radiusX,
                            float radiusY,
                            float rotationAngle,
                            float pressure,
                            int touchEventType,
                            uint32_t modifiers,
                            int pointerType)
{
  if (!pCefBrowser_)
    return;

  CefTouchEvent e;
  e.id = touchId;
  e.x = x;
  e.y = y;
  e.radius_x = radiusX;
  e.radius_y = radiusY;
  e.rotation_angle = rotationAngle;
  e.pressure = pressure;
  e.type = (cef_touch_event_type_t)touchEventType;
  e.modifiers = modifiers;
  e.pointer_type = (cef_pointer_type_t)pointerType;
  pCefBrowser_->GetHost()->SendTouchEvent(e);
}

void
CCefBrowser::sendKeyEvent(CefViewKeyEventType type,
                          uint32_t modifiers,
                          int windowsKeyCode,
                          int nativeKeyCode,
                          bool isSysKey,
                          uint16_t character,
                          uint16_t umodifiedCharacter,
                          bool isFocusOnEditableField)
{
  if (!pCefBrowser_)
    return;

  CefKeyEvent e;
  e.type = (cef_key_event_type_t)type;
  e.modifiers = modifiers;
  e.windows_key_code = windowsKeyCode;
  e.native_key_code = nativeKeyCode;
  e.is_system_key = isSysKey;
  e.character = character;
  e.unmodified_character = umodifiedCharacter;
  e.focus_on_editable_field = isFocusOnEditableField;
  pCefBrowser_->GetHost()->SendKeyEvent(e);
}

void
CCefBrowser::notifyMoveOrResizeStarted()
{
  if (!pCefBrowser_)
    return;

  pCefBrowser_->GetHost()->NotifyMoveOrResizeStarted();
}

void
CCefBrowser::notifyScreenChanged()
{
  if (!pCefBrowser_)
    return;

  pCefBrowser_->GetHost()->NotifyScreenInfoChanged();
}

void
CCefBrowser::imeSetComposition(const std::string& text,
                               CefViewCompositionUnderline underlines[],
                               int count,
                               CefViewRange replacement_range,
                               CefViewRange selection_range)
{
  if (!pCefBrowser_)
    return;

  std::vector<CefCompositionUnderline> underlineBuffer;
  for (int i = 0; i < count; i++) {
    auto& current = *(underlines + i);
    CefViewCompositionUnderline ul;
    ul.range = current.range;
    ul.color = current.color;
    ul.background_color = current.background_color;
    ul.thick = current.thick;
    ul.style = current.style;
    underlineBuffer.push_back(ul);
  }
  pCefBrowser_->GetHost()->ImeSetComposition(CefString(text), underlineBuffer, replacement_range, selection_range);
}

void
CCefBrowser::imeCommitText(const std::string& text, CefViewRange replacement_range, int relative_cursor_pos)
{
  if (!pCefBrowser_)
    return;

  CefString t;
  t.FromString(text);
  pCefBrowser_->GetHost()->ImeCommitText(t, replacement_range, relative_cursor_pos);
}

void
CCefBrowser::imeFinishComposingText(bool keep_selection)
{
  if (!pCefBrowser_)
    return;

  pCefBrowser_->GetHost()->ImeFinishComposingText(keep_selection);
}

void
CCefBrowser::imeCancelComposition()
{
  if (!pCefBrowser_)
    return;
  pCefBrowser_->GetHost()->ImeCancelComposition();
}

bool
CCefBrowser::continueJSDialog(void* dialogHandle, bool success, const std::string& userInput)
{
  CefRefPtr<CefJSDialogCallback> callback = nullptr;
  {
    std::lock_guard<std::mutex> lock(jsDialogCallbacksMutex_);
    const auto it = jsDialogCallbacks_.find(dialogHandle);
    if (it == jsDialogCallbacks_.end())
      return false;

    callback = it->second;
    jsDialogCallbacks_.erase(it);
  }

  if (!callback)
    return false;

  callback->Continue(success, userInput);
  return true;
}

void
CCefBrowser::cefQueryRequest(int browserId, const std::string& frameId, const CCefQuery* query)
{
  if (callbackTable_.pfnCefQueryRequest)
    callbackTable_.pfnCefQueryRequest(browserId, frameId.c_str(), query);
}

void
CCefBrowser::invokeMethod(int browserId,
                          const std::string& frameId,
                          const std::string& method,
                          const std::string& arguments)
{
  if (callbackTable_.pfnInvokeMethod)
    callbackTable_.pfnInvokeMethod(browserId, frameId.c_str(), method.c_str(), arguments.c_str());
}

void
CCefBrowser::reportJavascriptResult(int browserId,
                                    const std::string& frameId,
                                    const std::string& context,
                                    const std::string& result)
{
  if (callbackTable_.pfnReportJavascriptResult)
    callbackTable_.pfnReportJavascriptResult(browserId, frameId.c_str(), context.c_str(), result.c_str());
}

void
CCefBrowser::inputStateChanged(int browserId, const std::string& frameId, bool editable)
{
  if (callbackTable_.pfnInputStateChanged)
    callbackTable_.pfnInputStateChanged(browserId, frameId.c_str(), editable);
}

bool
CCefBrowser::startDragging(CefRefPtr<CefDragData>& dragData, CefViewDragOperation allowedOps, int x, int y)
{
  auto host = GetBrowserHost(pCefBrowser_);
  if (!host || !dragData)
    return false;

  sourceDragData_ = dragData->Clone();
  if (!sourceDragData_)
    return false;

  // DragTargetDragEnter does not accept embedded file contents.
  sourceDragData_->ResetFileContents();
  sourceDragAllowedOps_ = allowedOps;
  sourceDragActive_ = true;
  sourceDragEntered_ = false;

  if (callbackTable_.pfnStartDragging && !callbackTable_.pfnStartDragging(browserId(), allowedOps, x, y)) {
    resetSourceDragState();
    return false;
  }

  return true;
}

bool
CCefBrowser::sendEventNotifyMessage(const std::string& frameId, const std::string& name, const std::string& args)
{
  CefRefPtr<CefProcessMessage> msg = CefProcessMessage::Create(kCefViewClientBrowserTriggerEventMessage);
  CefRefPtr<CefListValue> arguments = msg->GetArgumentList();

  //** arguments(CefValueList)
  //** +------------+
  //** | event name |
  //** | event arg1 |
  //** | event arg2 |
  //** | event arg3 |
  //** | event arg4 |
  //** |    ...     |
  //** |    ...     |
  //** |    ...     |
  //** |    ...     |
  //** +------------+
  int idx = 0;
  CefString eventName = name;
  arguments->SetString(idx++, eventName);
  auto cefVal = CefValue::Create();
  if (!ValueConvertor::JsonStringToCefValue(cefVal.get(), args)) {
    return false;
  }

  auto cefValList = cefVal->GetList();
  if (!cefValList) {
    return false;
  }

  for (int i = 0; i < cefValList->GetSize(); i++) {
    auto cVal = cefValList->GetValue(i);
    arguments->SetValue(idx++, cVal);
  }

  return pClient_->TriggerEvent(pCefBrowser_, frameId, msg);
}

void
CCefBrowser::insertJSDialogCallback(CefRefPtr<CefJSDialogCallback> callback)
{
  std::lock_guard<std::mutex> lock(jsDialogCallbacksMutex_);
  jsDialogCallbacks_[callback.get()] = callback;
}

void
CCefBrowser::removeJSDialogCallback(CefRefPtr<CefJSDialogCallback> callback)
{
  std::lock_guard<std::mutex> lock(jsDialogCallbacksMutex_);
  jsDialogCallbacks_.erase(callback.get());
}

void
CCefBrowser::clearJSDialogCallbacks()
{
  std::lock_guard<std::mutex> lock(jsDialogCallbacksMutex_);
  jsDialogCallbacks_.clear();
}

void
CCefBrowser::updateSourceDragTarget(int x, int y, uint32_t modifiers)
{
  auto host = GetBrowserHost(pCefBrowser_);
  if (!host || !sourceDragData_)
    return;

  CefMouseEvent event = BuildMouseEvent(x, y, modifiers);
  if (!sourceDragEntered_) {
    host->DragTargetDragEnter(
      sourceDragData_, event, static_cast<CefBrowserHost::DragOperationsMask>(sourceDragAllowedOps_));
    sourceDragEntered_ = true;
  } else {
    host->DragTargetDragOver(event, static_cast<CefBrowserHost::DragOperationsMask>(sourceDragAllowedOps_));
  }
}

void
CCefBrowser::endSourceDrag(int x, int y, uint32_t modifiers, bool canceled)
{
  if (!sourceDragActive_) {
    resetSourceDragState();
    return;
  }

  auto host = GetBrowserHost(pCefBrowser_);
  if (!host) {
    resetSourceDragState();
    return;
  }

  if (sourceDragEntered_) {
    CefMouseEvent event = BuildMouseEvent(x, y, modifiers);
    if (canceled) {
      host->DragTargetDragLeave();
    } else {
      host->DragTargetDragOver(event, static_cast<CefBrowserHost::DragOperationsMask>(sourceDragAllowedOps_));
      host->DragTargetDrop(event);
    }
  }

  const auto endOp = canceled ? static_cast<CefBrowserHost::DragOperationsMask>(DRAG_OPERATION_NONE)
                              : static_cast<CefBrowserHost::DragOperationsMask>(sourceDragAllowedOps_);
  host->DragSourceEndedAt(x, y, endOp);
  host->DragSourceSystemDragEnded();
  resetSourceDragState();
}

void
CCefBrowser::resetSourceDragState()
{
  sourceDragActive_ = false;
  sourceDragEntered_ = false;
  sourceDragData_ = nullptr;
  sourceDragAllowedOps_ = DRAG_OPERATION_NONE;
}
