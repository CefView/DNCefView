#pragma warning disable CS8603
using System;
using System.Runtime.InteropServices;

namespace DNCefView
{
    // Source: CCefBrowser 
    public partial class CefBrowser : IDisposable
    {
        private IntPtr _native;
        public IntPtr NativeObject
        {
            get { return _native; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [DllImport("CCefView")]
        private static extern void CCefBrowser_Delete(IntPtr p);
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // TODO: cleanup the managed resources
            }

            // cleanup unmanaged resources
            if (_native != IntPtr.Zero)
            {
                CCefBrowser_Delete(_native);
                _native = IntPtr.Zero;
            }
        }

        // Source: CCefBrowser(CefBrowserCallback, const std::string &, const CCefSetting *)
        [DllImport("CCefView")]
        private static extern IntPtr CCefBrowser_new0(CefBrowserCallback callback, [MarshalAs(UnmanagedType.LPUTF8Str)] string url, IntPtr setting);

        // Source: void addLocalFolderResource(const std::string &, const std::string &, int)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_addLocalFolderResource(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string path, [MarshalAs(UnmanagedType.LPUTF8Str)] string url, int priority);
        public void AddLocalFolderResource(string path, string url, int priority)
        {
            CCefBrowser_addLocalFolderResource(_native, path, url, priority);
        }

        // Source: void addArchiveResource(const std::string &, const std::string &, const std::string &, int)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_addArchiveResource(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string path, [MarshalAs(UnmanagedType.LPUTF8Str)] string url, [MarshalAs(UnmanagedType.LPUTF8Str)] string password, int priority);
        public void AddArchiveResource(string path, string url, string password, int priority)
        {
            CCefBrowser_addArchiveResource(_native, path, url, password, priority);
        }

        // Source: int browserId()
        [DllImport("CCefView")]
        private static extern int CCefBrowser_browserId(IntPtr thiz);
        public int BrowserId()
        {
            return CCefBrowser_browserId(_native);
        }

        // Source: void navigateToString(const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_navigateToString(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string content);
        public void NavigateToString(string content)
        {
            CCefBrowser_navigateToString(_native, content);
        }

        // Source: void navigateToUrl(const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_navigateToUrl(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string url);
        public void NavigateToUrl(string url)
        {
            CCefBrowser_navigateToUrl(_native, url);
        }

        // Source: bool canGoBack()
        [DllImport("CCefView")]
        private static extern bool CCefBrowser_canGoBack(IntPtr thiz);
        public bool CanGoBack()
        {
            return CCefBrowser_canGoBack(_native);
        }

        // Source: bool canGoForward()
        [DllImport("CCefView")]
        private static extern bool CCefBrowser_canGoForward(IntPtr thiz);
        public bool CanGoForward()
        {
            return CCefBrowser_canGoForward(_native);
        }

        // Source: void goBack()
        [DllImport("CCefView")]
        private static extern void CCefBrowser_goBack(IntPtr thiz);
        public void GoBack()
        {
            CCefBrowser_goBack(_native);
        }

        // Source: void goForward()
        [DllImport("CCefView")]
        private static extern void CCefBrowser_goForward(IntPtr thiz);
        public void GoForward()
        {
            CCefBrowser_goForward(_native);
        }

        // Source: bool isLoading()
        [DllImport("CCefView")]
        private static extern bool CCefBrowser_isLoading(IntPtr thiz);
        public bool IsLoading()
        {
            return CCefBrowser_isLoading(_native);
        }

        // Source: void reload()
        [DllImport("CCefView")]
        private static extern void CCefBrowser_reload(IntPtr thiz);
        public void Reload()
        {
            CCefBrowser_reload(_native);
        }

        // Source: void stopLoad()
        [DllImport("CCefView")]
        private static extern void CCefBrowser_stopLoad(IntPtr thiz);
        public void StopLoad()
        {
            CCefBrowser_stopLoad(_native);
        }

        // Source: bool triggerEventOnMainFrame(const std::string &, const std::string &)
        [DllImport("CCefView")]
        private static extern bool CCefBrowser_triggerEventOnMainFrame(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string evtName, [MarshalAs(UnmanagedType.LPUTF8Str)] string evtArgs);
        public bool TriggerEventOnMainFrame(string evtName, string evtArgs)
        {
            return CCefBrowser_triggerEventOnMainFrame(_native, evtName, evtArgs);
        }

        // Source: bool triggerEventOnFrame(const std::string &, const std::string &, const std::string &)
        [DllImport("CCefView")]
        private static extern bool CCefBrowser_triggerEventOnFrame(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string evtName, [MarshalAs(UnmanagedType.LPUTF8Str)] string evtArgs, [MarshalAs(UnmanagedType.LPUTF8Str)] string frameId);
        public bool TriggerEventOnFrame(string evtName, string evtArgs, string frameId)
        {
            return CCefBrowser_triggerEventOnFrame(_native, evtName, evtArgs, frameId);
        }

