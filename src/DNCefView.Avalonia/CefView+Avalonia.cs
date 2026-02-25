using Avalonia;
using Avalonia.Controls;


namespace DNCefView.Avalonia
{
    public partial class CefView : Control
    {
        public static readonly StyledProperty<string> SourceProperty =
            AvaloniaProperty.Register<CefView, string>(nameof(Source));

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

        public CefView() : this(null, "")
        {
        }

        public CefView(CefSetting? setting) : this(setting, "")
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
            if (string.IsNullOrEmpty(source))
            {
                source = "about:blank";
            }
            SetCurrentValue(SourceProperty, source);
            SetCurrentValue(FocusAdornerProperty, null);

            CreateNativeBrowser(source, setting);
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
    }
}
