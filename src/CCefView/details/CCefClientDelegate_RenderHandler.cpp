#include "CCefClientDelegate.h"

#include "../CefBrowser.h"
#include "../CefBrowser_Impl.h"

#if defined(CEF_USE_OSR)

bool
CCefClientDelegate::getRootScreenRect(CefRefPtr<CefBrowser> browser, CefRect& rect)
{
  if (!IsValidBrowser(browser))
    return false;

  if (pCefView_->callbackTable_.pfnGetRootScreenRect) {
    pCefView_->callbackTable_.pfnGetRootScreenRect(browser->GetIdentifier(), &rect);
  }

  return true;
}

void
CCefClientDelegate::getViewRect(CefRefPtr<CefBrowser> browser, CefRect& rect)
{
  if (!IsValidBrowser(browser)) {
    rect.Set(0, 0, 1, 1);
    return;
  }

  if (pCefView_->callbackTable_.pfnGetViewRect) {
    pCefView_->callbackTable_.pfnGetViewRect(browser->GetIdentifier(), &rect);
  }

  return;
}

bool
CCefClientDelegate::getScreenPoint(CefRefPtr<CefBrowser> browser, int viewX, int viewY, int& screenX, int& screenY)
{
  if (!IsValidBrowser(browser))
    return false;

  if (pCefView_->callbackTable_.pfnGetScreenPoint) {
    return pCefView_->callbackTable_.pfnGetScreenPoint(browser->GetIdentifier(), viewX, viewY, &screenX, &screenY);
  }

  return false;
}

bool
CCefClientDelegate::getScreenInfo(CefRefPtr<CefBrowser> browser, CefScreenInfo& screen_info)
{
  if (!IsValidBrowser(browser))
    return false;

  if (pCefView_->callbackTable_.pfnGetScreenInfo) {
    pCefView_->callbackTable_.pfnGetScreenInfo(browser->GetIdentifier(), &screen_info);
  }

  return true;
}

void
CCefClientDelegate::onPopupShow(CefRefPtr<CefBrowser> browser, bool show)
{
  if (!IsValidBrowser(browser))
    return;

  if (pCefView_->callbackTable_.pfnOnPopupShow) {
    pCefView_->callbackTable_.pfnOnPopupShow(browser->GetIdentifier(), show);
  }
}

void
CCefClientDelegate::onPopupSize(CefRefPtr<CefBrowser> browser, const CefRect& rect)
{
  if (!IsValidBrowser(browser))
    return;

  if (pCefView_->callbackTable_.pfnOnPopupSize) {
    pCefView_->callbackTable_.pfnOnPopupSize(browser->GetIdentifier(), rect);
  }
}

void
CCefClientDelegate::onPaint(CefRefPtr<CefBrowser> browser,
                            CefRenderHandler::PaintElementType type,
                            const CefRenderHandler::RectList& dirtyRects,
                            const void* buffer,
                            int width,
                            int height)
{
  if (!IsValidBrowser(browser))
    return;

  std::vector<cef_rect_t> dirtyRectsBuffer;
  for (auto const& rc : dirtyRects)
    dirtyRectsBuffer.push_back(rc);

  if (pCefView_->callbackTable_.pfnOnPaint) {
    pCefView_->callbackTable_.pfnOnPaint(browser->GetIdentifier(),
                                         type,
                                         dirtyRectsBuffer.data(),
                                         dirtyRectsBuffer.size(),
                                         buffer,
                                         width * height * 4,
                                         width,
                                         height);
  }
}

void
CCefClientDelegate::onAcceleratedPaint(CefRefPtr<CefBrowser> browser,
                                       CefRenderHandler::PaintElementType type,
                                       const CefRenderHandler::RectList& dirtyRects,
                                       void* shared_handle)
{}

bool
CCefClientDelegate::startDragging(CefRefPtr<CefBrowser> browser,
                                  CefRefPtr<CefDragData> drag_data,
                                  CefRenderHandler::DragOperationsMask allowed_ops,
                                  int x,
                                  int y)
{
  return false;
}

void
CCefClientDelegate::updateDragCursor(CefRefPtr<CefBrowser> browser, CefRenderHandler::DragOperation operation)
{
  return;
}

void
CCefClientDelegate::onScrollOffsetChanged(CefRefPtr<CefBrowser> browser, double x, double y)
{}

void
CCefClientDelegate::onImeCompositionRangeChanged(CefRefPtr<CefBrowser> browser,
                                                 const CefRange& selected_range,
                                                 const CefRenderHandler::RectList& character_bounds)
{
  if (!IsValidBrowser(browser))
    return;

  std::vector<cef_rect_t> characterBoundsBuffer;
  for (auto const& rc : character_bounds)
    characterBoundsBuffer.push_back(rc);

  if (pCefView_->callbackTable_.pfnOnImeCompositionRangeChanged) {
    pCefView_->callbackTable_.pfnOnImeCompositionRangeChanged(
      browser->GetIdentifier(), selected_range, characterBoundsBuffer.data(), characterBoundsBuffer.size());
  }

  return;
}

void
CCefClientDelegate::onTextSelectionChanged(CefRefPtr<CefBrowser> browser,
                                           const CefString& selected_text,
                                           const CefRange& selected_range)
{}

void
CCefClientDelegate::onVirtualKeyboardRequested(CefRefPtr<CefBrowser> browser,
                                               CefRenderHandler::TextInputMode input_mode)
{}

#endif
