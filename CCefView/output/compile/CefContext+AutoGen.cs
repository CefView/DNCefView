#pragma warning disable CS8603
using System;
using System.Runtime.InteropServices;

namespace DNCefView
{
    // Source: CCefContext 
    public partial class CefContext : IDisposable
    {
        private IntPtr _native;
        public IntPtr NativeObject
        {
            get { return _native; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [DllImport("CCefView")]
        internal static extern void CCefContext_Delete(IntPtr p);
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // TODO: cleanup the managed resources
            }

            // cleanup unmanaged resources
            if (_native != IntPtr.Zero)
            {
                CCefContext_Delete(_native);
                _native = IntPtr.Zero;
            }
        }

        // Source: CCefContext(const CCefConfig *)
        [DllImport("CCefView")]
        internal static extern IntPtr CCefContext_new0(IntPtr config);

        // Source: void addFolderResource(const std::string &, const std::string &, int)
        [DllImport("CCefView")]
        // No Return Value
        internal static extern void CCefContext_addFolderResource(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string path, [MarshalAs(UnmanagedType.LPUTF8Str)] string url, int priority);
        public void AddFolderResource(string path, string url, int priority)
        {
            CCefContext_addFolderResource(_native, path, url, priority);
        }

        // Source: void addArchiveResource(const std::string &, const std::string &, const std::string &, int)
        [DllImport("CCefView")]
        // No Return Value
        internal static extern void CCefContext_addArchiveResource(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string path, [MarshalAs(UnmanagedType.LPUTF8Str)] string url, [MarshalAs(UnmanagedType.LPUTF8Str)] string password, int priority);
        public void AddArchiveResource(string path, string url, string password, int priority)
        {
            CCefContext_addArchiveResource(_native, path, url, password, priority);
        }

        // Source: bool addCookie(const std::string &, const std::string &, const std::string &, const std::string &)
        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CCefContext_addCookie(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string value, [MarshalAs(UnmanagedType.LPUTF8Str)] string domain, [MarshalAs(UnmanagedType.LPUTF8Str)] string url);
        public bool AddCookie(string name, string value, string domain, string url)
        {
            return CCefContext_addCookie(_native, name, value, domain, url);
        }

        // Source: bool deleteCookie(const std::string &, const std::string &)
        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CCefContext_deleteCookie(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string url, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);
        public bool DeleteCookie(string url, string name)
        {
            return CCefContext_deleteCookie(_native, url, name);
        }

        // Source: bool deleteAllCookies()
        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CCefContext_deleteAllCookies(IntPtr thiz);
        public bool DeleteAllCookies()
        {
            return CCefContext_deleteAllCookies(_native);
        }

        // Source: bool addCrossOriginWhitelistEntry(const std::string &, const std::string &, const std::string &, bool)
        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CCefContext_addCrossOriginWhitelistEntry(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string sourceOrigin, [MarshalAs(UnmanagedType.LPUTF8Str)] string targetProtocol, [MarshalAs(UnmanagedType.LPUTF8Str)] string targetDomain, [MarshalAs(UnmanagedType.Bool)] bool allowTargetSubdomains);
        public bool AddCrossOriginWhitelistEntry(string sourceOrigin, string targetProtocol, string targetDomain, bool allowTargetSubdomains)
        {
            return CCefContext_addCrossOriginWhitelistEntry(_native, sourceOrigin, targetProtocol, targetDomain, allowTargetSubdomains);
        }

        // Source: bool removeCrossOriginWhitelistEntry(const std::string &, const std::string &, const std::string &, bool)
        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CCefContext_removeCrossOriginWhitelistEntry(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string sourceOrigin, [MarshalAs(UnmanagedType.LPUTF8Str)] string targetProtocol, [MarshalAs(UnmanagedType.LPUTF8Str)] string targetDomain, [MarshalAs(UnmanagedType.Bool)] bool allowTargetSubdomains);
        public bool RemoveCrossOriginWhitelistEntry(string sourceOrigin, string targetProtocol, string targetDomain, bool allowTargetSubdomains)
        {
            return CCefContext_removeCrossOriginWhitelistEntry(_native, sourceOrigin, targetProtocol, targetDomain, allowTargetSubdomains);
        }

        // Source: bool clearCrossOriginWhitelist()
        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CCefContext_clearCrossOriginWhitelist(IntPtr thiz);
        public bool ClearCrossOriginWhitelist()
        {
            return CCefContext_clearCrossOriginWhitelist(_native);
        }

        // Source: void doCefMessageLoopWork()
        [DllImport("CCefView")]
        // No Return Value
        internal static extern void CCefContext_doCefMessageLoopWork(IntPtr thiz);
        public void DoCefMessageLoopWork()
        {
            CCefContext_doCefMessageLoopWork(_native);
        }

        // Source: bool isSafeToShutdown()
        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CCefContext_isSafeToShutdown(IntPtr thiz);
        public bool IsSafeToShutdown()
        {
            return CCefContext_isSafeToShutdown(_native);
        }

    }
}