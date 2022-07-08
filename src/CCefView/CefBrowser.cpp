#include "CefBrowser.h"

#pragma region cef_headers
#include <include/cef_browser.h>
#include <include/cef_frame.h>
#include <include/cef_parser.h>
#pragma endregion cef_headers

#include <CefViewBrowserClient.h>
#include <CefViewCoreProtocol.h>

#include "details/CCefClientDelegate.h"
#include "details/utils/ValueConvertor.h"

#include "CefBrowser_Impl.h"
#include "CefContext.h"
#include "CefContext_Impl.h"

#define P_IMPL(x) auto& x = pImpl_->x

CCefBrowser::CCefBrowser(CefBrowserCallback callback, const std::string& url, const CCefSetting* setting)
  : pImpl_(std::make_unique<Implementation>())
  , callbackTable_(callback)
{
  auto pContext = CCefContext::instance();

  // create browser client handler delegate
  auto pClientDelegate = std::make_shared<CCefClientDelegate>(this);

  // create browser client handler
  CefRefPtr<CefViewBrowserClient> pClient = new CefViewBrowserClient(pContext->pImpl_->pApp_, pClientDelegate);

  for (auto& folderMapping : pContext->pImpl_->folderResourceMappingList_) {
    pClient->AddLocalDirectoryResourceProvider(folderMapping.path, folderMapping.url, folderMapping.priority);
  }

  for (auto& archiveMapping : pContext->pImpl_->archiveResourceMappingList_) {
    pClient->AddArchiveResourceProvider(
      archiveMapping.path, archiveMapping.url, archiveMapping.password, archiveMapping.priority);
  }

  // Set window info
  CefWindowInfo window_info;
  window_info.SetAsWindowless(0);

  // create the browser settings
  CefBrowserSettings browserSettings;
  CCefSetting::copyToCefBrowserSettings(setting, browserSettings);

  if (CefColorGetA(browserSettings.background_color) == 0)
    pImpl_->transparentPaintingEnabled = true;

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

  pImpl_->pClient_ = pClient;
  pImpl_->pClientDelegate_ = pClientDelegate;
  return;
}

CCefBrowser::~CCefBrowser()
{
  P_IMPL(pClient_);
  P_IMPL(pCefBrowser_);

  // clean all browsers
  pClient_->CloseAllBrowsers();
  pClient_ = nullptr;
  pCefBrowser_ = nullptr;
}

void
CCefBrowser::addLocalFolderResource(const std::string& path, const std::string& url, int priority /*= 0*/)
{
  P_IMPL(pClient_);
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
  P_IMPL(pClient_);
  if (pClient_) {
    pClient_->AddArchiveResourceProvider(path, url, password, priority);
  }
}

int
CCefBrowser::browserId()
{
  P_IMPL(pCefBrowser_);

  if (pCefBrowser_)
    return pCefBrowser_->GetIdentifier();

  return -1;
}

void
CCefBrowser::navigateToString(const std::string& content)
{
  P_IMPL(pCefBrowser_);
  if (pCefBrowser_) {
    auto data = CefURIEncode(CefBase64Encode(content.c_str(), content.size()), false).ToString();
    data = "data:text/html;base64," + data;
    pCefBrowser_->GetMainFrame()->LoadURL(data);
  }
}

void
CCefBrowser::navigateToUrl(const std::string& url)
{
  P_IMPL(pCefBrowser_);
  if (pCefBrowser_) {
    CefString strUrl;
    strUrl.FromString(url);
    pCefBrowser_->GetMainFrame()->LoadURL(strUrl);
  }
}

bool
CCefBrowser::canGoBack()
{
  P_IMPL(pCefBrowser_);
  if (pCefBrowser_)
    return pCefBrowser_->CanGoBack();

  return false;
}

bool
CCefBrowser::canGoForward()
{
  P_IMPL(pCefBrowser_);
  if (pCefBrowser_)
    return pCefBrowser_->CanGoForward();

  return false;
}

void
CCefBrowser::goBack()
{
  P_IMPL(pCefBrowser_);
  if (pCefBrowser_)
    pCefBrowser_->GoBack();
}

