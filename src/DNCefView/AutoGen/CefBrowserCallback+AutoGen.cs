#pragma warning disable CS8603
using System;
using System.Runtime.InteropServices;

namespace DNCefView
{
    // Source: CefBrowserCallback
    [StructLayout(LayoutKind.Sequential)]
    public partial struct CefBrowserCallback
    {
        // Source: void pfnCefQueryRequest(int, const char *, const CCefQuery *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void CefQueryRequestCallback(System.IntPtr host, int browserId, [MarshalAs(UnmanagedType.LPUTF8Str)] string frameId, IntPtr query);
        public CefQueryRequestCallback CefQueryRequestCb;

        // Source: void pfnInvokeMethod(int, const char *, const char *, const char *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void InvokeMethodCallback(System.IntPtr host, int browserId, [MarshalAs(UnmanagedType.LPUTF8Str)] string frameId, [MarshalAs(UnmanagedType.LPUTF8Str)] string method, [MarshalAs(UnmanagedType.LPUTF8Str)] string arguments);
        public InvokeMethodCallback InvokeMethodCb;

        // Source: void pfnReportJavascriptResult(int, const char *, const char *, const char *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ReportJavascriptResultCallback(System.IntPtr host, int browserId, [MarshalAs(UnmanagedType.LPUTF8Str)] string frameId, [MarshalAs(UnmanagedType.LPUTF8Str)] string context, [MarshalAs(UnmanagedType.LPUTF8Str)] string result);
        public ReportJavascriptResultCallback ReportJavascriptResultCb;

        // Source: void pfnInputStateChanged(int, const char *, bool)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void InputStateChangedCallback(System.IntPtr host, int browserId, [MarshalAs(UnmanagedType.LPUTF8Str)] string frameId, [MarshalAs(UnmanagedType.I1)] bool editable);
        public InputStateChangedCallback InputStateChangedCb;

        // Source: bool pfnOnFileDialog(int, int64_t, int, const char *, const char *, const char *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I1)]
        public delegate bool OnFileDialogCallback(System.IntPtr host, int browserId, long requestId, int mode, [MarshalAs(UnmanagedType.LPUTF8Str)] string title, [MarshalAs(UnmanagedType.LPUTF8Str)] string defaultFilePath, [MarshalAs(UnmanagedType.LPUTF8Str)] string filtersJson);
        public OnFileDialogCallback OnFileDialogCb;

        // Source: void pfnAddressChanged(int, const char *, const char *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void AddressChangedCallback(System.IntPtr host, int browserId, [MarshalAs(UnmanagedType.LPUTF8Str)] string frameId, [MarshalAs(UnmanagedType.LPUTF8Str)] string url);
        public AddressChangedCallback AddressChangedCb;

        // Source: void pfnTitleChanged(int, const char *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void TitleChangedCallback(System.IntPtr host, int browserId, [MarshalAs(UnmanagedType.LPUTF8Str)] string title);
        public TitleChangedCallback TitleChangedCb;

        // Source: void pfnFullscreenModeChanged(int, bool)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void FullscreenModeChangedCallback(System.IntPtr host, int browserId, [MarshalAs(UnmanagedType.I1)] bool fullscreen);
        public FullscreenModeChangedCallback FullscreenModeChangedCb;

        // Source: void pfnStatusMessage(int, const char *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void StatusMessageCallback(System.IntPtr host, int browserId, [MarshalAs(UnmanagedType.LPUTF8Str)] string message);
        public StatusMessageCallback StatusMessageCb;

        // Source: void pfnConsoleMessage(int, const char *, int)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ConsoleMessageCallback(System.IntPtr host, int browserId, [MarshalAs(UnmanagedType.LPUTF8Str)] string message, int level);
        public ConsoleMessageCallback ConsoleMessageCb;

        // Source: void pfnLoadingProgressChanged(int, double)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void LoadingProgressChangedCallback(System.IntPtr host, int browserId, double progress);
        public LoadingProgressChangedCallback LoadingProgressChangedCb;

        // Source: void pfnFaviconUrlChanged(int, const char *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void FaviconUrlChangedCallback(System.IntPtr host, int browserId, [MarshalAs(UnmanagedType.LPUTF8Str)] string faviconUrl);
        public FaviconUrlChangedCallback FaviconUrlChangedCb;

