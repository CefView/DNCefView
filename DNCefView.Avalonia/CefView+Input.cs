using Avalonia.Input;

namespace DNCefView.Avalonia;

public partial class CefView
{
    static void ClassInitializeInput()
    {
    }

    void InitializeInput()
    {
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        if (!IsFocused)
        {
            Focus();
        }

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
        using var _ = this.LogM($"KeyEventArgs: Key:{e.Key}, Modifiers:{e.KeyModifiers}, PhysicalKey:{e.PhysicalKey}, KeySymbol:{e.KeySymbol}");

        if (e.Key == Key.ImeProcessed)
        {
            return;
        }

        var keyEvent = new CefKeyEventData();
        MapKeyEventToCefKeyEvent(e, ref keyEvent);

        SendKeyEvent(CefViewKeyEventType.KEYEVENT_RAWKEYDOWN, keyEvent);

        e.Handled = true;
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        using var _ =
            this.LogM(
                $"KeyEventArgs: Key:{e.Key}, Modifiers:{e.KeyModifiers}, PhysicalKey:{e.PhysicalKey}, KeySymbol:{e.KeySymbol}");

        var keyEvent = new CefKeyEventData();
        MapKeyEventToCefKeyEvent(e, ref keyEvent);

        SendKeyEvent(CefViewKeyEventType.KEYEVENT_KEYUP, keyEvent);

        e.Handled = true;
    }

    #region Shared Input

    private void SendTextInputAsKeyEvents(string text)
    {
        if (OperatingSystem.IsWindows())
        {
            SendWindowsTextInputAsKeyEvents(text);
            return;
        }

        SendLinuxTextInputAsKeyEvents(text);
    }

    private void MapKeyEventToCefKeyEvent(KeyEventArgs e, ref CefKeyEventData keyEvent)
    {
        if (OperatingSystem.IsMacOS())
        {
            MapMacOSKeyEventToCefKeyEvent(e, ref keyEvent);
            return;
        }

        if (OperatingSystem.IsWindows())
        {
            MapWindowsKeyEventToCefKeyEvent(e, ref keyEvent);
            return;
        }

        if (OperatingSystem.IsLinux())
        {
            MapLinuxKeyEventToCefKeyEvent(e, ref keyEvent);
            return;
        }
    }

    private bool ShouldSendKeyCharEvent(KeyEventArgs e)
    {
        if (OperatingSystem.IsMacOS())
        {
            return ShouldSendMacOSKeyCharEvent(e);
        }

        return false;
    }

    private void SendKeyEvent(CefViewKeyEventType eventType, CefKeyEventData keyEvent)
    {
        using var _ =
            this.LogM(
                $" >>>>>>>> send cef key event: {keyEvent.Character} {keyEvent.NativeKeyCode} {keyEvent.Modifiers}");
        _cefBrowser?.SendKeyEvent(
            eventType,
            (uint)keyEvent.Modifiers,
            keyEvent.WindowsKeyCode,
            keyEvent.NativeKeyCode,
            keyEvent.IsSystemKey,
            keyEvent.Character,
            keyEvent.UnmodifiedCharacter,
            _isCefFocusedNodeEditable);
    }

    #endregion

    #region macOS

    private void MapMacOSKeyEventToCefKeyEvent(KeyEventArgs e, ref CefKeyEventData keyEvent)
    {
        keyEvent.NativeKeyCode = MacGetNativeKeyCode(e);
        keyEvent.Modifiers = MacGetKeyboardModifiers(e);
        keyEvent.Character = MacGetKeyCharacter(e);
        keyEvent.UnmodifiedCharacter = MacGetUnmodifiedKeyCharacter(e);
        keyEvent.IsSystemKey = (keyEvent.Modifiers & CefViewEventFlag.EVENTFLAG_ALT_DOWN) != 0;
    }

