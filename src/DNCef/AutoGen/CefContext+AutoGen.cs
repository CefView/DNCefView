using System;
using System.Runtime.InteropServices;

namespace DNCef
{
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
        private static extern void CCefContext_Delete(IntPtr p);
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

        [DllImport("CCefView")]
        private static extern IntPtr CCefContext_new0(IntPtr config);

        [DllImport("CCefView")]
        private static extern void CCefContext_addFolderResource(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string path, [MarshalAs(UnmanagedType.LPUTF8Str)] string url, Int32 priority);
        public void AddFolderResource(string path, string url, Int32 priority)
        {
            CCefContext_addFolderResource(_native, path, url, priority);
        }

        [DllImport("CCefView")]
        private static extern void CCefContext_addArchiveResource(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string path, [MarshalAs(UnmanagedType.LPUTF8Str)] string url, [MarshalAs(UnmanagedType.LPUTF8Str)] string password, Int32 priority);
        public void AddArchiveResource(string path, string url, string password, Int32 priority)
        {
            CCefContext_addArchiveResource(_native, path, url, password, priority);
        }

        [DllImport("CCefView")]
        private static extern bool CCefContext_addCookie(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string value, [MarshalAs(UnmanagedType.LPUTF8Str)] string domain, [MarshalAs(UnmanagedType.LPUTF8Str)] string url);
        public bool AddCookie(string name, string value, string domain, string url)
        {
            return CCefContext_addCookie(_native, name, value, domain, url);
        }

        [DllImport("CCefView")]
        private static extern void CCefContext_doCefMessageLoopWork(IntPtr thiz);
        public void DoCefMessageLoopWork()
        {
            CCefContext_doCefMessageLoopWork(_native);
        }

        [DllImport("CCefView")]
        private static extern bool CCefContext_isSafeToShutdown(IntPtr thiz);
        public bool IsSafeToShutdown()
        {
            return CCefContext_isSafeToShutdown(_native);
        }

    }
}