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
            get { if (_native == IntPtr.Zero) throw new ObjectDisposedException(nameof(CefQuery)); return _native; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [DllImport("CCefView")]
        private static extern void CCefQuery_Delete(IntPtr p);
        protected virtual void Dispose([MarshalAs(UnmanagedType.I1)] bool disposing)
        {
            var native = System.Threading.Interlocked.Exchange(ref _native, IntPtr.Zero);
            if (native != IntPtr.Zero) CCefQuery_Delete(native);
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
            return Marshal.PtrToStringUTF8(CCefQuery_getRequest(NativeObject));
        }

        // Source: const int64_t getId()
        [DllImport("CCefView")]
        private static extern Int64 CCefQuery_getId(IntPtr thiz);
        public Int64 GetId()
        {
            return CCefQuery_getId(NativeObject);
        }

        // Source: const std::string & getResponse()
        [DllImport("CCefView")]
        private static extern IntPtr CCefQuery_getResponse(IntPtr thiz);
        public string GetResponse()
        {
            return Marshal.PtrToStringUTF8(CCefQuery_getResponse(NativeObject));
        }

        // Source: const bool getResult()
        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CCefQuery_getResult(IntPtr thiz);
        public bool GetResult()
        {
            return CCefQuery_getResult(NativeObject);
        }

        // Source: const int getError()
        [DllImport("CCefView")]
        private static extern int CCefQuery_getError(IntPtr thiz);
        public int GetError()
        {
            return CCefQuery_getError(NativeObject);
        }

        // Source: void setResponseResult(bool, const std::string &, int)
        [DllImport("CCefView")]
        private static extern void CCefQuery_setResponseResult(IntPtr thiz, [MarshalAs(UnmanagedType.I1)] bool success, [MarshalAs(UnmanagedType.LPUTF8Str)] string response, int error);
        public void SetResponseResult([MarshalAs(UnmanagedType.I1)] bool success, string response, int error)
        {
            CCefQuery_setResponseResult(NativeObject, success, response, error);
        }

    }
}
