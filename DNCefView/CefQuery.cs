using System;

namespace DNCefView
{
    public partial class CefQuery
    {
        public long Id => GetId();

        public string Request => GetRequest();

        public string Response => GetResponse();

        public bool Result => GetResult();

        public int Error => GetError();

        internal CefQuery(IntPtr query)
        {
            _native = query;
        }

        ~CefQuery()
        {
            Dispose(false);
        }
    }
}