    private bool ShouldSendMacOSKeyCharEvent(KeyEventArgs e)
    {
        if (_isCefFocusedNodeEditable)
        {
            return false;
        }

        if ((GetKeyEventFlags(e.Key) & CefViewEventFlag.EVENTFLAG_IS_KEY_PAD) != 0)
        {
            return false;
        }

        return e.Key switch
        {
            Key.LWin => false,
            Key.RWin => false,
            Key.LeftCtrl => false,
            Key.RightCtrl => false,
            Key.LeftShift => false,
            Key.RightShift => false,
            Key.LeftAlt => false,
            Key.RightAlt => false,
            Key.CapsLock => false,
            _ => true,
        };
    }

    private CefViewEventFlag MacGetKeyboardModifiers(KeyEventArgs e)
    {
        return GetModifiers(e.KeyModifiers, null) | GetKeyEventFlags(e.Key);
    }

    private static ushort MacGetKeyCharacter(KeyEventArgs e)
    {
        if (e.Key is Key.LWin or Key.RWin)
        {
            return 0;
        }

        if (e.Key is >= Key.F1 and <= Key.F24)
        {
            return (ushort)(0xF704 + ((int)e.Key - (int)Key.F1));
        }

        return e.Key switch
        {
            Key.Up => 0xF700,
            Key.Down => 0xF701,
            Key.Left => 0xF702,
            Key.Right => 0xF703,
            Key.Insert => 0xF727,
            Key.Delete => 0xF728,
            Key.Home => 0xF729,
            Key.End => 0xF72B,
            Key.PageUp => 0xF72C,
            Key.PageDown => 0xF72D,
            Key.Scroll => 0xF72F,
            Key.Pause => 0xF730,
            Key.Print => 0xF738,
            Key.Clear => 0xF73A,
            Key.Help => 0xF746,
            _ => GetCharacter(e.KeySymbol?.ToString()),
        };
    }

    private ushort MacGetUnmodifiedKeyCharacter(KeyEventArgs e)
    {
        if (e.Key is Key.LWin or Key.RWin)
        {
            return 0;
        }

        if (e.Key >= Key.F1 && e.Key <= Key.F24)
        {
            return (ushort)(0xF704 + ((int)e.Key - (int)Key.F1));
        }

        return e.Key switch
        {
            Key.Up => 0xF700,
            Key.Down => 0xF701,
            Key.Left => 0xF702,
            Key.Right => 0xF703,
            Key.Insert => 0xF727,
            Key.Delete => 0xF728,
            Key.Home => 0xF729,
            Key.End => 0xF72B,
            Key.PageUp => 0xF72C,
            Key.PageDown => 0xF72D,
            Key.Scroll => 0xF72F,
            Key.Pause => 0xF730,
            Key.Print => 0xF738,
            Key.Clear => 0xF73A,
            Key.Help => 0xF746,
            _ => (ushort)GetWindowsVirtualKey(e.Key),
        };
    }