        // Source: bool broadcastEvent(const std::string &, const std::string &)
        [DllImport("CCefView")]
        private static extern bool CCefBrowser_broadcastEvent(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string evtName, [MarshalAs(UnmanagedType.LPUTF8Str)] string evtArgs);
        public bool BroadcastEvent(string evtName, string evtArgs)
        {
            return CCefBrowser_broadcastEvent(_native, evtName, evtArgs);
        }

        // Source: bool triggerEvent(const std::string &, const std::string &, const std::string &)
        [DllImport("CCefView")]
        private static extern bool CCefBrowser_triggerEvent(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string args, [MarshalAs(UnmanagedType.LPUTF8Str)] string frameId);
        public bool TriggerEvent(string name, string args, string frameId)
        {
            return CCefBrowser_triggerEvent(_native, name, args, frameId);
        }

        // Source: bool responseQCefQuery(const CCefQuery *)
        [DllImport("CCefView")]
        private static extern bool CCefBrowser_responseQCefQuery(IntPtr thiz, IntPtr query);
        public bool ResponseQCefQuery(CefQuery query)
        {
            return CCefBrowser_responseQCefQuery(_native, query.NativeObject);
        }

        // Source: bool executeJavascript(const std::string &, const std::string &, const std::string &)
        [DllImport("CCefView")]
        private static extern bool CCefBrowser_executeJavascript(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string frameId, [MarshalAs(UnmanagedType.LPUTF8Str)] string code, [MarshalAs(UnmanagedType.LPUTF8Str)] string url);
        public bool ExecuteJavascript(string frameId, string code, string url)
        {
            return CCefBrowser_executeJavascript(_native, frameId, code, url);
        }

        // Source: bool executeJavascriptWithResult(const std::string &, const std::string &, const std::string &, const std::string &)
        [DllImport("CCefView")]
        private static extern bool CCefBrowser_executeJavascriptWithResult(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string frameId, [MarshalAs(UnmanagedType.LPUTF8Str)] string code, [MarshalAs(UnmanagedType.LPUTF8Str)] string url, [MarshalAs(UnmanagedType.LPUTF8Str)] string context);
        public bool ExecuteJavascriptWithResult(string frameId, string code, string url, string context)
        {
            return CCefBrowser_executeJavascriptWithResult(_native, frameId, code, url, context);
        }

        // Source: bool setPreference(const std::string &, const std::string &)
        [DllImport("CCefView")]
        private static extern bool CCefBrowser_setPreference(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string value);
        public bool SetPreference(string name, string value)
        {
            return CCefBrowser_setPreference(_native, name, value);
        }

        // Source: void setDisablePopupContextMenu(bool)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_setDisablePopupContextMenu(IntPtr thiz, bool disable);
        public void SetDisablePopupContextMenu(bool disable)
        {
            CCefBrowser_setDisablePopupContextMenu(_native, disable);
        }

        // Source: bool isPopupContextMenuDisabled()
        [DllImport("CCefView")]
        private static extern bool CCefBrowser_isPopupContextMenuDisabled(IntPtr thiz);
        public bool IsPopupContextMenuDisabled()
        {
            return CCefBrowser_isPopupContextMenuDisabled(_native);
        }

        // Source: void setWindowlessFrameRate(int)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_setWindowlessFrameRate(IntPtr thiz, int rate);
        public void SetWindowlessFrameRate(int rate)
        {
            CCefBrowser_setWindowlessFrameRate(_native, rate);
        }

        // Source: void sendExternalBeginFrame()
        [DllImport("CCefView")]
        private static extern void CCefBrowser_sendExternalBeginFrame(IntPtr thiz);
        public void SendExternalBeginFrame()
        {
            CCefBrowser_sendExternalBeginFrame(_native);
        }

        // Source: void showDevTools()
        [DllImport("CCefView")]
        private static extern void CCefBrowser_showDevTools(IntPtr thiz);
        public void ShowDevTools()
        {
            CCefBrowser_showDevTools(_native);
        }

        // Source: void closeDevTools()
        [DllImport("CCefView")]
        private static extern void CCefBrowser_closeDevTools(IntPtr thiz);
        public void CloseDevTools()
        {
            CCefBrowser_closeDevTools(_native);
        }

        // Source: bool hasDevTools()
        [DllImport("CCefView")]
        private static extern bool CCefBrowser_hasDevTools(IntPtr thiz);
        public bool HasDevTools()
        {
            return CCefBrowser_hasDevTools(_native);
        }

