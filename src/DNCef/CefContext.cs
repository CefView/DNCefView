using System;

namespace DNCef
{
    public partial class CefContext
    {
        private CefConfig _config;
        public CefConfig Config
        {
            get { return _config; }
        }

        private static WeakReference _instance;

        public static CefContext Instance
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
            if (null != _instance && null != _instance.Target)
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
                var cefView = weakRef.Target as CefBrowser;
                if (null != cefView)
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
