using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.TextInput;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using System;


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
            FocusAdornerProperty.OverrideDefaultValue<CefView>(null);

            UrlProperty.Changed.AddClassHandler<CefView>((s, e) => s.OnUrlChanged(e));
            IsVisibleProperty.Changed.AddClassHandler<CefView>((s, e) => s.OnVisibleChanged(e));

            TextInputMethodClientRequestedEvent.AddClassHandler<CefView>((s, e) => s.OnTextInputMethodClientRequested(e));
        }

        public string Url
        {
            get { return GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }

        private Cursor? _currentCursor;

        private Rect _cefViewRect = new Rect(0, 0, 1, 1);
        private WriteableBitmap? _cefViewImage;

        private Rect _cefPopupRect = new Rect(0, 0, 1, 1);
        private WriteableBitmap? _cefPopupImage;

        private CefViewTextInputMethodClient? _imClient;

        public CefView() : this(null, "")
        {
        }

        public CefView(CefSetting? setting) : this(setting, "")
        {
        }

        public CefView(CefSetting? setting, string? url)
        {
            FocusAdorner = null;

            if (Design.IsDesignMode)
            {
                return;
            }

            _imClient = new CefViewTextInputMethodClient(this);

            if (string.IsNullOrEmpty(url))
            {
                url = "about:blank";
            }

            SetCurrentValue(UrlProperty, url);
            InitializeNative(url, setting);
        }

        #region CEF Callback Methods
        void UI_OnCefInputStateChanged(int browserId, string frameId, bool editable)
        {
            using var _ = this.LogM($"editable={editable}");

            _isCefFocusedNodeEditable = editable;

            RunInUIThread(() =>
            {
                RaiseEvent(new TextInputMethodClientRequestedEventArgs()
                {
                    RoutedEvent = InputElement.TextInputMethodClientRequestedEvent,
                });
            },
            block: false);
        }

        void UI_OnCefCursorChanged(int browserId, CefViewCursorType type, CefViewCursorInfo customCursorInfo)
        {
            Cursor? cursor = CreateAvaloniaCursor(type, customCursorInfo);

            RunInUIThread(() =>
            {
                Cursor = cursor;

                _currentCursor?.Dispose();
                _currentCursor = cursor;
            },
            block: false);
        }

        void UI_OnCefFocusReleasedByTabKey(int browserId, bool next)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                var focusManager = TopLevel.GetTopLevel(this)?.FocusManager;
                var nextElement = KeyboardNavigationHandler.GetNext(focusManager?.GetFocusedElement()!, next ? NavigationDirection.Next : NavigationDirection.Previous);
                nextElement?.Focus(NavigationMethod.Tab);
            });
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

        void UI_OnCefGetRootScreenRect(int browserId, ref CefViewRect rect)
        {
            PixelRect bounds = new PixelRect(0, 0, 1, 1);

            RunInUIThread(() =>
            {
                bounds = (VisualRoot as Window)?.Screens?.ScreenFromVisual(this)?.Bounds ?? new PixelRect(0, 0, 1, 1);
            });

            rect.X = bounds.X;
            rect.Y = bounds.Y;
            rect.Width = bounds.Width;
            rect.Height = bounds.Height;
        }

        void UI_OnCefGetViewRect(int browserId, ref CefViewRect rect)
        {
            rect.X = 0;
            rect.Y = 0;
            rect.Width = (int)Bounds.Width;
            rect.Height = (int)Bounds.Height;
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
            var scale = VisualRoot?.RenderScaling ?? 1.0;
            PixelRect bounds = new PixelRect(0, 0, 1, 1);
            PixelRect workingArea = new PixelRect(0, 0, 1, 1);

            RunInUIThread(() =>
            {
                var screen = (VisualRoot as Window)?.Screens?.ScreenFromVisual(this);
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
            info.DeviceScaleFactor = (float)scale;

            return true;
        }

        void UI_OnCefPopupSize(int browserId, CefViewRect rect)
        {
            _cefPopupRect = new Rect(rect.X, rect.Y, rect.Width, rect.Height);
        }

        void UI_OnCefPaint(int browserId, CefViewPaintElementType type, CefViewRect[] dirtyRects, int dirtyRectCount, IntPtr imageBytesBuffer, int imageBytesCount, int width, int height)
        {
            RunInUIThread(() =>
            {
                WriteableBitmap? targetBitmap = (type == CefViewPaintElementType.PET_VIEW) ? _cefViewImage : _cefPopupImage;

                if (targetBitmap == null || targetBitmap.PixelSize.Width != width || targetBitmap.PixelSize.Height != height)
                {
                    targetBitmap?.Dispose();
                    targetBitmap = new WriteableBitmap(new PixelSize(width, height), new Vector(96, 96), PixelFormat.Bgra8888, AlphaFormat.Premul);
                    var scale = VisualRoot?.RenderScaling ?? 1.0;

                    if (type == CefViewPaintElementType.PET_VIEW)
                    {
                        _cefViewRect = _cefViewRect.WithWidth(width / scale).WithHeight(height / scale);
                        _cefViewImage = targetBitmap;
                    }
                    else
                    {
                        _cefPopupRect = _cefPopupRect.WithWidth(width / scale).WithHeight(height / scale);
                        _cefPopupImage = targetBitmap;
                    }
                }

                using (var fb = targetBitmap.Lock())
                {
                    unsafe
                    {
                        Buffer.MemoryCopy((void*)imageBytesBuffer, (void*)fb.Address, imageBytesCount, imageBytesCount);
                    }
                }

                InvalidateVisual();
            });
        }

        void UI_OnCefAcceleratedPaint(int browserId, CefViewPaintElementType type, CefViewRect[] dirtyRects, int dirtyRectCount, IntPtr sharedHandle, int planeBytesCount)
        {
        }

        void UI_OnCefImeCompositionRangeChanged(int browserId, CefViewRange selectedRange, CefViewRect[] characterBounds, int characterBoundsCount)
        {
            using var _ = this.LogM();

            var imeClient = _imClient;
            if (!_isCefFocusedNodeEditable || imeClient == null)
            {
                return;
            }

            RunInUIThread(() =>
            {
                imeClient.UpdateComposition(selectedRange, characterBounds);
            },
            block: true);
        }

        void UI_OnCefImeTextSelectionChanged(int browserId, string selectedText, CefViewRange selectedRange)
        {
        }
        #endregion

        #region UIElement Override And Event Handler
        private void OnUrlChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (_isCreated && e.NewValue is string url)
            {
                _cefBrowser?.NavigateToUrl(url);
            }
        }

        private void OnVisibleChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool isVisible)
            {
                _cefBrowser?.WasHidden(!isVisible);
            }
        }

        private void OnTextInputMethodClientRequested(TextInputMethodClientRequestedEventArgs e)
        {
            using var _ = this.LogM();

            if (IsFocused && _isCefFocusedNodeEditable)
            {
                e.Client = _imClient;
                this.LogI("set IME client to _imeClient");
            }
            else
            {
                e.Client = null;
                this.LogI("set IME client to null");
            }

            e.Handled = true;
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
            if (!_hasCefGotFocus)
            {
                return;
            }

            int virtualKey = GetWindowsVirtualKey(e.Key);
            var modifiers = GetModifiers(e.KeyModifiers, null) | GetKeyEventFlags(e.Key);
            var isSystemKey = ((modifiers & CefViewEventFlag.EVENTFLAG_ALT_DOWN) != 0);

            _cefBrowser?.SendKeyEvent(CefViewKeyEventType.KEYEVENT_KEYDOWN, (uint)modifiers, virtualKey, 0, isSystemKey, 0, 0, false);

            // skip tab/shift+tab key
            e.Handled = e.Key == Key.Tab;
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (!_hasCefGotFocus)
            {
                return;
            }

            int virtualKey = GetWindowsVirtualKey(e.Key);
            var modifiers = GetModifiers(e.KeyModifiers, null) | GetKeyEventFlags(e.Key);
            var isSystemKey = ((modifiers & CefViewEventFlag.EVENTFLAG_ALT_DOWN) != 0);

            _cefBrowser?.SendKeyEvent(CefViewKeyEventType.KEYEVENT_KEYUP, (uint)modifiers, virtualKey, 0, isSystemKey, 0, 0, false);

            // skip tab/shift+tab key
            e.Handled = e.Key == Key.Tab;
            base.OnKeyUp(e);
        }

        protected override void OnTextInput(TextInputEventArgs e)
        {
            using var _ = this.LogM();

            e.Handled = true;

            if (string.IsNullOrEmpty(e.Text))
            {
                return;
            }

            ImeCommitText(e.Text, CefViewTextInputMethodClient.InvalidRange, 0);
            _imClient?.ResetCompositionState();
        }

        protected override void OnGotFocus(GotFocusEventArgs e)
        {
            if (!_hasCefGotFocus)
            {
                _cefBrowser?.SetFocus(true);
            }

            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            // clear IME composition
            _imClient?.ResetCompositionState();

            // cancel context menu: TODO

            _hasCefGotFocus = false;
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
                context.DrawImage(_cefViewImage, srcRect, _cefViewRect);
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

        private Cursor CreateAvaloniaCursor(CefViewCursorType type, CefViewCursorInfo customCursorInfo)
        {
            if (type == CefViewCursorType.CT_CUSTOM)
            {
                var customCursor = CreateCustomCursor(customCursorInfo);
                if (customCursor != null)
                {
                    return customCursor;
                }
            }

            var standardType = type switch
            {
                CefViewCursorType.CT_POINTER => StandardCursorType.Arrow,
                CefViewCursorType.CT_CROSS => StandardCursorType.Cross,
                CefViewCursorType.CT_HAND => StandardCursorType.Hand,
                CefViewCursorType.CT_IBEAM => StandardCursorType.Ibeam,
                CefViewCursorType.CT_WAIT => StandardCursorType.Wait,
                CefViewCursorType.CT_HELP => StandardCursorType.Help,

                CefViewCursorType.CT_EASTRESIZE => StandardCursorType.RightSide,
                CefViewCursorType.CT_NORTHRESIZE => StandardCursorType.TopSide,
                CefViewCursorType.CT_NORTHEASTRESIZE => StandardCursorType.TopRightCorner,
                CefViewCursorType.CT_NORTHWESTRESIZE => StandardCursorType.TopLeftCorner,
                CefViewCursorType.CT_SOUTHRESIZE => StandardCursorType.BottomSide,
                CefViewCursorType.CT_SOUTHEASTRESIZE => StandardCursorType.BottomRightCorner,
                CefViewCursorType.CT_SOUTHWESTRESIZE => StandardCursorType.BottomLeftCorner,
                CefViewCursorType.CT_WESTRESIZE => StandardCursorType.LeftSide,
                CefViewCursorType.CT_NORTHSOUTHRESIZE => StandardCursorType.SizeNorthSouth,
                CefViewCursorType.CT_EASTWESTRESIZE => StandardCursorType.SizeWestEast,
                CefViewCursorType.CT_NORTHEASTSOUTHWESTRESIZE => StandardCursorType.TopRightCorner,
                CefViewCursorType.CT_NORTHWESTSOUTHEASTRESIZE => StandardCursorType.TopLeftCorner,
                CefViewCursorType.CT_COLUMNRESIZE => StandardCursorType.SizeWestEast,
                CefViewCursorType.CT_ROWRESIZE => StandardCursorType.SizeNorthSouth,

                CefViewCursorType.CT_MIDDLEPANNING => StandardCursorType.SizeAll,
                CefViewCursorType.CT_EASTPANNING => StandardCursorType.RightSide,
                CefViewCursorType.CT_NORTHPANNING => StandardCursorType.TopSide,
                CefViewCursorType.CT_NORTHEASTPANNING => StandardCursorType.TopRightCorner,
                CefViewCursorType.CT_NORTHWESTPANNING => StandardCursorType.TopLeftCorner,
                CefViewCursorType.CT_SOUTHPANNING => StandardCursorType.BottomSide,
                CefViewCursorType.CT_SOUTHEASTPANNING => StandardCursorType.BottomRightCorner,
                CefViewCursorType.CT_SOUTHWESTPANNING => StandardCursorType.BottomLeftCorner,
                CefViewCursorType.CT_WESTPANNING => StandardCursorType.LeftSide,
                CefViewCursorType.CT_MOVE => StandardCursorType.SizeAll,
                CefViewCursorType.CT_VERTICALTEXT => StandardCursorType.Ibeam,
                CefViewCursorType.CT_CELL => StandardCursorType.Cross,
                CefViewCursorType.CT_CONTEXTMENU => StandardCursorType.Arrow,
                CefViewCursorType.CT_ALIAS => StandardCursorType.DragLink,
                CefViewCursorType.CT_PROGRESS => StandardCursorType.AppStarting,
                CefViewCursorType.CT_NODROP => StandardCursorType.No,
                CefViewCursorType.CT_COPY => StandardCursorType.DragCopy,
                CefViewCursorType.CT_NONE => StandardCursorType.None,
                CefViewCursorType.CT_NOTALLOWED => StandardCursorType.No,
                CefViewCursorType.CT_ZOOMIN => StandardCursorType.Arrow,
                CefViewCursorType.CT_ZOOMOUT => StandardCursorType.Arrow,
                CefViewCursorType.CT_GRAB => StandardCursorType.Hand,
                CefViewCursorType.CT_GRABBING => StandardCursorType.SizeAll,
                CefViewCursorType.CT_MIDDLE_PANNING_VERTICAL => StandardCursorType.SizeNorthSouth,
                CefViewCursorType.CT_MIDDLE_PANNING_HORIZONTAL => StandardCursorType.SizeWestEast,
                CefViewCursorType.CT_DND_NONE => StandardCursorType.No,
                CefViewCursorType.CT_DND_MOVE => StandardCursorType.DragMove,
                CefViewCursorType.CT_DND_COPY => StandardCursorType.DragCopy,
                CefViewCursorType.CT_DND_LINK => StandardCursorType.DragLink,
                _ => StandardCursorType.Arrow
            };

            return new Cursor(standardType);
        }

        private Cursor? CreateCustomCursor(CefViewCursorInfo customCursorInfo)
        {
            int width = customCursorInfo.Size.Width;
            int height = customCursorInfo.Size.Height;

            if (customCursorInfo.Buffer == IntPtr.Zero || width <= 0 || height <= 0)
            {
                return null;
            }

            int hotspotX = Math.Clamp(customCursorInfo.Hotspot.X, 0, width - 1);
            int hotspotY = Math.Clamp(customCursorInfo.Hotspot.Y, 0, height - 1);
            var scale = customCursorInfo.ImageScaleFactor > 0 ? customCursorInfo.ImageScaleFactor : 1.0f;
            try
            {
                var bitmap = new Bitmap(
                    PixelFormat.Bgra8888,
                    AlphaFormat.Premul,
                    customCursorInfo.Buffer,
                    new PixelSize(width, height),
                    new Vector(96.0 * scale, 96.0 * scale),
                    width * 4);

                try
                {
                    return new Cursor(bitmap, new PixelPoint(hotspotX, hotspotY));
                }
                finally
                {
                    bitmap.Dispose();
                }
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}
