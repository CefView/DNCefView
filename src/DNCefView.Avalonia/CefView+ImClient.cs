using Avalonia;
using Avalonia.Input.TextInput;
using Avalonia.Threading;
using System.Linq;


namespace DNCefView.Avalonia
{
    internal sealed class CefViewTextInputMethodClient : TextInputMethodClient
    {
        private readonly CefView? _owner;

        private Rect _cursorRectangle;

        internal CefViewTextInputMethodClient(CefView owner)
        {
            _owner = owner;
        }

        internal void UpdateComposition(CefViewRange selectedRange, CefViewRect[] charBounds)
        {
            using var _ = this.LogM();

            if (_owner == null)
            {
                return;
            }

            if (charBounds == null)
            {
                return;
            }

            _cursorRectangle = charBounds
                .Select(r => new Rect(r.X, r.Y, r.Width, r.Height))
                .Aggregate((acc, r) => acc.Union(r));

            this.LogD($"new _cursorRectangle: ({_cursorRectangle.X}, {_cursorRectangle.Y}) - [{_cursorRectangle.Width}, {_cursorRectangle.Height}]");

            Dispatcher.UIThread.Post(() =>
            {
                RaiseCursorRectangleChanged();
            });
        }

        internal Rect GetCursorRectangle()
        {
            return _cursorRectangle;
        }

        #region TextInputMethodClient
        public override Visual TextViewVisual => _owner!;
        public override Rect CursorRectangle => GetCursorRectangle();
        public override bool SupportsPreedit => true;
        public override void SetPreeditText(string? text)
        {
            using var _ = this.LogM($"text={text}");

            if (null == _owner)
            {
                return;
            }

            var underline = new CefViewCompositionUnderline()
            {
                BackgroundColor = 0,
                Range = new CefViewRange(0, (uint)(text?.Length ?? 0)),
                Style = CefViewCompositionUnderlineStyle.CEF_CUS_DOT,
            };

            if (!string.IsNullOrEmpty(text))
            {
                // in composing
                this.LogD($"composing update");
                _owner.ImeSetComposition(text, [underline], new(uint.MaxValue, uint.MaxValue), new((uint)text.Length, (uint)text.Length));
            }
            else
            {
                // composing end
                this.LogD($"composing end");
                _owner.ImeCancelComposition();
            }
        }

        public override TextSelection Selection { get; set; } = new();
        public override bool SupportsSurroundingText => false;
        public override string SurroundingText => string.Empty;
        #endregion
    }
}
