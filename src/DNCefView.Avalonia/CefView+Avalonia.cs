using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using System;
using System.Runtime.InteropServices;


namespace DNCefView.Avalonia
{
    public partial class CefView : Control
    {
        public static readonly StyledProperty<string> UrlProperty =
            AvaloniaProperty.Register<CefView, string>(nameof(Url));

        static CefView()
        {
            FocusableProperty.OverrideDefaultValue<CefView>(true);
            IsTabStopProperty.OverrideDefaultValue<CefView>(true);
            KeyboardNavigation.TabNavigationProperty.OverrideDefaultValue<CefView>(KeyboardNavigationMode.None);

            UrlProperty.Changed.AddClassHandler<CefView>((s, e) => s.OnUrlChanged(e));
        }

        public string Url
        {
            get { return GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }

        private void OnUrlChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (_isCreated && e.NewValue is string url)
            {
                _cefBrowser?.NavigateToUrl(url);
            }
        }

        private Rect _cefViewRect;
        private WriteableBitmap? _cefViewImage;

        private Rect _cefPopupRect;
        private WriteableBitmap? _cefPopupImage;

        public CefView() : this(null, "")
        {
        }

        public CefView(CefSetting? setting) : this(setting, "")
        {
        }

        public CefView(CefSetting? setting, string? url)
        {
            if (Design.IsDesignMode)
            {
                return;
            }

            if (string.IsNullOrEmpty(url))
            {
                url = "about:blank";
            }

            IsVisibleProperty.Changed.AddClassHandler<CefView>((s, e) => s.OnVisibleChanged(e));

            SetCurrentValue(UrlProperty, url);
            InitializeNative(url, setting);
        }

        #region CEF Callback Methods
        bool UI_OnCefInputStateChanged(int browserId, string frameId, bool editable)
        {
            // IME handling is platform specific and complex in Avalonia.
            // Placeholder for future implementation.
            return false;
        }

