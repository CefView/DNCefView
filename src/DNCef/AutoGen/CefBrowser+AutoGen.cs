using System;
using System.Runtime.InteropServices;

namespace DNCef
{
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

        [DllImport("CCefView")]
        private static extern IntPtr CCefBrowser_new0(CefBrowserCallback callback, [MarshalAs(UnmanagedType.LPUTF8Str)] string url, IntPtr setting);

        [DllImport("CCefView")]
        private static extern void CCefBrowser_addLocalFolderResource(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string path, [MarshalAs(UnmanagedType.LPUTF8Str)] string url, Int32 priority);
        public void AddLocalFolderResource(string path, string url, Int32 priority)
        {
            CCefBrowser_addLocalFolderResource(_native, path, url, priority);
        }

        [DllImport("CCefView")]
        private static extern void CCefBrowser_addArchiveResource(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string path, [MarshalAs(UnmanagedType.LPUTF8Str)] string url, [MarshalAs(UnmanagedType.LPUTF8Str)] string password, Int32 priority);
        public void AddArchiveResource(string path, string url, string password, Int32 priority)
        {
            CCefBrowser_addArchiveResource(_native, path, url, password, priority);
        }

        [DllImport("CCefView")]
        private static extern Int32 CCefBrowser_browserId(IntPtr thiz);
        public Int32 BrowserId()
        {
            return CCefBrowser_browserId(_native);
        }

        [DllImport("CCefView")]
        private static extern void CCefBrowser_navigateToString(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string content);
        public void NavigateToString(string content)
        {
            CCefBrowser_navigateToString(_native, content);
        }

        [DllImport("CCefView")]
        private static extern void CCefBrowser_navigateToUrl(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string url);
        public void NavigateToUrl(string url)
        {
            CCefBrowser_navigateToUrl(_native, url);
        }

        [DllImport("CCefView")]
        private static extern bool CCefBrowser_canGoBack(IntPtr thiz);
        public bool CanGoBack()
        {
            return CCefBrowser_canGoBack(_native);
        }

        [DllImport("CCefView")]
        private static extern bool CCefBrowser_canGoForward(IntPtr thiz);
        public bool CanGoForward()
        {
            return CCefBrowser_canGoForward(_native);
        }

        [DllImport("CCefView")]
        private static extern void CCefBrowser_goBack(IntPtr thiz);
        public void GoBack()
        {
            CCefBrowser_goBack(_native);
        }

        [DllImport("CCefView")]
        private static extern void CCefBrowser_goForward(IntPtr thiz);
        public void GoForward()
        {
            CCefBrowser_goForward(_native);
        }

        [DllImport("CCefView")]
        private static extern bool CCefBrowser_isLoading(IntPtr thiz);
        public bool IsLoading()
        {
            return CCefBrowser_isLoading(_native);
        }

        [DllImport("CCefView")]
        private static extern void CCefBrowser_reload(IntPtr thiz);
        public void Reload()
        {
            CCefBrowser_reload(_native);
        }

        [DllImport("CCefView")]
        private static extern void CCefBrowser_stopLoad(IntPtr thiz);
        public void StopLoad()
        {
            CCefBrowser_stopLoad(_native);
        }

        [DllImport("CCefView")]
        private static extern bool CCefBrowser_triggerEventOnMainFrame(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string evtName, [MarshalAs(UnmanagedType.LPUTF8Str)] string evtArgs);
        public bool TriggerEventOnMainFrame(string evtName, string evtArgs)
        {
            return CCefBrowser_triggerEventOnMainFrame(_native, evtName, evtArgs);
        }

        [DllImport("CCefView")]
        private static extern bool CCefBrowser_triggerEventOnFrame(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string evtName, [MarshalAs(UnmanagedType.LPUTF8Str)] string evtArgs, Int64 frameId);
        public bool TriggerEventOnFrame(string evtName, string evtArgs, Int64 frameId)
        {
            return CCefBrowser_triggerEventOnFrame(_native, evtName, evtArgs, frameId);
        }

        [DllImport("CCefView")]
        private static extern bool CCefBrowser_broadcastEvent(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string evtName, [MarshalAs(UnmanagedType.LPUTF8Str)] string evtArgs);
        public bool BroadcastEvent(string evtName, string evtArgs)
        {
            return CCefBrowser_broadcastEvent(_native, evtName, evtArgs);
        }

        [DllImport("CCefView")]
        private static extern bool CCefBrowser_triggerEvent(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string args, Int64 frameId);
        public bool TriggerEvent(string name, string args, Int64 frameId)
        {
            return CCefBrowser_triggerEvent(_native, name, args, frameId);
        }

        [DllImport("CCefView")]
        private static extern bool CCefBrowser_responseQCefQuery(IntPtr thiz, IntPtr query);
        public bool ResponseQCefQuery(CefQuery query)
        {
            return CCefBrowser_responseQCefQuery(_native, query.NativeObject);
        }

        [DllImport("CCefView")]
        private static extern bool CCefBrowser_executeJavascript(IntPtr thiz, Int64 frameId, [MarshalAs(UnmanagedType.LPUTF8Str)] string code, [MarshalAs(UnmanagedType.LPUTF8Str)] string url);
        public bool ExecuteJavascript(Int64 frameId, string code, string url)
        {
            return CCefBrowser_executeJavascript(_native, frameId, code, url);
        }

