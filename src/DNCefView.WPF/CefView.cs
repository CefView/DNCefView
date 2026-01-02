using System;
using System.Collections.Generic;
using System.Text.Json;

namespace DNCefView.WPF
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CefView : ICefViewDelegate
    {
        /// <summary>
        /// 
        /// </summary>
        protected CefSetting _cefSetting;
        public CefSetting Setting
        {
            get { return _cefSetting; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected bool _editable;
        public bool Editable
        {
            get { return _editable; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected CefBrowser _cefBrowser;

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
        public event CefQueryRequestCallback CefQueryRequest;

        public delegate void InvokeMethodCallback(int browserId, string frameId, string method, List<dynamic> arguments);
        public event InvokeMethodCallback InvokeMethod;

        public delegate void ReportJavascriptResultCallback(int browserId, string frameId, string context, string result);
        public event ReportJavascriptResultCallback ReportJavascriptResult;

        public delegate void InputStateChangedCallback(int browserId, string frameId, bool editable);
        public event InputStateChangedCallback InputStateChanged;

        public delegate void LoadingStateChangedCallback(int browserId, bool isLoading, bool canGoBack, bool canGoForward);
        public event LoadingStateChangedCallback LoadingStateChanged;

        public delegate void LoadStartCallback(int browserId, string frameId, bool isMainFrame, int transition_type);
        public event LoadStartCallback LoadStart;

        public delegate void LoadEndCallback(int browserId, string frameId, bool isMainFrame, int httpStatusCode);
        public event LoadEndCallback LoadEnd;

        public delegate bool LoadErrorCallback(int browserId, string frameId, bool isMainFrame, int errorCode, string errorMsg, string failedUrl);
        public LoadErrorCallback LoadError;

        public delegate void AddressChangedCallback(string frameId, string url);
        public event AddressChangedCallback AddressChanged;

        public delegate void TitleChangedCallback(string title);
        public event TitleChangedCallback TitleChanged;

        public delegate void FullScreenModeChangedCallback(bool fullscreen);
        public event FullScreenModeChangedCallback FullScreenModeChanged;

        public delegate void StatusMessageCallback(string message);
        public event StatusMessageCallback StatusMessage;

        public delegate void ConsoleMessageCallback(string message, int level);
        public event ConsoleMessageCallback ConsoleMessage;

        public delegate void LoadingProgressChangedCallback(double progress);
        public event LoadingProgressChangedCallback LoadingProgressChanged;
        #endregion

        private void InitializeNative(string url, CefSetting setting)
        {
            if (null != _cefSetting)
            {
                _cefSetting = setting;
            }
            else
            {
                _cefSetting = new CefSetting();
                _cefSetting.SetWindowlessFrameRate(60);
            }

            _cefBrowser = new CefBrowser(this, url, _cefSetting);
        }

        #region Browser Methods
        public void AddLocalFolderResource(string path, string url, int priority)
        {
            _cefBrowser.AddLocalFolderResource(path, url, priority);
        }

        public void AddArchiveResource(string path, string url, string password, int priority)
        {
            _cefBrowser.AddArchiveResource(path, url, password, priority);
        }

        public int BrowserId()
        {
            return _cefBrowser.BrowserId();
        }

        public void NavigateToString(string content)
        {
            _cefBrowser.NavigateToString(content);
        }

        public void NavigateToUrl(string url)
        {
            _cefBrowser.NavigateToUrl(url);
        }

        public bool CanGoBack()
        {
            return _cefBrowser.CanGoBack();
        }

        public bool CanGoForward()
        {
            return _cefBrowser.CanGoForward();
        }

        public void GoBack()
        {
            _cefBrowser.GoBack();
        }

        public void GoForward()
        {
            _cefBrowser.GoForward();
        }

        public bool IsLoading()
        {
            return _cefBrowser.IsLoading();
        }

        public void Reload()
        {
            _cefBrowser.Reload();
        }

        public void StopLoad()
        {
            _cefBrowser.StopLoad();
        }

        public bool TriggerEventOnMainFrame(string evtName, List<object> evtArgs)
        {
            var argsString = JsonSerializer.Serialize(evtArgs);
            return _cefBrowser.TriggerEventOnMainFrame(evtName, argsString);
        }

        public bool TriggerEventOnFrame(string evtName, List<object> evtArgs, string frameId)
        {
            var argsString = JsonSerializer.Serialize(evtArgs);
            return _cefBrowser.TriggerEventOnFrame(evtName, argsString, frameId);
        }

        public bool BroadcastEvent(string evtName, List<object> evtArgs)
        {
            var argsString = JsonSerializer.Serialize(evtArgs);
            return _cefBrowser.BroadcastEvent(evtName, argsString);
        }

        public bool TriggerEvent(string name, List<object> args, string frameId)
        {
            var argsString = JsonSerializer.Serialize(args);
            return _cefBrowser.TriggerEvent(name, argsString, frameId);
        }

        public bool ResponseQCefQuery(CefQuery query)
        {
            return _cefBrowser.ResponseQCefQuery(query);
        }

        public bool ExecuteJavascript(string frameId, string code, string url)
        {
            return _cefBrowser.ExecuteJavascript(frameId, code, url);
        }

        public bool ExecuteJavascriptWithResult(string frameId, string code, string url, string context)
        {
            return (_cefBrowser.ExecuteJavascriptWithResult(frameId, code, url, context));
        }

        public bool SetPreference(string name, string value)
        {
            return _cefBrowser.SetPreference(name, value);
        }

        public void ImeSetComposition(string text, CefViewCompositionUnderline[] underlines, CefViewRange replacement_range, CefViewRange selection_range)
        {
            _cefBrowser.ImeSetComposition(text, underlines, underlines.Length, replacement_range, selection_range);
        }

        public void ImeCommitText(string text, CefViewRange replacement_range, int relative_cursor_pos)
        {
            _cefBrowser.ImeCommitText(text, replacement_range, relative_cursor_pos);
        }

        public void ImeFinishComposingText(bool keep_selection)
        {
            _cefBrowser.ImeFinishComposingText(keep_selection);
        }
        #endregion

        #region CEF Callbacks
        void ICefViewDelegate.OnCefQueryRequest(int browserId, string frameId, CefQuery query)
        {
            CefQueryRequest?.Invoke(browserId, frameId, query);
        }

        void ICefViewDelegate.OnCefInvokeMethod(int browserId, string frameId, string method, string arguments)
        {
            List<dynamic> argList;
            try
            {
                argList = JsonSerializer.Deserialize<List<dynamic>>(arguments);
            }
            catch (Exception)
            {
                argList = new List<dynamic>();
            }

            InvokeMethod?.Invoke(browserId, frameId, method, argList);
        }

        void ICefViewDelegate.OnCefReportJavascriptResult(int browserId, string frameId, string context, string result)
        {
            ReportJavascriptResult?.Invoke(browserId, frameId, context, result);
        }

        void ICefViewDelegate.OnCefInputStateChanged(int browserId, string frameId, bool editable)
        {
            _editable = editable;
            if (WPF_OnCefInputStateChanged(browserId, frameId, editable))
                InputStateChanged?.Invoke(browserId, frameId, editable);
        }

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

        void ICefViewDelegate.OnCefAddressChanged(string frameId, string url)
        {
            AddressChanged?.Invoke(frameId, url);
        }

        void ICefViewDelegate.OnCefTitleChanged(string title)
        {
            TitleChanged?.Invoke(title);
        }

        void ICefViewDelegate.OnCefFullScreenModeChanged(bool fullscreen)
        {
            FullScreenModeChanged?.Invoke(fullscreen);
        }

        void ICefViewDelegate.OnCefStatusMessage(string message)
        {
            StatusMessage?.Invoke(message);
        }

        void ICefViewDelegate.OnCefConsoleMessage(string message, int level)
        {
            ConsoleMessage?.Invoke(message, level);
        }

        void ICefViewDelegate.OnCefLoadingProgressChanged(double progress)
        {
            LoadingProgressChanged?.Invoke(progress);
        }

        void ICefViewDelegate.OnCefAfterCreated()
        {
            WPF_OnCefAfterCreated();
        }

        void ICefViewDelegate.OnCefFocusReleasedByTabKey(int browserId, bool next)
        {
            WPF_OnCefFocusReleasedByTabKey(browserId, next);
        }

        bool ICefViewDelegate.OnCefSetFocus(int browserId)
        {
            return false;
        }

        void ICefViewDelegate.OnCefGotFocus(int browserId)
        {

        }

        void ICefViewDelegate.OnCefGetRootScreenRect(int browserId, ref CefViewRect rect)
        {
            WPF_OnCefGetRootScreenRect(browserId, ref rect);
        }

        void ICefViewDelegate.OnCefGetViewRect(int browserId, ref CefViewRect rect)
        {
            WPF_OnCefGetViewRect(browserId, ref rect);
        }

        bool ICefViewDelegate.OnCefGetScreenPoint(int browserId, int viewX, int viewY, ref int screenX, ref int screenY)
        {
            return WPF_OnCefGetScreenPoint(browserId, viewX, viewY, ref screenX, ref screenY);
        }

        bool ICefViewDelegate.OnCefGetScreenInfo(int browserId, ref CefViewScreenInfo info)
        {
            return WPF_OnCefGetScreenInfo(browserId, ref info);
        }

        void ICefViewDelegate.OnCefPopupShow(int browserId, bool show)
        {
            _isShowPopup = show;
        }

        void ICefViewDelegate.OnCefPopupSize(int browserId, CefViewRect rect)
        {
            WPF_OnCefPopupSize(browserId, rect);
        }

        void ICefViewDelegate.OnCefPaint(int browserId, CefViewPaintElementType type, CefViewRect[] dirtyRects, int dirtyRectCount, byte[] imageBytes, int imageBytesCount, int width, int height)
        {
            WPF_OnCefPaint(browserId, type, dirtyRects, dirtyRectCount, imageBytes, imageBytesCount, width, height);
        }

        void ICefViewDelegate.OnCefImeCompositionRangeChanged(int browserId, CefViewRange range, CefViewRect[] characterBounds, int characterBoundsCount)
        {
            WPF_OnCefImeCompositionRangeChanged(browserId, range, characterBounds, characterBoundsCount);
        }
        #endregion
    }
}
