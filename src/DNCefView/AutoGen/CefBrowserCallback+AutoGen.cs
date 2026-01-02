using System;
using System.Runtime.InteropServices;

namespace DNCefView
{
    [StructLayout(LayoutKind.Sequential)]
    public partial struct CefBrowserCallback
    {
        // void (*)(int, const char *, const CCefQuery *) pfnCefQueryRequest;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void CefQueryRequestCallback(int browserId, string frameId, IntPtr query);
        public CefQueryRequestCallback CefQueryRequestCb;

        // void (*)(int, const char *, const char *, const char *) pfnInvokeMethod;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void InvokeMethodCallback(int browserId, string frameId, string method, string arguments);
        public InvokeMethodCallback InvokeMethodCb;

        // void (*)(int, const char *, const char *, const char *) pfnReportJavascriptResult;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ReportJavascriptResultCallback(int browserId, string frameId, string context, string result);
        public ReportJavascriptResultCallback ReportJavascriptResultCb;

        // void (*)(int, const char *, bool) pfnInputStateChanged;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void InputStateChangedCallback(int browserId, string frameId, bool editable);
        public InputStateChangedCallback InputStateChangedCb;

        // void (*)(int, bool, bool, bool) pfnLoadingStateChanged;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void LoadingStateChangedCallback(int browserId, bool isLoading, bool canGoBack, bool canGoForward);
        public LoadingStateChangedCallback LoadingStateChangedCb;

        // void (*)(int, const char *, bool, int) pfnLoadStart;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void LoadStartCallback(int browserId, string frameId, bool isMainFrame, int transition_type);
        public LoadStartCallback LoadStartCb;

        // void (*)(int, const char *, bool, int) pfnLoadEnd;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void LoadEndCallback(int browserId, string frameId, bool isMainFrame, int httpStatusCode);
        public LoadEndCallback LoadEndCb;

        // bool (*)(int, const char *, bool, int, const char *, const char *) pfnLoadError;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool LoadErrorCallback(int browserId, string frameId, bool isMainFrame, int errorCode, string errorMsg, string failedUrl);
        public LoadErrorCallback LoadErrorCb;

        // void (*)(const _cef_draggable_region_t *, int) pfnDraggableRegionChanged;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void DraggableRegionChangedCallback(CefViewDraggableRegion draggableRegion, int count);
        public DraggableRegionChangedCallback DraggableRegionChangedCb;

        // void (*)(const char *, const char *) pfnAddressChanged;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void AddressChangedCallback(string frameId, string url);
        public AddressChangedCallback AddressChangedCb;

        // void (*)(const char *) pfnTitleChanged;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void TitleChangedCallback(string title);
        public TitleChangedCallback TitleChangedCb;

        // void (*)(bool) pfnFullscreenModeChanged;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void FullscreenModeChangedCallback(bool fullscreen);
        public FullscreenModeChangedCallback FullscreenModeChangedCb;

        // void (*)(const char *) pfnStatusMessage;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void StatusMessageCallback(string message);
        public StatusMessageCallback StatusMessageCb;

        // void (*)(const char *, int) pfnConsoleMessage;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ConsoleMessageCallback(string message, int level);
        public ConsoleMessageCallback ConsoleMessageCb;

        // void (*)(double) pfnLoadingProgressChanged;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void LoadingProgressChangedCallback(double progress);
        public LoadingProgressChangedCallback LoadingProgressChangedCb;

        // bool (*)(void *, cef_cursor_type_t, _cef_cursor_info_t) pfnCursorChanged;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool CursorChangedCallback(IntPtr cursor, CefViewCursorType type, CefViewCursorInfo customCursorInfo);
        public CursorChangedCallback CursorChangedCb;

        // void (*)() pfnOnAfterCreated;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnAfterCreatedCallback();
        public OnAfterCreatedCallback OnAfterCreatedCb;

