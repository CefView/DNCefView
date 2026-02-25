using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace DNCefView.Avalonia
{
    public partial class CefView
    {
        static void ClassInitializeFocus()
        {
        }

        private bool _hasCefGotFocus = false;

        void InitializeFocus()
        {
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

        bool UI_OnCefSetFocus(int browserId)
        {
            using var _ = this.LogM();

            return false;
        }

        void UI_OnCefGotFocus(int browserId)
        {
            using var _ = this.LogM();

            _hasCefGotFocus = true;

            RunInUIThread(() =>
            {
                if (!IsFocused)
                {
                    Focus();
                }
            },
            block: false);
        }

        protected override void OnGotFocus(GotFocusEventArgs e)
        {
            using var _ = this.LogM();

            base.OnGotFocus(e);

            if (!_hasCefGotFocus)
            {
                this.LogD("set cef focus");
                _cefBrowser?.SetFocus(true);
            }
            else
            {
                this.LogD("cef has got focus already, skip setting focus to avoid infinite loop.");
            }
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            using var _ = this.LogM();

            base.OnLostFocus(e);

            // cancel context menu: TODO

            _hasCefGotFocus = false;
            _cefBrowser?.SetFocus(false);
        }
    }
}
