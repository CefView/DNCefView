using System;
using System.Collections.Generic;
using System.Drawing;

namespace DNCef
{
    public partial class CefBrowser
    {
        internal static HashSet<WeakReference> LiveInstances = new HashSet<WeakReference>();

        private WeakReference _weakSelf;

        private WeakReference _dnCefViewDelegate;

        private CefBrowserCallback _callbackTable;

        public bool TransparentPaintingEnabled = false;

        public bool ShowPopup = false;

        public Rectangle PopupRect;

        public Rectangle ImeCursorRect;

        public byte[] CefViewFrameData;

        public byte[] CefPopupFrameData;

        public CefBrowser(ICefViewDelegate del, string url, CefSetting setting)
        {
            if (null == CefContext.Instance)
            {
                throw new Exception("CefContext must be instantiated first");
            }

            _weakSelf = new WeakReference(this);
            LiveInstances.Add(_weakSelf);

            _dnCefViewDelegate = new WeakReference(del);

            _callbackTable.cefQueryRequestCallback = OnCefQueryRequest;
            _callbackTable.invokeMethodCallback = OnCefInvokeMethod;
            _callbackTable.reportJavascriptResultCallback = OnCefReportJavascriptResult;
            _callbackTable.inputStateChangedCallback = OnCefInputStateChanged;

            _callbackTable.loadingStateChangedCallback = OnCefLoadingStateChanged;
            _callbackTable.loadStartCallback = OnCefLoadStart;
            _callbackTable.loadEndCallback = OnCefLoadEnd;
            _callbackTable.loadErrorCallback = OnCefLoadError;

            _callbackTable.addressChangedCallback = OnCefAddressChanged;
            _callbackTable.titleChangedCallback = OnCefTitleChanged;
            _callbackTable.fullScreenModeChangedCallback = OnCefFullScreenModeChanged;
            _callbackTable.statusMessageCallback = OnCefStatusMessage;
            _callbackTable.consoleMessageCallback = OnCefConsoleMessage;
            _callbackTable.loadingProgressChangedCallback = OnCefLoadingProgressChanged;

            _callbackTable.afterCreatedCallback = OnCefAfterCreated;

            _callbackTable.releaseFocusCallback = OnCefReleaseFocus;
            _callbackTable.setFocusCallback = OnCefSetFocus;
            _callbackTable.gotFocusCallback = OnCefGotFocus;

            _callbackTable.getRootScreenRectCallback = OnCefGetRootScreenRect;
            _callbackTable.getViewRectCallback = OnCefGetViewRect;
            _callbackTable.getScreenPointCallback = OnCefGetScreenPoint;
            _callbackTable.getScreenInfoCallback = OnCefGetScreenInfo;
            _callbackTable.onPopupShowCallback = OnCefPopupShow;
            _callbackTable.onPopupSizeCallback = OnCefPopupSize;
            _callbackTable.onPaintCallback = OnCefPaint;
            _callbackTable.onImeCompositionRangeChangedCallback = OnCefImeCompositionRangeChanged;

            _native = CCefBrowser_new0(_callbackTable, url, setting.NativeObject);
        }

        ~CefBrowser()
        {
            Dispose(false);

            LiveInstances.Remove(_weakSelf);
        }

        #region CEF Callbacks
        public void OnCefQueryRequest(int browserId, Int64 frameId, IntPtr query)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefQueryRequest(browserId, frameId, new CefQuery(query));
            }
        }

        public void OnCefInvokeMethod(int browserId, Int64 frameId, string method, string arguments)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefInvokeMethod(browserId, frameId, method, arguments);
            }
        }

        public void OnCefReportJavascriptResult(int browserId, Int64 frameId, Int64 context, string result)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefReportJavascriptResult(browserId, frameId, context, result);
            }
        }

        public void OnCefInputStateChanged(int browserId, Int64 frameId, bool editable)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefInputStateChanged(browserId, frameId, editable);
            }
        }

        public void OnCefLoadingStateChanged(int browserId, bool isLoading, bool canGoBack, bool canGoForward)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefLoadingStateChanged(browserId, isLoading, canGoBack, canGoForward);
            }
        }

        public void OnCefLoadStart(int browserId, Int64 frameId, bool isMainFrame, int transition_type)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefLoadStart(browserId, frameId, isMainFrame, transition_type);
            }
        }

        public void OnCefLoadEnd(int browserId, Int64 frameId, bool isMainFrame, int httpStatusCode)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefLoadEnd(browserId, frameId, isMainFrame, httpStatusCode);
            }
        }

        bool OnCefLoadError(int browserId, Int64 frameId, bool isMainFrame, int errorCode, string errorMsg, string failedUrl)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefLoadError(browserId, frameId, isMainFrame, errorCode, errorMsg, failedUrl);
            }

            return false;
        }

        public void OnCefAddressChanged(Int64 frameId, string url)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefAddressChanged(frameId, url);
            }
        }

        public void OnCefTitleChanged(string title)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefTitleChanged(title);
            }
        }

        public void OnCefFullScreenModeChanged(bool fullscreen)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefFullScreenModeChanged(fullscreen);
            }
        }

        public void OnCefStatusMessage(string message)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefStatusMessage(message);
            }
        }

        public void OnCefConsoleMessage(string message, int level)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefConsoleMessage(message, level);
            }
        }

        public void OnCefLoadingProgressChanged(double progress)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefLoadingProgressChanged(progress);
            }
        }

        public void OnCefAfterCreated()
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefAfterCreated();
            }
        }

        public void OnCefReleaseFocus(int browserId, bool next)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefFocusReleasedByTabKey(browserId, next);
            }
        }

        bool OnCefSetFocus(int browserId)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefSetFocus(browserId);
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

        bool OnCefGetScreenPoint(int browserId, int viewX, int viewY, out int screenX, out int screenY)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                return del.OnCefGetScreenPoint(browserId, viewX, viewY, out screenX, out screenY);
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

        public void OnCefPaint(int browserId, CefViewPaintElementType type, CefViewRect[] dirtyRects, int dirtyRectCount, byte[] imageBytes, int imageBytesCount, int width, int height)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefPaint(browserId, type, dirtyRects, dirtyRectCount, imageBytes, imageBytesCount, width, height);
            }
        }

        public void OnCefImeCompositionRangeChanged(int browserId, CefViewRange range, CefViewRect[] characterBounds, int characterBoundsCount)
        {
            var del = _dnCefViewDelegate.Target as ICefViewDelegate;
            if (null != del)
            {
                del.OnCefImeCompositionRangeChanged(browserId, range, characterBounds, characterBoundsCount);
            }
        }
        #endregion
    }
}
