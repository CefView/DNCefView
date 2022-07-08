using System;

namespace DNCef
{
    public partial class CefQuery
    {
        public long Id
        {
            get { return GetId(); }
        }

        public string Request
        {
            get { return GetRequest(); }
        }

        public string Response
        {
            get { return GetResponse(); }
        }

        public bool Result
        {
            get { return GetResult(); }
        }

        public int Error
        {
            get { return GetError(); }
        }

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