        void UI_OnCefAfterCreated()
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                _isCreated = true;
                _cefBrowser?.WasHidden(!IsVisible);
                _cefBrowser?.WasResized();
                _cefBrowser?.NavigateToUrl(Url);
            });
        }

        void UI_OnCefFocusReleasedByTabKey(int browserId, bool next)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                if (next)
                {
                    var focusManager = TopLevel.GetTopLevel(this)?.FocusManager;
                    KeyboardNavigationHandler.GetNext(focusManager?.GetFocusedElement()!, NavigationDirection.Next);
                }
                else
                {
                    var focusManager = TopLevel.GetTopLevel(this)?.FocusManager;
                    KeyboardNavigationHandler.GetNext(focusManager?.GetFocusedElement()!, NavigationDirection.Previous);
                }
            });
        }

        void UI_OnCefGetRootScreenRect(int browserId, ref CefViewRect rect)
        {
            PixelRect bounds = new PixelRect(0, 0, 1, 1);
            void Action()
            {
                var topLevel = TopLevel.GetTopLevel(this);
                if (topLevel != null)
                {
                    var screen = topLevel?.Screens?.ScreenFromVisual(this);
                    if (screen != null)
                    {
                        bounds = screen.Bounds;
                    }
                }
            }

            if (Dispatcher.UIThread.CheckAccess())
                Action();
            else
                Dispatcher.UIThread.Invoke(Action);

            rect.X = bounds.X;
            rect.Y = bounds.Y;
            rect.Width = bounds.Width;
            rect.Height = bounds.Height;
        }

        void UI_OnCefGetViewRect(int browserId, ref CefViewRect rect)
        {
            double w = 1.0;
            double h = 1.0;

            if (Dispatcher.UIThread.CheckAccess())
            {
                w = Bounds.Width;
                h = Bounds.Height;
            }
            else
            {
                var size = Dispatcher.UIThread.Invoke(() => Bounds.Size);
                w = size.Width;
                h = size.Height;
            }

            rect.X = 0;
            rect.Y = 0;
            rect.Width = (int)w;
            rect.Height = (int)h;
            if (rect.Width <= 0) rect.Width = 1;
            if (rect.Height <= 0) rect.Height = 1;
        }

        bool UI_OnCefGetScreenPoint(int browserId, int viewX, int viewY, ref int screenX, ref int screenY)
        {
            PixelPoint p = new PixelPoint(0, 0);
            bool success = false;
            void Action()
            {
                var topLevel = TopLevel.GetTopLevel(this);
                if (topLevel != null)
                {
                    p = this.PointToScreen(new Point(viewX, viewY));
                    success = true;
                }
            }

            if (Dispatcher.UIThread.CheckAccess())
                Action();
            else
                Dispatcher.UIThread.Invoke(Action);

            if (success)
            {
                screenX = p.X;
                screenY = p.Y;
                return true;
            }
            return false;
        }

        bool UI_OnCefGetScreenInfo(int browserId, ref CefViewScreenInfo info)
        {
            double scale = 1.0;
            PixelRect bounds = new PixelRect(0, 0, 1, 1);
            PixelRect workingArea = new PixelRect(0, 0, 1, 1);
            void Action()
            {
                var topLevel = TopLevel.GetTopLevel(this);
                if (topLevel != null)
                {
                    scale = topLevel.RenderScaling;
                    var screen = topLevel?.Screens?.ScreenFromVisual(this);
                    if (screen != null)
                    {
                        bounds = screen.Bounds;
                        workingArea = screen.WorkingArea;
                    }
                }
            }

            if (Dispatcher.UIThread.CheckAccess())
                Action();
            else
                Dispatcher.UIThread.Invoke(Action);

            info.Rect.X = bounds.X;
            info.Rect.Y = bounds.Y;
            info.Rect.Width = bounds.Width;
            info.Rect.Height = bounds.Height;
            info.AvailableRect.X = workingArea.X;
            info.AvailableRect.Y = workingArea.Y;
            info.AvailableRect.Width = workingArea.Width;
            info.AvailableRect.Height = workingArea.Height;
            info.Depth = 32;
            info.DepthPerComponent = 8;
            info.IsMonochrome = 0;
            info.DeviceScaleFactor = (float)scale;

            return true;
        }

        void UI_OnCefPopupSize(int browserId, CefViewRect rect)
        {
            _cefPopupRect = new Rect(rect.X, rect.Y, rect.Width, rect.Height);
        }

        void UI_OnCefPaint(int browserId, CefViewPaintElementType type, CefViewRect[] dirtyRects, int dirtyRectCount, byte[] imageBytes, int imageBytesCount, int width, int height)
        {
            void Paint()
            {
                WriteableBitmap? targetBitmap = (type == CefViewPaintElementType.PET_VIEW) ? _cefViewImage : _cefPopupImage;

                if (targetBitmap == null || targetBitmap.PixelSize.Width != width || targetBitmap.PixelSize.Height != height)
                {
                    targetBitmap?.Dispose();
                    targetBitmap = new WriteableBitmap(new PixelSize(width, height), new Vector(96, 96), PixelFormat.Bgra8888, AlphaFormat.Premul);

                    if (type == CefViewPaintElementType.PET_VIEW)
                    {
                        var scale = TopLevel.GetTopLevel(this)?.RenderScaling ?? 1.0;
                        _cefViewRect = new Rect(0, 0, (int)(width / scale), (int)(height / scale));
                        _cefViewImage = targetBitmap;
                    }
                    else
                    {
                        _cefPopupImage = targetBitmap;
                    }
                }

                using (var fb = targetBitmap.Lock())
                {
                    Marshal.Copy(imageBytes, 0, fb.Address, imageBytesCount);
                }

                InvalidateVisual();
            }

            if (Dispatcher.UIThread.CheckAccess())
                Paint();
            else
                Dispatcher.UIThread.InvokeAsync(Paint);
        }

        void UI_OnCefAcceleratedPaint(int browserId, CefViewPaintElementType type, CefViewRect[] dirtyRects, int dirtyRectCount, IntPtr sharedHandle, int planeBytesCount)
        {
        }

        void UI_OnCefImeCompositionRangeChanged(int browserId, CefViewRange range, CefViewRect[] characterBounds, int characterBoundsCount)
        {
            // IME Placeholder
        }
        #endregion

        #region UIElement Override And Event Handler
        private void OnVisibleChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool isVisible)
            {
                _cefBrowser?.WasHidden(!isVisible);
            }
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            var p = e.GetPosition(this);
            var modifiers = GetModifiers(e.KeyModifiers, e.GetCurrentPoint(this).Properties);
            var mouseButton = e.GetCurrentPoint(this).Properties.PointerUpdateKind switch
            {
                PointerUpdateKind.LeftButtonPressed => CefViewMouseButtonType.MBT_LEFT,
                PointerUpdateKind.RightButtonPressed => CefViewMouseButtonType.MBT_RIGHT,
                PointerUpdateKind.MiddleButtonPressed => CefViewMouseButtonType.MBT_MIDDLE,
                _ => CefViewMouseButtonType.MBT_LEFT
            };

            Focus();

            _cefBrowser?.SendMouseClickEvent((int)p.X, (int)p.Y, (uint)modifiers, mouseButton, false, 1);
            base.OnPointerPressed(e);
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            var p = e.GetPosition(this);
            var modifiers = GetModifiers(e.KeyModifiers, e.GetCurrentPoint(this).Properties);
            var mouseButton = e.GetCurrentPoint(this).Properties.PointerUpdateKind switch
            {
                PointerUpdateKind.LeftButtonReleased => CefViewMouseButtonType.MBT_LEFT,
                PointerUpdateKind.RightButtonReleased => CefViewMouseButtonType.MBT_RIGHT,
                PointerUpdateKind.MiddleButtonReleased => CefViewMouseButtonType.MBT_MIDDLE,
                _ => CefViewMouseButtonType.MBT_LEFT
            };

            _cefBrowser?.SendMouseClickEvent((int)p.X, (int)p.Y, (uint)modifiers, mouseButton, true, 1);
            base.OnPointerReleased(e);
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            var p = e.GetPosition(this);
            var modifiers = GetModifiers(e.KeyModifiers, e.GetCurrentPoint(this).Properties);
            _cefBrowser?.SendMouseMoveEvent((int)p.X, (int)p.Y, (uint)modifiers, false);
            base.OnPointerMoved(e);
        }

        protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
        {
            var p = e.GetPosition(this);
            var modifiers = GetModifiers(e.KeyModifiers, e.GetCurrentPoint(this).Properties);
            int deltaX = (int)(e.Delta.X * 100);
            int deltaY = (int)(e.Delta.Y * 100);
            _cefBrowser?.SendWheelEvent((int)p.X, (int)p.Y, (uint)modifiers, deltaX, deltaY);
            base.OnPointerWheelChanged(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            var modifiers = GetModifiers(e.KeyModifiers, null);
            int virtualKey = (int)e.Key; // Note: Proper Key mapping required for production
            var isSystemKey = ((modifiers & CefViewEventFlag.EVENTFLAG_ALT_DOWN) != 0);

            _cefBrowser?.SendKeyEvent(CefViewKeyEventType.KEYEVENT_KEYDOWN, (uint)modifiers, virtualKey, 0, isSystemKey, 0, 0, false);
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            var modifiers = GetModifiers(e.KeyModifiers, null);
            int virtualKey = (int)e.Key;
            var isSystemKey = ((modifiers & CefViewEventFlag.EVENTFLAG_ALT_DOWN) != 0);

            _cefBrowser?.SendKeyEvent(CefViewKeyEventType.KEYEVENT_KEYUP, (uint)modifiers, virtualKey, 0, isSystemKey, 0, 0, false);
            base.OnKeyUp(e);
        }

        protected override void OnTextInput(TextInputEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Text))
            {
                foreach (char c in e.Text)
                {
                    uint modifiers = 0;
                    _cefBrowser?.SendKeyEvent(CefViewKeyEventType.KEYEVENT_CHAR, modifiers, c, 0, false, c, c, false);
                }
            }
            base.OnTextInput(e);
        }

        protected override void OnGotFocus(GotFocusEventArgs e)
        {
            _cefBrowser?.SetFocus(true);
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            _cefBrowser?.SetFocus(false);
            base.OnLostFocus(e);
        }

        protected override void OnSizeChanged(SizeChangedEventArgs e)
        {
            _cefBrowser?.WasResized();
            _cefBrowser?.NotifyMoveOrResizeStarted();
            base.OnSizeChanged(e);
        }

        public override void Render(DrawingContext context)
        {
            if (_cefViewImage != null)
            {
                var srcRect = new Rect(0, 0, _cefViewImage.PixelSize.Width, _cefViewImage.PixelSize.Height);
                var destRect = new Rect(0, 0, Bounds.Width, Bounds.Height);
                context.DrawImage(_cefViewImage, srcRect, destRect);
            }

            if (_isShowPopup && _cefPopupImage != null)
            {
                var srcRect = new Rect(0, 0, _cefPopupImage.PixelSize.Width, _cefPopupImage.PixelSize.Height);
                context.DrawImage(_cefPopupImage, srcRect, _cefPopupRect);
            }
            base.Render(context);
        }
        #endregion

        #region Private Uitls
        private CefViewEventFlag GetModifiers(KeyModifiers? keys, PointerPointProperties? mouse)
        {
            CefViewEventFlag modifiers = 0;

            if (keys != null)
            {
                if (keys?.HasFlag(KeyModifiers.Control) == true) modifiers |= CefViewEventFlag.EVENTFLAG_CONTROL_DOWN;
                if (keys?.HasFlag(KeyModifiers.Shift) == true) modifiers |= CefViewEventFlag.EVENTFLAG_SHIFT_DOWN;
                if (keys?.HasFlag(KeyModifiers.Alt) == true) modifiers |= CefViewEventFlag.EVENTFLAG_ALT_DOWN;
                if (keys?.HasFlag(KeyModifiers.Meta) == true) modifiers |= CefViewEventFlag.EVENTFLAG_COMMAND_DOWN;
            }

            if (mouse != null)
            {
                if (mouse?.IsLeftButtonPressed == true) modifiers |= CefViewEventFlag.EVENTFLAG_LEFT_MOUSE_BUTTON;
                if (mouse?.IsRightButtonPressed == true) modifiers |= CefViewEventFlag.EVENTFLAG_RIGHT_MOUSE_BUTTON;
                if (mouse?.IsMiddleButtonPressed == true) modifiers |= CefViewEventFlag.EVENTFLAG_MIDDLE_MOUSE_BUTTON;
            }

            return modifiers;
        }
        #endregion
    }
}
