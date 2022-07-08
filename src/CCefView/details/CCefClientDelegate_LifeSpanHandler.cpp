#include "CCefClientDelegate.h"

#include "../CefBrowser_Impl.h"

bool
CCefClientDelegate::onBeforePopup(CefRefPtr<CefBrowser>& browser,
                                  int64_t frameId,
                                  const std::string& targetUrl,
                                  const std::string& targetFrameName,
                                  CefLifeSpanHandler::WindowOpenDisposition targetDisposition,
                                  CefWindowInfo& windowInfo,
                                  CefBrowserSettings& settings,
                                  bool& DisableJavascriptAccess)
{
  bool result = false;
  // if (pCefView_) {
  //   auto url = QString::fromStdString(targetUrl);
  //   auto name = QString::fromStdString(targetFrameName);

  //  QCefSetting s;
  //  QCefView::CefWindowOpenDisposition d = (QCefView::CefWindowOpenDisposition)targetDisposition;
  //  QCefSettingPrivate::CopyFromCefBrowserSettings(&s, &settings);

  //  Qt::ConnectionType c =
  //    pCefView_->q_ptr->thread() == QThread::currentThread() ? Qt::DirectConnection : Qt::BlockingQueuedConnection;
  //  QMetaObject::invokeMethod(pCefView_->q_ptr,
  //                            "onBeforePopup",                              //
  //                            c,                                            //
  //                            Q_RETURN_ARG(bool, result),                   //
  //                            Q_ARG(qint64, frameId),                       //
  //                            Q_ARG(const QString&, url),                   //
  //                            Q_ARG(const QString&, name),                  //
  //                            Q_ARG(QCefView::CefWindowOpenDisposition, d), //
  //                            Q_ARG(QCefSetting&, s),                       //
  //                            Q_ARG(bool&, DisableJavascriptAccess)         //
  //  );
  //  QCefSettingPrivate::CopyToCefBrowserSettings(&s, &settings);
  //}
  return result;
}

void
CCefClientDelegate::onAfterCreate(CefRefPtr<CefBrowser>& browser)
{
  if (!pCefView_)
    return;

  if (browser->IsPopup()) {
    // pop-up browser

  } else {
    // main browser
    pCefView_->pImpl_->pCefBrowser_ = browser;

    if (pCefView_->callbackTable_.pfnOnAfterCreated)
      pCefView_->callbackTable_.pfnOnAfterCreated();
  }
}

bool
CCefClientDelegate::doClose(CefRefPtr<CefBrowser> browser)
{
  if (!pCefView_)
    return false;

  bool rt = false;

  // Qt::ConnectionType c =
  //   pCefView_->q_ptr->thread() == QThread::currentThread() ? Qt::DirectConnection : Qt::BlockingQueuedConnection;

  // QMetaObject::invokeMethod(
  //   pCefView_,
  //   [this, browser, &rt]() {
  //     CefRefPtr<CefBrowser> b(browser);
  //     rt = pCefView_->onCefDoCloseBrowser(b);
  //   },
  //   c);
  return rt;
}

void
CCefClientDelegate::OnBeforeClose(CefRefPtr<CefBrowser> browser)
{
  if (!pCefView_)
    return;

  // Qt::ConnectionType c =
  //   pCefView_->q_ptr->thread() == QThread::currentThread() ? Qt::DirectConnection : Qt::BlockingQueuedConnection;

  // QMetaObject::invokeMethod(
  //   pCefView_,
  //   [this, browser]() {
  //     CefRefPtr<CefBrowser> b(browser);
  //     pCefView_->onCefBeforeCloseBrowser(b);
  //   },
  //   c);
}
