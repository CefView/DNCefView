#include "CCefClientDelegate.h"

#include "utils/ValueConvertor.h"

#include "../CefBrowser.h"
#include "../CefBrowser_Impl.h"

CCefClientDelegate::CCefClientDelegate(CCefBrowser* p)
  : pCefView_(p)
{}

CCefClientDelegate::~CCefClientDelegate()
{
  return;
}

void
CCefClientDelegate::processUrlRequest(const std::string& url)
{
  // deprecated feature
  // auto view = take(browser);
  // if (view) {
  //  auto u = QString::fromStdString(url);
  //  view->onQCefUrlRequest(u);
  //}
}

void
CCefClientDelegate::processQueryRequest(CefRefPtr<CefBrowser>& browser,
                                        int64_t frameId,
                                        const std::string& request,
                                        const int64_t query_id)
{
  if (!IsValidBrowser(browser))
    return;

  auto browserId = browser->GetIdentifier();
  auto query = new CCefQuery(request, query_id);
  pCefView_->cefQueryRequest(browserId, frameId, query);
}

void
CCefClientDelegate::focusedEditableNodeChanged(CefRefPtr<CefBrowser>& browser,
                                               int64_t frameId,
                                               bool focusOnEditableNode)
{
  if (!IsValidBrowser(browser))
    return;

  auto browserId = browser->GetIdentifier();
  pCefView_->inputStateChanged(browserId, frameId, focusOnEditableNode);
}

void
CCefClientDelegate::invokeMethodNotify(CefRefPtr<CefBrowser>& browser,
                                       int64_t frameId,
                                       const std::string& method,
                                       const CefRefPtr<CefListValue>& arguments)
{
  if (!IsValidBrowser(browser))
    return;

  auto cefValue = CefValue::Create();
  cefValue->SetList(arguments);
  std::string args;
  if (!ValueConvertor::CefValueToJsonString(args, cefValue.get())) {
    // TODO
    return;
  }

  auto browserId = browser->GetIdentifier();
  pCefView_->invokeMethod(browserId, frameId, method, args);
}

void
CCefClientDelegate::reportJSResult(CefRefPtr<CefBrowser>& browser,
                                   int64_t frameId,
                                   int64_t contextId,
                                   const CefRefPtr<CefValue>& result)
{
  if (!IsValidBrowser(browser))
    return;

  auto browserId = browser->GetIdentifier();
  std::string jsonString;

  if (!ValueConvertor::CefValueToJsonString(jsonString, result.get())) {
    // TODO
    return;
  }

  pCefView_->reportJavascriptResult(browserId, frameId, contextId, jsonString);
}