        // Source: bool pfnCursorChanged(int, const void *, cef_cursor_type_t, _cef_cursor_info_t)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I1)]
        public delegate bool CursorChangedCallback(System.IntPtr host, int browserId, IntPtr cursorHandle, CefViewCursorType type, CefViewCursorInfo customCursorInfo);
        public CursorChangedCallback CursorChangedCb;

        // Source: bool pfnOnBeforeDownload(int, int64_t, const char *, const char *, const char *, int64_t)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I1)]
        public delegate bool OnBeforeDownloadCallback(System.IntPtr host, int browserId, long downloadId, [MarshalAs(UnmanagedType.LPUTF8Str)] string url, [MarshalAs(UnmanagedType.LPUTF8Str)] string suggestedName, [MarshalAs(UnmanagedType.LPUTF8Str)] string mimeType, long totalBytes);
        public OnBeforeDownloadCallback OnBeforeDownloadCb;

        // Source: void pfnOnDownloadUpdated(int, int64_t, int, double, int64_t, int64_t, int64_t)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnDownloadUpdatedCallback(System.IntPtr host, int browserId, long downloadId, int state, double percent, long speed, long receivedBytes, long totalBytes);
        public OnDownloadUpdatedCallback OnDownloadUpdatedCb;

        // Source: void pfnDraggableRegionChanged(const _cef_draggable_region_t *, int)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void DraggableRegionChangedCallback(System.IntPtr host, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] CefViewDraggableRegion[] draggableRegion, int count);
        public DraggableRegionChangedCallback DraggableRegionChangedCb;

        // Source: void pfnOnFindResult(int, int, int, int, bool, _cef_rect_t)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnFindResultCallback(System.IntPtr host, int browserId, int identifier, int count, int activeMatchOrdinal, [MarshalAs(UnmanagedType.I1)] bool finalUpdate, CefViewRect selectionRect);
        public OnFindResultCallback OnFindResultCb;

        // Source: void pfnOnFocusReleasedByTabKey(int, bool)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnFocusReleasedByTabKeyCallback(System.IntPtr host, int browserId, [MarshalAs(UnmanagedType.I1)] bool next);
        public OnFocusReleasedByTabKeyCallback OnFocusReleasedByTabKeyCb;

        // Source: bool pfnOnRequestSetFocus(int)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I1)]
        public delegate bool OnRequestSetFocusCallback(System.IntPtr host, int browserId);
        public OnRequestSetFocusCallback OnRequestSetFocusCb;

        // Source: void pfnOnGotFocus(int)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnGotFocusCallback(System.IntPtr host, int browserId);
        public OnGotFocusCallback OnGotFocusCb;

        // Source: bool pfnOnJSDialog(int, int64_t, const char *, int, const char *, const char *, bool)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I1)]
        public delegate bool OnJSDialogCallback(System.IntPtr host, int browserId, long requestId, [MarshalAs(UnmanagedType.LPUTF8Str)] string originUrl, int dialogType, [MarshalAs(UnmanagedType.LPUTF8Str)] string messageText, [MarshalAs(UnmanagedType.LPUTF8Str)] string defaultPromptText, [MarshalAs(UnmanagedType.I1)] bool suppressMessage);
        public OnJSDialogCallback OnJSDialogCb;

        // Source: bool pfnOnBeforeUnloadDialog(int, int64_t, const char *, bool)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I1)]
        public delegate bool OnBeforeUnloadDialogCallback(System.IntPtr host, int browserId, long requestId, [MarshalAs(UnmanagedType.LPUTF8Str)] string messageText, [MarshalAs(UnmanagedType.I1)] bool isReload);
        public OnBeforeUnloadDialogCallback OnBeforeUnloadDialogCb;

        // Source: bool pfnOnBeforeNewPopupCreate(const char *, const char *, const char *, cef_window_open_disposition_t, _cef_rect_t *, CCefSetting *, bool *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I1)]
        public delegate bool OnBeforeNewPopupCreateCallback(System.IntPtr host, [MarshalAs(UnmanagedType.LPUTF8Str)] string frameId, [MarshalAs(UnmanagedType.LPUTF8Str)] string targetUrl, [MarshalAs(UnmanagedType.LPUTF8Str)] string targetFrameName, CefViewWindowOpenDisposition targetDisposition, ref CefViewRect rect, IntPtr settings, [MarshalAs(UnmanagedType.I1)] ref bool DisableJavascriptAccess);
        public OnBeforeNewPopupCreateCallback OnBeforeNewPopupCreateCb;

