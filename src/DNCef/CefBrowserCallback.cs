using System;
using System.Runtime.InteropServices;

namespace DNCef
{
    public partial struct CefBrowserCallback
    {
        //////////////////////////////////////////////////////////////////////////
        // CefView events

        // void (*)(int, long long, const CCefQuery *) pfnCefQueryRequest;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void CefQueryRequestCallback(int browserId, Int64 frameId, IntPtr query);
        public CefQueryRequestCallback cefQueryRequestCallback;

        // void (*)(int, long long, const char *, const char *) pfnInvokeMethod;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void InvokeMethodCallback(int browserId, Int64 frameId, string method, string arguments);
        public InvokeMethodCallback invokeMethodCallback;

        // void (*)(int, long long, long long, const char *) pfnReportJavascriptResult;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ReportJavascriptResultCallback(int browserId, Int64 frameId, Int64 context, string result);
        public ReportJavascriptResultCallback reportJavascriptResultCallback;

        // void (*)(int, long long, bool) pfnInputStateChanged;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void InputStateChangedCallback(int browserId, Int64 frameId, bool editable);
        public InputStateChangedCallback inputStateChangedCallback;

        //////////////////////////////////////////////////////////////////////////
        // LoadHandler

        // void (*)(int, bool, bool, bool) pfnLoadingStateChanged;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void LoadingStateChangedCallback(int browserId, bool isLoading, bool canGoBack, bool canGoForward);
        public LoadingStateChangedCallback loadingStateChangedCallback;

        // void (*)(int, long long, bool, int) pfnLoadStart;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void LoadStartCallback(int browserId, Int64 frameId, bool isMainFrame, int transition_type);
        public LoadStartCallback loadStartCallback;

        // void (*)(int, long long, bool, int) pfnLoadEnd;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void LoadEndCallback(int browserId, Int64 frameId, bool isMainFrame, int httpStatusCode);
        public LoadEndCallback loadEndCallback;

        // bool (*)(int, long long, bool, int, const char *, const char *) pfnLoadError;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool LoadErrorCallback(int browserId, Int64 frameId, bool isMainFrame, int errorCode, string errorMsg, string failedUrl);
        public LoadErrorCallback loadErrorCallback;

        //////////////////////////////////////////////////////////////////////////
        // DisplayHandler

        // void (*)(const _cef_draggable_region_t *, int) pfnDraggableRegionChanged;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void DraggableRegionChangedCallback(
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] CefViewDraggableRegion[] regions,
            int count);
        public DraggableRegionChangedCallback draggableRegionChangedCallback;

        // void (*)(long long, const char *) pfnAddressChanged;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void AddressChangedCallback(Int64 frameId, string url);
        public AddressChangedCallback addressChangedCallback;

        // void (*)(const char *) pfnTitleChanged;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void TitleChangedCallback(string title);
        public TitleChangedCallback titleChangedCallback;

        // void (*)(bool) pfnFullscreenModeChanged;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void FullScreenModeChangedCallback(bool fullscreen);
        public FullScreenModeChangedCallback fullScreenModeChangedCallback;

        // void (*)(const char *) pfnStatusMessage;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void StatusMessageCallback(string message);
        public StatusMessageCallback statusMessageCallback;

        // void (*)(const char *, int) pfnConsoleMessage;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ConsoleMessageCallback(string message, int level);
        public ConsoleMessageCallback consoleMessageCallback;

        // void (*)(double) pfnLoadingProgressChanged;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void LoadingProgressChangedCallback(double progress);
        public LoadingProgressChangedCallback loadingProgressChangedCallback;

        // bool (*)(void *, cef_cursor_type_t, _cef_cursor_info_t) pfnCursorChanged;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void CursorChangedCallback(IntPtr handle, CefViewCursorType type, CefViewCursorInfo info);
        public CursorChangedCallback cursorChangedCallback;

        //////////////////////////////////////////////////////////////////////////
        // LifespanHandler

        // void (*)() pfnOnAfterCreated;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void AfterCreatedCallback();
        public AfterCreatedCallback afterCreatedCallback;

        //////////////////////////////////////////////////////////////////////////
        // FocusHandler

        // void (*)(int, bool) pfnTakeFocus;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ReleaseFocusCallback(int browserId, bool next);
        public ReleaseFocusCallback releaseFocusCallback;

        // bool (*)(int) pfnSetFocus;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool SetFocusCallback(int browserId);
        public SetFocusCallback setFocusCallback;

        // void (*)(int) pfnGotFocus;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void GotFocusCallback(int browserId);
        public GotFocusCallback gotFocusCallback;

        //////////////////////////////////////////////////////////////////////////
        // RenderHandler

        // void (*)(int, _cef_rect_t *) pfnGetRootScreenRect;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void GetRootScreenRectCallback(int browserId, ref CefViewRect rect);
        public GetRootScreenRectCallback getRootScreenRectCallback;

        // void (*)(int, _cef_rect_t *) pfnGetViewRect;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void GetViewRectCallback(int browserId, ref CefViewRect rect);
        public GetViewRectCallback getViewRectCallback;

        // bool (*)(int, int, int, int *, int *) pfnGetScreenPoint;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetScreenPointCallback(int browserId, int viewX, int viewY, out int screenX, out int screenY);
        public GetScreenPointCallback getScreenPointCallback;

        // bool (*)(int, _cef_screen_info_t *) pfnGetScreenInfo;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool GetScreenInfoCallback(int browserId, ref CefViewScreenInfo info);
        public GetScreenInfoCallback getScreenInfoCallback;

        // void (*)(int, bool) pfnOnPopupShow;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnPopupShowCallback(int browserId, bool show);
        public OnPopupShowCallback onPopupShowCallback;

        // void (*)(int, _cef_rect_t) pfnOnPopupSize;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnPopupSizeCallback(int browserId, CefViewRect rect);
        public OnPopupSizeCallback onPopupSizeCallback;

        // void (*)(int, cef_paint_element_type_t, const _cef_rect_t *, int, const void *, int, int) pfnOnPaint;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnPaintCallback(
            int browserId,
            CefViewPaintElementType type,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] CefViewRect[] dirtyRects,
            int dirtyRectCount,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)] byte[] imageBytes,
            int imageBytesCount,
            int width,
            int height);
        public OnPaintCallback onPaintCallback;

        // void (*)(int, _cef_range_t, const _cef_rect_t *, int) pfnOnImeCompositionRangeChanged;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OnImeCompositionRangeChangedCallback(
            int browserId,
            CefViewRange range,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] CefViewRect[] characterBounds,
            int characterBoundsCount);
        public OnImeCompositionRangeChangedCallback onImeCompositionRangeChangedCallback;
    }
}