        // void (*)(int, bool) pfnFocusReleasedByTabKey;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void FocusReleasedByTabKeyCallback(int browserId, bool next);
        public FocusReleasedByTabKeyCallback FocusReleasedByTabKeyCb;

        // bool (*)(int) pfnSetFocus;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetFocusCallback(int browserId);
        public SetFocusCallback SetFocusCb;

        // void (*)(int) pfnGotFocus;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void GotFocusCallback(int browserId);
        public GotFocusCallback GotFocusCb;

        // void (*)(int, _cef_rect_t *) pfnGetRootScreenRect;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void GetRootScreenRectCallback(int browserId, ref CefViewRect rect);
        public GetRootScreenRectCallback GetRootScreenRectCb;

        // void (*)(int, _cef_rect_t *) pfnGetViewRect;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void GetViewRectCallback(int browserId, ref CefViewRect rect);
        public GetViewRectCallback GetViewRectCb;

        // bool (*)(int, int, int, int *, int *) pfnGetScreenPoint;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetScreenPointCallback(int browserId, int viewX, int viewY, out int screenX, out int screenY);
        public GetScreenPointCallback GetScreenPointCb;

        // bool (*)(int, _cef_screen_info_t *) pfnGetScreenInfo;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetScreenInfoCallback(int browserId, ref CefViewScreenInfo screenInfo);
        public GetScreenInfoCallback GetScreenInfoCb;

        // void (*)(int, bool) pfnOnPopupShow;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnPopupShowCallback(int browserId, bool show);
        public OnPopupShowCallback OnPopupShowCb;

        // void (*)(int, _cef_rect_t) pfnOnPopupSize;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnPopupSizeCallback(int browserId, CefViewRect rect);
        public OnPopupSizeCallback OnPopupSizeCb;

        // void (*)(int, cef_paint_element_type_t, const _cef_rect_t *, int, const void *, int, int, int) pfnOnPaint;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnPaintCallback(int browserId,
            CefViewPaintElementType type,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] CefViewRect[] dirtyRects,
            int dirtyRectCount,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)] byte[] imageBytes,
            int imageBytesCount,
            int width,
            int height);
        public OnPaintCallback OnPaintCb;

        // void (*)(int, cef_paint_element_type_t, const _cef_rect_t *, int, void *, int) pfnOnAcceleratedPaint;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnAcceleratedPaintCallback(int browserId, CefViewPaintElementType type, CefViewRect dirtyRects, int dirtyRectCount, IntPtr sharedHandle, int planeBytesCount);
        public OnAcceleratedPaintCallback OnAcceleratedPaintCb;

        // void (*)(int, cef_drag_operations_mask_t) pfnUpdateDragCursor;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void UpdateDragCursorCallback(int browserId, CefViewDragOperation operation);
        public UpdateDragCursorCallback UpdateDragCursorCb;

        // void (*)(int, double, double) pfnOnScrollOffsetChanged;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnScrollOffsetChangedCallback(int browserId, double x, double y);
        public OnScrollOffsetChangedCallback OnScrollOffsetChangedCb;

        // void (*)(int, _cef_range_t, const _cef_rect_t *, int) pfnOnImeCompositionRangeChanged;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnImeCompositionRangeChangedCallback(int browserId,
            CefViewRange range,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] CefViewRect[] characterBounds,
            int characterBoundsCount);
        public OnImeCompositionRangeChangedCallback OnImeCompositionRangeChangedCb;

        // void (*)(int, const char *, _cef_range_t) pfnOnTextSelectionChanged;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnTextSelectionChangedCallback(int browserId, string selectedText, CefViewRange selectedRange);
        public OnTextSelectionChangedCallback OnTextSelectionChangedCb;

        // void (*)(int, cef_text_input_mode_t) pfnOnVirtualKeyboardRequested;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnVirtualKeyboardRequestedCallback(int browserId, CefViewTextInputMode inputMode);
        public OnVirtualKeyboardRequestedCallback OnVirtualKeyboardRequestedCb;

    }

}