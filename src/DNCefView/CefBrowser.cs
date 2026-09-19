using System;
using System.Collections.Generic;

namespace DNCefView
{
    public partial class CefBrowser
    {
        internal static readonly HashSet<CefBrowser> LiveInstances = new HashSet<CefBrowser>();


        private WeakReference _dnCefViewDelegate;

        private CefBrowserCallback _callbackTable;

        public const string MainFrameID = "0";

        public const string AllFrameID = "-1";

        public CefBrowser(ICefViewDelegate del, string url, CefSetting setting)
        {
            if (null == CefContext.Instance)
            {
                throw new Exception("CefContext must be instantiated first");
            }

            lock (LiveInstances) LiveInstances.Add(this);

            _dnCefViewDelegate = new WeakReference(del);

            #region CefView events
            _callbackTable.CefQueryRequestCb = OnCefQueryRequest;
            _callbackTable.InvokeMethodCb = OnCefInvokeMethod;
            _callbackTable.ReportJavascriptResultCb = OnCefReportJavascriptResult;
            _callbackTable.InputStateChangedCb = OnCefInputStateChanged;
            #endregion

            #region DisplayHandler
            _callbackTable.AddressChangedCb = OnCefAddressChanged;
            _callbackTable.TitleChangedCb = OnCefTitleChanged;
            _callbackTable.FullscreenModeChangedCb = OnCefFullScreenModeChanged;
            _callbackTable.StatusMessageCb = OnCefStatusMessage;
            _callbackTable.ConsoleMessageCb = OnCefConsoleMessage;
            _callbackTable.LoadingProgressChangedCb = OnCefLoadingProgressChanged;
            _callbackTable.FaviconUrlChangedCb = OnCefFaviconUrlChanged;
            _callbackTable.CursorChangedCb = OnCefCursorChanged;
            _callbackTable.DraggableRegionChangedCb = OnCefDraggableRegionChanged;
            #endregion

            #region FocusHandler
            _callbackTable.OnFocusReleasedByTabKeyCb = OnCefReleasedFocusByTabKey;
            _callbackTable.OnRequestSetFocusCb = OnCefRequestSetFocus;
            _callbackTable.OnGotFocusCb = OnCefGotFocus;
            _callbackTable.OnJSDialogCb = OnCefJSDialog;
            #endregion

            #region LifespanHandler
            _callbackTable.OnBeforeNewPopupCreateCb = OnCefBeforeNewPopupCreate;
            _callbackTable.OnBeforeNewBrowserCreateCb = OnCefBeforeNewBrowserCreate;
            _callbackTable.DoCloseCb = OnCefDoClose;
            _callbackTable.RequestCloseCb = OnCefRequestClose;
            _callbackTable.OnAfterCreatedCb = OnCefAfterCreated;
            _callbackTable.OnBeforeCloseCb = OnCefBeforeClose;
            #endregion

            #region LoadHandler
            _callbackTable.LoadingStateChangedCb = OnCefLoadingStateChanged;
            _callbackTable.LoadStartCb = OnCefLoadStart;
            _callbackTable.LoadEndCb = OnCefLoadEnd;
            _callbackTable.LoadErrorCb = OnCefLoadError;
            #endregion

            #region RenderHandler
            _callbackTable.GetRootScreenRectCb = OnCefGetRootScreenRect;
            _callbackTable.GetViewRectCb = OnCefGetViewRect;
            _callbackTable.GetScreenPointCb = OnCefGetScreenPoint;
            _callbackTable.GetScreenInfoCb = OnCefGetScreenInfo;
            _callbackTable.OnPopupShowCb = OnCefPopupShow;
            _callbackTable.OnPopupSizeCb = OnCefPopupSize;
            _callbackTable.OnPaintCb = OnCefPaint;
            _callbackTable.OnAcceleratedPaintCb = OnCefAcceleratedPaint;
            // ABI 3 callbacks.
            _callbackTable.OnBeforeUnloadDialogCb = OnCefBeforeUnloadDialog;
            _callbackTable.OnFileDialogCb = OnCefFileDialog;
            _callbackTable.OnBeforeDownloadCb = OnCefBeforeDownload;
            _callbackTable.OnDownloadUpdatedCb = OnCefDownloadUpdated;
            _callbackTable.OnFindResultCb = OnCefFindResult;
            _callbackTable.OnContextMenuCb = OnCefContextMenu;
            _callbackTable.OnContextMenuDismissedCb = OnCefContextMenuDismissed;
            _callbackTable.OnPermissionPromptCb = OnCefPermissionPrompt;
            _callbackTable.OnRenderProcessTerminatedCb = OnCefRenderProcessTerminated;
            _callbackTable.StartDraggingCb = OnCefStartDragging;
            _callbackTable.UpdateDragCursorCb = OnCefUpdateDragCursor;
            _callbackTable.OnImeCompositionRangeChangedCb = OnCefImeCompositionRangeChanged;
            _callbackTable.OnTextSelectionChangedCb = OnCefTextSelectionChanged;
            #endregion

            _native = CCefBrowser_new0(_callbackTable, url, setting.NativeObject);
            if (_native == IntPtr.Zero)
            {
                lock (LiveInstances) LiveInstances.Remove(this);
                throw new InvalidOperationException("Native browser creation failed");
            }
        }

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