        // Source: void closeBrowser(bool)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_closeBrowser(IntPtr thiz, bool forceClose);
        public void CloseBrowser(bool forceClose)
        {
            CCefBrowser_closeBrowser(_native, forceClose);
        }

        // Source: bool continueJSDialog(int64_t, bool, const std::string &)
        [DllImport("CCefView")]
        private static extern bool CCefBrowser_continueJSDialog(IntPtr thiz, long requestId, bool success, [MarshalAs(UnmanagedType.LPUTF8Str)] string userInput);
        public bool ContinueJSDialog(long requestId, bool success, string userInput)
        {
            return CCefBrowser_continueJSDialog(_native, requestId, success, userInput);
        }
        // Source: void setFocus(bool)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_setFocus(IntPtr thiz, bool focused);
        public void SetFocus(bool focused)
        {
            CCefBrowser_setFocus(_native, focused);
        }

        // Source: void wasResized()
        [DllImport("CCefView")]
        private static extern void CCefBrowser_wasResized(IntPtr thiz);
        public void WasResized()
        {
            CCefBrowser_wasResized(_native);
        }

        // Source: void wasHidden(bool)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_wasHidden(IntPtr thiz, bool hidden);
        public void WasHidden(bool hidden)
        {
            CCefBrowser_wasHidden(_native, hidden);
        }

        // Source: void sendMouseMoveEvent(int, int, uint32_t, bool)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_sendMouseMoveEvent(IntPtr thiz, int x, int y, UInt32 modifiers, bool leave);
        public void SendMouseMoveEvent(int x, int y, UInt32 modifiers, bool leave)
        {
            CCefBrowser_sendMouseMoveEvent(_native, x, y, modifiers, leave);
        }

        // Source: void sendMouseClickEvent(int, int, uint32_t, CefViewMouseButtonType, bool, int)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_sendMouseClickEvent(IntPtr thiz, int x, int y, UInt32 modifiers, CefViewMouseButtonType type, bool mouseUp, int clickCount);
        public void SendMouseClickEvent(int x, int y, UInt32 modifiers, CefViewMouseButtonType type, bool mouseUp, int clickCount)
        {
            CCefBrowser_sendMouseClickEvent(_native, x, y, modifiers, type, mouseUp, clickCount);
        }

        // Source: void sendWheelEvent(int, int, uint32_t, int, int)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_sendWheelEvent(IntPtr thiz, int x, int y, UInt32 modifiers, int deltaX, int deltaY);
        public void SendWheelEvent(int x, int y, UInt32 modifiers, int deltaX, int deltaY)
        {
            CCefBrowser_sendWheelEvent(_native, x, y, modifiers, deltaX, deltaY);
        }
        // Source: void dragTargetDragEnterText(int, int, uint32_t, const std::string &, const std::string &, const std::string &, CefViewDragOperation)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_dragTargetDragEnterText(IntPtr thiz, int x, int y, UInt32 modifiers, [MarshalAs(UnmanagedType.LPUTF8Str)] string text, [MarshalAs(UnmanagedType.LPUTF8Str)] string html, [MarshalAs(UnmanagedType.LPUTF8Str)] string baseUrl, CefViewDragOperation allowedOps);
        public void DragTargetDragEnterText(int x, int y, UInt32 modifiers, string text, string html, string baseUrl, CefViewDragOperation allowedOps)
        {
            CCefBrowser_dragTargetDragEnterText(_native, x, y, modifiers, text, html, baseUrl, allowedOps);
        }

        // Source: void dragTargetDragEnterFiles(int, int, uint32_t, const std::string &, CefViewDragOperation)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_dragTargetDragEnterFiles(IntPtr thiz, int x, int y, UInt32 modifiers, [MarshalAs(UnmanagedType.LPUTF8Str)] string filePaths, CefViewDragOperation allowedOps);
        public void DragTargetDragEnterFiles(int x, int y, UInt32 modifiers, string filePaths, CefViewDragOperation allowedOps)
        {
            CCefBrowser_dragTargetDragEnterFiles(_native, x, y, modifiers, filePaths, allowedOps);
        }

        // Source: void dragTargetDragOver(int, int, uint32_t, CefViewDragOperation)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_dragTargetDragOver(IntPtr thiz, int x, int y, UInt32 modifiers, CefViewDragOperation allowedOps);
        public void DragTargetDragOver(int x, int y, UInt32 modifiers, CefViewDragOperation allowedOps)
        {
            CCefBrowser_dragTargetDragOver(_native, x, y, modifiers, allowedOps);
        }

        // Source: void dragTargetDragLeave()
        [DllImport("CCefView")]
        private static extern void CCefBrowser_dragTargetDragLeave(IntPtr thiz);
        public void DragTargetDragLeave()
        {
            CCefBrowser_dragTargetDragLeave(_native);
        }

