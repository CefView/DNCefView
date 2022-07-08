using DNCef;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace DNCef.WPF.Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private CefContext _context;

        protected override void OnStartup(StartupEventArgs e)
        {
            CefConfig config = new CefConfig();
            config.SetMultiThreadedMessageLoop(true);
            config.SetLogLevel(CefViewLogLevel.LOGSEVERITY_DEFAULT);
            config.SetRemoteDebuggingPort(9000);
            config.SetUserAgent("DNCefView");
            config.SetBridgeObjectName("CallBridge");

            _context = new CefContext(config);
            var webresDir = Path.Combine(Directory.GetCurrentDirectory(), "webres");
            _context.AddFolderResource(webresDir, "https://demo.dncefview.com", 0);
            _context.AddCookie("test", "value", "www.a.com", "path");

            base.OnStartup(e);
        }

        protected void CheckSafeExit(DispatcherFrame frame)
        {
            if (_context.IsSafeToShutdown())
            {
                // dispose context
                _context.Dispose();

                // exit current dispatcher frame
                frame.Continue = false;
            }
            else
            {
                // keep checking
                Dispatcher.CurrentDispatcher.BeginInvoke(
                    DispatcherPriority.SystemIdle,
                    CheckSafeExit,
                    frame);
            }
        }

        protected void SafeShutdownCef()
        {
            _context.CloseAllBrowsers();

            var frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(
                DispatcherPriority.SystemIdle,
                CheckSafeExit,
                frame);
            Dispatcher.PushFrame(frame);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            SafeShutdownCef();
            base.OnExit(e);
        }
    }
}
