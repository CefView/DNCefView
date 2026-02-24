using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Logging;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using System.IO;

namespace DNCefView.Avalonia.Demo
{
    public partial class App : Application
    {
        private CefContext? _context;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            InputElement.GotFocusEvent.AddClassHandler<Control>((control, args) =>
            {
                Logger.TryGet(LogEventLevel.Information, LogArea.Control)?.Log(control, $"---- Focus changed to: {control.Name} ({control.GetType().Name})");
            });

            if (!Design.IsDesignMode)
            {
                CefConfig config = new CefConfig();
                config.SetMultiThreadedMessageLoop(true);
                config.SetLogLevel(CefViewLogLevel.LOGSEVERITY_DEFAULT);
                config.SetRemoteDebuggingPort(9222);
                config.SetUserAgent("DNCefView");
                config.SetBridgeObjectName("CallBridge");

                _context = new CefContext(config);
                var webresDir = Path.Combine(Directory.GetCurrentDirectory(), "webres");
                _context.AddFolderResource(webresDir, "https://demo.dncefview.com", 0);
                _context.AddCookie("test", "value", "dncefview.com", "path");
            }
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
                desktop.Exit += Desktop_Exit;
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void Desktop_Exit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
        {
            if (!Design.IsDesignMode)
            {
                SafeShutdownCef();
            }
        }

        protected void CheckSafeExit(DispatcherFrame frame)
        {
            if (_context != null && _context.IsSafeToShutdown())
            {
                // dispose context
                _context.Dispose();

                // exit current dispatcher frame
                frame.Continue = false;
            }
            else
            {
                // keep checking
                Dispatcher.UIThread.InvokeAsync(
                    () => CheckSafeExit(frame),
                    DispatcherPriority.SystemIdle);
            }
        }

        protected void SafeShutdownCef()
        {
            _context?.CloseAllBrowsers();

            var frame = new DispatcherFrame();
            Dispatcher.UIThread.InvokeAsync(
                () => CheckSafeExit(frame),
                DispatcherPriority.SystemIdle);
            Dispatcher.UIThread.PushFrame(frame);
        }
    }
}