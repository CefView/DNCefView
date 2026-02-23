using Avalonia;
using Avalonia.Input.TextInput;
using System.Linq;


namespace DNCefView.Avalonia
{
    internal sealed class CefViewTextInputMethodClient : TextInputMethodClient
    {
        internal static readonly CefViewRange InvalidRange = new CefViewRange(0xFFFFFFFF, 0xFFFFFFFF);

        private readonly CefView? _owner;

        private bool _isComposing = false;

        private Rect _cursorRectangle;

        private TextSelection _selectedRange;

        internal CefViewTextInputMethodClient(CefView owner)
        {
            _owner = owner;
        }

        internal bool IsComposing => _isComposing;

        internal void UpdateComposition(CefViewRange selectedRange, CefViewRect[] charBounds)
        {
            using var _ = this.LogM();

            _selectedRange = new TextSelection((int)selectedRange.From, (int)selectedRange.To);
            this.LogD($"new _selectedRange: {_selectedRange.Start} -> {_selectedRange.End}");

            _cursorRectangle = charBounds
                .Select(r => new Rect(r.X, r.Y, r.Width, r.Height))
                .Aggregate((acc, r) => acc.Union(r));
            this.LogD($"new _cursorRectangle: [{_cursorRectangle.X}, {_cursorRectangle.Y}] - [{_cursorRectangle.Width}, {_cursorRectangle.Height}]");

            RaiseCursorRectangleChanged();
            RaiseSelectionChanged();
        }

        internal void ResetCompositionState()
        {
            _isComposing = false;
            _cursorRectangle = new();
            _selectedRange = new();
        }

        #region TextInputMethodClient
        public override Visual TextViewVisual => _owner!;

        public override bool SupportsPreedit => true;

        public override bool SupportsSurroundingText => false;

        public override string SurroundingText => string.Empty;

        public override Rect CursorRectangle => _cursorRectangle;

        public override TextSelection Selection
        {
            get => _selectedRange;

            set
            {
                using var _ = this.LogM($"range:{value.Start} -> {value.End}");

                if (_isComposing)
                {
                    _selectedRange = value;
                }
            }
        }

        public override void SetPreeditText(string? text)
        {
            using var _ = this.LogM($"text={text}");

            if (null == _owner)
            {
                return;
            }

            if (string.IsNullOrEmpty(text))
            {
                if (_isComposing)
                {
                    // composing ends 
                    this.LogI("---- IME composing ends");
                    _isComposing = false;

                    _owner.ImeCancelComposition();
                }
                else
                {
                    // composing begins
                    this.LogI("---- IME composing begins");
                    _isComposing = true;

                }

                return;
            }

            var underline = new CefViewCompositionUnderline()
            {
                BackgroundColor = 0,
                Range = new CefViewRange(0, (uint)text.Length),
                Style = CefViewCompositionUnderlineStyle.CEF_CUS_DOT,
            };

            var replacementRange = new CefViewRange()
            {
                From = InvalidRange.From,
                To = InvalidRange.To,
            };

            var selectedRange = new CefViewRange()
            {
                From = (uint)text.Length,
                To = (uint)text.Length,
            };

            this.LogD($"update cef composition, selected range:{selectedRange.From} -> {selectedRange.To}");
            _owner.ImeSetComposition(text, [underline], replacementRange, selectedRange);
        }
        #endregion
    }
}
