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
  void(STDCALL* pfnCefQueryRequest)(void* host, const int browserId, const char* frameId, const CCefQuery* query);

  void(STDCALL* pfnInvokeMethod)(void* host, const int browserId, const char* frameId, const char* method, const char* arguments);

  void(STDCALL* pfnReportJavascriptResult)(void* host, const int browserId,
                                           const char* frameId,
                                           const char* context,
                                           const char* result);

  void(STDCALL* pfnInputStateChanged)(void* host, const int browserId, const char* frameId, const bool editable);

  //////////////////////////////////////////////////////////////////////////
  // DialogHandler
  // The host returns true when it takes ownership of requestId; the native side
  // keeps the CefFileDialogCallback alive until continueFileDialog/cancelFileDialog
  // or browser teardown clears the reserve map.
  bool(STDCALL* pfnOnFileDialog)(void* host, const int browserId,
                                 const int64_t requestId,
                                 const int mode,
                                 const char* title,
                                 const char* defaultFilePath,
                                 const char* filtersJson); // { "filters": [...], "extensions": [...], "descriptions": [...] }

  //////////////////////////////////////////////////////////////////////////
  // DisplayHandler
  void(STDCALL* pfnAddressChanged)(void* host, const int browserId, const char* frameId, const char* url);

  void(STDCALL* pfnTitleChanged)(void* host, const int browserId, const char* title);

  void(STDCALL* pfnFullscreenModeChanged)(void* host, const int browserId, const bool fullscreen);

  void(STDCALL* pfnStatusMessage)(void* host, const int browserId, const char* message);

  void(STDCALL* pfnConsoleMessage)(void* host, const int browserId, const char* message, const int level);

  void(STDCALL* pfnLoadingProgressChanged)(void* host, const int browserId, double progress);

  void(STDCALL* pfnFaviconUrlChanged)(void* host, const int browserId, const char* faviconUrl);

  bool(STDCALL* pfnCursorChanged)(void* host, const int browserId,
                                  const void* cursorHandle,
                                  const CefViewCursorType type,
                                  const CefViewCursorInfo customCursorInfo);

  //////////////////////////////////////////////////////////////////////////
  // DownloadHandler
  // The host returns true when it takes ownership of requestId; the native side
  // keeps the CefBeforeDownloadCallback alive until continueDownload/cancelDownload.
  bool(STDCALL* pfnOnBeforeDownload)(void* host, const int browserId,
                                     const int64_t downloadId,
                                     const char* url,
                                     const char* suggestedName,
                                     const char* mimeType,
                                     const int64_t totalBytes);

  // state mirrors CefDownloadItem::DownloadState; the native side drops its
  // CefDownloadItemCallback map entry on COMPLETE/CANCELED/INTERRUPTED.
  void(STDCALL* pfnOnDownloadUpdated)(void* host, const int browserId,
                                      const int64_t downloadId,
                                      const int state,
                                      const double percent,
                                      const int64_t speed,
                                      const int64_t receivedBytes,
                                      const int64_t totalBytes);

  //////////////////////////////////////////////////////////////////////////
  // DragHandler
  void(STDCALL* pfnDraggableRegionChanged)(void* host, const CefViewDraggableRegion draggableRegion[], const int count);

  //////////////////////////////////////////////////////////////////////////
  // FindHandler
  void(STDCALL* pfnOnFindResult)(void* host, const int browserId,
                                 const int identifier,
                                 const int count,
                                 const int activeMatchOrdinal,
                                 const bool finalUpdate,
                                 const CefViewRect selectionRect);

  //////////////////////////////////////////////////////////////////////////
  // FocusHandler
  void(STDCALL* pfnOnFocusReleasedByTabKey)(void* host, const int browserId, const bool next);

  bool(STDCALL* pfnOnRequestSetFocus)(void* host, const int browserId);

  void(STDCALL* pfnOnGotFocus)(void* host, const int browserId);

  //////////////////////////////////////////////////////////////////////////
  // JSDialogHandler
  bool(STDCALL* pfnOnJSDialog)(void* host, const int browserId,
                               const int64_t requestId,
                               const char* originUrl,
                               const int dialogType,
                               const char* messageText,
                               const char* defaultPromptText,
                               const bool suppressMessage);

  // Before-unload confirmation; the answer goes through the same
  // continueJSDialog(requestId, success, userInput) channel as pfnOnJSDialog.
  bool(STDCALL* pfnOnBeforeUnloadDialog)(void* host, const int browserId,
                                         const int64_t requestId,
                                         const char* messageText,
                                         const bool isReload);

  //////////////////////////////////////////////////////////////////////////
  // TODO: KeyboardHandler

  //////////////////////////////////////////////////////////////////////////
  // LifespanHandler
  bool(STDCALL* pfnOnBeforeNewPopupCreate)(void* host, const char* frameId,
                                           const char* targetUrl,
                                           const char* targetFrameName,
                                           const CefViewWindowOpenDisposition targetDisposition,
                                           CefViewRect* rect,
                                           CCefSetting* settings,
                                           bool* DisableJavascriptAccess);

  bool(STDCALL* pfnOnBeforeNewBrowserCreate)(void* host, const char* frameId,
                                             const char* targetUrl,
                                             const char* targetFrameName,
                                             const CefViewWindowOpenDisposition targetDisposition,
                                             const CefViewRect rect,
                                             const CCefSetting* settings);

  bool(STDCALL* pfnDoClose)(void* host);

  bool(STDCALL* pfnRequestClose)(void* host);

  void(STDCALL* pfnOnAfterCreated)(void* host);

  void(STDCALL* pfnOnBeforeClose)(void* host);

  //////////////////////////////////////////////////////////////////////////
  // LoadHandler
  void(STDCALL* pfnLoadingStateChanged)(void* host, const int browserId,
                                        const bool isLoading,
                                        const bool canGoBack,
                                        const bool canGoForward);

  void(STDCALL* pfnLoadStart)(void* host, const int browserId,
                              const char* frameId,
                              const bool isMainFrame,
                              const int transition_type);

  void(STDCALL* pfnLoadEnd)(void* host, const int browserId, const char* frameId, const bool isMainFrame, const int httpStatusCode);

  bool(STDCALL* pfnLoadError)(void* host, const int browserId,
                              const char* frameId,
                              const bool isMainFrame,
                              const int errorCode,
                              const char* errorMsg,
                              const char* failedUrl);

  //////////////////////////////////////////////////////////////////////////
  // ContextMenuHandler
  // The host returns true when it takes ownership of requestId; the native side
  // keeps the CefRunContextMenuCallback alive until continueContextMenu(commandId,
  // eventFlags)/cancelContextMenu or onContextMenuDismissed clears the reserve map.
  bool(STDCALL* pfnOnContextMenu)(void* host, const int browserId,
                                  const int64_t requestId,
                                  const char* contextParamsJson,
                                  const char* menuJson);

  void(STDCALL* pfnOnContextMenuDismissed)(void* host, const int browserId);

  //////////////////////////////////////////////////////////////////////////
  // PermissionHandler (CEF 106+)
  // The host returns true when it takes ownership of promptId; the native side
  // keeps the CefPermissionPromptCallback alive until continuePermissionPrompt.
  bool(STDCALL* pfnOnPermissionPrompt)(void* host, const int browserId,
                                       const uint64_t promptId,
                                       const char* requestingOrigin,
                                       const unsigned int requestedPermissions);

  //////////////////////////////////////////////////////////////////////////
  // RequestHandler
  // Renderer process termination; the native side reloads the URL after forwarding.
  void(STDCALL* pfnOnRenderProcessTerminated)(void* host, const int browserId,
                                              const int status,
                                              const int errorCode,
                                              const char* errorString);

  //////////////////////////////////////////////////////////////////////////
  // RenderHandler
  void(STDCALL* pfnGetRootScreenRect)(void* host, const int browserId, CefViewRect* rect);

  void(STDCALL* pfnGetViewRect)(void* host, const int browserId, CefViewRect* rect);

  bool(STDCALL* pfnGetScreenPoint)(void* host, const int browserId, const int viewX, const int viewY, int* screenX, int* screenY);

  bool(STDCALL* pfnGetScreenInfo)(void* host, const int browserId, CefViewScreenInfo* screenInfo);

  void(STDCALL* pfnOnPopupShow)(void* host, const int browserId, const bool show);

  void(STDCALL* pfnOnPopupSize)(void* host, const int browserId, const CefViewRect rect);

  void(STDCALL* pfnOnPaint)(void* host, const int browserId,
                            const CefViewPaintElementType type,
                            const CefViewRect dirtyRects[],
                            const int dirtyRectCount,
                            const void* imageBytesBuffer,
                            const int imageBytesCount,
                            const int width,
                            const int height);

  void(STDCALL* pfnOnAcceleratedPaint)(void* host, const int browserId,
                                       const CefViewPaintElementType type,
                                       const CefViewRect dirtyRects[],
                                       const int dirtyRectCount,
                                       const void* sharedHandle,
                                       const int planeBytesCount);
  bool(STDCALL* pfnStartDragging)(void* host, const int browserId, const CefViewDragOperation allowedOps, const int x, const int y);

  void(STDCALL* pfnUpdateDragCursor)(void* host, const int browserId, const CefViewDragOperation operation);

  void(STDCALL* pfnOnScrollOffsetChanged)(void* host, const int browserId, const double x, const double y);

  void(STDCALL* pfnOnImeCompositionRangeChanged)(void* host, const int browserId,
                                                 const CefViewRange selectedRange,
                                                 const CefViewRect characterBounds[],
                                                 const int characterBoundsCount);

  void(STDCALL* pfnOnTextSelectionChanged)(void* host, const int browserId,
                                           const char* selectedText,
                                           const CefViewRange selectedRange);

  void(STDCALL* pfnOnVirtualKeyboardRequested)(void* host, const int browserId, const CefViewTextInputMode inputMode);
};

#endif