void
CCefBrowser::goForward()
{
  P_IMPL(pCefBrowser_);
  if (pCefBrowser_)
    pCefBrowser_->GoForward();
}

bool
CCefBrowser::isLoading()
{
  P_IMPL(pCefBrowser_);
  if (pCefBrowser_)
    return pCefBrowser_->IsLoading();

  return false;
}

void
CCefBrowser::reload()
{
  P_IMPL(pCefBrowser_);
  if (pCefBrowser_)
    pCefBrowser_->Reload();
}

void
CCefBrowser::stopLoad()
{
  P_IMPL(pCefBrowser_);
  if (pCefBrowser_)
    pCefBrowser_->StopLoad();
}

bool
CCefBrowser::triggerEventOnMainFrame(const std::string& evtName, const std::string& evtArgs)
{
  return triggerEvent(evtName, evtArgs, CCefBrowser::MainFrameID);
}

bool
CCefBrowser::triggerEventOnFrame(const std::string& evtName, const std::string& evtArgs, int64_t frameId)
{
  return triggerEvent(evtName, evtArgs, frameId);
}

bool
CCefBrowser::broadcastEvent(const std::string& evtName, const std::string& evtArgs)
{
  return triggerEvent(evtName, evtArgs, CefViewBrowserClient::ALL_FRAMES);
}

bool
CCefBrowser::triggerEvent(const std::string& name, const std::string& args, int64_t frameId /*= CCefView::MainFrameID*/)
{
  if (!name.empty()) {
    return sendEventNotifyMessage(frameId, name, args);
  }

  return false;
}

bool
CCefBrowser::responseQCefQuery(const CCefQuery* query)
{
  P_IMPL(pClient_);
  if (pClient_) {
    CefString res;
    res.FromString(query->getResponse());
    return pClient_->ResponseQuery(query->getId(), query->getResult(), res, query->getError());
  }
  return false;
}