        [DllImport("CCefView")]
        private static extern bool CCefBrowser_executeJavascriptWithResult(IntPtr thiz, Int64 frameId, [MarshalAs(UnmanagedType.LPUTF8Str)] string code, [MarshalAs(UnmanagedType.LPUTF8Str)] string url, Int64 context);
        public bool ExecuteJavascriptWithResult(Int64 frameId, string code, string url, Int64 context)
        {
            return CCefBrowser_executeJavascriptWithResult(_native, frameId, code, url, context);
        }

        [DllImport("CCefView")]
        private static extern bool CCefBrowser_setPreference(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string value);
        public bool SetPreference(string name, string value)
        {
            return CCefBrowser_setPreference(_native, name, value);
        }

        [DllImport("CCefView")]
        private static extern void CCefBrowser_setDisablePopupContextMenu(IntPtr thiz, bool disable);
        public void SetDisablePopupContextMenu(bool disable)
        {
            CCefBrowser_setDisablePopupContextMenu(_native, disable);
        }

        [DllImport("CCefView")]
        private static extern bool CCefBrowser_isPopupContextMenuDisabled(IntPtr thiz);
        public bool IsPopupContextMenuDisabled()
        {
            return CCefBrowser_isPopupContextMenuDisabled(_native);
        }

        [DllImport("CCefView")]
        private static extern void CCefBrowser_ImeSetComposition(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string text, CefViewCompositionUnderline[] underlines, Int32 count, CefViewRange replacement_range, CefViewRange selection_range);
        public void ImeSetComposition(string text, CefViewCompositionUnderline[] underlines, CefViewRange replacement_range, CefViewRange selection_range)
        {
            CCefBrowser_ImeSetComposition(_native, text, underlines, underlines.Length, replacement_range, selection_range);
        }

        [DllImport("CCefView")]
        private static extern void CCefBrowser_ImeCommitText(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string text, CefViewRange replacement_range, Int32 relative_cursor_pos);
        public void ImeCommitText(string text, CefViewRange replacement_range, Int32 relative_cursor_pos)
        {
            CCefBrowser_ImeCommitText(_native, text, replacement_range, relative_cursor_pos);
        }

        [DllImport("CCefView")]
        private static extern void CCefBrowser_ImeFinishComposingText(IntPtr thiz, bool keep_selection);
        public void ImeFinishComposingText(bool keep_selection)
        {
            CCefBrowser_ImeFinishComposingText(_native, keep_selection);
        }

        [DllImport("CCefView")]
        private static extern void CCefBrowser_setFocus(IntPtr thiz, bool focused);
        public void SetFocus(bool focused)
        {
            CCefBrowser_setFocus(_native, focused);
        }

        [DllImport("CCefView")]
        private static extern void CCefBrowser_wasResized(IntPtr thiz);
        public void WasResized()
        {
            CCefBrowser_wasResized(_native);
        }

        [DllImport("CCefView")]
        private static extern void CCefBrowser_wasHidden(IntPtr thiz, bool hidden);
        public void WasHidden(bool hidden)
        {
            CCefBrowser_wasHidden(_native, hidden);
        }

        [DllImport("CCefView")]
        private static extern void CCefBrowser_sendMouseMoveEvent(IntPtr thiz, Int32 x, Int32 y, UInt32 modifiers, bool leave);
        public void SendMouseMoveEvent(Int32 x, Int32 y, UInt32 modifiers, bool leave)
        {
            CCefBrowser_sendMouseMoveEvent(_native, x, y, modifiers, leave);
        }

        [DllImport("CCefView")]
        private static extern void CCefBrowser_sendMouseClickEvent(IntPtr thiz, Int32 x, Int32 y, UInt32 modifiers, CefViewMouseButtonType type, bool mouseUp, Int32 clickCount);
        public void SendMouseClickEvent(Int32 x, Int32 y, UInt32 modifiers, CefViewMouseButtonType type, bool mouseUp, Int32 clickCount)
        {
            CCefBrowser_sendMouseClickEvent(_native, x, y, modifiers, type, mouseUp, clickCount);
        }

        [DllImport("CCefView")]
        private static extern void CCefBrowser_sendWheelEvent(IntPtr thiz, Int32 x, Int32 y, UInt32 modifiers, Int32 deltaX, Int32 deltaY);
        public void SendWheelEvent(Int32 x, Int32 y, UInt32 modifiers, Int32 deltaX, Int32 deltaY)
        {
            CCefBrowser_sendWheelEvent(_native, x, y, modifiers, deltaX, deltaY);
        }

        [DllImport("CCefView")]
        private static extern void CCefBrowser_sendKeyEvent(IntPtr thiz, CefViewKeyEventType type, UInt32 modifiers, Int32 windowsKeyCode, Int32 nativeKeyCode, bool isSysKey, UInt16 character, UInt16 umodifiedCharacter, bool isFocusOnEditableField);
        public void SendKeyEvent(CefViewKeyEventType type, UInt32 modifiers, Int32 windowsKeyCode, Int32 nativeKeyCode, bool isSysKey, UInt16 character, UInt16 umodifiedCharacter, bool isFocusOnEditableField)
        {
            CCefBrowser_sendKeyEvent(_native, type, modifiers, windowsKeyCode, nativeKeyCode, isSysKey, character, umodifiedCharacter, isFocusOnEditableField);
        }

        [DllImport("CCefView")]
        private static extern void CCefBrowser_notifyMoveOrResizeStarted(IntPtr thiz);
        public void NotifyMoveOrResizeStarted()
        {
            CCefBrowser_notifyMoveOrResizeStarted(_native);
        }

        [DllImport("CCefView")]
        private static extern void CCefBrowser_notifyScreenChanged(IntPtr thiz);
        public void NotifyScreenChanged()
        {
            CCefBrowser_notifyScreenChanged(_native);
        }

    }
}