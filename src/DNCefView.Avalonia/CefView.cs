using System;
using System.Collections.Generic;
using System.Text.Json;

namespace DNCefView.Avalonia
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CefView : ICefViewDelegate
    {
        /// <summary>
        /// 
        /// </summary>
        protected CefSetting? _cefSetting;
        public CefSetting? Setting
        {
            get { return _cefSetting; }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool _hasCefGotFocus = false;

        /// <summary>
        /// 
        /// </summary>
        protected bool _isCefFocusedNodeEditable = false;

        /// <summary>
        /// 
        /// </summary>
        protected CefBrowser? _cefBrowser;

        /// <summary>
        /// 
        /// </summary>
        protected bool _isCreated = false;

        /// <summary>
        /// 
        /// </summary>
        protected bool _isShowPopup = false;

        #region Events
        public delegate void CefQueryRequestCallback(int browserId, string frameId, CefQuery query);
        public event CefQueryRequestCallback? CefQueryRequest;

        public delegate void InvokeMethodCallback(int browserId, string frameId, string method, List<dynamic> arguments);
        public event InvokeMethodCallback? InvokeMethod;

        public delegate void ReportJavascriptResultCallback(int browserId, string frameId, string context, string result);
        public event ReportJavascriptResultCallback? ReportJavascriptResult;


        public delegate void LoadingStateChangedCallback(int browserId, bool isLoading, bool canGoBack, bool canGoForward);
        public event LoadingStateChangedCallback? LoadingStateChanged;

        public delegate void LoadStartCallback(int browserId, string frameId, bool isMainFrame, int transition_type);
        public event LoadStartCallback? LoadStart;

        public delegate void LoadEndCallback(int browserId, string frameId, bool isMainFrame, int httpStatusCode);
        public event LoadEndCallback? LoadEnd;

        public delegate bool LoadErrorCallback(int browserId, string frameId, bool isMainFrame, int errorCode, string errorMsg, string failedUrl);
        public LoadErrorCallback? LoadError;

        public delegate void AddressChangedCallback(int browserId, string frameId, string url);
        public event AddressChangedCallback? AddressChanged;

        public delegate void TitleChangedCallback(int browserId, string title);
        public event TitleChangedCallback? TitleChanged;

        public delegate void FullScreenModeChangedCallback(int browserId, bool fullscreen);
        public event FullScreenModeChangedCallback? FullScreenModeChanged;

        public delegate void StatusMessageCallback(int browserId, string message);
        public event StatusMessageCallback? StatusMessage;

        public delegate void ConsoleMessageCallback(int browserId, string message, int level);
        public event ConsoleMessageCallback? ConsoleMessage;

        public delegate void LoadingProgressChangedCallback(int browserId, double progress);
        public event LoadingProgressChangedCallback? LoadingProgressChanged;
        #endregion

        private void InitializeNative(string url, CefSetting? setting)
        {
            if (null != setting)
            {
                _cefSetting = setting;
            }
            else
            {
                this.LogI("Use default CefSettings");
                _cefSetting = new CefSetting();
                _cefSetting.SetWindowlessFrameRate(60);
            }

            _cefBrowser = new CefBrowser(this, url, _cefSetting);
        }

        #region Browser Methods
        public void AddLocalFolderResource(string path, string url, int priority)
        {
            _cefBrowser?.AddLocalFolderResource(path, url, priority);
        }

        public void AddArchiveResource(string path, string url, string password, int priority)
        {
            _cefBrowser?.AddArchiveResource(path, url, password, priority);
        }

        public int BrowserId()
        {
            return _cefBrowser?.BrowserId() ?? -1;
        }

        public void NavigateToString(string content)
        {
            _cefBrowser?.NavigateToString(content);
        }

        public void NavigateToUrl(string url)
        {
            _cefBrowser?.NavigateToUrl(url);
        }

        public bool CanGoBack()
        {
            return _cefBrowser?.CanGoBack() ?? false;
        }

        public bool CanGoForward()
        {
            return _cefBrowser?.CanGoForward() ?? false;
        }

        public void GoBack()
        {
            _cefBrowser?.GoBack();
        }

        public void GoForward()
        {
            _cefBrowser?.GoForward();
        }

        public bool IsLoading()
        {
            return _cefBrowser?.IsLoading() ?? false; ;
        }

        public void Reload()
        {
            _cefBrowser?.Reload();
        }

        public void StopLoad()
        {
            _cefBrowser?.StopLoad();
        }

        public bool TriggerEventOnMainFrame(string evtName, List<object> evtArgs)
        {
            var argsString = JsonSerializer.Serialize(evtArgs);
            return _cefBrowser?.TriggerEventOnMainFrame(evtName, argsString) ?? false;
        }

        public bool TriggerEventOnFrame(string evtName, List<object> evtArgs, string frameId)
        {
            var argsString = JsonSerializer.Serialize(evtArgs);
            return _cefBrowser?.TriggerEventOnFrame(evtName, argsString, frameId) ?? false;
        }

        public bool BroadcastEvent(string evtName, List<object> evtArgs)
        {
            var argsString = JsonSerializer.Serialize(evtArgs);
            return _cefBrowser?.BroadcastEvent(evtName, argsString) ?? false;
        }

        public bool TriggerEvent(string name, List<object> args, string frameId)
        {
            var argsString = JsonSerializer.Serialize(args);
            return _cefBrowser?.TriggerEvent(name, argsString, frameId) ?? false;
        }

        public bool ResponseQCefQuery(CefQuery query)
        {
            return _cefBrowser?.ResponseQCefQuery(query) ?? false;
        }

        public bool ExecuteJavascript(string frameId, string code, string url)
        {
            return _cefBrowser?.ExecuteJavascript(frameId, code, url) ?? false;
        }

        public bool ExecuteJavascriptWithResult(string frameId, string code, string url, string context)
        {
            return _cefBrowser?.ExecuteJavascriptWithResult(frameId, code, url, context) ?? false;
        }

        public bool SetPreference(string name, string value)
        {
            return _cefBrowser?.SetPreference(name, value) ?? false;
        }

        public void ImeSetComposition(string text, CefViewCompositionUnderline[] underlines, CefViewRange replacement_range, CefViewRange selection_range)
        {
            _cefBrowser?.ImeSetComposition(text, underlines, underlines.Length, replacement_range, selection_range);
        }

        public void ImeCommitText(string text, CefViewRange replacement_range, int relative_cursor_pos)
        {
            _cefBrowser?.ImeCommitText(text, replacement_range, relative_cursor_pos);
        }

        public void ImeFinishComposingText(bool keep_selection)
        {
            _cefBrowser?.ImeFinishComposingText(keep_selection);
        }

        public void ImeCancelComposition()
        {
            _cefBrowser?.ImeCancelComposition();
        }
        #endregion

        #region CEF Callbacks
        #region CefView events
        void ICefViewDelegate.OnCefQueryRequest(int browserId, string frameId, CefQuery query)
        {
            CefQueryRequest?.Invoke(browserId, frameId, query);
        }

        void ICefViewDelegate.OnCefInvokeMethod(int browserId, string frameId, string method, string arguments)
        {
            List<dynamic>? argList = new List<dynamic>();
            try
            {
                argList = JsonSerializer.Deserialize<List<dynamic>>(arguments);
            }
            catch (Exception)
            {
            }

            InvokeMethod?.Invoke(browserId, frameId, method, argList!);
        }

        void ICefViewDelegate.OnCefReportJavascriptResult(int browserId, string frameId, string context, string result)
        {
            ReportJavascriptResult?.Invoke(browserId, frameId, context, result);
        }

        void ICefViewDelegate.OnCefInputStateChanged(int browserId, string frameId, bool editable)
        {
            UI_OnCefInputStateChanged(browserId, frameId, editable);
        }
        #endregion

        #region DisplayHandler
        void ICefViewDelegate.OnCefAddressChanged(int browserId, string frameId, string url)
        {
            AddressChanged?.Invoke(browserId, frameId, url);
        }

        void ICefViewDelegate.OnCefTitleChanged(int browserId, string title)
        {
            TitleChanged?.Invoke(browserId, title);
        }

        void ICefViewDelegate.OnCefFullScreenModeChanged(int browserId, bool fullscreen)
        {
            FullScreenModeChanged?.Invoke(browserId, fullscreen);
        }

        void ICefViewDelegate.OnCefStatusMessage(int browserId, string message)
        {
            StatusMessage?.Invoke(browserId, message);
        }

        void ICefViewDelegate.OnCefConsoleMessage(int browserId, string message, int level)
        {
            ConsoleMessage?.Invoke(browserId, message, level);
        }

        void ICefViewDelegate.OnCefLoadingProgressChanged(int browserId, double progress)
        {
            LoadingProgressChanged?.Invoke(browserId, progress);
        }

        void ICefViewDelegate.OnCefCursorChanged(int browserId, CefViewCursorType type, CefViewCursorInfo customCursorInfo)
        {
            UI_OnCefCursorChanged(browserId, type, customCursorInfo);
        }
        #endregion

        #region FocusHandler
        void ICefViewDelegate.OnCefFocusReleasedByTabKey(int browserId, bool next)
        {
            UI_OnCefFocusReleasedByTabKey(browserId, next);
        }

        bool ICefViewDelegate.OnCefSetFocus(int browserId)
        {
            return false;
        }

        void ICefViewDelegate.OnCefGotFocus(int browserId)
        {
            _hasCefGotFocus = true;
        }
        #endregion

        #region LifespanHandler
        void ICefViewDelegate.OnCefAfterCreated()
        {
            UI_OnCefAfterCreated();
        }
        #endregion

        #region LoadHandler
        void ICefViewDelegate.OnCefLoadingStateChanged(int browserId, bool isLoading, bool canGoBack, bool canGoForward)
        {
            LoadingStateChanged?.Invoke(browserId, isLoading, canGoBack, canGoForward);
        }

        void ICefViewDelegate.OnCefLoadStart(int browserId, string frameId, bool isMainFrame, int transition_type)
        {
            LoadStart?.Invoke(browserId, frameId, isMainFrame, transition_type);
        }

        void ICefViewDelegate.OnCefLoadEnd(int browserId, string frameId, bool isMainFrame, int httpStatusCode)
        {
            LoadEnd?.Invoke(browserId, frameId, isMainFrame, httpStatusCode);
        }

        bool ICefViewDelegate.OnCefLoadError(int browserId, string frameId, bool isMainFrame, int errorCode, string errorMsg, string failedUrl)
        {
            if (null != LoadError)
                return LoadError.Invoke(browserId, frameId, isMainFrame, errorCode, errorMsg, failedUrl);

            return false;
        }
        #endregion

        #region RenderHandler
        void ICefViewDelegate.OnCefGetRootScreenRect(int browserId, ref CefViewRect rect)
        {
            UI_OnCefGetRootScreenRect(browserId, ref rect);
        }

        void ICefViewDelegate.OnCefGetViewRect(int browserId, ref CefViewRect rect)
        {
            UI_OnCefGetViewRect(browserId, ref rect);
        }

        bool ICefViewDelegate.OnCefGetScreenPoint(int browserId, int viewX, int viewY, ref int screenX, ref int screenY)
        {
            return UI_OnCefGetScreenPoint(browserId, viewX, viewY, ref screenX, ref screenY);
        }

        bool ICefViewDelegate.OnCefGetScreenInfo(int browserId, ref CefViewScreenInfo info)
        {
            return UI_OnCefGetScreenInfo(browserId, ref info);
        }

        void ICefViewDelegate.OnCefPopupShow(int browserId, bool show)
        {
            _isShowPopup = show;
        }

        void ICefViewDelegate.OnCefPopupSize(int browserId, CefViewRect rect)
        {
            UI_OnCefPopupSize(browserId, rect);
        }

        void ICefViewDelegate.OnCefPaint(int browserId, CefViewPaintElementType type, CefViewRect[] dirtyRects, int dirtyRectCount, IntPtr imageBytesBuffer, int imageBytesCount, int width, int height)
        {
            UI_OnCefPaint(browserId, type, dirtyRects, dirtyRectCount, imageBytesBuffer, imageBytesCount, width, height);
        }

        void ICefViewDelegate.OnCefAcceleratedPaint(int browserId, CefViewPaintElementType type, CefViewRect[] dirtyRects, int dirtyRectCount, IntPtr sharedHandle, int planeBytesCount)
        {
            UI_OnCefAcceleratedPaint(browserId, type, dirtyRects, dirtyRectCount, sharedHandle, planeBytesCount);
        }

        void ICefViewDelegate.OnCefImeCompositionRangeChanged(int browserId, CefViewRange selectedRange, CefViewRect[] characterBounds, int characterBoundsCount)
        {
            UI_OnCefImeCompositionRangeChanged(browserId, selectedRange, characterBounds, characterBoundsCount);
        }

        public void OnCefTextSelectionChanged(int browserId, string selectedText, CefViewRange selectedRange)
        {
            UI_OnCefImeTextSelectionChanged(browserId, selectedText, selectedRange);
        }
        #endregion
        #endregion
    }
}
