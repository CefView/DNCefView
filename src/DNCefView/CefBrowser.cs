using System;
using System.Collections.Generic;

namespace DNCefView
{
    public partial class CefBrowser
    {
        internal static HashSet<WeakReference> LiveInstances = new HashSet<WeakReference>();

        private WeakReference _weakSelf;

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

            _weakSelf = new WeakReference(this);
            LiveInstances.Add(_weakSelf);

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
            _callbackTable.StartDraggingCb = OnCefStartDragging;
            _callbackTable.UpdateDragCursorCb = OnCefUpdateDragCursor;
            _callbackTable.OnImeCompositionRangeChangedCb = OnCefImeCompositionRangeChanged;
            _callbackTable.OnTextSelectionChangedCb = OnCefTextSelectionChanged;
            #endregion

            _native = CCefBrowser_new0(_callbackTable, url, setting.NativeObject);
        }

        ~CefBrowser()
        {
            Dispose(false);

            LiveInstances.Remove(_weakSelf);
        }

        #region CEF Callbacks
        #region CefView events
        public void OnCefQueryRequest(int browserId, string frameId, IntPtr query)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefQueryRequest(browserId, frameId, new CefQuery(query));
            }
        }

        public void OnCefInvokeMethod(int browserId, string frameId, string method, string arguments)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefInvokeMethod(browserId, frameId, method, arguments);
            }
        }

        public void OnCefReportJavascriptResult(int browserId, string frameId, string context, string result)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefReportJavascriptResult(browserId, frameId, context, result);
            }
        }

        public void OnCefInputStateChanged(int browserId, string frameId, bool editable)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefInputStateChanged(browserId, frameId, editable);
            }
        }
        #endregion

        #region DisplayHandler
        public void OnCefAddressChanged(int browserId, string frameId, string url)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefAddressChanged(browserId, frameId, url);
            }
        }

        public void OnCefTitleChanged(int browserId, string title)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefTitleChanged(browserId, title);
            }
        }

        public void OnCefFullScreenModeChanged(int browserId, bool fullscreen)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefFullScreenModeChanged(browserId, fullscreen);
            }
        }

        public void OnCefStatusMessage(int browserId, string message)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefStatusMessage(browserId, message);
            }
        }

        public void OnCefConsoleMessage(int browserId, string message, int level)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefConsoleMessage(browserId, message, level);
            }
        }

        public void OnCefLoadingProgressChanged(int browserId, double progress)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefLoadingProgressChanged(browserId, progress);
            }
        }

        public void OnCefFaviconUrlChanged(int browserId, string faviconUrl)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefFaviconUrlChanged(browserId, faviconUrl);
            }
        }

        public bool OnCefCursorChanged(int browserId, IntPtr cursor, CefViewCursorType type, CefViewCursorInfo customCursorInfo)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefCursorChanged(browserId, type, customCursorInfo);
                return true;
            }
            return false;
        }

        public void OnCefDraggableRegionChanged(CefViewDraggableRegion[] draggableRegion, int count)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefDraggableRegionChanged(draggableRegion, count);
            }
        }
        #endregion

        #region FocusHandler
        public void OnCefReleasedFocusByTabKey(int browserId, bool next)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefFocusReleasedByTabKey(browserId, next);
            }
        }

        bool OnCefRequestSetFocus(int browserId)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                return del.OnCefRequestSetFocus(browserId);
            }

            return false;
        }

        public void OnCefGotFocus(int browserId)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefGotFocus(browserId);
            }
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
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                return del.OnCefBeforeNewPopupCreate(frameId, targetUrl, targetFrameName, targetDisposition, ref rect, settings, ref disableJavascriptAccess);
            }

            return true;
        }

        public bool OnCefBeforeNewBrowserCreate(string frameId, string targetUrl, string targetFrameName, CefViewWindowOpenDisposition targetDisposition, CefViewRect rect, IntPtr settings)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                return del.OnCefBeforeNewBrowserCreate(frameId, targetUrl, targetFrameName, targetDisposition, rect, settings);
            }

            return true;
        }

        public bool OnCefDoClose()
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                return del.OnCefDoClose();
            }

            return false;
        }

        public bool OnCefRequestClose()
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                return del.OnCefRequestClose();
            }

            return true;
        }

        public void OnCefAfterCreated()
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefAfterCreated();
            }
        }

        public void OnCefBeforeClose()
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefBeforeClose();
            }
        }
        #endregion

        #region LoadHandler
        public void OnCefLoadingStateChanged(int browserId, bool isLoading, bool canGoBack, bool canGoForward)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefLoadingStateChanged(browserId, isLoading, canGoBack, canGoForward);
            }
        }

        public void OnCefLoadStart(int browserId, string frameId, bool isMainFrame, int transition_type)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefLoadStart(browserId, frameId, isMainFrame, transition_type);
            }
        }

        public void OnCefLoadEnd(int browserId, string frameId, bool isMainFrame, int httpStatusCode)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefLoadEnd(browserId, frameId, isMainFrame, httpStatusCode);
            }
        }

        bool OnCefLoadError(int browserId, string frameId, bool isMainFrame, int errorCode, string errorMsg, string failedUrl)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                return del.OnCefLoadError(browserId, frameId, isMainFrame, errorCode, errorMsg, failedUrl);
            }

            return false;
        }
        #endregion

        #region RenderHandler
        public void OnCefGetRootScreenRect(int browserId, ref CefViewRect rect)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefGetRootScreenRect(browserId, ref rect);
            }
        }

        public void OnCefGetViewRect(int browserId, ref CefViewRect rect)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefGetViewRect(browserId, ref rect);
            }
        }

        bool OnCefGetScreenPoint(int browserId, int viewX, int viewY, ref int screenX, ref int screenY)
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

        bool OnCefGetScreenInfo(int browserId, ref CefViewScreenInfo info)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                return del.OnCefGetScreenInfo(browserId, ref info);
            }

            return false;
        }

        public void OnCefPopupShow(int browserId, bool show)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefPopupShow(browserId, show);
            }
        }

        public void OnCefPopupSize(int browserId, CefViewRect rect)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefPopupSize(browserId, rect);
            }
        }

        public void OnCefPaint(int browserId, CefViewPaintElementType type, CefViewRect[] dirtyRects, int dirtyRectCount, IntPtr imageBytesBuffer, int imageBytesCount, int width, int height)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefPaint(browserId, type, dirtyRects, dirtyRectCount, imageBytesBuffer, imageBytesCount, width, height);
            }
        }

        public void OnCefAcceleratedPaint(int browserId, CefViewPaintElementType type, CefViewRect[] dirtyRects, int dirtyRectCount, IntPtr sharedHandle, int planeBytesCount)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefAcceleratedPaint(browserId, type, dirtyRects, dirtyRectCount, sharedHandle, planeBytesCount);
            }
        }

        public bool OnCefStartDragging(int browserId, CefViewDragOperation allowedOps, int x, int y)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                return del.OnCefStartDragging(browserId, allowedOps, x, y);
            }

            return true;
        }

        public void OnCefUpdateDragCursor(int browserId, CefViewDragOperation operation)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefUpdateDragCursor(browserId, operation);
            }
        }

        public void OnCefImeCompositionRangeChanged(int browserId, CefViewRange selectedRange, CefViewRect[] characterBounds, int characterBoundsCount)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefImeCompositionRangeChanged(browserId, selectedRange, characterBounds, characterBoundsCount);
            }
        }

        public void OnCefTextSelectionChanged(int browserId, string selectedText, CefViewRange selectedRange)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefTextSelectionChanged(browserId, selectedText, selectedRange);
            }
        }
        #endregion
        #endregion
    }
}