        // Source: void dragTargetDrop(int, int, uint32_t)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_dragTargetDrop(IntPtr thiz, int x, int y, UInt32 modifiers);
        public void DragTargetDrop(int x, int y, UInt32 modifiers)
        {
            CCefBrowser_dragTargetDrop(_native, x, y, modifiers);
        }

        // Source: void dragSourceEndedAt(int, int, CefViewDragOperation)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_dragSourceEndedAt(IntPtr thiz, int x, int y, CefViewDragOperation operation);
        public void DragSourceEndedAt(int x, int y, CefViewDragOperation operation)
        {
            CCefBrowser_dragSourceEndedAt(_native, x, y, operation);
        }

        // Source: void dragSourceSystemDragEnded()
        [DllImport("CCefView")]
        private static extern void CCefBrowser_dragSourceSystemDragEnded(IntPtr thiz);
        public void DragSourceSystemDragEnded()
        {
            CCefBrowser_dragSourceSystemDragEnded(_native);
        }

        // Source: void sendTouchEvent(int, float, float, float, float, float, float, int, uint32_t, int)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_sendTouchEvent(IntPtr thiz, int touchId, float x, float y, float radiusX, float radiusY, float rotationAngle, float pressure, int touchEventType, UInt32 modifiers, int pointerType);
        public void SendTouchEvent(int touchId, float x, float y, float radiusX, float radiusY, float rotationAngle, float pressure, int touchEventType, UInt32 modifiers, int pointerType)
        {
            CCefBrowser_sendTouchEvent(_native, touchId, x, y, radiusX, radiusY, rotationAngle, pressure, touchEventType, modifiers, pointerType);
        }

        // Source: void sendKeyEvent(CefViewKeyEventType, uint32_t, int, int, bool, uint16_t, uint16_t, bool)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_sendKeyEvent(IntPtr thiz, CefViewKeyEventType type, UInt32 modifiers, int windowsKeyCode, int nativeKeyCode, bool isSysKey, UInt16 character, UInt16 umodifiedCharacter, bool isFocusOnEditableField);
        public void SendKeyEvent(CefViewKeyEventType type, UInt32 modifiers, int windowsKeyCode, int nativeKeyCode, bool isSysKey, UInt16 character, UInt16 umodifiedCharacter, bool isFocusOnEditableField)
        {
            CCefBrowser_sendKeyEvent(_native, type, modifiers, windowsKeyCode, nativeKeyCode, isSysKey, character, umodifiedCharacter, isFocusOnEditableField);
        }

        // Source: void notifyMoveOrResizeStarted()
        [DllImport("CCefView")]
        private static extern void CCefBrowser_notifyMoveOrResizeStarted(IntPtr thiz);
        public void NotifyMoveOrResizeStarted()
        {
            CCefBrowser_notifyMoveOrResizeStarted(_native);
        }

        // Source: void notifyScreenChanged()
        [DllImport("CCefView")]
        private static extern void CCefBrowser_notifyScreenChanged(IntPtr thiz);
        public void NotifyScreenChanged()
        {
            CCefBrowser_notifyScreenChanged(_native);
        }

        // Source: void imeSetComposition(const std::string &, CefViewCompositionUnderline *, int, CefViewRange, CefViewRange)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_imeSetComposition(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string text, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] CefViewCompositionUnderline[] underlines, int count, CefViewRange replacement_range, CefViewRange selection_range);
        public void ImeSetComposition(string text, CefViewCompositionUnderline[] underlines, int count, CefViewRange replacement_range, CefViewRange selection_range)
        {
            CCefBrowser_imeSetComposition(_native, text, underlines, count, replacement_range, selection_range);
        }

        // Source: void imeCommitText(const std::string &, CefViewRange, int)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_imeCommitText(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string text, CefViewRange replacement_range, int relative_cursor_pos);
        public void ImeCommitText(string text, CefViewRange replacement_range, int relative_cursor_pos)
        {
            CCefBrowser_imeCommitText(_native, text, replacement_range, relative_cursor_pos);
        }

        // Source: void imeFinishComposingText(bool)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_imeFinishComposingText(IntPtr thiz, bool keep_selection);
        public void ImeFinishComposingText(bool keep_selection)
        {
            CCefBrowser_imeFinishComposingText(_native, keep_selection);
        }

        // Source: void imeCancelComposition()
        [DllImport("CCefView")]
        private static extern void CCefBrowser_imeCancelComposition(IntPtr thiz);
        public void ImeCancelComposition()
        {
            CCefBrowser_imeCancelComposition(_native);
        }

    }
}
