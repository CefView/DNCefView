namespace DNCef
{
    public partial class CefSetting
    {
        public CefSetting()
        {
            _native = CCefSetting_new0();
        }

        ~CefSetting()
        {
            Dispose(false);
        }
    }
}
