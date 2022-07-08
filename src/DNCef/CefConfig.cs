namespace DNCef
{
    public partial class CefConfig
    {
        public CefConfig()
        {
            _native = CCefConfig_new0();
        }

        ~CefConfig()
        {
            Dispose(false);
        }
    }
}
