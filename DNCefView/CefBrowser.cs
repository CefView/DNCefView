using System;
using System.Collections.Generic;

namespace DNCefView
{
    public partial class CefBrowser
    {
        internal static readonly HashSet<WeakReference> LiveInstances = new HashSet<WeakReference>();

        private readonly WeakReference _weakSelf;

        private readonly WeakReference _dnCefViewDelegate;

        private readonly CefBrowserCallback _callbackTable;

        public const string MainFrameId = "0";

        public const string AllFrameId = "-1";

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
            _callbackTable.CursorChangedCb = OnCefCursorChanged;
            #endregion

            #region FocusHandler
            _callbackTable.OnFocusReleasedByTabKeyCb = OnCefReleasedFocusByTabKey;
            _callbackTable.OnRequestSetFocusCb = OnCefRequestSetFocus;
            _callbackTable.OnGotFocusCb = OnCefGotFocus;
            #endregion

            #region LifespanHandler
            _callbackTable.OnAfterCreatedCb = OnCefAfterCreated;
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
            if (_dnCefViewDelegate.Target is ICefViewDelegate del)
            {
                del.OnCefQueryRequest(browserId, frameId, new CefQuery(query));
            }
        }

        public void OnCefInvokeMethod(int browserId, string frameId, string method, string arguments)
        {
            if (_dnCefViewDelegate.Target is ICefViewDelegate del)
            {
                del.OnCefInvokeMethod(browserId, frameId, method, arguments);
            }
        }

        public void OnCefReportJavascriptResult(int browserId, string frameId, string context, string result)
        {
            if (_dnCefViewDelegate.Target is ICefViewDelegate del)
            {
                del.OnCefReportJavascriptResult(browserId, frameId, context, result);
            }
        }

        public void OnCefInputStateChanged(int browserId, string frameId, bool editable)
        {
            if (_dnCefViewDelegate.Target is ICefViewDelegate del)
            {
                del.OnCefInputStateChanged(browserId, frameId, editable);
            }
        }
        #endregion

        #region DisplayHandler
        public void OnCefAddressChanged(int browserId, string frameId, string url)
        {
            if (_dnCefViewDelegate.Target is ICefViewDelegate del)
            {
                del.OnCefAddressChanged(browserId, frameId, url);
            }
        }

        public void OnCefTitleChanged(int browserId, string title)
        {
            if (_dnCefViewDelegate.Target is ICefViewDelegate del)
            {
                del.OnCefTitleChanged(browserId, title);
            }
        }

        public void OnCefFullScreenModeChanged(int browserId, bool fullscreen)
        {
            if (_dnCefViewDelegate.Target is ICefViewDelegate del)
            {
                del.OnCefFullScreenModeChanged(browserId, fullscreen);
            }
        }

        public void OnCefStatusMessage(int browserId, string message)
        {
            if (_dnCefViewDelegate.Target is ICefViewDelegate del)
            {
                del.OnCefStatusMessage(browserId, message);
            }
        }

        public void OnCefConsoleMessage(int browserId, string message, int level)
        {
            if (_dnCefViewDelegate.Target is ICefViewDelegate del)
            {
                del.OnCefConsoleMessage(browserId, message, level);
            }
        }

        public void OnCefLoadingProgressChanged(int browserId, double progress)
        {
            if (_dnCefViewDelegate.Target is ICefViewDelegate del)
            {
                del.OnCefLoadingProgressChanged(browserId, progress);
            }
        }

        public bool OnCefCursorChanged(int browserId, IntPtr cursor, CefViewCursorType type, CefViewCursorInfo customCursorInfo)
        {
            if (_dnCefViewDelegate.Target is ICefViewDelegate del)
            {
                del.OnCefCursorChanged(browserId, type, customCursorInfo);
                return true;
            }
            return false;
        }
        #endregion

        #region FocusHandler
        public void OnCefReleasedFocusByTabKey(int browserId, bool next)
        {
            if (_dnCefViewDelegate.Target is ICefViewDelegate del)
            {
                del.OnCefFocusReleasedByTabKey(browserId, next);
            }
        }

        bool OnCefRequestSetFocus(int browserId)
        {
            if (_dnCefViewDelegate.Target is ICefViewDelegate del)
            {
                del.OnCefRequestSetFocus(browserId);
            }

            return false;
        }

        public void OnCefGotFocus(int browserId)
        {
            if (_dnCefViewDelegate.Target is ICefViewDelegate del)
            {
                del.OnCefGotFocus(browserId);
            }
        }
        #endregion

        #region LifespanHandler
        public void OnCefAfterCreated()
        {
            if (_dnCefViewDelegate.Target is ICefViewDelegate del)
            {
                del.OnCefAfterCreated();
            }
        }
        #endregion

