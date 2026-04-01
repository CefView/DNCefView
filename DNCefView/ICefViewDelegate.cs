using System;

namespace DNCefView
{
    public interface ICefViewDelegate
    {
        #region CefView events
        void OnCefQueryRequest(int browserId, string frameId, CefQuery query);
        void OnCefInvokeMethod(int browserId, string frameId, string method, string arguments);
        void OnCefReportJavascriptResult(int browserId, string frameId, string context, string result);
        void OnCefInputStateChanged(int browserId, string frameId, bool editable);
        #endregion

        #region DisplayHandler
        void OnCefAddressChanged(int browserId, string frameId, string url);
        void OnCefTitleChanged(int browserId, string title);
        void OnCefFullScreenModeChanged(int browserId, bool fullscreen);
        void OnCefStatusMessage(int browserId, string message);
        void OnCefConsoleMessage(int browserId, string message, int level);
        void OnCefLoadingProgressChanged(int browserId, double progress);
        void OnCefFaviconUrlChanged(int browserId, string faviconUrl);
        void OnCefCursorChanged(int browserId, CefViewCursorType type, CefViewCursorInfo customCursorInfo);
        #endregion

        #region DragHandler
        void OnCefDraggableRegionChanged(CefViewDraggableRegion[] draggableRegion, int count);
        #endregion

        #region FocusHandler
        void OnCefFocusReleasedByTabKey(int browserId, bool next);
        bool OnCefRequestSetFocus(int browserId);
        void OnCefGotFocus(int browserId);
        #endregion

        #region JSDialogHandler
        bool OnCefJSDialog(int browserId, IntPtr dialogHandle, string originUrl, int dialogType, string messageText, string defaultPromptText, bool suppressMessage);
        #endregion

        #region LifespanHandler
        bool OnCefBeforeNewPopupCreate(string frameId,
                                       string targetUrl,
                                       string targetFrameName,
                                       CefViewWindowOpenDisposition targetDisposition,
                                       ref CefViewRect rect,
                                       IntPtr settings,
                                       ref bool disableJavascriptAccess);
        bool OnCefBeforeNewBrowserCreate(string frameId,
                                         string targetUrl,
                                         string targetFrameName,
                                         CefViewWindowOpenDisposition targetDisposition,
                                         CefViewRect rect,
                                         IntPtr settings);
        void OnCefAfterCreated();
        bool OnCefDoClose();
        bool OnCefRequestClose();
        void OnCefBeforeClose();
        #endregion

        #region LoadHandler
        void OnCefLoadingStateChanged(int browserId, bool isLoading, bool canGoBack, bool canGoForward);
        void OnCefLoadStart(int browserId, string frameId, bool isMainFrame, int transitionType);
        void OnCefLoadEnd(int browserId, string frameId, bool isMainFrame, int httpStatusCode);
        bool OnCefLoadError(int browserId, string frameId, bool isMainFrame, int errorCode, string errorMsg, string failedUrl);
        #endregion

        #region RenderHandler
        void OnCefGetRootScreenRect(int browserId, ref CefViewRect rect);
        void OnCefGetViewRect(int browserId, ref CefViewRect rect);
        bool OnCefGetScreenPoint(int browserId, int viewX, int viewY, ref int screenX, ref int screenY);
        bool OnCefGetScreenInfo(int browserId, ref CefViewScreenInfo info);
        void OnCefPopupShow(int browserId, bool show);
        void OnCefPopupSize(int browserId, CefViewRect rect);
        void OnCefPaint(int browserId, CefViewPaintElementType type, CefViewRect[] dirtyRects, int dirtyRectCount, IntPtr imageBytesBuffer, int imageBytesCount, int width, int height);
        void OnCefAcceleratedPaint(int browserId, CefViewPaintElementType type, CefViewRect[] dirtyRects, int dirtyRectCount, IntPtr sharedHandle, int planeBytesCount);
        bool OnCefStartDragging(int browserId, CefViewDragOperation allowedOps, int x, int y);
        void OnCefUpdateDragCursor(int browserId, CefViewDragOperation operation);
        void OnCefTextSelectionChanged(int browserId, string selectedText, CefViewRange selectedRange);
        void OnCefImeCompositionRangeChanged(int browserId, CefViewRange selectedRange, CefViewRect[] characterBounds, int characterBoundsCount);
        #endregion
    }
}
