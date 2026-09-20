// ABI 4 static thunk table: route registration + the Thunk_OnCef* entry
// points native code calls. IL2CPP cannot marshal instance-method
// delegates, so every pfn callback routes through ConcurrentDictionary via
// the CCefBrowser* host pointer. Registration happens between
// CCefBrowser_new0 and CCefBrowser_start (see CefBrowser.cs ctor).
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DNCefView
{
    public partial class CefBrowser
    {
        #region ABI 4 static thunks (IL2CPP cannot marshal instance-method delegates)
        // The native side passes the CCefBrowser pointer (host) back on every callback;
        // the route maps it to this instance and forwards to the instance method.
        private static readonly System.Collections.Concurrent.ConcurrentDictionary<IntPtr, CefBrowser> _route =
            new System.Collections.Concurrent.ConcurrentDictionary<IntPtr, CefBrowser>();
        internal static void RegisterRoute(IntPtr host, CefBrowser browser) => _route[host] = browser;
        internal static void UnregisterRoute(IntPtr host) => _route.TryRemove(host, out _);
        private static CefBrowser Route(IntPtr host) => _route.TryGetValue(host, out var b) ? b : null;

        private static void Thunk_OnCefQueryRequest(System.IntPtr host, int browserId, string frameId, IntPtr query)
        {
            Route(host)?.OnCefQueryRequest(browserId, frameId, query);
        }
        private static void Thunk_OnCefInvokeMethod(System.IntPtr host, int browserId, string frameId, string method, string arguments)
        {
            Route(host)?.OnCefInvokeMethod(browserId, frameId, method, arguments);
        }
        private static void Thunk_OnCefReportJavascriptResult(System.IntPtr host, int browserId, string frameId, string context, string result)
        {
            Route(host)?.OnCefReportJavascriptResult(browserId, frameId, context, result);
        }
        private static void Thunk_OnCefInputStateChanged(System.IntPtr host, int browserId, string frameId, bool editable)
        {
            Route(host)?.OnCefInputStateChanged(browserId, frameId, editable);
        }
        private static void Thunk_OnCefAddressChanged(System.IntPtr host, int browserId, string frameId, string url)
        {
            Route(host)?.OnCefAddressChanged(browserId, frameId, url);
        }
        private static void Thunk_OnCefTitleChanged(System.IntPtr host, int browserId, string title)
        {
            Route(host)?.OnCefTitleChanged(browserId, title);
        }
        private static void Thunk_OnCefFullScreenModeChanged(System.IntPtr host, int browserId, bool fullscreen)
        {
            Route(host)?.OnCefFullScreenModeChanged(browserId, fullscreen);
        }
        private static void Thunk_OnCefStatusMessage(System.IntPtr host, int browserId, string message)
        {
            Route(host)?.OnCefStatusMessage(browserId, message);
        }
        private static void Thunk_OnCefConsoleMessage(System.IntPtr host, int browserId, string message, int level)
        {
            Route(host)?.OnCefConsoleMessage(browserId, message, level);
        }
        private static void Thunk_OnCefLoadingProgressChanged(System.IntPtr host, int browserId, double progress)
        {
            Route(host)?.OnCefLoadingProgressChanged(browserId, progress);
        }
        private static void Thunk_OnCefFaviconUrlChanged(System.IntPtr host, int browserId, string faviconUrl)
        {
            Route(host)?.OnCefFaviconUrlChanged(browserId, faviconUrl);
        }
        private static bool Thunk_OnCefCursorChanged(System.IntPtr host, int browserId, IntPtr cursor, CefViewCursorType type, CefViewCursorInfo customCursorInfo)
        {
            var b = Route(host);
            return b != null ? b.OnCefCursorChanged(browserId, cursor, type, customCursorInfo) : false;
        }
        private static void Thunk_OnCefDraggableRegionChanged(System.IntPtr host, CefViewDraggableRegion[] draggableRegion, int count)
        {
            Route(host)?.OnCefDraggableRegionChanged(draggableRegion, count);
        }
        private static void Thunk_OnCefReleasedFocusByTabKey(System.IntPtr host, int browserId, bool next)
        {
            Route(host)?.OnCefReleasedFocusByTabKey(browserId, next);
        }
        private static bool Thunk_OnCefRequestSetFocus(System.IntPtr host, int browserId)
        {
            var b = Route(host);
            return b != null ? b.OnCefRequestSetFocus(browserId) : false;
        }
        private static void Thunk_OnCefGotFocus(System.IntPtr host, int browserId)
        {
            Route(host)?.OnCefGotFocus(browserId);
        }
        private static bool Thunk_OnCefJSDialog(System.IntPtr host, int browserId, long requestId, string originUrl, int dialogType, string messageText, string defaultPromptText, bool suppressMessage)
        {
            var b = Route(host);
            return b != null ? b.OnCefJSDialog(browserId, requestId, originUrl, dialogType, messageText, defaultPromptText, suppressMessage) : false;
        }
        private static bool Thunk_OnCefBeforeNewPopupCreate(System.IntPtr host, string frameId, string targetUrl, string targetFrameName, CefViewWindowOpenDisposition targetDisposition, ref CefViewRect rect, IntPtr settings, ref bool disableJavascriptAccess)
        {
            var b = Route(host);
            return b != null ? b.OnCefBeforeNewPopupCreate(frameId, targetUrl, targetFrameName, targetDisposition, ref rect, settings, ref disableJavascriptAccess) : false;
        }
        private static bool Thunk_OnCefBeforeNewBrowserCreate(System.IntPtr host, string frameId, string targetUrl, string targetFrameName, CefViewWindowOpenDisposition targetDisposition, CefViewRect rect, IntPtr settings)
        {
            var b = Route(host);
            return b != null ? b.OnCefBeforeNewBrowserCreate(frameId, targetUrl, targetFrameName, targetDisposition, rect, settings) : false;
        }
        private static bool Thunk_OnCefDoClose(System.IntPtr host)
        {
            var b = Route(host);
            return b != null ? b.OnCefDoClose() : false;
        }
        private static bool Thunk_OnCefRequestClose(System.IntPtr host)
        {
            var b = Route(host);
            return b != null ? b.OnCefRequestClose() : false;
        }
        private static void Thunk_OnCefAfterCreated(System.IntPtr host)
        {
            Route(host)?.OnCefAfterCreated();
        }
        private static void Thunk_OnCefBeforeClose(System.IntPtr host)
        {
            Route(host)?.OnCefBeforeClose();
        }
        private static void Thunk_OnCefLoadingStateChanged(System.IntPtr host, int browserId, bool isLoading, bool canGoBack, bool canGoForward)
        {
            Route(host)?.OnCefLoadingStateChanged(browserId, isLoading, canGoBack, canGoForward);
        }
        private static void Thunk_OnCefLoadStart(System.IntPtr host, int browserId, string frameId, bool isMainFrame, int transition_type)
        {
            Route(host)?.OnCefLoadStart(browserId, frameId, isMainFrame, transition_type);
        }
        private static void Thunk_OnCefLoadEnd(System.IntPtr host, int browserId, string frameId, bool isMainFrame, int httpStatusCode)
        {
            Route(host)?.OnCefLoadEnd(browserId, frameId, isMainFrame, httpStatusCode);
        }
        private static bool Thunk_OnCefLoadError(System.IntPtr host, int browserId, string frameId, bool isMainFrame, int errorCode, string errorMsg, string failedUrl)
        {
            var b = Route(host);
            return b != null ? b.OnCefLoadError(browserId, frameId, isMainFrame, errorCode, errorMsg, failedUrl) : false;
        }
        private static void Thunk_OnCefGetRootScreenRect(System.IntPtr host, int browserId, ref CefViewRect rect)
        {
            Route(host)?.OnCefGetRootScreenRect(browserId, ref rect);
        }
        private static void Thunk_OnCefGetViewRect(System.IntPtr host, int browserId, ref CefViewRect rect)
        {
            Route(host)?.OnCefGetViewRect(browserId, ref rect);
        }
        private static bool Thunk_OnCefGetScreenPoint(System.IntPtr host, int browserId, int viewX, int viewY, ref int screenX, ref int screenY)
        {
            var b = Route(host);
            return b != null ? b.OnCefGetScreenPoint(browserId, viewX, viewY, ref screenX, ref screenY) : false;
        }
        private static bool Thunk_OnCefGetScreenInfo(System.IntPtr host, int browserId, ref CefViewScreenInfo info)
        {
            var b = Route(host);
            return b != null ? b.OnCefGetScreenInfo(browserId, ref info) : false;
        }
        private static void Thunk_OnCefPopupShow(System.IntPtr host, int browserId, bool show)
        {
            Route(host)?.OnCefPopupShow(browserId, show);
        }
        private static void Thunk_OnCefPopupSize(System.IntPtr host, int browserId, CefViewRect rect)
        {
            Route(host)?.OnCefPopupSize(browserId, rect);
        }
        private static void Thunk_OnCefPaint(System.IntPtr host, int browserId, CefViewPaintElementType type, CefViewRect[] dirtyRects, int dirtyRectCount, IntPtr imageBytesBuffer, int imageBytesCount, int width, int height)
        {
            Route(host)?.OnCefPaint(browserId, type, dirtyRects, dirtyRectCount, imageBytesBuffer, imageBytesCount, width, height);
        }
        private static void Thunk_OnCefAcceleratedPaint(System.IntPtr host, int browserId, CefViewPaintElementType type, CefViewRect[] dirtyRects, int dirtyRectCount, IntPtr sharedHandle, int planeBytesCount)
        {
            Route(host)?.OnCefAcceleratedPaint(browserId, type, dirtyRects, dirtyRectCount, sharedHandle, planeBytesCount);
        }
        private static bool Thunk_OnCefBeforeUnloadDialog(System.IntPtr host, int browserId, long requestId, string messageText, bool isReload)
        {
            var b = Route(host);
            return b != null ? b.OnCefBeforeUnloadDialog(browserId, requestId, messageText, isReload) : false;
        }
        private static bool Thunk_OnCefFileDialog(System.IntPtr host, int browserId, long requestId, int mode, string title, string defaultFilePath, string filtersJson)
        {
            var b = Route(host);
            return b != null ? b.OnCefFileDialog(browserId, requestId, mode, title, defaultFilePath, filtersJson) : false;
        }
        private static bool Thunk_OnCefBeforeDownload(System.IntPtr host, int browserId, long downloadId, string url, string suggestedName, string mimeType, long totalBytes)
        {
            var b = Route(host);
            return b != null ? b.OnCefBeforeDownload(browserId, downloadId, url, suggestedName, mimeType, totalBytes) : false;
        }
        private static void Thunk_OnCefDownloadUpdated(System.IntPtr host, int browserId, long downloadId, int state, double percent, long speed, long receivedBytes, long totalBytes)
        {
            Route(host)?.OnCefDownloadUpdated(browserId, downloadId, state, percent, speed, receivedBytes, totalBytes);
        }
        private static void Thunk_OnCefFindResult(System.IntPtr host, int browserId, int identifier, int count, int activeMatchOrdinal, bool finalUpdate, CefViewRect selectionRect)
        {
            Route(host)?.OnCefFindResult(browserId, identifier, count, activeMatchOrdinal, finalUpdate, selectionRect);
        }
        private static bool Thunk_OnCefContextMenu(System.IntPtr host, int browserId, long requestId, string contextParamsJson, string menuJson)
        {
            var b = Route(host);
            return b != null ? b.OnCefContextMenu(browserId, requestId, contextParamsJson, menuJson) : false;
        }
        private static void Thunk_OnCefContextMenuDismissed(System.IntPtr host, int browserId)
        {
            Route(host)?.OnCefContextMenuDismissed(browserId);
        }
        private static bool Thunk_OnCefPermissionPrompt(System.IntPtr host, int browserId, ulong promptId, string requestingOrigin, uint requestedPermissions)
        {
            var b = Route(host);
            return b != null ? b.OnCefPermissionPrompt(browserId, promptId, requestingOrigin, requestedPermissions) : false;
        }
        private static void Thunk_OnCefRenderProcessTerminated(System.IntPtr host, int browserId, int status, int errorCode, string errorString)
        {
            Route(host)?.OnCefRenderProcessTerminated(browserId, status, errorCode, errorString);
        }
        private static bool Thunk_OnCefStartDragging(System.IntPtr host, int browserId, CefViewDragOperation allowedOps, int x, int y)
        {
            var b = Route(host);
            return b != null ? b.OnCefStartDragging(browserId, allowedOps, x, y) : false;
        }
        private static void Thunk_OnCefUpdateDragCursor(System.IntPtr host, int browserId, CefViewDragOperation operation)
        {
            Route(host)?.OnCefUpdateDragCursor(browserId, operation);
        }
        private static void Thunk_OnCefImeCompositionRangeChanged(System.IntPtr host, int browserId, CefViewRange selectedRange, CefViewRect[] characterBounds, int characterBoundsCount)
        {
            Route(host)?.OnCefImeCompositionRangeChanged(browserId, selectedRange, characterBounds, characterBoundsCount);
        }
        private static void Thunk_OnCefTextSelectionChanged(System.IntPtr host, int browserId, string selectedText, CefViewRange selectedRange)
        {
            Route(host)?.OnCefTextSelectionChanged(browserId, selectedText, selectedRange);
        }
        #endregion
    }
}
