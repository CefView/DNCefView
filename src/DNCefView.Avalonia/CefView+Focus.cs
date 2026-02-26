using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace DNCefView.Avalonia
{
    public partial class CefView
    {
        static void ClassInitializeFocus()
        {
        }

        private bool _syncingFocusByCef = false;

        private bool _syncingFocusByAva = false;

        void InitializeFocus()
        {
        }

        void UI_OnCefFocusReleasedByTabKey(int browserId, bool next)
        {
            RunInUIThread(() =>
            {
                var focusManager = TopLevel.GetTopLevel(this)?.FocusManager;
                if (focusManager != null)
                {
                    KeyboardNavigationHandler
                    .GetNext(focusManager?.GetFocusedElement()!, next ? NavigationDirection.Next : NavigationDirection.Previous)?
                    .Focus(NavigationMethod.Tab);
                }
            });
        }

        bool UI_OnCefRequestSetFocus(int browserId)
        {
            using var _ = this.LogM();

            if (_syncingFocusByAva || _syncingFocusByCef)
            {
                return false;
            }

            _syncingFocusByCef = true;
            RunInUIThread(() => Focus());
            _syncingFocusByCef = false;

            return false;
        }

        void UI_OnCefGotFocus(int browserId)
        {
            using var _ = this.LogM();
        }

        protected override void OnGotFocus(GotFocusEventArgs e)
        {
            using var _ = this.LogM();

            base.OnGotFocus(e);

            if (_syncingFocusByAva || _syncingFocusByCef)
            {
                return;
            }

            _syncingFocusByAva = true;
            _cefBrowser?.SetFocus(true);
            _syncingFocusByAva = false;
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            using var _ = this.LogM();

            base.OnLostFocus(e);

            // cancel context menu: TODO

            _cefBrowser?.SetFocus(false);
        }
    }
}
