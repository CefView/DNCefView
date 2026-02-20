using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using Avalonia.VisualTree;
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

            RunInUIThread(() =>
            {
                var screen = (this.GetVisualRoot() as Window)?.Screens?.ScreenFromVisual(this);
                if (screen != null)
                {
                    bounds = screen.Bounds;
                }
            });

            rect.X = bounds.X;
            rect.Y = bounds.Y;
            rect.Width = bounds.Width;
            rect.Height = bounds.Height;
        }

        void UI_OnCefGetViewRect(int browserId, ref CefViewRect rect)
        {
            double w = 1.0;
            double h = 1.0;

            RunInUIThread(() =>
            {
                w = Bounds.Width;
                h = Bounds.Height;
            });

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

            RunInUIThread(() =>
            {
                p = this.PointToScreen(new Point(viewX, viewY));
                success = true;
            });

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
            float scale = 1.0f;
            PixelRect bounds = new PixelRect(0, 0, 1, 1);
            PixelRect workingArea = new PixelRect(0, 0, 1, 1);

            RunInUIThread(() =>
            {
                var screen = (this.GetVisualRoot() as Window)?.Screens?.ScreenFromVisual(this);
                if (screen != null)
                {
                    bounds = screen.Bounds;
                    workingArea = screen.WorkingArea;
                }
            });

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
            info.DeviceScaleFactor = scale;

            return true;
        }

        void UI_OnCefPopupSize(int browserId, CefViewRect rect)
        {
            _cefPopupRect = new Rect(rect.X, rect.Y, rect.Width, rect.Height);
        }

        void UI_OnCefPaint(int browserId, CefViewPaintElementType type, CefViewRect[] dirtyRects, int dirtyRectCount, byte[] imageBytes, int imageBytesCount, int width, int height)
        {
            RunInUIThread(() =>
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
            }, block: false);
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
            modifiers |= GetKeyEventFlags(e.Key);
            int virtualKey = GetWindowsVirtualKey(e.Key);
            var isSystemKey = ((modifiers & CefViewEventFlag.EVENTFLAG_ALT_DOWN) != 0);

            _cefBrowser?.SendKeyEvent(CefViewKeyEventType.KEYEVENT_KEYDOWN, (uint)modifiers, virtualKey, 0, isSystemKey, 0, 0, false);
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            var modifiers = GetModifiers(e.KeyModifiers, null);
            modifiers |= GetKeyEventFlags(e.Key);
            int virtualKey = GetWindowsVirtualKey(e.Key);
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
        private void RunInUIThread(Action action, bool block = true)
        {
            if (Dispatcher.UIThread.CheckAccess())
            {
                // invoke directly
                action();
            }
            else if (block)
            {
                // invoke and block 
                Dispatcher.UIThread.Invoke(action);
            }
            else
            {
                // invoke asynchronously
                Dispatcher.UIThread.InvokeAsync(action);
            }
        }

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

        private CefViewEventFlag GetKeyEventFlags(Key key)
        {
            CefViewEventFlag modifiers = 0;

            switch (key)
            {
                case Key.LeftShift:
                case Key.LeftCtrl:
                case Key.LeftAlt:
                case Key.LWin:
                    modifiers |= CefViewEventFlag.EVENTFLAG_IS_LEFT;
                    break;
                case Key.RightShift:
                case Key.RightCtrl:
                case Key.RightAlt:
                case Key.RWin:
                    modifiers |= CefViewEventFlag.EVENTFLAG_IS_RIGHT;
                    break;
            }

            if ((key >= Key.NumPad0 && key <= Key.NumPad9) ||
                key == Key.Multiply ||
                key == Key.Add ||
                key == Key.Separator ||
                key == Key.Subtract ||
                key == Key.Decimal ||
                key == Key.Divide)
            {
                modifiers |= CefViewEventFlag.EVENTFLAG_IS_KEY_PAD;
            }

            return modifiers;
        }

        private int GetWindowsVirtualKey(Key key)
        {
            if (key >= Key.A && key <= Key.Z)
                return 0x41 + ((int)key - (int)Key.A);

            if (key >= Key.D0 && key <= Key.D9)
                return 0x30 + ((int)key - (int)Key.D0);

            if (key >= Key.NumPad0 && key <= Key.NumPad9)
                return 0x60 + ((int)key - (int)Key.NumPad0);

            if (key >= Key.F1 && key <= Key.F24)
                return 0x70 + ((int)key - (int)Key.F1);

            return key switch
            {
                Key.Cancel => 0x03,
                Key.Back => 0x08,
                Key.Tab => 0x09,
                Key.LineFeed => 0x0A,
                Key.Clear => 0x0C,
                Key.Return | Key.Enter => 0x0D,
                Key.Pause => 0x13,
                Key.CapsLock | Key.Capital => 0x14,
                Key.HangulMode | Key.KanaMode => 0x15,
                Key.JunjaMode => 0x17,
                Key.FinalMode => 0x18,
                Key.HanjaMode | Key.KanjiMode => 0x19,
                Key.Escape => 0x1B,
                Key.ImeConvert => 0x1C,
                Key.ImeNonConvert => 0x1D,
                Key.ImeAccept => 0x1E,
                Key.ImeModeChange => 0x1F,
                Key.Space => 0x20,
                Key.PageUp | Key.Prior => 0x21,
                Key.PageDown | Key.Next => 0x22,
                Key.End => 0x23,
                Key.Home => 0x24,
                Key.Left => 0x25,
                Key.Up => 0x26,
                Key.Right => 0x27,
                Key.Down => 0x28,
                Key.Select => 0x29,
                Key.Print => 0x2A,
                Key.Execute => 0x2B,
                Key.Snapshot | Key.PrintScreen => 0x2C,
                Key.Insert => 0x2D,
                Key.Delete => 0x2E,
                Key.Help => 0x2F,
                Key.LWin => 0x5B,
                Key.RWin => 0x5C,
                Key.Apps => 0x5D,
                Key.Sleep => 0x5F,
                Key.Multiply => 0x6A,
                Key.Add => 0x6B,
                Key.Separator => 0x6C,
                Key.Subtract => 0x6D,
                Key.Decimal => 0x6E,
                Key.Divide => 0x6F,
                Key.NumLock => 0x90,
                Key.Scroll => 0x91,
                Key.LeftShift => 0xA0,
                Key.RightShift => 0xA1,
                Key.LeftCtrl => 0xA2,
                Key.RightCtrl => 0xA3,
                Key.LeftAlt => 0xA4,
                Key.RightAlt => 0xA5,
                Key.BrowserBack => 0xA6,
                Key.BrowserForward => 0xA7,
                Key.BrowserRefresh => 0xA8,
                Key.BrowserStop => 0xA9,
                Key.BrowserSearch => 0xAA,
                Key.BrowserFavorites => 0xAB,
                Key.BrowserHome => 0xAC,
                Key.VolumeMute => 0xAD,
                Key.VolumeDown => 0xAE,
                Key.VolumeUp => 0xAF,
                Key.MediaNextTrack => 0xB0,
                Key.MediaPreviousTrack => 0xB1,
                Key.MediaStop => 0xB2,
                Key.MediaPlayPause => 0xB3,
                Key.LaunchMail => 0xB4,
                Key.SelectMedia => 0xB5,
                Key.LaunchApplication1 => 0xB6,
                Key.LaunchApplication2 => 0xB7,
                Key.OemSemicolon | Key.Oem1 => 0xBA,
                Key.OemPlus => 0xBB,
                Key.OemComma => 0xBC,
                Key.OemMinus => 0xBD,
                Key.OemPeriod => 0xBE,
                Key.OemQuestion | Key.Oem2 => 0xBF,
                Key.OemTilde | Key.Oem3 => 0xC0,
                Key.OemOpenBrackets | Key.Oem4 => 0xDB,
                Key.OemPipe | Key.Oem5 => 0xDC,
                Key.OemCloseBrackets | Key.Oem6 => 0xDD,
                Key.OemQuotes | Key.Oem7 => 0xDE,
                Key.Oem8 => 0xDF,
                Key.OemBackslash | Key.Oem102 => 0xE2,
                _ => 0
            };
        }
        #endregion
    }
}
