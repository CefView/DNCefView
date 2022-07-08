using System;
using System.Runtime.InteropServices;

namespace DNCef
{
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

        [DllImport("CCefView")]
        private static extern IntPtr CCefQuery_new0();

        [DllImport("CCefView")]
        private static extern IntPtr CCefQuery_new1([MarshalAs(UnmanagedType.LPUTF8Str)] string req, Int64 query);

        [DllImport("CCefView")]
        private static extern IntPtr CCefQuery_getRequest(IntPtr thiz);
        public string GetRequest()
        {
            return Marshal.PtrToStringUTF8(CCefQuery_getRequest(_native));
        }

        [DllImport("CCefView")]
        private static extern Int64 CCefQuery_getId(IntPtr thiz);
        public Int64 GetId()
        {
            return CCefQuery_getId(_native);
        }

        [DllImport("CCefView")]
        private static extern IntPtr CCefQuery_getResponse(IntPtr thiz);
        public string GetResponse()
        {
            return Marshal.PtrToStringUTF8(CCefQuery_getResponse(_native));
        }

        [DllImport("CCefView")]
        private static extern bool CCefQuery_getResult(IntPtr thiz);
        public bool GetResult()
        {
            return CCefQuery_getResult(_native);
        }

        [DllImport("CCefView")]
        private static extern Int32 CCefQuery_getError(IntPtr thiz);
        public Int32 GetError()
        {
            return CCefQuery_getError(_native);
        }

        [DllImport("CCefView")]
        private static extern void CCefQuery_setResponseResult(IntPtr thiz, bool success, [MarshalAs(UnmanagedType.LPUTF8Str)] string response, Int32 error);
        public void SetResponseResult(bool success, string response, Int32 error)
        {
            CCefQuery_setResponseResult(_native, success, response, error);
        }

    }
}