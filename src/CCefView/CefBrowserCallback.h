#ifndef CCEFVIEWDELEGATE_H
#define CCEFVIEWDELEGATE_H

#pragma once
#include <cstdint>

#include <include/cef_app.h>

#include "CefQuery.h"
#include "CefTypes.h"

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

  typedef void(STDCALL* Type_CefQueryRequest)(int browserId, int64_t frameId, const CCefQuery* query);
  Type_CefQueryRequest pfnCefQueryRequest;

  typedef void(STDCALL* Type_InvokeMethod)(int browserId, int64_t frameId, const char* method, const char* arguments);
  Type_InvokeMethod pfnInvokeMethod;

  typedef void(STDCALL* Type_ReportJavascriptResult)(int browserId,
                                                     int64_t frameId,
                                                     int64_t context,
                                                     const char* result);
  Type_ReportJavascriptResult pfnReportJavascriptResult;

  typedef void(STDCALL* Type_InputStateChanged)(int browserId, int64_t frameId, bool editable);
  Type_InputStateChanged pfnInputStateChanged;

  //////////////////////////////////////////////////////////////////////////
  // LoadHandler

  typedef void(STDCALL* Type_LoadingStateChanged)(int browserId, bool isLoading, bool canGoBack, bool canGoForward);
  Type_LoadingStateChanged pfnLoadingStateChanged;

  typedef void(STDCALL* Type_LoadStart)(int browserId, int64_t frameId, bool isMainFrame, int transition_type);
  Type_LoadStart pfnLoadStart;

  typedef void(STDCALL* Type_LoadEnd)(int browserId, int64_t frameId, bool isMainFrame, int httpStatusCode);
  Type_LoadEnd pfnLoadEnd;

  typedef bool(STDCALL* Type_LoadError)(int browserId,
                                        int64_t frameId,
                                        bool isMainFrame,
                                        int errorCode,
                                        const char* errorMsg,
                                        const char* failedUrl);
  Type_LoadError pfnLoadError;

  //////////////////////////////////////////////////////////////////////////
  // DisplayHandler

  typedef void(STDCALL* Type_DraggableRegionChanged)(const CefViewDraggableRegion* draggableRegion, int count);
  Type_DraggableRegionChanged pfnDraggableRegionChanged;

  typedef void(STDCALL* Type_AddressChanged)(int64_t frameId, const char* url);
  Type_AddressChanged pfnAddressChanged;

  typedef void(STDCALL* Type_TitleChanged)(const char* title);
  Type_TitleChanged pfnTitleChanged;

  typedef void(STDCALL* Type_FullscreenModeChanged)(bool fullscreen);
  Type_FullscreenModeChanged pfnFullscreenModeChanged;

  typedef void(STDCALL* Type_StatusMessage)(const char* message);
  Type_StatusMessage pfnStatusMessage;

  typedef void(STDCALL* Type_ConsoleMessage)(const char*, int level);
  Type_ConsoleMessage pfnConsoleMessage;

  typedef void(STDCALL* Type_LoadingProgressChanged)(double progress);
  Type_LoadingProgressChanged pfnLoadingProgressChanged;

  typedef bool(STDCALL* Type_CursorChanged)(void* cursor,
                                            CefViewCursorType type,
                                            const CefViewCursorInfo customCursorInfo);
  Type_CursorChanged pfnCursorChanged;

  //////////////////////////////////////////////////////////////////////////
  // LifespanHandler

  // typedef bool(STDCALL* Type_OnBeforePopup)(int64_t frameId,
  //                                            const char* targetUrl,
  //                                            const char* targetFrameName,
  //                                            CefWindowOpenDisposition targetDisposition,
  //                                            CCefSetting* settings,
  //                                            bool* DisableJavascriptAccess);
  // Type_OnBeforePopup pfnOnBeforePopup;

  typedef void(STDCALL* Type_OnAfterCreated)();
  Type_OnAfterCreated pfnOnAfterCreated;

  //////////////////////////////////////////////////////////////////////////
  // FocusHandler

  typedef void(STDCALL* Type_FocusReleasedByTabKey)(int browserId, bool next);
  Type_FocusReleasedByTabKey pfnFocusReleasedByTabKey;

  typedef bool(STDCALL* Type_SetFocus)(int browserId);
  Type_SetFocus pfnSetFocus;

  typedef void(STDCALL* Type_GotFocus)(int browserId);
  Type_GotFocus pfnGotFocus;

  //////////////////////////////////////////////////////////////////////////
  // RenderHandler
  typedef void(STDCALL* Type_GetRootScreenRect)(int browserId, CefViewRect* rect);
  Type_GetRootScreenRect pfnGetRootScreenRect;

  typedef void(STDCALL* Type_GetViewRect)(int browserId, CefViewRect* rect);
  Type_GetViewRect pfnGetViewRect;

  typedef bool(STDCALL* Type_GetScreenPoint)(int browserId, int viewX, int viewY, int* screenX, int* screenY);
  Type_GetScreenPoint pfnGetScreenPoint;

  typedef bool(STDCALL* Type_GetScreenInfo)(int browserId, CefViewScreenInfo* screenInfo);
  Type_GetScreenInfo pfnGetScreenInfo;

  typedef void(STDCALL* Type_OnPopupShow)(int browserId, bool show);
  Type_OnPopupShow pfnOnPopupShow;

  typedef void(STDCALL* Type_OnPopupSize)(int browserId, const CefViewRect rect);
  Type_OnPopupSize pfnOnPopupSize;

  typedef void(STDCALL* Type_OnPaint)(int browserId,
                                      CefViewPaintElementType type,
                                      const CefViewRect* dirtyRects,
                                      int dirtyRectCount,
                                      const void* imageBytes,
                                      int imageBytesCount,
                                      int width,
                                      int height);
  Type_OnPaint pfnOnPaint;

  typedef void(STDCALL* Type_OnAcceleratedPaint)(int browserId,
                                                 CefViewPaintElementType type,
                                                 const CefViewRect* dirtyRects,
                                                 int dirtyRectCount,
                                                 void* sharedHandle);

  typedef bool(
    STDCALL* Type_StartDragging)(int browserId, CefRefPtr<CefDragData> dragData, uint32_t allowedOps, int x, int y);
  typedef void(STDCALL* Type_UpdateDragCursor)(int browserId, CefViewDragOperation operation);
  typedef void(STDCALL* Type_OnScrollOffsetChanged)(int browserId, double x, double y);
  typedef void(STDCALL* Type_OnImeCompositionRangeChanged)(int browserId,
                                                           const CefViewRange selectedRange,
                                                           const CefViewRect* characterBounds,
                                                           int characterBoundsCount);
  Type_OnImeCompositionRangeChanged pfnOnImeCompositionRangeChanged;

  typedef void(STDCALL* Type_OnTextSelectionChanged)(int browserId,
                                                     const char* selectedText,
                                                     const CefViewRange selectedRange);
  typedef void(STDCALL* Type_OnVirtualKeyboardRequested)(int browserId, CefViewTextInputMode inputMode);
};

#endif
