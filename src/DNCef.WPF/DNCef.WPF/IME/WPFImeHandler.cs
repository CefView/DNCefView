using DNCef;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using System.Windows.Interop;


namespace IMESupport
{
    public class WPFImeHandler
    {
        CefView _owner;
        HwndSource _source;
        IntPtr _inputContext;
        CefViewRect _compositionBounds = new CefViewRect();
        Func<IntPtr, int> _loWord;
        CefViewRange _compositionRange;

        internal bool IsActive { get; set; }

        public WPFImeHandler(CefView owner)
        {
            _owner = owner;

            if (IntPtr.Size == sizeof(Int32))
                _loWord = x => x.ToInt32();
            else
                _loWord = LOWORD64;
        }

        public void InitialiseHWND(HwndSource hwnd)
        {
            Init(hwnd);
        }

        public void EnableInputMethod()
        {
            if (IsActive)
                return;

            InputMethod.SetIsInputMethodEnabled(_owner, true);
            InputMethod.SetIsInputMethodSuspended(_owner, true);
            IsActive = true;
        }

        public void DisableInputMethod()
        {
            if (!IsActive)
                return;

            IsActive = false;
            InputMethod.SetIsInputMethodEnabled(_owner, false);
            InputMethod.SetIsInputMethodSuspended(_owner, false);
        }

        internal void UpdateComposition(CefViewRange range, CefViewRect bounds)
        {
            _compositionRange = range;
            _compositionBounds = bounds;
            UpdateCompositionWindow(_source.Handle);
        }

        private void Init(HwndSource source)
        {
            _source = source;
            _source.AddHook(SourceHook);

            _inputContext = NativeIME.ImmGetContext(_source.Handle);
            if (IntPtr.Zero == _inputContext)
            {
                _inputContext = NativeIME.ImmCreateContext();
                NativeIME.ImmAssociateContext(_source.Handle, _inputContext);
            }

            // TODO: need to find a better way to trigger setting context on the window
            _owner.Focus();
        }

        private IntPtr SourceHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (handled || _owner == null || !IsActive)
                return IntPtr.Zero;

            // take over all IME messages
            switch (msg)
            {
                case NativeIME.WM_IME_SETCONTEXT:
                    OnIMESetContext(hwnd, (uint)msg, wParam, lParam);
                    handled = true;
                    break;

                case NativeIME.WM_IME_STARTCOMPOSITION:
                    OnIMEStartComposition(hwnd);
                    handled = true;
                    break;

                case NativeIME.WM_IME_COMPOSITION:
                    OnIMEComposition(hwnd, _loWord(lParam));
                    handled = true;
                    break;

                case NativeIME.WM_IME_ENDCOMPOSITION:
                    OnIMEEndComposition(hwnd);
                    handled = true;
                    break;
            }

            return handled ? IntPtr.Zero : new IntPtr(1);
        }

        private void OnIMESetContext(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            NativeIME.DefWindowProc(hwnd, msg, wParam, lParam);

            UpdateCompositionWindow(hwnd);
        }

        private void OnIMEStartComposition(IntPtr hwnd)
        {
            // we don't know the position of the caret
            UpdateCompositionWindow(hwnd);
        }

        private void OnIMEComposition(IntPtr hwnd, int lParam)
        {
            string text = string.Empty;

            using (var handler = IMEHandler.Attach(hwnd))
            {
                if (handler.GetResult((uint)lParam, out text))
                {
                    _owner.ImeCommitText(text, new CefViewRange(-1, -1), 0);
                    return;
                }
            }

            using (var handler = IMEHandler.Attach(hwnd))
            {
                var underlines = new List<CefViewCompositionUnderline>();
                int compositionStart = 0;

                if (handler.GetComposition((uint)lParam, underlines, ref compositionStart, out text))
                {
                    _owner.ImeSetComposition(text, underlines.ToArray(), new CefViewRange(-1, -1), new CefViewRange(compositionStart, compositionStart));
                }
            }
        }

        private void OnIMEEndComposition(IntPtr hwnd)
        {
            _owner.ImeFinishComposingText(false);
        }

        private void UpdateCompositionWindow(IntPtr hwnd)
        {
            if (!_owner.IsFocused)
                return;

            var candidateForm = new NativeIME.TagCandidateForm
            {

                DwStyle = NativeIME.CFS_EXCLUDE,
                PtCurrentPos = new NativeIME.TagPoint
                {
                    X = _compositionBounds.X,
                    Y = _compositionBounds.Y
                },
                RcArea = new NativeIME.TagRect
                {
                    Left = _compositionBounds.X,
                    Top = _compositionBounds.Y,
                    Right = _compositionBounds.X + _compositionBounds.Width,
                    Bottom = _compositionBounds.Y + _compositionBounds.Height
                }
            };

            using (var handler = IMEHandler.Attach(hwnd))
            {
                NativeIME.ImmSetCandidateWindow(handler._hIMC, ref candidateForm);
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private static Int32 HIWORD64(IntPtr ptr)
        {
            return (Int32)((ptr.ToInt64() >> 16) & 0xFFFFFFFF);
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private static Int32 LOWORD64(IntPtr ptr)
        {
            return (Int32)(ptr.ToInt64() & 0xFFFFFFFF);
        }
    }
}
