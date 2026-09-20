// CEF callback surface of CefBrowser: the ABI 4 static thunks (route
// table + Thunk_OnCef*) and the instance OnCef* methods they forward to.
// Split out of CefBrowser.cs (1014 lines); the class is partial, ctor and
// lifecycle stay in CefBrowser.cs / CefBrowser.Lifetime.cs.
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
        #region CEF Callbacks
        #region CefView events
        public void OnCefQueryRequest(int browserId, string frameId, IntPtr query)
        {
            var ownedQuery = new CefQuery(query);
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    del.OnCefQueryRequest(browserId, frameId, ownedQuery);
                }
                else ownedQuery.Dispose();

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }

        public void OnCefInvokeMethod(int browserId, string frameId, string method, string arguments)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    del.OnCefInvokeMethod(browserId, frameId, method, arguments);
                }

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }

        public void OnCefReportJavascriptResult(int browserId, string frameId, string context, string result)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    del.OnCefReportJavascriptResult(browserId, frameId, context, result);
                }

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }

        public void OnCefInputStateChanged(int browserId, string frameId, bool editable)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    del.OnCefInputStateChanged(browserId, frameId, editable);
                }

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }
        #endregion

        #region DisplayHandler
        public void OnCefAddressChanged(int browserId, string frameId, string url)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    del.OnCefAddressChanged(browserId, frameId, url);
                }

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }

        public void OnCefTitleChanged(int browserId, string title)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    del.OnCefTitleChanged(browserId, title);
                }

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }

        public void OnCefFullScreenModeChanged(int browserId, bool fullscreen)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    del.OnCefFullScreenModeChanged(browserId, fullscreen);
                }

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }

        public void OnCefStatusMessage(int browserId, string message)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    del.OnCefStatusMessage(browserId, message);
                }

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }

        public void OnCefConsoleMessage(int browserId, string message, int level)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    del.OnCefConsoleMessage(browserId, message, level);
                }

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }

        public void OnCefLoadingProgressChanged(int browserId, double progress)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    del.OnCefLoadingProgressChanged(browserId, progress);
                }

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }

        public void OnCefFaviconUrlChanged(int browserId, string faviconUrl)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    del.OnCefFaviconUrlChanged(browserId, faviconUrl);
                }

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }

        public bool OnCefCursorChanged(int browserId, IntPtr cursor, CefViewCursorType type, CefViewCursorInfo customCursorInfo)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    del.OnCefCursorChanged(browserId, type, customCursorInfo);
                    return true;
                }
                return false;

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); return false; }
        }

        public void OnCefDraggableRegionChanged(CefViewDraggableRegion[] draggableRegion, int count)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    del.OnCefDraggableRegionChanged(draggableRegion, count);
                }

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }
        #endregion

        #region FocusHandler
        public void OnCefReleasedFocusByTabKey(int browserId, bool next)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    del.OnCefFocusReleasedByTabKey(browserId, next);
                }

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }

        bool OnCefRequestSetFocus(int browserId)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    return del.OnCefRequestSetFocus(browserId);
                }

                return false;

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); return false; }
        }

        public void OnCefGotFocus(int browserId)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    del.OnCefGotFocus(browserId);
                }

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }
        #endregion

        #region JSDialogHandler
        public bool OnCefJSDialog(int browserId, long requestId, string originUrl, int dialogType, string messageText, string defaultPromptText, bool suppressMessage)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                return del.OnCefJSDialog(browserId, requestId, originUrl, dialogType, messageText, defaultPromptText, suppressMessage);
            }

            return false;
        }
        #endregion

        #region LifespanHandler
        public bool OnCefBeforeNewPopupCreate(string frameId, string targetUrl, string targetFrameName, CefViewWindowOpenDisposition targetDisposition, ref CefViewRect rect, IntPtr settings, ref bool disableJavascriptAccess)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    return del.OnCefBeforeNewPopupCreate(frameId, targetUrl, targetFrameName, targetDisposition, ref rect, settings, ref disableJavascriptAccess);
                }

                return true;

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); return false; }
        }

        public bool OnCefBeforeNewBrowserCreate(string frameId, string targetUrl, string targetFrameName, CefViewWindowOpenDisposition targetDisposition, CefViewRect rect, IntPtr settings)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    return del.OnCefBeforeNewBrowserCreate(frameId, targetUrl, targetFrameName, targetDisposition, rect, settings);
                }

                return true;

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); return false; }
        }

        public bool OnCefDoClose()
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    return del.OnCefDoClose();
                }

                return false;

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); return false; }
        }

        public bool OnCefRequestClose()
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    return del.OnCefRequestClose();
                }

                return false;

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); return false; }
        }

        public void OnCefAfterCreated()
        {
            System.Diagnostics.Trace.WriteLine("[ABI4] OnCefAfterCreated instance");
            try
            {
                _created = true;
                if (_closeRequested) CloseBrowser(true);
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    del.OnCefAfterCreated();
                }

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }

        public void OnCefBeforeClose()
        {
            try
            {
                _closed = true;
                QueueNativeDelete();
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    del.OnCefBeforeClose();
                }

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }
        #endregion

        #region LoadHandler
        public void OnCefLoadingStateChanged(int browserId, bool isLoading, bool canGoBack, bool canGoForward)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    del.OnCefLoadingStateChanged(browserId, isLoading, canGoBack, canGoForward);
                }

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }

        public void OnCefLoadStart(int browserId, string frameId, bool isMainFrame, int transition_type)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    del.OnCefLoadStart(browserId, frameId, isMainFrame, transition_type);
                }

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }

        public void OnCefLoadEnd(int browserId, string frameId, bool isMainFrame, int httpStatusCode)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    del.OnCefLoadEnd(browserId, frameId, isMainFrame, httpStatusCode);
                }

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }

        bool OnCefLoadError(int browserId, string frameId, bool isMainFrame, int errorCode, string errorMsg, string failedUrl)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    return del.OnCefLoadError(browserId, frameId, isMainFrame, errorCode, errorMsg, failedUrl);
                }

                return false;

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); return false; }
        }
        #endregion

        #region RenderHandler
        public void OnCefGetRootScreenRect(int browserId, ref CefViewRect rect)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    del.OnCefGetRootScreenRect(browserId, ref rect);
                }

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }

        public void OnCefGetViewRect(int browserId, ref CefViewRect rect)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    del.OnCefGetViewRect(browserId, ref rect);
                }

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }

        bool OnCefGetScreenPoint(int browserId, int viewX, int viewY, ref int screenX, ref int screenY)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    return del.OnCefGetScreenPoint(browserId, viewX, viewY, ref screenX, ref screenY);
                }

                screenX = 0;
                screenY = 0;
                return false;

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); return false; }
        }

        bool OnCefGetScreenInfo(int browserId, ref CefViewScreenInfo info)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    return del.OnCefGetScreenInfo(browserId, ref info);
                }

                return false;

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); return false; }
        }

        public void OnCefPopupShow(int browserId, bool show)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    del.OnCefPopupShow(browserId, show);
                }

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }

        public void OnCefPopupSize(int browserId, CefViewRect rect)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    del.OnCefPopupSize(browserId, rect);
                }

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }

        public void OnCefPaint(int browserId, CefViewPaintElementType type, CefViewRect[] dirtyRects, int dirtyRectCount, IntPtr imageBytesBuffer, int imageBytesCount, int width, int height)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    del.OnCefPaint(browserId, type, dirtyRects, dirtyRectCount, imageBytesBuffer, imageBytesCount, width, height);
                }

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }

        public void OnCefAcceleratedPaint(int browserId, CefViewPaintElementType type, CefViewRect[] dirtyRects, int dirtyRectCount, IntPtr sharedHandle, int planeBytesCount)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    del.OnCefAcceleratedPaint(browserId, type, dirtyRects, dirtyRectCount, sharedHandle, planeBytesCount);
                }

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }

        // ABI 3 callbacks: forward to the delegate with the same guarded pattern.
        public bool OnCefBeforeUnloadDialog(int browserId, long requestId, string messageText, bool isReload)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del) return del.OnCefBeforeUnloadDialog(browserId, requestId, messageText, isReload);
            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
            return false;
        }

        public bool OnCefFileDialog(int browserId, long requestId, int mode, string title, string defaultFilePath, string filtersJson)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del) return del.OnCefFileDialog(browserId, requestId, mode, title, defaultFilePath, filtersJson);
            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
            return false;
        }

        public bool OnCefBeforeDownload(int browserId, long downloadId, string url, string suggestedName, string mimeType, long totalBytes)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del) return del.OnCefBeforeDownload(browserId, downloadId, url, suggestedName, mimeType, totalBytes);
            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
            return false;
        }

        public void OnCefDownloadUpdated(int browserId, long downloadId, int state, double percent, long speed, long receivedBytes, long totalBytes)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del) del.OnCefDownloadUpdated(browserId, downloadId, state, percent, speed, receivedBytes, totalBytes);
            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }

        public void OnCefFindResult(int browserId, int identifier, int count, int activeMatchOrdinal, bool finalUpdate, CefViewRect selectionRect)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del) del.OnCefFindResult(browserId, identifier, count, activeMatchOrdinal, finalUpdate, selectionRect);
            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }

        public bool OnCefContextMenu(int browserId, long requestId, string contextParamsJson, string menuJson)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del) return del.OnCefContextMenu(browserId, requestId, contextParamsJson, menuJson);
            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
            return false;
        }

        public void OnCefContextMenuDismissed(int browserId)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del) del.OnCefContextMenuDismissed(browserId);
            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }

        public bool OnCefPermissionPrompt(int browserId, ulong promptId, string requestingOrigin, uint requestedPermissions)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del) return del.OnCefPermissionPrompt(browserId, promptId, requestingOrigin, requestedPermissions);
            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
            return false;
        }

        public void OnCefRenderProcessTerminated(int browserId, int status, int errorCode, string errorString)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del) del.OnCefRenderProcessTerminated(browserId, status, errorCode, errorString);
            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }

        public bool OnCefStartDragging(int browserId, CefViewDragOperation allowedOps, int x, int y)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    return del.OnCefStartDragging(browserId, allowedOps, x, y);
                }

                return true;

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); return false; }
        }

        public void OnCefUpdateDragCursor(int browserId, CefViewDragOperation operation)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    del.OnCefUpdateDragCursor(browserId, operation);
                }

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }

        public void OnCefImeCompositionRangeChanged(int browserId, CefViewRange selectedRange, CefViewRect[] characterBounds, int characterBoundsCount)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    del.OnCefImeCompositionRangeChanged(browserId, selectedRange, characterBounds, characterBoundsCount);
                }

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }

        public void OnCefTextSelectionChanged(int browserId, string selectedText, CefViewRange selectedRange)
        {
            try
            {
                var del = _dnCefViewDelegate.Target as ICefViewDelegate;
                if (null != del)
                {
                    del.OnCefTextSelectionChanged(browserId, selectedText, selectedRange);
                }

            }
            catch (Exception exception) { System.Diagnostics.Trace.TraceError(exception.ToString()); }
        }
        #endregion
        #endregion
    }
}