    private int MacGetNativeKeyCode(KeyEventArgs e)
    {
        return e.PhysicalKey switch
        {
            PhysicalKey.A => 0x00,
            PhysicalKey.S => 0x01,
            PhysicalKey.D => 0x02,
            PhysicalKey.F => 0x03,
            PhysicalKey.H => 0x04,
            PhysicalKey.G => 0x05,
            PhysicalKey.Z => 0x06,
            PhysicalKey.X => 0x07,
            PhysicalKey.C => 0x08,
            PhysicalKey.V => 0x09,
            PhysicalKey.B => 0x0B,
            PhysicalKey.Q => 0x0C,
            PhysicalKey.W => 0x0D,
            PhysicalKey.E => 0x0E,
            PhysicalKey.R => 0x0F,
            PhysicalKey.Y => 0x10,
            PhysicalKey.T => 0x11,
            PhysicalKey.Digit1 => 0x12,
            PhysicalKey.Digit2 => 0x13,
            PhysicalKey.Digit3 => 0x14,
            PhysicalKey.Digit4 => 0x15,
            PhysicalKey.Digit6 => 0x16,
            PhysicalKey.Digit5 => 0x17,
            PhysicalKey.Equal => 0x18,
            PhysicalKey.Digit9 => 0x19,
            PhysicalKey.Digit7 => 0x1A,
            PhysicalKey.Minus => 0x1B,
            PhysicalKey.Digit8 => 0x1C,
            PhysicalKey.Digit0 => 0x1D,
            PhysicalKey.BracketRight => 0x1E,
            PhysicalKey.O => 0x1F,
            PhysicalKey.U => 0x20,
            PhysicalKey.BracketLeft => 0x21,
            PhysicalKey.I => 0x22,
            PhysicalKey.P => 0x23,
            PhysicalKey.Enter => 0x24,
            PhysicalKey.L => 0x25,
            PhysicalKey.J => 0x26,
            PhysicalKey.Quote => 0x27,
            PhysicalKey.K => 0x28,
            PhysicalKey.Semicolon => 0x29,
            PhysicalKey.Backslash => 0x2A,
            PhysicalKey.Comma => 0x2B,
            PhysicalKey.Slash => 0x2C,
            PhysicalKey.N => 0x2D,
            PhysicalKey.M => 0x2E,
            PhysicalKey.Period => 0x2F,
            PhysicalKey.Tab => 0x30,
            PhysicalKey.Space => 0x31,
            PhysicalKey.Backquote => 0x32,
            PhysicalKey.Backspace => 0x33,
            PhysicalKey.Escape => 0x35,
            PhysicalKey.MetaLeft => 0x37,
            PhysicalKey.MetaRight => 0x36,
            PhysicalKey.ShiftLeft => 0x38,
            PhysicalKey.CapsLock => 0x39,
            PhysicalKey.AltLeft => 0x3A,
            PhysicalKey.ControlLeft => 0x3B,
            PhysicalKey.ShiftRight => 0x3C,
            PhysicalKey.AltRight => 0x3D,
            PhysicalKey.ControlRight => 0x3E,
            PhysicalKey.F17 => 0x40,
            PhysicalKey.NumPadDecimal => 0x41,
            PhysicalKey.NumPadMultiply => 0x43,
            PhysicalKey.NumPadAdd => 0x45,
            PhysicalKey.NumLock => 0x47,
            PhysicalKey.AudioVolumeUp => 0x48,
            PhysicalKey.AudioVolumeDown => 0x49,
            PhysicalKey.AudioVolumeMute => 0x4A,
            PhysicalKey.NumPadDivide => 0x4B,
            PhysicalKey.NumPadEnter => 0x4C,
            PhysicalKey.NumPadSubtract => 0x4E,
            PhysicalKey.F18 => 0x4F,
            PhysicalKey.F19 => 0x50,
            PhysicalKey.NumPadEqual => 0x51,
            PhysicalKey.NumPad0 => 0x52,
            PhysicalKey.NumPad1 => 0x53,
            PhysicalKey.NumPad2 => 0x54,
            PhysicalKey.NumPad3 => 0x55,
            PhysicalKey.NumPad4 => 0x56,
            PhysicalKey.NumPad5 => 0x57,
            PhysicalKey.NumPad6 => 0x58,
            PhysicalKey.NumPad7 => 0x59,
            PhysicalKey.F20 => 0x5A,
            PhysicalKey.NumPad8 => 0x5B,
            PhysicalKey.NumPad9 => 0x5C,
            PhysicalKey.F5 => 0x60,
            PhysicalKey.F6 => 0x61,
            PhysicalKey.F7 => 0x62,
            PhysicalKey.F3 => 0x63,
            PhysicalKey.F8 => 0x64,
            PhysicalKey.F9 => 0x65,
            PhysicalKey.F11 => 0x67,
            PhysicalKey.F13 => 0x69,
            PhysicalKey.F16 => 0x6A,
            PhysicalKey.F14 => 0x6B,
            PhysicalKey.F10 => 0x6D,
            PhysicalKey.F12 => 0x6F,
            PhysicalKey.F15 => 0x71,
            PhysicalKey.Help => 0x72,
            PhysicalKey.Home => 0x73,
            PhysicalKey.PageUp => 0x74,
            PhysicalKey.Delete => 0x75,
            PhysicalKey.F4 => 0x76,
            PhysicalKey.End => 0x77,
            PhysicalKey.F2 => 0x78,
            PhysicalKey.PageDown => 0x79,
            PhysicalKey.F1 => 0x7A,
            PhysicalKey.ArrowLeft => 0x7B,
            PhysicalKey.ArrowRight => 0x7C,
            PhysicalKey.ArrowDown => 0x7D,
            PhysicalKey.ArrowUp => 0x7E,
            _ => 0,
        };
    }

