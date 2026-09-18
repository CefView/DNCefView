#nullable enable
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace DNCefView
{
    public partial class CefContext
    {
        private CefConfig? _config;
        private bool _editorLease;
        private static WeakReference? _instance;
        public CefConfig? Config => _config;
        public static CefContext? Instance => _instance?.Target as CefContext;
        [DllImport("CCefView")] private static extern int CCefView_GetUnityAbiVersion();
        [DllImport("CCefView")] private static extern IntPtr CCefContext_AcquireEditor(IntPtr config);
        public CefContext(CefConfig config) : this(config, false) { }
        public CefContext(CefConfig config, bool editorLease)
        {
            if (Instance != null) throw new InvalidOperationException("Only one CEF context is allowed");
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (CCefView_GetUnityAbiVersion() != 2) throw new InvalidOperationException("Mismatched CCefView ABI; deploy the complete runtime");
            _config = config;
            _editorLease = editorLease;
            _native = editorLease ? CCefContext_AcquireEditor(config.NativeObject) : CCefContext_new0(config.NativeObject);
            if (_native == IntPtr.Zero) throw new InvalidOperationException("CEF initialization failed");
            _instance = new WeakReference(this);
        }
        public void CloseAllBrowsers()
        {
            CefBrowser[] browsers;
            lock (CefBrowser.LiveInstances) browsers = CefBrowser.LiveInstances.ToArray();
            foreach (var browser in browsers) browser.Dispose();
            var deadline = DateTime.UtcNow.AddSeconds(15);
            while (browsers.Any(browser => !browser.IsDisposed))
            {
                if (DateTime.UtcNow >= deadline) throw new TimeoutException("CEF close did not complete; restart the host before reloading managed code");
                Thread.Sleep(1);
            }
        }
        // CEF must be shut down on its initialization thread, never the finalizer thread.
    }
}