        #region LoadHandler
        public void OnCefLoadingStateChanged(int browserId, bool isLoading, bool canGoBack, bool canGoForward)
        {
            if (_dnCefViewDelegate.Target is ICefViewDelegate del)
            {
                del.OnCefLoadingStateChanged(browserId, isLoading, canGoBack, canGoForward);
            }
        }

        public void OnCefLoadStart(int browserId, string frameId, bool isMainFrame, int transitionType)
        {
            if (_dnCefViewDelegate.Target is ICefViewDelegate del)
            {
                del.OnCefLoadStart(browserId, frameId, isMainFrame, transitionType);
            }
        }

        public void OnCefLoadEnd(int browserId, string frameId, bool isMainFrame, int httpStatusCode)
        {
            if (_dnCefViewDelegate.Target is ICefViewDelegate del)
            {
                del.OnCefLoadEnd(browserId, frameId, isMainFrame, httpStatusCode);
            }
        }

        bool OnCefLoadError(int browserId, string frameId, bool isMainFrame, int errorCode, string errorMsg, string failedUrl)
        {
            if (_dnCefViewDelegate.Target is ICefViewDelegate del)
            {
                del.OnCefLoadError(browserId, frameId, isMainFrame, errorCode, errorMsg, failedUrl);
            }

            return false;
        }
        #endregion

        #region RenderHandler
        public void OnCefGetRootScreenRect(int browserId, ref CefViewRect rect)
        {
            if (_dnCefViewDelegate.Target is ICefViewDelegate del)
            {
                del.OnCefGetRootScreenRect(browserId, ref rect);
            }
        }

        public void OnCefGetViewRect(int browserId, ref CefViewRect rect)
        {
            if (_dnCefViewDelegate.Target is ICefViewDelegate del)
            {
                del.OnCefGetViewRect(browserId, ref rect);
            }
        }

        bool OnCefGetScreenPoint(int browserId, int viewX, int viewY, ref int screenX, ref int screenY)
        {
            if (_dnCefViewDelegate.Target is ICefViewDelegate del)
            {
                return del.OnCefGetScreenPoint(browserId, viewX, viewY, ref screenX, ref screenY);
            }

            screenX = 0;
            screenY = 0;
            return false;
        }

        bool OnCefGetScreenInfo(int browserId, ref CefViewScreenInfo info)
        {
            if (_dnCefViewDelegate.Target is ICefViewDelegate del)
            {
                return del.OnCefGetScreenInfo(browserId, ref info);
            }

            return false;
        }

        public void OnCefPopupShow(int browserId, bool show)
        {
            if (_dnCefViewDelegate.Target is ICefViewDelegate del)
            {
                del.OnCefPopupShow(browserId, show);
            }
        }

        public void OnCefPopupSize(int browserId, CefViewRect rect)
        {
            if (_dnCefViewDelegate.Target is ICefViewDelegate del)
            {
                del.OnCefPopupSize(browserId, rect);
            }
        }

        public void OnCefPaint(int browserId, CefViewPaintElementType type, CefViewRect[] dirtyRects, int dirtyRectCount, IntPtr imageBytesBuffer, int imageBytesCount, int width, int height)
        {
            if (_dnCefViewDelegate.Target is ICefViewDelegate del)
            {
                del.OnCefPaint(browserId, type, dirtyRects, dirtyRectCount, imageBytesBuffer, imageBytesCount, width, height);
            }
        }

        public void OnCefAcceleratedPaint(int browserId, CefViewPaintElementType type, CefViewRect[] dirtyRects, int dirtyRectCount, IntPtr sharedHandle, int planeBytesCount)
        {
            if (_dnCefViewDelegate.Target is ICefViewDelegate del)
            {
                del.OnCefAcceleratedPaint(browserId, type, dirtyRects, dirtyRectCount, sharedHandle, planeBytesCount);
            }
        }

        public void OnCefImeCompositionRangeChanged(int browserId, CefViewRange selectedRange, CefViewRect[] characterBounds, int characterBoundsCount)
        {
            if (_dnCefViewDelegate.Target is ICefViewDelegate del)
            {
                del.OnCefImeCompositionRangeChanged(browserId, selectedRange, characterBounds, characterBoundsCount);
            }
        }

        public void OnCefTextSelectionChanged(int browserId, string selectedText, CefViewRange selectedRange)
        {
            if (_dnCefViewDelegate.Target is ICefViewDelegate del)
            {
                del.OnCefTextSelectionChanged(browserId, selectedText, selectedRange);
            }
        }
        #endregion
        #endregion
    }
}
