using System;
using System.Runtime.InteropServices;

namespace DNCef
{
    [StructLayout(LayoutKind.Sequential)]
    public partial struct CefBrowserCallback
    {
        // void (*)(int, long long, const CCefQuery *) pfnCefQueryRequest;
        // void (*)(int, long long, const char *, const char *) pfnInvokeMethod;
        // void (*)(int, long long, long long, const char *) pfnReportJavascriptResult;
        // void (*)(int, long long, bool) pfnInputStateChanged;
        // void (*)(int, bool, bool, bool) pfnLoadingStateChanged;
        // void (*)(int, long long, bool, int) pfnLoadStart;
        // void (*)(int, long long, bool, int) pfnLoadEnd;
        // bool (*)(int, long long, bool, int, const char *, const char *) pfnLoadError;
        // void (*)(const _cef_draggable_region_t *, int) pfnDraggableRegionChanged;
        // void (*)(long long, const char *) pfnAddressChanged;
        // void (*)(const char *) pfnTitleChanged;
        // void (*)(bool) pfnFullscreenModeChanged;
        // void (*)(const char *) pfnStatusMessage;
        // void (*)(const char *, int) pfnConsoleMessage;
        // void (*)(double) pfnLoadingProgressChanged;
        // bool (*)(void *, cef_cursor_type_t, _cef_cursor_info_t) pfnCursorChanged;
        // void (*)() pfnOnAfterCreated;
        // void (*)(int, bool) pfnFocusReleasedByTabKey;
        // bool (*)(int) pfnSetFocus;
        // void (*)(int) pfnGotFocus;
        // void (*)(int, _cef_rect_t *) pfnGetRootScreenRect;
        // void (*)(int, _cef_rect_t *) pfnGetViewRect;
        // bool (*)(int, int, int, int *, int *) pfnGetScreenPoint;
        // bool (*)(int, _cef_screen_info_t *) pfnGetScreenInfo;
        // void (*)(int, bool) pfnOnPopupShow;
        // void (*)(int, _cef_rect_t) pfnOnPopupSize;
        // void (*)(int, cef_paint_element_type_t, const _cef_rect_t *, int, const void *, int, int, int) pfnOnPaint;
        // void (*)(int, _cef_range_t, const _cef_rect_t *, int) pfnOnImeCompositionRangeChanged;
    }

}