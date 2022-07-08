#include "CCefClientDelegate.h"

#include <CefBrowser.h>

#include "details/utils/CommonUtils.h"
#include "details/utils/ValueConvertor.h"

CCefClientDelegate::CCefClientDelegate(CCefBrowser* p)
  : pCefView_(p)
{
}

CCefClientDelegate::~CCefClientDelegate()
{
  return;
}

void
CCefClientDelegate::processUrlRequest(CefRefPtr<CefBrowser>& browser, CefRefPtr<CefFrame>& frame, const CefString& url)
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
                                        CefRefPtr<CefFrame>& frame,
                                        const CefString& query,
                                        const int64_t query_id)
{
  if (!IsValidBrowser(browser))
    return;

  auto browserId = browser->GetIdentifier();
  auto frameId = frame->GetIdentifier();
  auto queryObject = new CCefQuery(query, query_id);
  pCefView_->cefQueryRequest(browserId, FrameIdC2X(frameId), queryObject);
}

void
CCefClientDelegate::focusedEditableNodeChanged(CefRefPtr<CefBrowser>& browser,
                                               CefRefPtr<CefFrame>& frame,
                                               bool focusOnEditableNode)
{
  if (!IsValidBrowser(browser))
    return;

  auto browserId = browser->GetIdentifier();
  auto frameId = frame->GetIdentifier();
  pCefView_->inputStateChanged(browserId, FrameIdC2X(frameId), focusOnEditableNode);
}

void
CCefClientDelegate::invokeMethodNotify(CefRefPtr<CefBrowser>& browser,
                                       CefRefPtr<CefFrame>& frame,
                                       const CefString& method,
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
  auto frameId = frame->GetIdentifier();
  pCefView_->invokeMethod(browserId, FrameIdC2X(frameId), method, args);
}

void
CCefClientDelegate::reportJSResult(CefRefPtr<CefBrowser>& browser,
                                   CefRefPtr<CefFrame>& frame,
                                   const CefString& context,
                                   const CefRefPtr<CefValue>& result)
{
  if (!IsValidBrowser(browser))
    return;

  auto browserId = browser->GetIdentifier();
  auto frameId = frame->GetIdentifier();

  std::string jsonString;
  if (!ValueConvertor::CefValueToJsonString(jsonString, result.get())) {
    // TODO
    return;
  }

  pCefView_->reportJavascriptResult(browserId, FrameIdC2X(frameId), context, jsonString);
}
