#ifndef CCEFVIEWDELEGATE_H
#define CCEFVIEWDELEGATE_H

#pragma once
// stl
#include <cstdint>

// cef
#include <include/cef_app.h>

// project
#include <CefQuery.h>
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

  void(STDCALL* pfnCefQueryRequest)(int browserId, const char* frameId, const CCefQuery* query);

  void(STDCALL* pfnInvokeMethod)(int browserId, const char* frameId, const char* method, const char* arguments);

  void(STDCALL* pfnReportJavascriptResult)(int browserId, const char* frameId, const char* context, const char* result);

  void(STDCALL* pfnInputStateChanged)(int browserId, const char* frameId, bool editable);

  //////////////////////////////////////////////////////////////////////////
  // LoadHandler

  void(STDCALL* pfnLoadingStateChanged)(int browserId, bool isLoading, bool canGoBack, bool canGoForward);

  void(STDCALL* pfnLoadStart)(int browserId, const char* frameId, bool isMainFrame, int transition_type);

  void(STDCALL* pfnLoadEnd)(int browserId, const char* frameId, bool isMainFrame, int httpStatusCode);

  bool(STDCALL* pfnLoadError)(int browserId,
                              const char* frameId,
                              bool isMainFrame,
                              int errorCode,
                              const char* errorMsg,
                              const char* failedUrl);

  //////////////////////////////////////////////////////////////////////////
  // DisplayHandler

  void(STDCALL* pfnDraggableRegionChanged)(const CefViewDraggableRegion* draggableRegion, int count);

  void(STDCALL* pfnAddressChanged)(const char* frameId, const char* url);

  void(STDCALL* pfnTitleChanged)(const char* title);

  void(STDCALL* pfnFullscreenModeChanged)(bool fullscreen);

  void(STDCALL* pfnStatusMessage)(const char* message);

  void(STDCALL* pfnConsoleMessage)(const char* message, int level);

  void(STDCALL* pfnLoadingProgressChanged)(double progress);

  bool(STDCALL* pfnCursorChanged)(void* cursor, CefViewCursorType type, const CefViewCursorInfo customCursorInfo);

  //////////////////////////////////////////////////////////////////////////
  // LifespanHandler

  // bool(STDCALL* pfnOnBeforePopup)(int64_t frameId,
  //                                 const char* targetUrl,
  //                                 const char* targetFrameName,
  //                                 CefViewWindowOpenDisposition targetDisposition,
  //                                 CCefSetting* settings,
  //                                 bool* DisableJavascriptAccess);

  void(STDCALL* pfnOnAfterCreated)();

  //////////////////////////////////////////////////////////////////////////
  // FocusHandler

  void(STDCALL* pfnFocusReleasedByTabKey)(int browserId, bool next);

  bool(STDCALL* pfnSetFocus)(int browserId);

  void(STDCALL* pfnGotFocus)(int browserId);

  //////////////////////////////////////////////////////////////////////////
  // RenderHandler
  void(STDCALL* pfnGetRootScreenRect)(int browserId, CefViewRect* rect);

  void(STDCALL* pfnGetViewRect)(int browserId, CefViewRect* rect);

  bool(STDCALL* pfnGetScreenPoint)(int browserId, int viewX, int viewY, int* screenX, int* screenY);

  bool(STDCALL* pfnGetScreenInfo)(int browserId, CefViewScreenInfo* screenInfo);

  void(STDCALL* pfnOnPopupShow)(int browserId, bool show);

  void(STDCALL* pfnOnPopupSize)(int browserId, const CefViewRect rect);

  void(STDCALL* pfnOnPaint)(int browserId,
                            CefViewPaintElementType type,
                            const CefViewRect* dirtyRects,
                            int dirtyRectCount,
                            const void* imageBytes,
                            int imageBytesCount,
                            int width,
                            int height);

  void(STDCALL* pfnOnAcceleratedPaint)(int browserId,
                                       CefViewPaintElementType type,
                                       const CefViewRect* dirtyRects,
                                       int dirtyRectCount,
                                       void* sharedHandle,
                                       int planeBytesCount);

  // bool(STDCALL* pfnStartDragging)(int browserId, CefRefPtr<CefDragData> dragData, uint32_t allowedOps, int x, int y);

  void(STDCALL* pfnUpdateDragCursor)(int browserId, CefViewDragOperation operation);

  void(STDCALL* pfnOnScrollOffsetChanged)(int browserId, double x, double y);

  void(STDCALL* pfnOnImeCompositionRangeChanged)(int browserId,
                                                 const CefViewRange selectedRange,
                                                 const CefViewRect* characterBounds,
                                                 int characterBoundsCount);

  void(STDCALL* pfnOnTextSelectionChanged)(int browserId, const char* selectedText, const CefViewRange selectedRange);

  void(STDCALL* pfnOnVirtualKeyboardRequested)(int browserId, CefViewTextInputMode inputMode);
};

#endif
