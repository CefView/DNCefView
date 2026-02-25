using Avalonia.Input;


namespace DNCefView.Avalonia
{
    public partial class CefView
    {
        static void ClassInitializeInput()
        {
        }

        void InitializeInput()
        {
        }

        #region UIElement Override And Event Handler
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            var p = e.GetPosition(this);
            var modifiers = GetModifiers(e.KeyModifiers, e.GetCurrentPoint(this).Properties);
            var mouseButton = e.GetCurrentPoint(this).Properties.PointerUpdateKind switch
            {
                PointerUpdateKind.LeftButtonPressed => CefViewMouseButtonType.MBT_LEFT,
                PointerUpdateKind.RightButtonPressed => CefViewMouseButtonType.MBT_RIGHT,
                PointerUpdateKind.MiddleButtonPressed => CefViewMouseButtonType.MBT_MIDDLE,
                _ => CefViewMouseButtonType.MBT_LEFT
            };

            _cefBrowser?.SendMouseClickEvent((int)p.X, (int)p.Y, (uint)modifiers, mouseButton, false, 1);
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            base.OnPointerReleased(e);

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
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);

            var p = e.GetPosition(this);
            var modifiers = GetModifiers(e.KeyModifiers, e.GetCurrentPoint(this).Properties);

            _cefBrowser?.SendMouseMoveEvent((int)p.X, (int)p.Y, (uint)modifiers, false);
        }

        protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
        {
            base.OnPointerWheelChanged(e);

            var p = e.GetPosition(this);
            var modifiers = GetModifiers(e.KeyModifiers, e.GetCurrentPoint(this).Properties);
            int deltaX = (int)(e.Delta.X * 100);
            int deltaY = (int)(e.Delta.Y * 100);

            _cefBrowser?.SendWheelEvent((int)p.X, (int)p.Y, (uint)modifiers, deltaX, deltaY);
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
        #endregion

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
    }
}
