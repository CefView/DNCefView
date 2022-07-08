
#ifndef CCEFVIEW_IMPL_H
#define CCEFVIEW_IMPL_H

#include "CefBrowser.h"

#include <CefViewBrowserClient.h>

#include "details/CCefClientDelegate.h"

#include "CefContext.h"
#include "CefContext_Impl.h"

class CCefBrowser::Implementation
{
public:
  /// <summary>
  ///
  /// </summary>
  bool disablePopuContextMenu_ = false;

  /// <summary>
  ///
  /// </summary>
  bool transparentPaintingEnabled = false;

  /// <summary>
  ///
  /// </summary>
  bool showPopup_ = false;

  /// <summary>
  ///
  /// </summary>
  CCefClientDelegate::RefPtr pClientDelegate_ = nullptr;

  /// <summary>
  ///
  /// </summary>
  CefRefPtr<CefViewBrowserClient> pClient_ = nullptr;

  /// <summary>
  ///
  /// </summary>
  CefRefPtr<CefBrowser> pCefBrowser_ = nullptr;

  /// <summary>
  ///
  /// </summary>
  CefRect qPopupRect_;

  /// <summary>
  ///
  /// </summary>
  CefRect qImeCursorRect_;
};

#endif
