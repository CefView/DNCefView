#pragma warning disable CS8603
using System;
using System.Runtime.InteropServices;

namespace DNCefView
{
    // Source: CCefQuery 
    public partial class CefQuery : IDisposable
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
        private static extern void CCefQuery_Delete(IntPtr p);
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // TODO: cleanup the managed resources
            }

            // cleanup unmanaged resources
            if (_native != IntPtr.Zero)
            {
                CCefQuery_Delete(_native);
                _native = IntPtr.Zero;
            }
        }

        // Source: CCefQuery()
        [DllImport("CCefView")]
        private static extern IntPtr CCefQuery_new0();

        // Source: CCefQuery(const std::string &, const int64_t)
        [DllImport("CCefView")]
        private static extern IntPtr CCefQuery_new1([MarshalAs(UnmanagedType.LPUTF8Str)] string req, Int64 query);

        // Source: const std::string & getRequest()
        [DllImport("CCefView")]
        private static extern IntPtr CCefQuery_getRequest(IntPtr thiz);
        public string GetRequest()
        {
            return Marshal.PtrToStringUTF8(CCefQuery_getRequest(_native));
        }

        // Source: const int64_t getId()
        [DllImport("CCefView")]
        private static extern Int64 CCefQuery_getId(IntPtr thiz);
        public Int64 GetId()
        {
            return CCefQuery_getId(_native);
        }

        // Source: const std::string & getResponse()
        [DllImport("CCefView")]
        private static extern IntPtr CCefQuery_getResponse(IntPtr thiz);
        public string GetResponse()
        {
            return Marshal.PtrToStringUTF8(CCefQuery_getResponse(_native));
        }

        // Source: const bool getResult()
        [DllImport("CCefView")]
        private static extern bool CCefQuery_getResult(IntPtr thiz);
        public bool GetResult()
        {
            return CCefQuery_getResult(_native);
        }

        // Source: const int getError()
        [DllImport("CCefView")]
        private static extern int CCefQuery_getError(IntPtr thiz);
        public int GetError()
        {
            return CCefQuery_getError(_native);
        }

        // Source: void setResponseResult(bool, const std::string &, int)
        [DllImport("CCefView")]
        private static extern void CCefQuery_setResponseResult(IntPtr thiz, bool success, [MarshalAs(UnmanagedType.LPUTF8Str)] string response, int error);
        public void SetResponseResult(bool success, string response, int error)
        {
            CCefQuery_setResponseResult(_native, success, response, error);
        }

    }
}