        // Source: bool pfnOnBeforeNewBrowserCreate(const char *, const char *, const char *, cef_window_open_disposition_t, _cef_rect_t, const CCefSetting *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I1)]
        public delegate bool OnBeforeNewBrowserCreateCallback(System.IntPtr host, [MarshalAs(UnmanagedType.LPUTF8Str)] string frameId, [MarshalAs(UnmanagedType.LPUTF8Str)] string targetUrl, [MarshalAs(UnmanagedType.LPUTF8Str)] string targetFrameName, CefViewWindowOpenDisposition targetDisposition, CefViewRect rect, IntPtr settings);
        public OnBeforeNewBrowserCreateCallback OnBeforeNewBrowserCreateCb;

        // Source: bool pfnDoClose()
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I1)]
        public delegate bool DoCloseCallback(System.IntPtr host);
        public DoCloseCallback DoCloseCb;

        // Source: bool pfnRequestClose()
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I1)]
        public delegate bool RequestCloseCallback(System.IntPtr host);
        public RequestCloseCallback RequestCloseCb;

        // Source: void pfnOnAfterCreated()
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnAfterCreatedCallback(System.IntPtr host);
        public OnAfterCreatedCallback OnAfterCreatedCb;

        // Source: void pfnOnBeforeClose()
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnBeforeCloseCallback(System.IntPtr host);
        public OnBeforeCloseCallback OnBeforeCloseCb;

        // Source: void pfnLoadingStateChanged(int, bool, bool, bool)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void LoadingStateChangedCallback(System.IntPtr host, int browserId, [MarshalAs(UnmanagedType.I1)] bool isLoading, [MarshalAs(UnmanagedType.I1)] bool canGoBack, [MarshalAs(UnmanagedType.I1)] bool canGoForward);
        public LoadingStateChangedCallback LoadingStateChangedCb;

        // Source: void pfnLoadStart(int, const char *, bool, int)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void LoadStartCallback(System.IntPtr host, int browserId, [MarshalAs(UnmanagedType.LPUTF8Str)] string frameId, [MarshalAs(UnmanagedType.I1)] bool isMainFrame, int transition_type);
        public LoadStartCallback LoadStartCb;

        // Source: void pfnLoadEnd(int, const char *, bool, int)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void LoadEndCallback(System.IntPtr host, int browserId, [MarshalAs(UnmanagedType.LPUTF8Str)] string frameId, [MarshalAs(UnmanagedType.I1)] bool isMainFrame, int httpStatusCode);
        public LoadEndCallback LoadEndCb;

        // Source: bool pfnLoadError(int, const char *, bool, int, const char *, const char *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I1)]
        public delegate bool LoadErrorCallback(System.IntPtr host, int browserId, [MarshalAs(UnmanagedType.LPUTF8Str)] string frameId, [MarshalAs(UnmanagedType.I1)] bool isMainFrame, int errorCode, [MarshalAs(UnmanagedType.LPUTF8Str)] string errorMsg, [MarshalAs(UnmanagedType.LPUTF8Str)] string failedUrl);
        public LoadErrorCallback LoadErrorCb;

        // Source: bool pfnOnContextMenu(int, int64_t, const char *, const char *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I1)]
        public delegate bool OnContextMenuCallback(System.IntPtr host, int browserId, long requestId, [MarshalAs(UnmanagedType.LPUTF8Str)] string contextParamsJson, [MarshalAs(UnmanagedType.LPUTF8Str)] string menuJson);
        public OnContextMenuCallback OnContextMenuCb;

        // Source: void pfnOnContextMenuDismissed(int)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnContextMenuDismissedCallback(System.IntPtr host, int browserId);
        public OnContextMenuDismissedCallback OnContextMenuDismissedCb;

        // Source: bool pfnOnPermissionPrompt(int, uint64_t, const char *, unsigned int)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I1)]
        public delegate bool OnPermissionPromptCallback(System.IntPtr host, int browserId, ulong promptId, [MarshalAs(UnmanagedType.LPUTF8Str)] string requestingOrigin, uint requestedPermissions);
        public OnPermissionPromptCallback OnPermissionPromptCb;

        // Source: void pfnOnRenderProcessTerminated(int, int, int, const char *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnRenderProcessTerminatedCallback(System.IntPtr host, int browserId, int status, int errorCode, [MarshalAs(UnmanagedType.LPUTF8Str)] string errorString);
        public OnRenderProcessTerminatedCallback OnRenderProcessTerminatedCb;

        // Source: void pfnGetRootScreenRect(int, _cef_rect_t *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void GetRootScreenRectCallback(System.IntPtr host, int browserId, ref CefViewRect rect);
        public GetRootScreenRectCallback GetRootScreenRectCb;

        // Source: void pfnGetViewRect(int, _cef_rect_t *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void GetViewRectCallback(System.IntPtr host, int browserId, ref CefViewRect rect);
        public GetViewRectCallback GetViewRectCb;

        // Source: bool pfnGetScreenPoint(int, int, int, int *, int *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I1)]
        public delegate bool GetScreenPointCallback(System.IntPtr host, int browserId, int viewX, int viewY, ref int screenX, ref int screenY);
        public GetScreenPointCallback GetScreenPointCb;

        // Source: bool pfnGetScreenInfo(int, _cef_screen_info_t *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I1)]
        public delegate bool GetScreenInfoCallback(System.IntPtr host, int browserId, ref CefViewScreenInfo screenInfo);
        public GetScreenInfoCallback GetScreenInfoCb;

        // Source: void pfnOnPopupShow(int, bool)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnPopupShowCallback(System.IntPtr host, int browserId, [MarshalAs(UnmanagedType.I1)] bool show);
        public OnPopupShowCallback OnPopupShowCb;

        // Source: void pfnOnPopupSize(int, _cef_rect_t)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnPopupSizeCallback(System.IntPtr host, int browserId, CefViewRect rect);
        public OnPopupSizeCallback OnPopupSizeCb;

        // Source: void pfnOnPaint(int, cef_paint_element_type_t, const _cef_rect_t *, int, const void *, int, int, int)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnPaintCallback(System.IntPtr host, int browserId, CefViewPaintElementType type, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] CefViewRect[] dirtyRects, int dirtyRectCount, IntPtr imageBytesBuffer, int imageBytesCount, int width, int height);
        public OnPaintCallback OnPaintCb;

        // Source: void pfnOnAcceleratedPaint(int, cef_paint_element_type_t, const _cef_rect_t *, int, const void *, int)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnAcceleratedPaintCallback(System.IntPtr host, int browserId, CefViewPaintElementType type, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] CefViewRect[] dirtyRects, int dirtyRectCount, IntPtr sharedHandle, int planeBytesCount);
        public OnAcceleratedPaintCallback OnAcceleratedPaintCb;

        // Source: bool pfnStartDragging(int, cef_drag_operations_mask_t, int, int)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I1)]
        public delegate bool StartDraggingCallback(System.IntPtr host, int browserId, CefViewDragOperation allowedOps, int x, int y);
        public StartDraggingCallback StartDraggingCb;
        // Source: void pfnUpdateDragCursor(int, cef_drag_operations_mask_t)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void UpdateDragCursorCallback(System.IntPtr host, int browserId, CefViewDragOperation operation);
        public UpdateDragCursorCallback UpdateDragCursorCb;

        // Source: void pfnOnScrollOffsetChanged(int, double, double)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnScrollOffsetChangedCallback(System.IntPtr host, int browserId, double x, double y);
        public OnScrollOffsetChangedCallback OnScrollOffsetChangedCb;

        // Source: void pfnOnImeCompositionRangeChanged(int, _cef_range_t, const _cef_rect_t *, int)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnImeCompositionRangeChangedCallback(System.IntPtr host, int browserId, CefViewRange selectedRange, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] CefViewRect[] characterBounds, int characterBoundsCount);
        public OnImeCompositionRangeChangedCallback OnImeCompositionRangeChangedCb;

        // Source: void pfnOnTextSelectionChanged(int, const char *, _cef_range_t)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnTextSelectionChangedCallback(System.IntPtr host, int browserId, [MarshalAs(UnmanagedType.LPUTF8Str)] string selectedText, CefViewRange selectedRange);
        public OnTextSelectionChangedCallback OnTextSelectionChangedCb;

        // Source: void pfnOnVirtualKeyboardRequested(int, cef_text_input_mode_t)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnVirtualKeyboardRequestedCallback(System.IntPtr host, int browserId, CefViewTextInputMode inputMode);
        public OnVirtualKeyboardRequestedCallback OnVirtualKeyboardRequestedCb;

    }

}
