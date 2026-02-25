using Avalonia;
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

        private void OnSourceChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (_isCreated && e.NewValue is string source)
            {
                _cefBrowser?.NavigateToUrl(source);
            }
        }

        private void OnVisibleChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool isVisible)
            {
                _cefBrowser?.WasHidden(!isVisible);
            }
        }

        protected override void OnGotFocus(GotFocusEventArgs e)
        {
            base.OnGotFocus(e);

            if (!_hasCefGotFocus)
            {
                _cefBrowser?.SetFocus(true);
            }
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            // cancel context menu: TODO

            _hasCefGotFocus = false;
            _cefBrowser?.SetFocus(false);
        }

        protected override void OnSizeChanged(SizeChangedEventArgs e)
        {
            base.OnSizeChanged(e);

            _cefBrowser?.WasResized();
            _cefBrowser?.NotifyMoveOrResizeStarted();
        }
    }
}
