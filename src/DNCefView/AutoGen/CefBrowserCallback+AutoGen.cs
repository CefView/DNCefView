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
        public delegate void CefQueryRequestCallback(int browserId, [MarshalAs(UnmanagedType.LPUTF8Str)] string frameId, IntPtr query);
        public CefQueryRequestCallback CefQueryRequestCb;

        // Source: void pfnInvokeMethod(int, const char *, const char *, const char *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void InvokeMethodCallback(int browserId, [MarshalAs(UnmanagedType.LPUTF8Str)] string frameId, [MarshalAs(UnmanagedType.LPUTF8Str)] string method, [MarshalAs(UnmanagedType.LPUTF8Str)] string arguments);
        public InvokeMethodCallback InvokeMethodCb;

        // Source: void pfnReportJavascriptResult(int, const char *, const char *, const char *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ReportJavascriptResultCallback(int browserId, [MarshalAs(UnmanagedType.LPUTF8Str)] string frameId, [MarshalAs(UnmanagedType.LPUTF8Str)] string context, [MarshalAs(UnmanagedType.LPUTF8Str)] string result);
        public ReportJavascriptResultCallback ReportJavascriptResultCb;

        // Source: void pfnInputStateChanged(int, const char *, bool)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void InputStateChangedCallback(int browserId, [MarshalAs(UnmanagedType.LPUTF8Str)] string frameId, [MarshalAs(UnmanagedType.I1)] bool editable);
        public InputStateChangedCallback InputStateChangedCb;

        // Source: void pfnAddressChanged(int, const char *, const char *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void AddressChangedCallback(int browserId, [MarshalAs(UnmanagedType.LPUTF8Str)] string frameId, [MarshalAs(UnmanagedType.LPUTF8Str)] string url);
        public AddressChangedCallback AddressChangedCb;

        // Source: void pfnTitleChanged(int, const char *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void TitleChangedCallback(int browserId, [MarshalAs(UnmanagedType.LPUTF8Str)] string title);
        public TitleChangedCallback TitleChangedCb;

        // Source: void pfnFullscreenModeChanged(int, bool)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void FullscreenModeChangedCallback(int browserId, [MarshalAs(UnmanagedType.I1)] bool fullscreen);
        public FullscreenModeChangedCallback FullscreenModeChangedCb;

        // Source: void pfnStatusMessage(int, const char *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void StatusMessageCallback(int browserId, [MarshalAs(UnmanagedType.LPUTF8Str)] string message);
        public StatusMessageCallback StatusMessageCb;

        // Source: void pfnConsoleMessage(int, const char *, int)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ConsoleMessageCallback(int browserId, [MarshalAs(UnmanagedType.LPUTF8Str)] string message, int level);
        public ConsoleMessageCallback ConsoleMessageCb;

        // Source: void pfnLoadingProgressChanged(int, double)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void LoadingProgressChangedCallback(int browserId, double progress);
        public LoadingProgressChangedCallback LoadingProgressChangedCb;

        // Source: void pfnFaviconUrlChanged(int, const char *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void FaviconUrlChangedCallback(int browserId, [MarshalAs(UnmanagedType.LPUTF8Str)] string faviconUrl);
        public FaviconUrlChangedCallback FaviconUrlChangedCb;

        // Source: bool pfnCursorChanged(int, const void *, cef_cursor_type_t, _cef_cursor_info_t)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I1)]
        public delegate bool CursorChangedCallback(int browserId, IntPtr cursorHandle, CefViewCursorType type, CefViewCursorInfo customCursorInfo);
        public CursorChangedCallback CursorChangedCb;

        // Source: void pfnDraggableRegionChanged(const _cef_draggable_region_t *, int)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void DraggableRegionChangedCallback([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] CefViewDraggableRegion[] draggableRegion, int count);
        public DraggableRegionChangedCallback DraggableRegionChangedCb;

        // Source: void pfnOnFocusReleasedByTabKey(int, bool)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnFocusReleasedByTabKeyCallback(int browserId, [MarshalAs(UnmanagedType.I1)] bool next);
        public OnFocusReleasedByTabKeyCallback OnFocusReleasedByTabKeyCb;

        // Source: bool pfnOnRequestSetFocus(int)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I1)]
        public delegate bool OnRequestSetFocusCallback(int browserId);
        public OnRequestSetFocusCallback OnRequestSetFocusCb;

        // Source: void pfnOnGotFocus(int)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnGotFocusCallback(int browserId);
        public OnGotFocusCallback OnGotFocusCb;

        // Source: bool pfnOnJSDialog(int, int64_t, const char *, int, const char *, const char *, bool)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I1)]
        public delegate bool OnJSDialogCallback(int browserId, long requestId, [MarshalAs(UnmanagedType.LPUTF8Str)] string originUrl, int dialogType, [MarshalAs(UnmanagedType.LPUTF8Str)] string messageText, [MarshalAs(UnmanagedType.LPUTF8Str)] string defaultPromptText, [MarshalAs(UnmanagedType.I1)] bool suppressMessage);
        public OnJSDialogCallback OnJSDialogCb;

        // Source: bool pfnOnBeforeNewPopupCreate(const char *, const char *, const char *, cef_window_open_disposition_t, _cef_rect_t *, CCefSetting *, bool *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I1)]
        public delegate bool OnBeforeNewPopupCreateCallback([MarshalAs(UnmanagedType.LPUTF8Str)] string frameId, [MarshalAs(UnmanagedType.LPUTF8Str)] string targetUrl, [MarshalAs(UnmanagedType.LPUTF8Str)] string targetFrameName, CefViewWindowOpenDisposition targetDisposition, ref CefViewRect rect, IntPtr settings, [MarshalAs(UnmanagedType.I1)] ref bool DisableJavascriptAccess);
        public OnBeforeNewPopupCreateCallback OnBeforeNewPopupCreateCb;

        // Source: bool pfnOnBeforeNewBrowserCreate(const char *, const char *, const char *, cef_window_open_disposition_t, _cef_rect_t, const CCefSetting *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I1)]
        public delegate bool OnBeforeNewBrowserCreateCallback([MarshalAs(UnmanagedType.LPUTF8Str)] string frameId, [MarshalAs(UnmanagedType.LPUTF8Str)] string targetUrl, [MarshalAs(UnmanagedType.LPUTF8Str)] string targetFrameName, CefViewWindowOpenDisposition targetDisposition, CefViewRect rect, IntPtr settings);
        public OnBeforeNewBrowserCreateCallback OnBeforeNewBrowserCreateCb;

        // Source: bool pfnDoClose()
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I1)]
        public delegate bool DoCloseCallback();
        public DoCloseCallback DoCloseCb;

        // Source: bool pfnRequestClose()
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I1)]
        public delegate bool RequestCloseCallback();
        public RequestCloseCallback RequestCloseCb;

        // Source: void pfnOnAfterCreated()
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnAfterCreatedCallback();
        public OnAfterCreatedCallback OnAfterCreatedCb;

        // Source: void pfnOnBeforeClose()
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnBeforeCloseCallback();
        public OnBeforeCloseCallback OnBeforeCloseCb;

        // Source: void pfnLoadingStateChanged(int, bool, bool, bool)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void LoadingStateChangedCallback(int browserId, [MarshalAs(UnmanagedType.I1)] bool isLoading, [MarshalAs(UnmanagedType.I1)] bool canGoBack, [MarshalAs(UnmanagedType.I1)] bool canGoForward);
        public LoadingStateChangedCallback LoadingStateChangedCb;

        // Source: void pfnLoadStart(int, const char *, bool, int)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void LoadStartCallback(int browserId, [MarshalAs(UnmanagedType.LPUTF8Str)] string frameId, [MarshalAs(UnmanagedType.I1)] bool isMainFrame, int transition_type);
        public LoadStartCallback LoadStartCb;

        // Source: void pfnLoadEnd(int, const char *, bool, int)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void LoadEndCallback(int browserId, [MarshalAs(UnmanagedType.LPUTF8Str)] string frameId, [MarshalAs(UnmanagedType.I1)] bool isMainFrame, int httpStatusCode);
        public LoadEndCallback LoadEndCb;

        // Source: bool pfnLoadError(int, const char *, bool, int, const char *, const char *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I1)]
        public delegate bool LoadErrorCallback(int browserId, [MarshalAs(UnmanagedType.LPUTF8Str)] string frameId, [MarshalAs(UnmanagedType.I1)] bool isMainFrame, int errorCode, [MarshalAs(UnmanagedType.LPUTF8Str)] string errorMsg, [MarshalAs(UnmanagedType.LPUTF8Str)] string failedUrl);
        public LoadErrorCallback LoadErrorCb;

        // Source: void pfnGetRootScreenRect(int, _cef_rect_t *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void GetRootScreenRectCallback(int browserId, ref CefViewRect rect);
        public GetRootScreenRectCallback GetRootScreenRectCb;

        // Source: void pfnGetViewRect(int, _cef_rect_t *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void GetViewRectCallback(int browserId, ref CefViewRect rect);
        public GetViewRectCallback GetViewRectCb;

        // Source: bool pfnGetScreenPoint(int, int, int, int *, int *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I1)]
        public delegate bool GetScreenPointCallback(int browserId, int viewX, int viewY, ref int screenX, ref int screenY);
        public GetScreenPointCallback GetScreenPointCb;

        // Source: bool pfnGetScreenInfo(int, _cef_screen_info_t *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I1)]
        public delegate bool GetScreenInfoCallback(int browserId, ref CefViewScreenInfo screenInfo);
        public GetScreenInfoCallback GetScreenInfoCb;

        // Source: void pfnOnPopupShow(int, bool)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnPopupShowCallback(int browserId, [MarshalAs(UnmanagedType.I1)] bool show);
        public OnPopupShowCallback OnPopupShowCb;

        // Source: void pfnOnPopupSize(int, _cef_rect_t)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnPopupSizeCallback(int browserId, CefViewRect rect);
        public OnPopupSizeCallback OnPopupSizeCb;

        // Source: void pfnOnPaint(int, cef_paint_element_type_t, const _cef_rect_t *, int, const void *, int, int, int)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnPaintCallback(int browserId, CefViewPaintElementType type, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] CefViewRect[] dirtyRects, int dirtyRectCount, IntPtr imageBytesBuffer, int imageBytesCount, int width, int height);
        public OnPaintCallback OnPaintCb;

        // Source: void pfnOnAcceleratedPaint(int, cef_paint_element_type_t, const _cef_rect_t *, int, const void *, int)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnAcceleratedPaintCallback(int browserId, CefViewPaintElementType type, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] CefViewRect[] dirtyRects, int dirtyRectCount, IntPtr sharedHandle, int planeBytesCount);
        public OnAcceleratedPaintCallback OnAcceleratedPaintCb;

        // Source: bool pfnStartDragging(int, cef_drag_operations_mask_t, int, int)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I1)]
        public delegate bool StartDraggingCallback(int browserId, CefViewDragOperation allowedOps, int x, int y);
        public StartDraggingCallback StartDraggingCb;
        // Source: void pfnUpdateDragCursor(int, cef_drag_operations_mask_t)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void UpdateDragCursorCallback(int browserId, CefViewDragOperation operation);
        public UpdateDragCursorCallback UpdateDragCursorCb;

        // Source: void pfnOnScrollOffsetChanged(int, double, double)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnScrollOffsetChangedCallback(int browserId, double x, double y);
        public OnScrollOffsetChangedCallback OnScrollOffsetChangedCb;

        // Source: void pfnOnImeCompositionRangeChanged(int, _cef_range_t, const _cef_rect_t *, int)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnImeCompositionRangeChangedCallback(int browserId, CefViewRange selectedRange, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] CefViewRect[] characterBounds, int characterBoundsCount);
        public OnImeCompositionRangeChangedCallback OnImeCompositionRangeChangedCb;

        // Source: void pfnOnTextSelectionChanged(int, const char *, _cef_range_t)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnTextSelectionChangedCallback(int browserId, [MarshalAs(UnmanagedType.LPUTF8Str)] string selectedText, CefViewRange selectedRange);
        public OnTextSelectionChangedCallback OnTextSelectionChangedCb;

        // Source: void pfnOnVirtualKeyboardRequested(int, cef_text_input_mode_t)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnVirtualKeyboardRequestedCallback(int browserId, CefViewTextInputMode inputMode);
        public OnVirtualKeyboardRequestedCallback OnVirtualKeyboardRequestedCb;

    }

}
