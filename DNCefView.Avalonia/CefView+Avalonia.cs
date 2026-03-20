using Avalonia;
using Avalonia.Controls;


namespace DNCefView.Avalonia
{
    public partial class CefView : Control
    {
        public static readonly StyledProperty<string> SourceProperty =
            AvaloniaProperty.Register<CefView, string>(nameof(Source), defaultValue: string.Empty);

        static void ClassInitialize()
        {
            FocusableProperty.OverrideDefaultValue<CefView>(true);
            IsTabStopProperty.OverrideDefaultValue<CefView>(true);
            FocusAdornerProperty.OverrideDefaultValue<CefView>(null);

            SourceProperty.Changed.AddClassHandler<CefView>((s, e) => s.OnSourceChanged(e));
            IsVisibleProperty.Changed.AddClassHandler<CefView>((s, e) => s.OnVisibleChanged(e));
        }

        static CefView()
        {
            ClassInitialize();

            ClassInitializeContextMenu();
            ClassInitializeCursor();
            ClassInitializeFocus();
            ClassInitializeIME();
            ClassInitializeInput();
            ClassInitializeJSDialogs();
            ClassInitializeRender();
        }

        public CefView() : this(null, string.Empty)
        {
        }

        public CefView(CefSetting? setting) : this(setting, string.Empty)
        {
        }

        public CefView(CefSetting? setting, string? source)
        {
            if (Design.IsDesignMode)
            {
                return;
            }

            InitializeContextMenu();
            InitializeCursor();
            InitializeFocus();
            InitializeIME();
            InitializeInput();
            InitializeJSDialogs();
            InitializeRender();

            Initialize(setting, source);
        }

        public string Source
        {
            get { return GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        void Initialize(CefSetting? setting, string? source)
        {
            SetValue(FocusAdornerProperty, null);

            if (!string.IsNullOrEmpty(source))
            {
                Source = source;
            }

            CreateNativeBrowser(Source, setting);
        }

        void UI_OnCefAfterCreated()
        {
            RunInUIThread(() =>
            {
                _isCreated = true;
                _cefBrowser?.WasHidden(!IsVisible);
                _cefBrowser?.WasResized();
                _cefBrowser?.NavigateToUrl(Source);
            },
            block: false);
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

        protected override void OnSizeChanged(SizeChangedEventArgs e)
        {
            base.OnSizeChanged(e);

            _cefBrowser?.WasResized();
            _cefBrowser?.NotifyMoveOrResizeStarted();
        }
    }
}
