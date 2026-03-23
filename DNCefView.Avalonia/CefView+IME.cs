using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Avalonia;
using Avalonia.Input;
using Avalonia.Input.TextInput;
using Avalonia.Threading;

namespace DNCefView.Avalonia;

public partial class CefView
{
    /// <summary>
    /// 
    /// </summary>
    [SupportedOSPlatform("macos")]
    static partial class AvnViewPatch
    {
        [LibraryImport("/usr/lib/libobjc.dylib", StringMarshalling = StringMarshalling.Utf8)]
        private static partial IntPtr objc_getClass(string name);

        [LibraryImport("/usr/lib/libobjc.dylib")]
        private static partial IntPtr class_getInstanceMethod(IntPtr cls, IntPtr sel);

        [LibraryImport("/usr/lib/libobjc.dylib")]
        private static partial IntPtr method_setImplementation(IntPtr method, IntPtr imp);

        [LibraryImport("/usr/lib/libobjc.dylib", StringMarshalling = StringMarshalling.Utf8)]
        private static partial IntPtr sel_registerName(string name);

        [LibraryImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
        private static partial IntPtr objc_msgSend(IntPtr receiver, IntPtr sel);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr AttributedSubstringDelegate(
            IntPtr self,
            IntPtr sel,
            nuint rangeLocation,
            nuint rangeLength,
            IntPtr actualRangePtr);

        private static readonly AttributedSubstringDelegate SwizzledImplementation = PatchedAttributedSubstring;

        private static IntPtr _originalImplementation = IntPtr.Zero;

        const string AvnViewClassName = "AvnView";

        private const string AvnViewOriginalSelectorAttributedSubstringForProposedRange =
            "attributedSubstringForProposedRange:actualRange:";

        public static void Setup()
        {
            if (_originalImplementation != IntPtr.Zero) return;

            var cls = objc_getClass(AvnViewClassName);

            // get original method information
            var originalSelector = sel_registerName(AvnViewOriginalSelectorAttributedSubstringForProposedRange);
            var originalMethod = class_getInstanceMethod(cls, originalSelector);
            var swizzleImplementation = Marshal.GetFunctionPointerForDelegate(SwizzledImplementation);
            _originalImplementation = method_setImplementation(originalMethod, swizzleImplementation);
        }

        private static IntPtr PatchedAttributedSubstring(
            IntPtr self,
            IntPtr sel,
            nuint rangeLocation,
            nuint rangeLength,
            IntPtr actualRangePtr)
        {
            return objc_msgSend(objc_getClass("NSAttributedString"), sel_registerName("new"));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    sealed class CefViewTextInputMethodClient : TextInputMethodClient
    {
        private readonly CefView? _owner;

        private Rect _cursorRectangle;

        internal CefViewTextInputMethodClient(CefView owner)
        {
            _owner = owner;
        }

        internal void UpdateComposition(CefViewRange selectedRange, CefViewRect[] charBounds)
        {
            using var _ = this.LogM("CefView[IME]:");

            if (_owner == null)
            {
                return;
            }

            if (charBounds.Length == 0)
            {
                return;
            }

            _cursorRectangle = charBounds
                .Select(r => new Rect(r.X, r.Y, r.Width, r.Height))
                .Aggregate((acc, r) => acc.Union(r));

            this.LogD(
                $"new _cursorRectangle: ({_cursorRectangle.X}, {_cursorRectangle.Y}) - [{_cursorRectangle.Width}, {_cursorRectangle.Height}]");

            Dispatcher.UIThread.Post(RaiseCursorRectangleChanged);
        }

        private Rect GetCursorRectangle()
        {
            return _cursorRectangle;
        }

        #region TextInputMethodClient

        public override Visual TextViewVisual => _owner!;
        public override Rect CursorRectangle => GetCursorRectangle();
        public override bool SupportsPreedit => true;

        public override void SetPreeditText(string? text)
        {
            using var _ = this.LogM($"CefView[IME]:text={text}");

            if (null == _owner)
            {
                return;
            }

            if (!string.IsNullOrEmpty(text))
            {
                var underline = new CefViewCompositionUnderline()
                {
                    BackgroundColor = 0,
                    Range = new CefViewRange(0, (uint)(text?.Length ?? 0)),
                    Style = CefViewCompositionUnderlineStyle.CEF_CUS_DOT,
                };

                // in composing
                this.LogD($"composing update");
                _owner.ImeSetComposition(text, [underline],
                    new(uint.MaxValue, uint.MaxValue),
                    new((uint)text.Length, (uint)text.Length));
            }
            else
            {
                // composing end
                this.LogD($"composing end");
                Dispatcher.UIThread.Post(() => { _owner?.ImeCancelComposition(); }, DispatcherPriority.Input);
            }
        }

        public override TextSelection Selection { get; set; } = new();
        public override bool SupportsSurroundingText => false;
        public override string SurroundingText => string.Empty;

        #endregion
    }

    static void ClassInitializeIME()
    {
        if (OperatingSystem.IsMacOS())
            AvnViewPatch.Setup();
        
        TextInputMethodClientRequestedEvent.AddClassHandler<CefView>((s, e) => s.OnTextInputMethodClientRequested(e));
    }

    private bool _isCefFocusedNodeEditable;

    private CefViewTextInputMethodClient? _imClient;

    void InitializeIME()
    {
        _imClient = new CefViewTextInputMethodClient(this);
    }

    void UI_OnCefInputStateChanged(int browserId, string frameId, bool editable)
    {
        using var _ = this.LogM($"CefView[IME]:editable={editable}");

        _isCefFocusedNodeEditable = editable;

        RunInUIThread(() =>
            {
                RaiseEvent(new TextInputMethodClientRequeryRequestedEventArgs()
                {
                    RoutedEvent = InputMethod.TextInputMethodClientRequeryRequestedEvent,
                });
            },
            block: false);
    }

    void OnTextInputMethodClientRequested(TextInputMethodClientRequestedEventArgs e)
    {
        using var _ = this.LogM("CefView[IME]:");

        if (IsFocused && _isCefFocusedNodeEditable)
        {
            e.Client = _imClient;
            this.LogI("set IME client to _imeClient");

            // tricky code to trigger CEF updating of caret rect
            ImeSetComposition(" ", [], new(uint.MaxValue, uint.MaxValue), new(1, 1));
            Dispatcher.UIThread.Post(ImeCancelComposition, DispatcherPriority.Input);
        }
        else
        {
            e.Client = null;
            this.LogI("set IME client to null");
            ImeCancelComposition();
        }
    }

    void UI_OnCefImeCompositionRangeChanged(int browserId, CefViewRange selectedRange, CefViewRect[] characterBounds,
        int characterBoundsCount)
    {
        using var _ = this.LogM($"CefView[IME]:char bounds: {characterBounds.Length}");

        var imeClient = _imClient;
        if (!_isCefFocusedNodeEditable || imeClient == null)
        {
            return;
        }

        imeClient.UpdateComposition(selectedRange, characterBounds);
    }

    protected override void OnTextInput(TextInputEventArgs e)
    {
        using var _ = this.LogM($"text: {e.Text}");

        if (!_isCefFocusedNodeEditable)
        {
            return;
        }

        if (e.Handled)
        {
            return;
        }

        e.Handled = true;

        if (string.IsNullOrEmpty(e.Text))
        {
            return;
        }

        ImeCommitText(e.Text, new(uint.MaxValue, uint.MaxValue), 0);
    }
}