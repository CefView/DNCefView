namespace DNCefView
{
    public interface ICefViewDelegate
    {
        void OnCefQueryRequest(int browserId, string frameId, CefQuery query);

        void OnCefInvokeMethod(int browserId, string frameId, string method, string arguments);

        void OnCefReportJavascriptResult(int browserId, string frameId, string context, string result);

        void OnCefInputStateChanged(int browserId, string frameId, bool editable);

        void OnCefLoadingStateChanged(int browserId, bool isLoading, bool canGoBack, bool canGoForward);

        void OnCefLoadStart(int browserId, string frameId, bool isMainFrame, int transition_type);

        void OnCefLoadEnd(int browserId, string frameId, bool isMainFrame, int httpStatusCode);

        bool OnCefLoadError(int browserId, string frameId, bool isMainFrame, int errorCode, string errorMsg, string failedUrl);

        void OnCefAddressChanged(string frameId, string url);

        void OnCefTitleChanged(string title);

        void OnCefFullScreenModeChanged(bool fullscreen);

        void OnCefStatusMessage(string message);

        void OnCefConsoleMessage(string message, int level);

        void OnCefLoadingProgressChanged(double progress);

        void OnCefAfterCreated();

        void OnCefFocusReleasedByTabKey(int browserId, bool next);

        bool OnCefSetFocus(int browserId);

        void OnCefGotFocus(int browserId);

        void OnCefGetRootScreenRect(int browserId, ref CefViewRect rect);

        void OnCefGetViewRect(int browserId, ref CefViewRect rect);

        bool OnCefGetScreenPoint(int browserId, int viewX, int viewY, out int screenX, out int screenY);

        bool OnCefGetScreenInfo(int browserId, ref CefViewScreenInfo info);

        void OnCefPopupShow(int browserId, bool show);

        void OnCefPopupSize(int browserId, CefViewRect rect);

        void OnCefPaint(int browserId, CefViewPaintElementType type, CefViewRect[] dirtyRects, int dirtyRectCount, byte[] imageBytes, int imageBytesCount, int width, int height);

        void OnCefImeCompositionRangeChanged(int browserId, CefViewRange range, CefViewRect[] characterBounds, int characterBoundsCount);
    }
}
