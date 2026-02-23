#ifndef CCEFVIEWDELEGATE_H
#define CCEFVIEWDELEGATE_H

#pragma once
// stl
#include <cstdint>

// cef
#include <include/cef_app.h>

// project
#include <CefQuery.h>
#include <CefSetting.h>
#include <CefTypes.h>

#if defined(_WIN32)
#define STDCALL __stdcall
#else
#define STDCALL
#endif

/// <summary>
///
/// </summary>
struct CefBrowserCallback
{
public:
  //////////////////////////////////////////////////////////////////////////
  // CefView events
  void(STDCALL* pfnCefQueryRequest)(const int browserId, const char* frameId, const CCefQuery* query);

  void(STDCALL* pfnInvokeMethod)(const int browserId, const char* frameId, const char* method, const char* arguments);

  void(STDCALL* pfnReportJavascriptResult)(const int browserId,
                                           const char* frameId,
                                           const char* context,
                                           const char* result);

  void(STDCALL* pfnInputStateChanged)(const int browserId, const char* frameId, const bool editable);

  //////////////////////////////////////////////////////////////////////////
  // TODO: DialogHandler

  //////////////////////////////////////////////////////////////////////////
  // DisplayHandler
  void(STDCALL* pfnAddressChanged)(const int browserId, const char* frameId, const char* url);

  void(STDCALL* pfnTitleChanged)(const int browserId, const char* title);

  void(STDCALL* pfnFullscreenModeChanged)(const int browserId, const bool fullscreen);

  void(STDCALL* pfnStatusMessage)(const int browserId, const char* message);

  void(STDCALL* pfnConsoleMessage)(const int browserId, const char* message, const int level);

  void(STDCALL* pfnLoadingProgressChanged)(const int browserId, double progress);

  bool(STDCALL* pfnCursorChanged)(const int browserId,
                                  const void* cursorHandle,
                                  const CefViewCursorType type,
                                  const CefViewCursorInfo customCursorInfo);

  //////////////////////////////////////////////////////////////////////////
  // TODO: DownloadHandler

  //////////////////////////////////////////////////////////////////////////
  // DragHandler
  void(STDCALL* pfnDraggableRegionChanged)(const CefViewDraggableRegion draggableRegion[], const int count);

  //////////////////////////////////////////////////////////////////////////
  // TODO: FindHandler

  //////////////////////////////////////////////////////////////////////////
  // FocusHandler
  void(STDCALL* pfnFocusReleasedByTabKey)(const int browserId, const bool next);

  bool(STDCALL* pfnSetFocus)(const int browserId);

  void(STDCALL* pfnGotFocus)(const int browserId);

  //////////////////////////////////////////////////////////////////////////
  // TODO: JSDialogHandler

  //////////////////////////////////////////////////////////////////////////
  // TODO: KeyboardHandler

  //////////////////////////////////////////////////////////////////////////
  // LifespanHandler
  bool(STDCALL* pfnOnBeforeNewPopupCreate)(const char* frameId,
                                           const char* targetUrl,
                                           const char* targetFrameName,
                                           const CefViewWindowOpenDisposition targetDisposition,
                                           CefViewRect* rect,
                                           CCefSetting* settings,
                                           bool* DisableJavascriptAccess);

  bool(STDCALL* pfnOnBeforeNewBrowserCreate)(const char* frameId,
                                             const char* targetUrl,
                                             const char* targetFrameName,
                                             const CefViewWindowOpenDisposition targetDisposition,
                                             const CefViewRect rect,
                                             const CCefSetting* settings);

  bool(STDCALL* pfnDoClose)();

  bool(STDCALL* pfnRequestClose)();

  void(STDCALL* pfnOnAfterCreated)();

  void(STDCALL* pfnOnBeforeClose)();

  //////////////////////////////////////////////////////////////////////////
  // LoadHandler
  void(STDCALL* pfnLoadingStateChanged)(const int browserId,
                                        const bool isLoading,
                                        const bool canGoBack,
                                        const bool canGoForward);

  void(STDCALL* pfnLoadStart)(const int browserId,
                              const char* frameId,
                              const bool isMainFrame,
                              const int transition_type);

  void(STDCALL* pfnLoadEnd)(const int browserId, const char* frameId, const bool isMainFrame, const int httpStatusCode);

  bool(STDCALL* pfnLoadError)(const int browserId,
                              const char* frameId,
                              const bool isMainFrame,
                              const int errorCode,
                              const char* errorMsg,
                              const char* failedUrl);

  //////////////////////////////////////////////////////////////////////////
  // RenderHandler
  void(STDCALL* pfnGetRootScreenRect)(const int browserId, CefViewRect* rect);

  void(STDCALL* pfnGetViewRect)(const int browserId, CefViewRect* rect);

  bool(STDCALL* pfnGetScreenPoint)(const int browserId, const int viewX, const int viewY, int* screenX, int* screenY);

  bool(STDCALL* pfnGetScreenInfo)(const int browserId, CefViewScreenInfo* screenInfo);

  void(STDCALL* pfnOnPopupShow)(const int browserId, const bool show);

  void(STDCALL* pfnOnPopupSize)(const int browserId, const CefViewRect rect);

  void(STDCALL* pfnOnPaint)(const int browserId,
                            const CefViewPaintElementType type,
                            const CefViewRect dirtyRects[],
                            const int dirtyRectCount,
                            const void* imageBytesBuffer,
                            const int imageBytesCount,
                            const int width,
                            const int height);

  void(STDCALL* pfnOnAcceleratedPaint)(const int browserId,
                                       const CefViewPaintElementType type,
                                       const CefViewRect dirtyRects[],
                                       const int dirtyRectCount,
                                       const void* sharedHandle,
                                       const int planeBytesCount);

  // bool(STDCALL* pfnStartDragging)(const int browserId, CefRefPtr<CefDragData> dragData, uint32_t allowedOps, int x,
  // int y);

  void(STDCALL* pfnUpdateDragCursor)(const int browserId, const CefViewDragOperation operation);

  void(STDCALL* pfnOnScrollOffsetChanged)(const int browserId, const double x, const double y);

  void(STDCALL* pfnOnImeCompositionRangeChanged)(const int browserId,
                                                 const CefViewRange selectedRange,
                                                 const CefViewRect characterBounds[],
                                                 const int characterBoundsCount);

  void(STDCALL* pfnOnTextSelectionChanged)(const int browserId,
                                           const char* selectedText,
                                           const CefViewRange selectedRange);

  void(STDCALL* pfnOnVirtualKeyboardRequested)(const int browserId, const CefViewTextInputMode inputMode);
};

#endif
