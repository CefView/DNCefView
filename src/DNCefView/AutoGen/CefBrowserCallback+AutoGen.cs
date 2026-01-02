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
        public delegate void CefQueryRequestCallback(int browserId, string frameId, IntPtr query);
        public CefQueryRequestCallback CefQueryRequestCb;

        // Source: void pfnInvokeMethod(int, const char *, const char *, const char *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void InvokeMethodCallback(int browserId, string frameId, string method, string arguments);
        public InvokeMethodCallback InvokeMethodCb;

        // Source: void pfnReportJavascriptResult(int, const char *, const char *, const char *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ReportJavascriptResultCallback(int browserId, string frameId, string context, string result);
        public ReportJavascriptResultCallback ReportJavascriptResultCb;

        // Source: void pfnInputStateChanged(int, const char *, bool)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void InputStateChangedCallback(int browserId, string frameId, bool editable);
        public InputStateChangedCallback InputStateChangedCb;

        // Source: void pfnLoadingStateChanged(int, bool, bool, bool)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void LoadingStateChangedCallback(int browserId, bool isLoading, bool canGoBack, bool canGoForward);
        public LoadingStateChangedCallback LoadingStateChangedCb;

        // Source: void pfnLoadStart(int, const char *, bool, int)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void LoadStartCallback(int browserId, string frameId, bool isMainFrame, int transition_type);
        public LoadStartCallback LoadStartCb;

        // Source: void pfnLoadEnd(int, const char *, bool, int)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void LoadEndCallback(int browserId, string frameId, bool isMainFrame, int httpStatusCode);
        public LoadEndCallback LoadEndCb;

        // Source: bool pfnLoadError(int, const char *, bool, int, const char *, const char *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool LoadErrorCallback(int browserId, string frameId, bool isMainFrame, int errorCode, string errorMsg, string failedUrl);
        public LoadErrorCallback LoadErrorCb;

        // Source: void pfnDraggableRegionChanged(const _cef_draggable_region_t *, int)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void DraggableRegionChangedCallback([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] CefViewDraggableRegion[] draggableRegion, int count);
        public DraggableRegionChangedCallback DraggableRegionChangedCb;

        // Source: void pfnAddressChanged(const char *, const char *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void AddressChangedCallback(string frameId, string url);
        public AddressChangedCallback AddressChangedCb;

        // Source: void pfnTitleChanged(const char *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void TitleChangedCallback(string title);
        public TitleChangedCallback TitleChangedCb;

        // Source: void pfnFullscreenModeChanged(bool)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void FullscreenModeChangedCallback(bool fullscreen);
        public FullscreenModeChangedCallback FullscreenModeChangedCb;

        // Source: void pfnStatusMessage(const char *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void StatusMessageCallback(string message);
        public StatusMessageCallback StatusMessageCb;

        // Source: void pfnConsoleMessage(const char *, int)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ConsoleMessageCallback(string message, int level);
        public ConsoleMessageCallback ConsoleMessageCb;

        // Source: void pfnLoadingProgressChanged(double)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void LoadingProgressChangedCallback(double progress);
        public LoadingProgressChangedCallback LoadingProgressChangedCb;

        // Source: bool pfnCursorChanged(const void *, cef_cursor_type_t, _cef_cursor_info_t)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool CursorChangedCallback(IntPtr cursor, CefViewCursorType type, CefViewCursorInfo customCursorInfo);
        public CursorChangedCallback CursorChangedCb;

        // Source: void pfnOnAfterCreated()
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnAfterCreatedCallback();
        public OnAfterCreatedCallback OnAfterCreatedCb;

        // Source: void pfnFocusReleasedByTabKey(int, bool)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void FocusReleasedByTabKeyCallback(int browserId, bool next);
        public FocusReleasedByTabKeyCallback FocusReleasedByTabKeyCb;

        // Source: bool pfnSetFocus(int)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetFocusCallback(int browserId);
        public SetFocusCallback SetFocusCb;

        // Source: void pfnGotFocus(int)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void GotFocusCallback(int browserId);
        public GotFocusCallback GotFocusCb;

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
        public delegate bool GetScreenPointCallback(int browserId, int viewX, int viewY, ref int screenX, ref int screenY);
        public GetScreenPointCallback GetScreenPointCb;

        // Source: bool pfnGetScreenInfo(int, _cef_screen_info_t *)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetScreenInfoCallback(int browserId, ref CefViewScreenInfo screenInfo);
        public GetScreenInfoCallback GetScreenInfoCb;

        // Source: void pfnOnPopupShow(int, bool)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnPopupShowCallback(int browserId, bool show);
        public OnPopupShowCallback OnPopupShowCb;

        // Source: void pfnOnPopupSize(int, _cef_rect_t)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnPopupSizeCallback(int browserId, CefViewRect rect);
        public OnPopupSizeCallback OnPopupSizeCb;

        // Source: void pfnOnPaint(int, cef_paint_element_type_t, const _cef_rect_t *, int, const unsigned char *, int, int, int)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnPaintCallback(int browserId, CefViewPaintElementType type, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] CefViewRect[] dirtyRects, int dirtyRectCount, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)] byte[] imageBytes, int imageBytesCount, int width, int height);
        public OnPaintCallback OnPaintCb;

        // Source: void pfnOnAcceleratedPaint(int, cef_paint_element_type_t, const _cef_rect_t *, int, const void *, int)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnAcceleratedPaintCallback(int browserId, CefViewPaintElementType type, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] CefViewRect[] dirtyRects, int dirtyRectCount, IntPtr sharedHandle, int planeBytesCount);
        public OnAcceleratedPaintCallback OnAcceleratedPaintCb;

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
        public delegate void OnTextSelectionChangedCallback(int browserId, string selectedText, CefViewRange selectedRange);
        public OnTextSelectionChangedCallback OnTextSelectionChangedCb;

        // Source: void pfnOnVirtualKeyboardRequested(int, cef_text_input_mode_t)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnVirtualKeyboardRequestedCallback(int browserId, CefViewTextInputMode inputMode);
        public OnVirtualKeyboardRequestedCallback OnVirtualKeyboardRequestedCb;

    }

}