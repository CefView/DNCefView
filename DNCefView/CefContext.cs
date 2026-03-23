using System;

namespace DNCefView
{
    public partial class CefContext
    {
        private readonly CefConfig _config;
        public CefConfig Config => _config;

        private static WeakReference? _instance;

        public static CefContext? Instance
        {
            get
            {
                if (null == _instance)
                    return null;

                if (null == _instance.Target)
                    return null;

                return _instance.Target as CefContext;
            }
        }

        public CefContext(CefConfig config)
        {
            if (_instance is { Target: not null })
            {
                throw new Exception("Only 1 DNCefContext instance is allowed");
            }

            _config = config;
            _native = CCefContext_new0(_config.NativeObject);
            _instance = new WeakReference(this);
        }

        public void CloseAllBrowsers()
        {
            foreach (var weakRef in CefBrowser.LiveInstances)
            {
                if (weakRef.Target is CefBrowser cefView)
                {
                    cefView.Dispose();
                }
            }
            CefBrowser.LiveInstances.Clear();
        }

        ~CefContext()
        {
            Dispose(false);
        }
    }
}
