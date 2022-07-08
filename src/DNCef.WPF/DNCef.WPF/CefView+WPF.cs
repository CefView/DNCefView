using IMESupport;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DNCef
{
    public partial class CefView : FrameworkElement
    {
        public static readonly DependencyProperty UrlProperty
            = DependencyProperty.Register(
                "Url",
                typeof(string),
                typeof(CefView),
                new PropertyMetadata(new PropertyChangedCallback(OnPropertySet)));

        private static void OnPropertySet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CefView? cefView = d as CefView;
            string? url = e.NewValue as string;
            if (cefView != null && url != null)
            {
                cefView.Url = url;
                if (cefView._isCreated)
                {
                    cefView._cefBrowser.NavigateToString(url);
                }
            }
        }

        static CefView()
        {
            FocusableProperty.OverrideMetadata(
                typeof(CefView), new FrameworkPropertyMetadata(true));
            FocusVisualStyleProperty.OverrideMetadata(
                typeof(CefView), new FrameworkPropertyMetadata((object)null));

            KeyboardNavigation.IsTabStopProperty.OverrideMetadata(
                typeof(CefView), new FrameworkPropertyMetadata(true));
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(
                typeof(CefView), new FrameworkPropertyMetadata(KeyboardNavigationMode.None));

            InputMethod.IsInputMethodEnabledProperty.OverrideMetadata(
                typeof(CefView), new FrameworkPropertyMetadata(false));
            InputMethod.IsInputMethodSuspendedProperty.OverrideMetadata(
                typeof(CefView), new FrameworkPropertyMetadata(false));
        }

        public string Url
        {
            get { return (string)GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }

        private Rect _cefPopupRect;
        private ImageSource _cefPopupImage;

        private Rect _cefViewRect;
        private ImageSource _cefViewImage;

        float DevicePixelRatio = 1.0f;
        long DeviceDpi = 96;

        WPFImeHandler _wpfImeHandler;

        public CefView() : this(null, "")
        {
            _wpfImeHandler = new WPFImeHandler(this);
        }

        public CefView(CefSetting setting) : this(setting, "")
        {
        }

        public CefView(CefSetting setting, string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                url = "about:blank";
            }

            IsVisibleChanged += OnVisibleChanged;
            PresentationSource.AddSourceChangedHandler(this, OnPresentationSourceChanged);

            SetValue(UrlProperty, url);
            InitializeNative(url, setting);
        }

        #region CEF Callback Methods
        bool WPF_OnCefInputStateChanged(int browserId, long frameId, bool editable)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (editable)
                {
                    _wpfImeHandler.EnableInputMethod();
                }
                else
                {
                    _wpfImeHandler.DisableInputMethod();
                }
            });

            return true;
        }

        void WPF_OnCefAfterCreated()
        {
            this.Dispatcher.Invoke(() =>
            {
                _isCreated = true;
                _cefBrowser.WasHidden(!IsVisible);
                _cefBrowser.WasResized();
                _cefBrowser.NavigateToUrl(Url);
            });
        }

        void WPF_OnCefFocusReleasedByTabKey(int browserId, bool next)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (next)
                {
                    MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
                else
                {
                    MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
                }
            });
        }

        void WPF_OnCefGetRootScreenRect(int browserId, ref CefViewRect rect)
        {
            System.Drawing.Rectangle rc = new System.Drawing.Rectangle(0, 0, 1, 1);
            this.Dispatcher.Invoke(() =>
            {
                var window = Window.GetWindow(this);
                if (null == window)
                {
                    window = Application.Current.MainWindow;
                }

                if (null == window)
                {
                    // primary screen
                    rc.X = 0;
                    rc.Y = 0;
                    rc.Width = (int)SystemParameters.PrimaryScreenWidth;
                    rc.Height = (int)SystemParameters.PrimaryScreenHeight;
                }
                else
                {
                    // current screen
                    rc.X = (int)SystemParameters.WorkArea.Left;
                    rc.Y = (int)SystemParameters.WorkArea.Right;
                    rc.Width = (int)SystemParameters.WorkArea.Width;
                    rc.Height = (int)SystemParameters.WorkArea.Height;
                }
            });

            rect.X = rc.X;
            rect.Y = rc.Y;
            rect.Width = rc.Width;
            rect.Height = rc.Height;
        }

        void WPF_OnCefGetViewRect(int browserId, ref CefViewRect rect)
        {
            rect.X = 0;
            rect.Y = 0;
            rect.Width = RenderSize.Width == 0 ? 1 : (int)RenderSize.Width;
            rect.Height = RenderSize.Height == 0 ? 1 : (int)RenderSize.Height;
        }

        bool WPF_OnCefGetScreenPoint(int browserId, int viewX, int viewY, out int screenX, out int screenY)
        {
            Point p;
            this.Dispatcher.Invoke(() =>
            {
                p = PointToScreen(new Point(viewX, viewX));
            });

            screenX = (int)p.X;
            screenY = (int)p.Y;
            return true;
        }

        bool WPF_OnCefGetScreenInfo(int browserId, ref CefViewScreenInfo info)
        {
            System.Drawing.Rectangle rc = new System.Drawing.Rectangle(0, 0, 1, 1);
            this.Dispatcher.Invoke(() =>
            {
                var window = Window.GetWindow(this);
                if (null == window)
                {
                    window = Application.Current.MainWindow;
                }

                if (null == window)
                {
                    // primary screen
                    rc.X = 0;
                    rc.Y = 0;
                    rc.Width = (int)SystemParameters.PrimaryScreenWidth;
                    rc.Height = (int)SystemParameters.PrimaryScreenHeight;
                }
                else
                {
                    // current screen
                    rc.X = (int)SystemParameters.WorkArea.Left;
                    rc.Y = (int)SystemParameters.WorkArea.Right;
                    rc.Width = (int)SystemParameters.WorkArea.Width;
                    rc.Height = (int)SystemParameters.WorkArea.Height;
                }
            });

            info.Depth = 32;
            info.DepthPerComponent = 0;
            info.IsMonochrome = 0;
            info.Rect.X = rc.X;
            info.Rect.Y = rc.Y;
            info.Rect.Width = rc.Width;
            info.Rect.Height = rc.Height;
            info.AvailableRect.X = rc.X;
            info.AvailableRect.Y = rc.Y;
            info.AvailableRect.Width = rc.Width;
            info.AvailableRect.Height = rc.Height;
            info.DeviceScaleFactor = DevicePixelRatio;

            return true;
        }

        void WPF_OnCefPopupSize(int browserId, CefViewRect rect)
        {
            _cefPopupRect.X = rect.X;
            _cefPopupRect.Y = rect.Y;
            _cefPopupRect.Width = rect.Width;
            _cefPopupRect.Height = rect.Height;
        }

        void WPF_OnCefPaint(int browserId, CefViewPaintElementType type, CefViewRect[] dirtyRects, int dirtyRectCount, byte[] imageBytes, int imageBytesCount, int width, int height)
        {
            this.Dispatcher.Invoke(() =>
            {
                // create image source
                var imageSource = BitmapSource.Create(
                    width,
                    height,
                    DeviceDpi,
                    DeviceDpi,
                    PixelFormats.Bgra32,
                    null,
                    imageBytes,
                    4 * width);

                // update target image source
                if (type == CefViewPaintElementType.PET_VIEW)
                {
                    _cefViewRect = new Rect(0, 0, width, height);
                    _cefViewImage = imageSource;
                }
                else
                {
                    _cefPopupImage = imageSource;
                }

                // invalidate visual to produce repaint
                InvalidateVisual();
            });
        }

        void WPF_OnCefImeCompositionRangeChanged(int browserId, CefViewRange range, CefViewRect[] characterBounds, int characterBoundsCount)
        {
            var imeKeyboardHandler = _wpfImeHandler;
            if (imeKeyboardHandler.IsActive)
            {
                var scaleFactor = 1.0f;
                this.Dispatcher.Invoke(() =>
                {
                    var parentWindow = GetParentWindow();
                    if (parentWindow != null)
                    {
                        var offset = TransformToAncestor(parentWindow).Transform(new System.Windows.Point(0, 0));

                        int l = int.MaxValue;
                        int r = int.MinValue;
                        int t = int.MaxValue;
                        int b = int.MinValue;
                        foreach (var item in characterBounds)
                        {
                            l = Math.Min(l, item.X);
                            r = Math.Max(r, item.X + item.Width);
                            t = Math.Min(t, item.Y);
                            b = Math.Max(b, item.Y + item.Height);
                        }

                        var rect = new CefViewRect(
                            (int)((offset.X + l) * scaleFactor),
                            (int)((offset.Y + t) * scaleFactor),
                            (int)((r - l) * scaleFactor),
                            (int)((b - t) * scaleFactor));

                        imeKeyboardHandler.UpdateComposition(range, rect);
                    }
                });
            }

            Visual GetParentWindow()
            {
                var current = VisualTreeHelper.GetParent(this);
                while (current != null && !(current is Window))
                    current = VisualTreeHelper.GetParent(current);

                return current as Window;
            }
        }
        #endregion

        #region UIElement Override And Event Handler
        private void OnWindowLocationChanged(object? sender, EventArgs e)
        {
            _cefBrowser.NotifyMoveOrResizeStarted();
        }

        private void OnVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _cefBrowser.WasHidden(!(bool)e.NewValue);
        }

        private void OnPresentationSourceChanged(object sender, SourceChangedEventArgs e)
        {
            if (null != e.NewSource)
            {
                DevicePixelRatio = (float)e.NewSource.CompositionTarget.TransformToDevice.M11;
                DeviceDpi = (long)(96 * e.NewSource.CompositionTarget.TransformToDevice.M11);

                _wpfImeHandler.InitialiseHWND((HwndSource)e.NewSource);
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left || e.ChangedButton == MouseButton.Middle || e.ChangedButton == MouseButton.Right)
            {
                Focus();
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            var p = e.GetPosition(this);
            var modifiers = GetKeyboardModifiers() | GetMouseModifiers(e);
            _cefBrowser.SendMouseMoveEvent((int)p.X, (int)p.Y, (uint)modifiers, false);
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            var p = e.GetPosition(this);
            var modifiers = GetKeyboardModifiers() | GetMouseModifiers(e);
            _cefBrowser.SendMouseClickEvent((int)p.X, (int)p.Y, (uint)modifiers, CefViewMouseButtonType.MBT_LEFT, false, 1);
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            var p = e.GetPosition(this);
            var modifiers = GetKeyboardModifiers() | GetMouseModifiers(e);
            _cefBrowser.SendMouseClickEvent((int)p.X, (int)p.Y, (uint)modifiers, CefViewMouseButtonType.MBT_LEFT, true, 1);
            base.OnMouseLeftButtonUp(e);
        }

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            var p = e.GetPosition(this);
            var modifiers = GetKeyboardModifiers() | GetMouseModifiers(e);
            _cefBrowser.SendMouseClickEvent((int)p.X, (int)p.Y, (uint)modifiers, CefViewMouseButtonType.MBT_RIGHT, false, 1);
            base.OnMouseRightButtonDown(e);
        }

        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            var p = e.GetPosition(this);
            var modifiers = GetKeyboardModifiers() | GetMouseModifiers(e);
            _cefBrowser.SendMouseClickEvent((int)p.X, (int)p.Y, (uint)modifiers, CefViewMouseButtonType.MBT_RIGHT, true, 1);
            base.OnMouseRightButtonUp(e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            var p = e.GetPosition(this);
            var modifiers = GetKeyboardModifiers() | GetMouseModifiers(e);
            var isShiftDown = (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift));
            int deltaY = isShiftDown ? 0 : e.Delta;
            _cefBrowser.SendWheelEvent((int)p.X, (int)p.Y, (uint)modifiers, 0, deltaY);
            base.OnMouseWheel(e);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Tab ||
                e.Key == Key.Left ||
                e.Key == Key.Right ||
                e.Key == Key.Up ||
                e.Key == Key.Down)
            {
                OnKeyDown(e);
                e.Handled = true;
            }
        }

        protected override void OnPreviewKeyUp(KeyEventArgs e)
        {
            if (e.Key == Key.Tab ||
                e.Key == Key.Left ||
                e.Key == Key.Right ||
                e.Key == Key.Up ||
                e.Key == Key.Down)
            {
                OnKeyUp(e);
                e.Handled = true;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("+++++ OnKeyDown");
            var modifiers = GetKeyboardModifiers();
            var virtualKey = KeyInterop.VirtualKeyFromKey(e.Key);
            var isSystemKey = ((modifiers & CefViewEventFlag.EVENTFLAG_ALT_DOWN) != 0);

            // send key down event
            _cefBrowser.SendKeyEvent(CefViewKeyEventType.KEYEVENT_KEYDOWN, (uint)modifiers, virtualKey, 0, isSystemKey, 0, 0, false);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("----- OnKeyUp");
            var modifiers = GetKeyboardModifiers();
            var virtualKey = KeyInterop.VirtualKeyFromKey(e.Key);
            var isSystemKey = ((modifiers & CefViewEventFlag.EVENTFLAG_ALT_DOWN) != 0);

            // send key up event
            _cefBrowser.SendKeyEvent(CefViewKeyEventType.KEYEVENT_KEYUP, (uint)modifiers, virtualKey, 0, isSystemKey, 0, 0, false);
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
        }

        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
            {
                var modifiers = GetKeyboardModifiers();
                int winKeyCode = c;
                ushort character = c;
                ushort unmodifiedCharacter = c;

                // send key char event
                _cefBrowser.SendKeyEvent(CefViewKeyEventType.KEYEVENT_CHAR, (uint)modifiers, winKeyCode, 0, false, character, unmodifiedCharacter, false);
            }

            base.OnTextInput(e);
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            if (Editable)
            {
                _wpfImeHandler.EnableInputMethod();
            }
            else
            {
                _wpfImeHandler.DisableInputMethod();
            }

            _cefBrowser.SetFocus(true);
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            _wpfImeHandler.DisableInputMethod();

            _cefBrowser.SetFocus(false);
            base.OnLostFocus(e);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo info)
        {
            _cefBrowser.WasResized();
            _cefBrowser.NotifyMoveOrResizeStarted();
            base.OnRenderSizeChanged(info);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Rect rect = _cefViewRect;
            rect.Height /= DevicePixelRatio;
            rect.Width /= DevicePixelRatio;
            drawingContext.DrawImage(_cefViewImage, rect);
            if (_isShowPopup)
            {
                drawingContext.DrawImage(_cefPopupImage, _cefPopupRect);
            }
        }
        #endregion

        #region Private Uitls
        private CefViewEventFlag GetKeyboardModifiers()
        {
            CefViewEventFlag modifiers = 0;

            // CONTROL
            modifiers |= Keyboard.IsKeyDown(Key.LeftCtrl) ? CefViewEventFlag.EVENTFLAG_CONTROL_DOWN | CefViewEventFlag.EVENTFLAG_IS_LEFT : 0;
            modifiers |= Keyboard.IsKeyDown(Key.RightCtrl) ? CefViewEventFlag.EVENTFLAG_CONTROL_DOWN | CefViewEventFlag.EVENTFLAG_IS_RIGHT : 0;

            // SHIFT
            modifiers |= Keyboard.IsKeyDown(Key.LeftShift) ? CefViewEventFlag.EVENTFLAG_SHIFT_DOWN | CefViewEventFlag.EVENTFLAG_IS_LEFT : 0;
            modifiers |= Keyboard.IsKeyDown(Key.RightCtrl) ? CefViewEventFlag.EVENTFLAG_SHIFT_DOWN | CefViewEventFlag.EVENTFLAG_IS_RIGHT : 0;

            // ALT
            modifiers |= Keyboard.IsKeyDown(Key.LeftAlt) ? CefViewEventFlag.EVENTFLAG_ALT_DOWN | CefViewEventFlag.EVENTFLAG_IS_LEFT : 0;
            modifiers |= Keyboard.IsKeyDown(Key.RightAlt) ? CefViewEventFlag.EVENTFLAG_ALT_DOWN | CefViewEventFlag.EVENTFLAG_IS_RIGHT : 0;

            // WIN
            modifiers |= Keyboard.IsKeyDown(Key.LWin) ? CefViewEventFlag.EVENTFLAG_ALT_DOWN | CefViewEventFlag.EVENTFLAG_IS_LEFT : 0;
            modifiers |= Keyboard.IsKeyDown(Key.RWin) ? CefViewEventFlag.EVENTFLAG_ALT_DOWN | CefViewEventFlag.EVENTFLAG_IS_RIGHT : 0;

            return modifiers;
        }

        private CefViewEventFlag GetMouseModifiers(MouseEventArgs e)
        {
            CefViewEventFlag modifiers = 0;
            modifiers |= e.LeftButton == MouseButtonState.Pressed ? CefViewEventFlag.EVENTFLAG_LEFT_MOUSE_BUTTON : 0;
            modifiers |= e.RightButton == MouseButtonState.Pressed ? CefViewEventFlag.EVENTFLAG_RIGHT_MOUSE_BUTTON : 0;
            modifiers |= e.MiddleButton == MouseButtonState.Pressed ? CefViewEventFlag.EVENTFLAG_MIDDLE_MOUSE_BUTTON : 0;
            return modifiers;
        }
        #endregion
    }
}