    #endregion

    #region Windows

    private void SendWindowsTextInputAsKeyEvents(string text)
    {
        SendTextInputAsCharEvents(text);
    }

    private void MapWindowsKeyEventToCefKeyEvent(KeyEventArgs e, ref CefKeyEventData keyEvent)
    {
        keyEvent.WindowsKeyCode = WinGetWindowsKeyCode(e);
        keyEvent.Modifiers = WinGetKeyboardModifiers(e);
        keyEvent.IsSystemKey = (keyEvent.Modifiers & CefViewEventFlag.EVENTFLAG_ALT_DOWN) != 0;
    }

    private int WinGetWindowsKeyCode(KeyEventArgs e)
    {
        return GetWindowsVirtualKey(e.Key);
    }

    private CefViewEventFlag WinGetKeyboardModifiers(KeyEventArgs e)
    {
        return GetModifiers(e.KeyModifiers, null) | GetKeyEventFlags(e.Key);
    }

    #endregion

    #region Linux

    private void SendLinuxTextInputAsKeyEvents(string text)
    {
        SendTextInputAsCharEvents(text);
    }

    private void MapLinuxKeyEventToCefKeyEvent(KeyEventArgs e, ref CefKeyEventData keyEvent)
    {
        keyEvent.WindowsKeyCode = GetWindowsVirtualKey(e.Key);
        keyEvent.Modifiers = GetModifiers(e.KeyModifiers, null) | GetKeyEventFlags(e.Key);
        keyEvent.IsSystemKey = (keyEvent.Modifiers & CefViewEventFlag.EVENTFLAG_ALT_DOWN) != 0;
    }

    #endregion

    #region Common Key Helpers

    private void SendTextInputAsCharEvents(string text)
    {
        foreach (var rune in text.EnumerateRunes())
        {
            var utf16 = rune.ToString();
            if (string.IsNullOrEmpty(utf16))
            {
                continue;
            }

            var character = utf16[0];
            _cefBrowser?.SendKeyEvent(
                CefViewKeyEventType.KEYEVENT_CHAR,
                0,
                character,
                0,
                false,
                character,
                character,
                _isCefFocusedNodeEditable);
        }
    }

    private static ushort GetCharacter(string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return 0;
        }

        return text[0];
    }

    private struct CefKeyEventData
    {
        public int WindowsKeyCode;
        public int NativeKeyCode;
        public ushort Character;
        public ushort UnmodifiedCharacter;
        public CefViewEventFlag Modifiers;
        public bool IsSystemKey;
    }

    #endregion

    private static CefViewEventFlag GetModifiers(KeyModifiers? keys, PointerPointProperties? mouse)
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

    private static CefViewEventFlag GetKeyEventFlags(Key key)
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

        if (key
            is >= Key.NumPad0
            and <= Key.NumPad9
            or Key.Multiply
            or Key.Add
            or Key.Separator
            or Key.Subtract
            or Key.Decimal
            or Key.Divide)
        {
            modifiers |= CefViewEventFlag.EVENTFLAG_IS_KEY_PAD;
        }

        return modifiers;
    }

    private int GetWindowsVirtualKey(Key key)
    {
        if (key is >= Key.A and <= Key.Z)
            return 0x41 + ((int)key - (int)Key.A);

        if (key is >= Key.D0 and <= Key.D9)
            return 0x30 + ((int)key - (int)Key.D0);

        if (key is >= Key.NumPad0 and <= Key.NumPad9)
            return 0x60 + ((int)key - (int)Key.NumPad0);

        if (key is >= Key.F1 and <= Key.F24)
            return 0x70 + ((int)key - (int)Key.F1);

        return key switch
        {
            Key.Cancel => 0x03,
            Key.Back => 0x08,
            Key.Tab => 0x09,
            Key.LineFeed => 0x0A,
            Key.Clear => 0x0C,
            Key.Enter | Key.Return => 0x0D,
            Key.Pause => 0x13,
            Key.Capital | Key.CapsLock => 0x14,
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