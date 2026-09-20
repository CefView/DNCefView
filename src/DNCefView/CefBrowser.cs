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
            _callbackTable.CefQueryRequestCb = Thunk_OnCefQueryRequest;
            _callbackTable.InvokeMethodCb = Thunk_OnCefInvokeMethod;
            _callbackTable.ReportJavascriptResultCb = Thunk_OnCefReportJavascriptResult;
            _callbackTable.InputStateChangedCb = Thunk_OnCefInputStateChanged;
            #endregion

            #region DisplayHandler
            _callbackTable.AddressChangedCb = Thunk_OnCefAddressChanged;
            _callbackTable.TitleChangedCb = Thunk_OnCefTitleChanged;
            _callbackTable.FullscreenModeChangedCb = Thunk_OnCefFullScreenModeChanged;
            _callbackTable.StatusMessageCb = Thunk_OnCefStatusMessage;
            _callbackTable.ConsoleMessageCb = Thunk_OnCefConsoleMessage;
            _callbackTable.LoadingProgressChangedCb = Thunk_OnCefLoadingProgressChanged;
            _callbackTable.FaviconUrlChangedCb = Thunk_OnCefFaviconUrlChanged;
            _callbackTable.CursorChangedCb = Thunk_OnCefCursorChanged;
            _callbackTable.DraggableRegionChangedCb = Thunk_OnCefDraggableRegionChanged;
            #endregion

            #region FocusHandler
            _callbackTable.OnFocusReleasedByTabKeyCb = Thunk_OnCefReleasedFocusByTabKey;
            _callbackTable.OnRequestSetFocusCb = Thunk_OnCefRequestSetFocus;
            _callbackTable.OnGotFocusCb = Thunk_OnCefGotFocus;
            _callbackTable.OnJSDialogCb = Thunk_OnCefJSDialog;
            #endregion

            #region LifespanHandler
            _callbackTable.OnBeforeNewPopupCreateCb = Thunk_OnCefBeforeNewPopupCreate;
            _callbackTable.OnBeforeNewBrowserCreateCb = Thunk_OnCefBeforeNewBrowserCreate;
            _callbackTable.DoCloseCb = Thunk_OnCefDoClose;
            _callbackTable.RequestCloseCb = Thunk_OnCefRequestClose;
            _callbackTable.OnAfterCreatedCb = Thunk_OnCefAfterCreated;
            _callbackTable.OnBeforeCloseCb = Thunk_OnCefBeforeClose;
            #endregion

            #region LoadHandler
            _callbackTable.LoadingStateChangedCb = Thunk_OnCefLoadingStateChanged;
            _callbackTable.LoadStartCb = Thunk_OnCefLoadStart;
            _callbackTable.LoadEndCb = Thunk_OnCefLoadEnd;
            _callbackTable.LoadErrorCb = Thunk_OnCefLoadError;
            #endregion

            #region RenderHandler
            _callbackTable.GetRootScreenRectCb = Thunk_OnCefGetRootScreenRect;
            _callbackTable.GetViewRectCb = Thunk_OnCefGetViewRect;
            _callbackTable.GetScreenPointCb = Thunk_OnCefGetScreenPoint;
            _callbackTable.GetScreenInfoCb = Thunk_OnCefGetScreenInfo;
            _callbackTable.OnPopupShowCb = Thunk_OnCefPopupShow;
            _callbackTable.OnPopupSizeCb = Thunk_OnCefPopupSize;
            _callbackTable.OnPaintCb = Thunk_OnCefPaint;
            _callbackTable.OnAcceleratedPaintCb = Thunk_OnCefAcceleratedPaint;
            // ABI 3 callbacks.
            _callbackTable.OnBeforeUnloadDialogCb = Thunk_OnCefBeforeUnloadDialog;
            _callbackTable.OnFileDialogCb = Thunk_OnCefFileDialog;
            _callbackTable.OnBeforeDownloadCb = Thunk_OnCefBeforeDownload;
            _callbackTable.OnDownloadUpdatedCb = Thunk_OnCefDownloadUpdated;
            _callbackTable.OnFindResultCb = Thunk_OnCefFindResult;
            _callbackTable.OnContextMenuCb = Thunk_OnCefContextMenu;
            _callbackTable.OnContextMenuDismissedCb = Thunk_OnCefContextMenuDismissed;
            _callbackTable.OnPermissionPromptCb = Thunk_OnCefPermissionPrompt;
            _callbackTable.OnRenderProcessTerminatedCb = Thunk_OnCefRenderProcessTerminated;
            _callbackTable.StartDraggingCb = Thunk_OnCefStartDragging;
            _callbackTable.UpdateDragCursorCb = Thunk_OnCefUpdateDragCursor;
            _callbackTable.OnImeCompositionRangeChangedCb = Thunk_OnCefImeCompositionRangeChanged;
            _callbackTable.OnTextSelectionChangedCb = Thunk_OnCefTextSelectionChanged;
            #endregion

            _native = CCefBrowser_new0(_callbackTable, url, setting.NativeObject);
            if (_native == IntPtr.Zero)
            {
                lock (LiveInstances) LiveInstances.Remove(this);
                throw new InvalidOperationException("Native browser creation failed");
            }
            // ABI 4: register the static thunk route before browser creation so the
            // first native callback (GetViewRect during creation) cannot precede it.
            RegisterRoute(_native, this);
            System.Diagnostics.Trace.WriteLine($"[ABI4] route registered host={_native}");
            CCefBrowser_start(_native);
            System.Diagnostics.Trace.WriteLine("[ABI4] start returned");
        }

    }
}
