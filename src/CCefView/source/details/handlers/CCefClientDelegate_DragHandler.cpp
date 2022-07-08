#include "CCefClientDelegate.h"

#include <CefBrowser.h>

#include "details/utils/ValueConvertor.h"

bool
CCefClientDelegate::onDragEnter(CefRefPtr<CefBrowser>& browser,
                                CefRefPtr<CefDragData>& dragData,
                                CefDragHandler::DragOperationsMask mask)
{
  return true;
}

void
CCefClientDelegate::draggableRegionChanged(CefRefPtr<CefBrowser>& browser,
                                           CefRefPtr<CefFrame>& frame,
                                           const std::vector<CefDraggableRegion>& regions)
{
  if (!IsValidBrowser(browser))
    return;

  if (pCefView_->callbackTable_.pfnDraggableRegionChanged)
    pCefView_->callbackTable_.pfnDraggableRegionChanged(regions.data(), static_cast<int>(regions.size()));
}