bool
CCefBrowser::executeJavascript(int64_t frameId, const std::string& code, const std::string& url)
{
  if (code.empty())
    return false;

  P_IMPL(pCefBrowser_);
  if (pCefBrowser_) {
    CefRefPtr<CefFrame> frame = pCefBrowser_->GetFrame(frameId);
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
CCefBrowser::executeJavascriptWithResult(int64_t frameId,
                                         const std::string& code,
                                         const std::string& url,
                                         int64_t context)
{
  if (code.empty())
    return false;

  P_IMPL(pCefBrowser_);
  P_IMPL(pClient_);
  if (pClient_ && pCefBrowser_) {
    auto frame = frameId == 0 ? pCefBrowser_->GetMainFrame() : pCefBrowser_->GetFrame(frameId);
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
  P_IMPL(pCefBrowser_);
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
  pImpl_->disablePopuContextMenu_ = disable;
}

bool
CCefBrowser::isPopupContextMenuDisabled()
{
  return pImpl_->disablePopuContextMenu_;
}

void
CCefBrowser::ImeSetComposition(const std::string& text,
                               CefViewCompositionUnderline* underlines,
                               int count,
                               CefViewRange replacement_range,
                               CefViewRange selection_range)
{
  if (!pImpl_ || !pImpl_->pCefBrowser_)
    return;

  std::vector<CefCompositionUnderline> underlineBuffer;
  for (int i = 0; i < count; i++) {
    CefViewCompositionUnderline ul = *(underlines + i);
    underlineBuffer.push_back(ul);
  }
  pImpl_->pCefBrowser_->GetHost()->ImeSetComposition(
    CefString(text), underlineBuffer, replacement_range, selection_range);
}

void
CCefBrowser::ImeCommitText(const std::string& text, CefViewRange replacement_range, int relative_cursor_pos)
{
  if (!pImpl_ || !pImpl_->pCefBrowser_)
    return;

  CefString t;
  t.FromString(text);
  pImpl_->pCefBrowser_->GetHost()->ImeCommitText(t, replacement_range, relative_cursor_pos);
}

void
CCefBrowser::ImeFinishComposingText(bool keep_selection)
{
  if (!pImpl_ || !pImpl_->pCefBrowser_)
    return;

  pImpl_->pCefBrowser_->GetHost()->ImeFinishComposingText(keep_selection);
}

void
CCefBrowser::setFocus(bool focused)
{
  if (!pImpl_ || !pImpl_->pCefBrowser_)
    return;

  pImpl_->pCefBrowser_->GetHost()->SetFocus(focused);
}

void
CCefBrowser::wasResized()
{
  if (!pImpl_ || !pImpl_->pCefBrowser_)
    return;

  pImpl_->pCefBrowser_->GetHost()->WasResized();
}

void
CCefBrowser::wasHidden(bool hidden)
{
  if (!pImpl_ || !pImpl_->pCefBrowser_)
    return;

  pImpl_->pCefBrowser_->GetHost()->WasHidden(hidden);
}

void
CCefBrowser::sendMouseMoveEvent(int x, int y, uint32_t modifiers, bool leave)
{
  if (!pImpl_ || !pImpl_->pCefBrowser_)
    return;

  CefMouseEvent e;
  e.x = x;
  e.y = y;
  e.modifiers = modifiers;
  pImpl_->pCefBrowser_->GetHost()->SendMouseMoveEvent(e, leave);
}

void
CCefBrowser::sendMouseClickEvent(int x,
                                 int y,
                                 uint32_t modifiers,
                                 CefViewMouseButtonType type,
                                 bool mouseUp,
                                 int clickCount)
{
  if (!pImpl_ || !pImpl_->pCefBrowser_)
    return;

  CefMouseEvent e;
  e.x = x;
  e.y = y;
  e.modifiers = modifiers;
  pImpl_->pCefBrowser_->GetHost()->SendMouseClickEvent(e, (CefBrowserHost::MouseButtonType)type, mouseUp, clickCount);
}

void
CCefBrowser::sendWheelEvent(int x, int y, uint32_t modifiers, int deltaX, int deltaY)
{
  if (!pImpl_ || !pImpl_->pCefBrowser_)
    return;

  CefMouseEvent e;
  e.x = x;
  e.y = y;
  e.modifiers = modifiers;
  pImpl_->pCefBrowser_->GetHost()->SendMouseWheelEvent(e, deltaX, deltaY);
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
  if (!pImpl_ || !pImpl_->pCefBrowser_)
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
  pImpl_->pCefBrowser_->GetHost()->SendKeyEvent(e);
}

void
CCefBrowser::notifyMoveOrResizeStarted()
{
  if (!pImpl_ || !pImpl_->pCefBrowser_)
    return;

  pImpl_->pCefBrowser_->GetHost()->NotifyMoveOrResizeStarted();
}

void
CCefBrowser::notifyScreenChanged()
{
  if (!pImpl_ || !pImpl_->pCefBrowser_)
    return;

  pImpl_->pCefBrowser_->GetHost()->NotifyScreenInfoChanged();
}

void
CCefBrowser::cefQueryRequest(int browserId, int64_t frameId, const CCefQuery* query)
{
  if (callbackTable_.pfnCefQueryRequest)
    callbackTable_.pfnCefQueryRequest(browserId, frameId, query);
}

void
CCefBrowser::invokeMethod(int browserId, int64_t frameId, const std::string& method, const std::string& arguments)
{
  if (callbackTable_.pfnInvokeMethod)
    callbackTable_.pfnInvokeMethod(browserId, frameId, method.c_str(), arguments.c_str());
}

void
CCefBrowser::reportJavascriptResult(int browserId, int64_t frameId, int64_t context, const std::string& result)
{
  if (callbackTable_.pfnReportJavascriptResult)
    callbackTable_.pfnReportJavascriptResult(browserId, frameId, context, result.c_str());
}

void
CCefBrowser::inputStateChanged(int browserId, int64_t frameId, bool editable)
{
  if (callbackTable_.pfnInputStateChanged)
    callbackTable_.pfnInputStateChanged(browserId, frameId, editable);
}

bool
CCefBrowser::sendEventNotifyMessage(int64_t frameId, const std::string& name, const std::string& args)
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

  P_IMPL(pClient_);
  P_IMPL(pCefBrowser_);
  return pClient_->TriggerEvent(pCefBrowser_, frameId, msg);
}
