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
            get { if (_native == IntPtr.Zero) throw new ObjectDisposedException(nameof(CefBrowser)); return _native; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [DllImport("CCefView")]
        private static extern void CCefBrowser_Delete(IntPtr p);
        protected virtual void Dispose([MarshalAs(UnmanagedType.I1)] bool disposing)
        {
            RequestDispose();
        }

        // Source: CCefBrowser(CefBrowserCallback, const std::string &, const CCefSetting *)
        [DllImport("CCefView")]
        private static extern IntPtr CCefBrowser_new0(CefBrowserCallback callback, [MarshalAs(UnmanagedType.LPUTF8Str)] string url, IntPtr setting);

        // Source: void addLocalFolderResource(const std::string &, const std::string &, int)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_addLocalFolderResource(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string path, [MarshalAs(UnmanagedType.LPUTF8Str)] string url, int priority);
        public void AddLocalFolderResource(string path, string url, int priority)
        {
            using (AcquireCall())
            {
                CCefBrowser_addLocalFolderResource(NativeObject, path, url, priority);
            }
        }

        // Source: void addArchiveResource(const std::string &, const std::string &, const std::string &, int)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_addArchiveResource(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string path, [MarshalAs(UnmanagedType.LPUTF8Str)] string url, [MarshalAs(UnmanagedType.LPUTF8Str)] string password, int priority);
        public void AddArchiveResource(string path, string url, string password, int priority)
        {
            using (AcquireCall())
            {
                CCefBrowser_addArchiveResource(NativeObject, path, url, password, priority);
            }
        }

        // Source: int browserId()
        [DllImport("CCefView")]
        private static extern int CCefBrowser_browserId(IntPtr thiz);
        public int BrowserId()
        {
            using (AcquireCall())
            {
                return CCefBrowser_browserId(NativeObject);
            }
        }

        // Source: void navigateToString(const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_navigateToString(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string content);
        public void NavigateToString(string content)
        {
            using (AcquireCall())
            {
                CCefBrowser_navigateToString(NativeObject, content);
            }
        }

        // Source: void navigateToUrl(const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_navigateToUrl(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string url);
        public void NavigateToUrl(string url)
        {
            using (AcquireCall())
            {
                CCefBrowser_navigateToUrl(NativeObject, url);
            }
        }

        // Source: bool canGoBack()
        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CCefBrowser_canGoBack(IntPtr thiz);
        public bool CanGoBack()
        {
            using (AcquireCall())
            {
                return CCefBrowser_canGoBack(NativeObject);
            }
        }

        // Source: bool canGoForward()
        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CCefBrowser_canGoForward(IntPtr thiz);
        public bool CanGoForward()
        {
            using (AcquireCall())
            {
                return CCefBrowser_canGoForward(NativeObject);
            }
        }

        // Source: void goBack()
        [DllImport("CCefView")]
        private static extern void CCefBrowser_goBack(IntPtr thiz);
        public void GoBack()
        {
            using (AcquireCall())
            {
                CCefBrowser_goBack(NativeObject);
            }
        }

        // Source: void goForward()
        [DllImport("CCefView")]
        private static extern void CCefBrowser_goForward(IntPtr thiz);
        public void GoForward()
        {
            using (AcquireCall())
            {
                CCefBrowser_goForward(NativeObject);
            }
        }

        // Source: bool isLoading()
        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CCefBrowser_isLoading(IntPtr thiz);
        public bool IsLoading()
        {
            using (AcquireCall())
            {
                return CCefBrowser_isLoading(NativeObject);
            }
        }

        // Source: void reload()
        [DllImport("CCefView")]
        private static extern void CCefBrowser_reload(IntPtr thiz);
        public void Reload()
        {
            using (AcquireCall())
            {
                CCefBrowser_reload(NativeObject);
            }
        }

        // Source: void stopLoad()
        [DllImport("CCefView")]
        private static extern void CCefBrowser_stopLoad(IntPtr thiz);
        public void StopLoad()
        {
            using (AcquireCall())
            {
                CCefBrowser_stopLoad(NativeObject);
            }
        }

        // Source: bool triggerEventOnMainFrame(const std::string &, const std::string &)
        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CCefBrowser_triggerEventOnMainFrame(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string evtName, [MarshalAs(UnmanagedType.LPUTF8Str)] string evtArgs);
        public bool TriggerEventOnMainFrame(string evtName, string evtArgs)
        {
            using (AcquireCall())
            {
                return CCefBrowser_triggerEventOnMainFrame(NativeObject, evtName, evtArgs);
            }
        }

        // Source: bool triggerEventOnFrame(const std::string &, const std::string &, const std::string &)
        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CCefBrowser_triggerEventOnFrame(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string evtName, [MarshalAs(UnmanagedType.LPUTF8Str)] string evtArgs, [MarshalAs(UnmanagedType.LPUTF8Str)] string frameId);
        public bool TriggerEventOnFrame(string evtName, string evtArgs, string frameId)
        {
            using (AcquireCall())
            {
                return CCefBrowser_triggerEventOnFrame(NativeObject, evtName, evtArgs, frameId);
            }
        }

        // Source: bool broadcastEvent(const std::string &, const std::string &)
        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CCefBrowser_broadcastEvent(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string evtName, [MarshalAs(UnmanagedType.LPUTF8Str)] string evtArgs);
        public bool BroadcastEvent(string evtName, string evtArgs)
        {
            using (AcquireCall())
            {
                return CCefBrowser_broadcastEvent(NativeObject, evtName, evtArgs);
            }
        }

        // Source: bool triggerEvent(const std::string &, const std::string &, const std::string &)
        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CCefBrowser_triggerEvent(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string args, [MarshalAs(UnmanagedType.LPUTF8Str)] string frameId);
        public bool TriggerEvent(string name, string args, string frameId)
        {
            using (AcquireCall())
            {
                return CCefBrowser_triggerEvent(NativeObject, name, args, frameId);
            }
        }

        // Source: bool responseQCefQuery(const CCefQuery *)
        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CCefBrowser_responseQCefQuery(IntPtr thiz, IntPtr query);
        public bool ResponseQCefQuery(CefQuery query)
        {
            using (AcquireCall())
            {
                return CCefBrowser_responseQCefQuery(NativeObject, query.NativeObject);
            }
        }

        // Source: bool executeJavascript(const std::string &, const std::string &, const std::string &)
        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CCefBrowser_executeJavascript(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string frameId, [MarshalAs(UnmanagedType.LPUTF8Str)] string code, [MarshalAs(UnmanagedType.LPUTF8Str)] string url);
        public bool ExecuteJavascript(string frameId, string code, string url)
        {
            using (AcquireCall())
            {
                return CCefBrowser_executeJavascript(NativeObject, frameId, code, url);
            }
        }

        // Source: bool executeJavascriptWithResult(const std::string &, const std::string &, const std::string &, const std::string &)
        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CCefBrowser_executeJavascriptWithResult(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string frameId, [MarshalAs(UnmanagedType.LPUTF8Str)] string code, [MarshalAs(UnmanagedType.LPUTF8Str)] string url, [MarshalAs(UnmanagedType.LPUTF8Str)] string context);
        public bool ExecuteJavascriptWithResult(string frameId, string code, string url, string context)
        {
            using (AcquireCall())
            {
                return CCefBrowser_executeJavascriptWithResult(NativeObject, frameId, code, url, context);
            }
        }

        // Source: bool setPreference(const std::string &, const std::string &)
        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CCefBrowser_setPreference(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string value);
        public bool SetPreference(string name, string value)
        {
            using (AcquireCall())
            {
                return CCefBrowser_setPreference(NativeObject, name, value);
            }
        }

        // Source: void setDisablePopupContextMenu(bool)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_setDisablePopupContextMenu(IntPtr thiz, [MarshalAs(UnmanagedType.I1)] bool disable);
        public void SetDisablePopupContextMenu([MarshalAs(UnmanagedType.I1)] bool disable)
        {
            using (AcquireCall())
            {
                CCefBrowser_setDisablePopupContextMenu(NativeObject, disable);
            }
        }

        // Source: bool isPopupContextMenuDisabled()
        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CCefBrowser_isPopupContextMenuDisabled(IntPtr thiz);
        public bool IsPopupContextMenuDisabled()
        {
            using (AcquireCall())
            {
                return CCefBrowser_isPopupContextMenuDisabled(NativeObject);
            }
        }

        // Source: void setWindowlessFrameRate(int)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_setWindowlessFrameRate(IntPtr thiz, int rate);
        public void SetWindowlessFrameRate(int rate)
        {
            using (AcquireCall())
            {
                CCefBrowser_setWindowlessFrameRate(NativeObject, rate);
            }
        }

        // Source: void sendExternalBeginFrame()
        [DllImport("CCefView")]
        private static extern void CCefBrowser_sendExternalBeginFrame(IntPtr thiz);
        public void SendExternalBeginFrame()
        {
            using (AcquireCall())
            {
                CCefBrowser_sendExternalBeginFrame(NativeObject);
            }
        }

        // Source: void showDevTools()
        [DllImport("CCefView")]
        private static extern void CCefBrowser_showDevTools(IntPtr thiz);
        public void ShowDevTools()
        {
            using (AcquireCall())
            {
                CCefBrowser_showDevTools(NativeObject);
            }
        }

        // Source: void closeDevTools()
        [DllImport("CCefView")]
        private static extern void CCefBrowser_closeDevTools(IntPtr thiz);
        public void CloseDevTools()
        {
            using (AcquireCall())
            {
                CCefBrowser_closeDevTools(NativeObject);
            }
        }

        // Source: bool hasDevTools()
        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CCefBrowser_hasDevTools(IntPtr thiz);
        public bool HasDevTools()
        {
            using (AcquireCall())
            {
                return CCefBrowser_hasDevTools(NativeObject);
            }
        }

        // Source: void closeBrowser(bool)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_closeBrowser(IntPtr thiz, [MarshalAs(UnmanagedType.I1)] bool forceClose);
        public void CloseBrowser([MarshalAs(UnmanagedType.I1)] bool forceClose)
        {
            using (AcquireCall())
            {
                CCefBrowser_closeBrowser(NativeObject, forceClose);
            }
        }

        // Source: bool continueJSDialog(int64_t, bool, const std::string &)
        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CCefBrowser_continueJSDialog(IntPtr thiz, long requestId, [MarshalAs(UnmanagedType.I1)] bool success, [MarshalAs(UnmanagedType.LPUTF8Str)] string userInput);
        public bool ContinueJSDialog(long requestId, [MarshalAs(UnmanagedType.I1)] bool success, string userInput)
        {
            using (AcquireCall())
            {
                return CCefBrowser_continueJSDialog(NativeObject, requestId, success, userInput);
            }
        }
        // Source: void setFocus(bool)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_setFocus(IntPtr thiz, [MarshalAs(UnmanagedType.I1)] bool focused);
        public void SetFocus([MarshalAs(UnmanagedType.I1)] bool focused)
        {
            using (AcquireCall())
            {
                CCefBrowser_setFocus(NativeObject, focused);
            }
        }

        // Source: void wasResized()
        [DllImport("CCefView")]
        private static extern void CCefBrowser_wasResized(IntPtr thiz);
        public void WasResized()
        {
            using (AcquireCall())
            {
                CCefBrowser_wasResized(NativeObject);
            }
        }

        // Source: void wasHidden(bool)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_wasHidden(IntPtr thiz, [MarshalAs(UnmanagedType.I1)] bool hidden);
        public void WasHidden([MarshalAs(UnmanagedType.I1)] bool hidden)
        {
            using (AcquireCall())
            {
                CCefBrowser_wasHidden(NativeObject, hidden);
            }
        }

        // Source: void sendMouseMoveEvent(int, int, uint32_t, bool)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_sendMouseMoveEvent(IntPtr thiz, int x, int y, UInt32 modifiers, [MarshalAs(UnmanagedType.I1)] bool leave);
        public void SendMouseMoveEvent(int x, int y, UInt32 modifiers, [MarshalAs(UnmanagedType.I1)] bool leave)
        {
            using (AcquireCall())
            {
                CCefBrowser_sendMouseMoveEvent(NativeObject, x, y, modifiers, leave);
            }
        }

        // Source: void sendMouseClickEvent(int, int, uint32_t, CefViewMouseButtonType, bool, int)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_sendMouseClickEvent(IntPtr thiz, int x, int y, UInt32 modifiers, CefViewMouseButtonType type, [MarshalAs(UnmanagedType.I1)] bool mouseUp, int clickCount);
        public void SendMouseClickEvent(int x, int y, UInt32 modifiers, CefViewMouseButtonType type, [MarshalAs(UnmanagedType.I1)] bool mouseUp, int clickCount)
        {
            using (AcquireCall())
            {
                CCefBrowser_sendMouseClickEvent(NativeObject, x, y, modifiers, type, mouseUp, clickCount);
            }
        }

        // Source: void sendWheelEvent(int, int, uint32_t, int, int)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_sendWheelEvent(IntPtr thiz, int x, int y, UInt32 modifiers, int deltaX, int deltaY);
        public void SendWheelEvent(int x, int y, UInt32 modifiers, int deltaX, int deltaY)
        {
            using (AcquireCall())
            {
                CCefBrowser_sendWheelEvent(NativeObject, x, y, modifiers, deltaX, deltaY);
            }
        }
        // Source: void dragTargetDragEnterText(int, int, uint32_t, const std::string &, const std::string &, const std::string &, CefViewDragOperation)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_dragTargetDragEnterText(IntPtr thiz, int x, int y, UInt32 modifiers, [MarshalAs(UnmanagedType.LPUTF8Str)] string text, [MarshalAs(UnmanagedType.LPUTF8Str)] string html, [MarshalAs(UnmanagedType.LPUTF8Str)] string baseUrl, CefViewDragOperation allowedOps);
        public void DragTargetDragEnterText(int x, int y, UInt32 modifiers, string text, string html, string baseUrl, CefViewDragOperation allowedOps)
        {
            using (AcquireCall())
            {
                CCefBrowser_dragTargetDragEnterText(NativeObject, x, y, modifiers, text, html, baseUrl, allowedOps);
            }
        }

        // Source: void dragTargetDragEnterFiles(int, int, uint32_t, const std::string &, CefViewDragOperation)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_dragTargetDragEnterFiles(IntPtr thiz, int x, int y, UInt32 modifiers, [MarshalAs(UnmanagedType.LPUTF8Str)] string filePaths, CefViewDragOperation allowedOps);
        public void DragTargetDragEnterFiles(int x, int y, UInt32 modifiers, string filePaths, CefViewDragOperation allowedOps)
        {
            using (AcquireCall())
            {
                CCefBrowser_dragTargetDragEnterFiles(NativeObject, x, y, modifiers, filePaths, allowedOps);
            }
        }

        // Source: void dragTargetDragOver(int, int, uint32_t, CefViewDragOperation)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_dragTargetDragOver(IntPtr thiz, int x, int y, UInt32 modifiers, CefViewDragOperation allowedOps);
        public void DragTargetDragOver(int x, int y, UInt32 modifiers, CefViewDragOperation allowedOps)
        {
            using (AcquireCall())
            {
                CCefBrowser_dragTargetDragOver(NativeObject, x, y, modifiers, allowedOps);
            }
        }

        // Source: void dragTargetDragLeave()
        [DllImport("CCefView")]
        private static extern void CCefBrowser_dragTargetDragLeave(IntPtr thiz);
        public void DragTargetDragLeave()
        {
            using (AcquireCall())
            {
                CCefBrowser_dragTargetDragLeave(NativeObject);
            }
        }

        // Source: void dragTargetDrop(int, int, uint32_t)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_dragTargetDrop(IntPtr thiz, int x, int y, UInt32 modifiers);
        public void DragTargetDrop(int x, int y, UInt32 modifiers)
        {
            using (AcquireCall())
            {
                CCefBrowser_dragTargetDrop(NativeObject, x, y, modifiers);
            }
        }

        // Source: void dragSourceEndedAt(int, int, CefViewDragOperation)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_dragSourceEndedAt(IntPtr thiz, int x, int y, CefViewDragOperation operation);
        public void DragSourceEndedAt(int x, int y, CefViewDragOperation operation)
        {
            using (AcquireCall())
            {
                CCefBrowser_dragSourceEndedAt(NativeObject, x, y, operation);
            }
        }

        // Source: void dragSourceSystemDragEnded()
        [DllImport("CCefView")]
        private static extern void CCefBrowser_dragSourceSystemDragEnded(IntPtr thiz);
        public void DragSourceSystemDragEnded()
        {
            using (AcquireCall())
            {
                CCefBrowser_dragSourceSystemDragEnded(NativeObject);
            }
        }

        // Source: void sendTouchEvent(int, float, float, float, float, float, float, int, uint32_t, int)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_sendTouchEvent(IntPtr thiz, int touchId, float x, float y, float radiusX, float radiusY, float rotationAngle, float pressure, int touchEventType, UInt32 modifiers, int pointerType);
        public void SendTouchEvent(int touchId, float x, float y, float radiusX, float radiusY, float rotationAngle, float pressure, int touchEventType, UInt32 modifiers, int pointerType)
        {
            using (AcquireCall())
            {
                CCefBrowser_sendTouchEvent(NativeObject, touchId, x, y, radiusX, radiusY, rotationAngle, pressure, touchEventType, modifiers, pointerType);
            }
        }

        // Source: void sendKeyEvent(CefViewKeyEventType, uint32_t, int, int, bool, uint16_t, uint16_t, bool)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_sendKeyEvent(IntPtr thiz, CefViewKeyEventType type, UInt32 modifiers, int windowsKeyCode, int nativeKeyCode, [MarshalAs(UnmanagedType.I1)] bool isSysKey, UInt16 character, UInt16 umodifiedCharacter, [MarshalAs(UnmanagedType.I1)] bool isFocusOnEditableField);
        public void SendKeyEvent(CefViewKeyEventType type, UInt32 modifiers, int windowsKeyCode, int nativeKeyCode, [MarshalAs(UnmanagedType.I1)] bool isSysKey, UInt16 character, UInt16 umodifiedCharacter, [MarshalAs(UnmanagedType.I1)] bool isFocusOnEditableField)
        {
            using (AcquireCall())
            {
                CCefBrowser_sendKeyEvent(NativeObject, type, modifiers, windowsKeyCode, nativeKeyCode, isSysKey, character, umodifiedCharacter, isFocusOnEditableField);
            }
        }

        // Source: void notifyMoveOrResizeStarted()
        [DllImport("CCefView")]
        private static extern void CCefBrowser_notifyMoveOrResizeStarted(IntPtr thiz);
        public void NotifyMoveOrResizeStarted()
        {
            using (AcquireCall())
            {
                CCefBrowser_notifyMoveOrResizeStarted(NativeObject);
            }
        }

        // Source: void notifyScreenChanged()
        [DllImport("CCefView")]
        private static extern void CCefBrowser_notifyScreenChanged(IntPtr thiz);
        public void NotifyScreenChanged()
        {
            using (AcquireCall())
            {
                CCefBrowser_notifyScreenChanged(NativeObject);
            }
        }

        // Source: void imeSetComposition(const std::string &, CefViewCompositionUnderline *, int, CefViewRange, CefViewRange)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_imeSetComposition(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string text, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] CefViewCompositionUnderline[] underlines, int count, CefViewRange replacement_range, CefViewRange selection_range);
        public void ImeSetComposition(string text, CefViewCompositionUnderline[] underlines, int count, CefViewRange replacement_range, CefViewRange selection_range)
        {
            using (AcquireCall())
            {
                underlines = underlines ?? Array.Empty<CefViewCompositionUnderline>();
                if (count < 0 || count > underlines.Length) throw new ArgumentOutOfRangeException(nameof(count));
                CCefBrowser_imeSetComposition(NativeObject, text, underlines, count, replacement_range, selection_range);
            }
        }

        // Source: void imeCommitText(const std::string &, CefViewRange, int)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_imeCommitText(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string text, CefViewRange replacement_range, int relative_cursor_pos);
        public void ImeCommitText(string text, CefViewRange replacement_range, int relative_cursor_pos)
        {
            using (AcquireCall())
            {
                CCefBrowser_imeCommitText(NativeObject, text, replacement_range, relative_cursor_pos);
            }
        }

        // Source: void imeFinishComposingText(bool)
        [DllImport("CCefView")]
        private static extern void CCefBrowser_imeFinishComposingText(IntPtr thiz, [MarshalAs(UnmanagedType.I1)] bool keep_selection);
        public void ImeFinishComposingText([MarshalAs(UnmanagedType.I1)] bool keep_selection)
        {
            using (AcquireCall())
            {
                CCefBrowser_imeFinishComposingText(NativeObject, keep_selection);
            }
        }

        // Source: void imeCancelComposition()
        [DllImport("CCefView")]
        private static extern void CCefBrowser_imeCancelComposition(IntPtr thiz);
        public void ImeCancelComposition()
        {
            using (AcquireCall())
            {
                CCefBrowser_imeCancelComposition(NativeObject);
            }
        }

        // ABI 3 additions: editor commands, find, and dialog/download/context-menu/permission answers.
        [DllImport("CCefView")] private static extern void CCefBrowser_copy(IntPtr thiz);
        public void Copy() { using (AcquireCall()) { CCefBrowser_copy(NativeObject); } }

        [DllImport("CCefView")] private static extern void CCefBrowser_cut(IntPtr thiz);
        public void Cut() { using (AcquireCall()) { CCefBrowser_cut(NativeObject); } }

        [DllImport("CCefView")] private static extern void CCefBrowser_paste(IntPtr thiz);
        public void Paste() { using (AcquireCall()) { CCefBrowser_paste(NativeObject); } }

        [DllImport("CCefView")] private static extern void CCefBrowser_selectAll(IntPtr thiz);
        public void SelectAll() { using (AcquireCall()) { CCefBrowser_selectAll(NativeObject); } }

        [DllImport("CCefView")] private static extern void CCefBrowser_undo(IntPtr thiz);
        public void Undo() { using (AcquireCall()) { CCefBrowser_undo(NativeObject); } }

        [DllImport("CCefView")] private static extern void CCefBrowser_redo(IntPtr thiz);
        public void Redo() { using (AcquireCall()) { CCefBrowser_redo(NativeObject); } }

        [DllImport("CCefView")] private static extern void CCefBrowser_delete(IntPtr thiz);
        public void Delete() { using (AcquireCall()) { CCefBrowser_delete(NativeObject); } }

        [DllImport("CCefView")] private static extern void CCefBrowser_startFinding(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string searchText, [MarshalAs(UnmanagedType.I1)] bool forward, [MarshalAs(UnmanagedType.I1)] bool matchCase);
        public void StartFinding(string searchText, bool forward, bool matchCase) { using (AcquireCall()) { CCefBrowser_startFinding(NativeObject, searchText, forward, matchCase); } }

        [DllImport("CCefView")] private static extern void CCefBrowser_stopFinding(IntPtr thiz, [MarshalAs(UnmanagedType.I1)] bool clearSelection);
        public void StopFinding(bool clearSelection) { using (AcquireCall()) { CCefBrowser_stopFinding(NativeObject, clearSelection); } }

        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CCefBrowser_continueFileDialog(IntPtr thiz, long requestId, int filterIndex, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4, ArraySubType = UnmanagedType.LPUTF8Str)] string[] filePaths, int filePathCount);
        public bool ContinueFileDialog(long requestId, int filterIndex, string[] filePaths)
        {
            using (AcquireCall()) { return CCefBrowser_continueFileDialog(NativeObject, requestId, filterIndex, filePaths ?? Array.Empty<string>(), filePaths?.Length ?? 0); }
        }

        [DllImport("CCefView")] private static extern void CCefBrowser_cancelFileDialog(IntPtr thiz, long requestId);
        public void CancelFileDialog(long requestId) { using (AcquireCall()) { CCefBrowser_cancelFileDialog(NativeObject, requestId); } }

        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CCefBrowser_continueDownload(IntPtr thiz, long downloadId, [MarshalAs(UnmanagedType.LPUTF8Str)] string downloadPath, [MarshalAs(UnmanagedType.I1)] bool showDialog);
        public bool ContinueDownload(long downloadId, string downloadPath, bool showDialog) { using (AcquireCall()) { return CCefBrowser_continueDownload(NativeObject, downloadId, downloadPath, showDialog); } }

        [DllImport("CCefView")] private static extern void CCefBrowser_cancelDownload(IntPtr thiz, long downloadId);
        public void CancelDownload(long downloadId) { using (AcquireCall()) { CCefBrowser_cancelDownload(NativeObject, downloadId); } }

        [DllImport("CCefView")] private static extern void CCefBrowser_pauseDownload(IntPtr thiz, long downloadId);
        public void PauseDownload(long downloadId) { using (AcquireCall()) { CCefBrowser_pauseDownload(NativeObject, downloadId); } }

        [DllImport("CCefView")] private static extern void CCefBrowser_resumeDownload(IntPtr thiz, long downloadId);
        public void ResumeDownload(long downloadId) { using (AcquireCall()) { CCefBrowser_resumeDownload(NativeObject, downloadId); } }

        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CCefBrowser_continueContextMenu(IntPtr thiz, long requestId, int commandId, int eventFlags);
        public bool ContinueContextMenu(long requestId, int commandId, int eventFlags) { using (AcquireCall()) { return CCefBrowser_continueContextMenu(NativeObject, requestId, commandId, eventFlags); } }

        [DllImport("CCefView")] private static extern void CCefBrowser_cancelContextMenu(IntPtr thiz, long requestId);
        public void CancelContextMenu(long requestId) { using (AcquireCall()) { CCefBrowser_cancelContextMenu(NativeObject, requestId); } }

        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CCefBrowser_continuePermissionPrompt(IntPtr thiz, ulong promptId, [MarshalAs(UnmanagedType.I1)] bool allow);
        public bool ContinuePermissionPrompt(ulong promptId, bool allow) { using (AcquireCall()) { return CCefBrowser_continuePermissionPrompt(NativeObject, promptId, allow); } }

    }
}
