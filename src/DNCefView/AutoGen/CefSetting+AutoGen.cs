using System;
using System.Runtime.InteropServices;

namespace DNCefView
{
    // Source: CCefSetting 
    public partial class CefSetting : IDisposable
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
        private static extern void CCefSetting_Delete(IntPtr p);
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // TODO: cleanup the managed resources
            }

            // cleanup unmanaged resources
            if (_native != IntPtr.Zero)
            {
                CCefSetting_Delete(_native);
                _native = IntPtr.Zero;
            }
        }

        // Source: CCefSetting()
        [DllImport("CCefView")]
        private static extern IntPtr CCefSetting_new0();

        // Source: void setStandardFontFamily(const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefSetting_setStandardFontFamily(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string value);
        public void SetStandardFontFamily(string value)
        {
            CCefSetting_setStandardFontFamily(_native, value);
        }

        // Source: const std::string & standardFontFamily()
        [DllImport("CCefView")]
        private static extern IntPtr CCefSetting_standardFontFamily(IntPtr thiz);
        public string StandardFontFamily()
        {
            return Marshal.PtrToStringUTF8(CCefSetting_standardFontFamily(_native));
        }

        // Source: void setFixedFontFamily(const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefSetting_setFixedFontFamily(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string value);
        public void SetFixedFontFamily(string value)
        {
            CCefSetting_setFixedFontFamily(_native, value);
        }

        // Source: const std::string & fixedFontFamily()
        [DllImport("CCefView")]
        private static extern IntPtr CCefSetting_fixedFontFamily(IntPtr thiz);
        public string FixedFontFamily()
        {
            return Marshal.PtrToStringUTF8(CCefSetting_fixedFontFamily(_native));
        }

        // Source: void setSerifFontFamily(const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefSetting_setSerifFontFamily(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string value);
        public void SetSerifFontFamily(string value)
        {
            CCefSetting_setSerifFontFamily(_native, value);
        }

        // Source: const std::string & serifFontFamily()
        [DllImport("CCefView")]
        private static extern IntPtr CCefSetting_serifFontFamily(IntPtr thiz);
        public string SerifFontFamily()
        {
            return Marshal.PtrToStringUTF8(CCefSetting_serifFontFamily(_native));
        }

        // Source: void setSansSerifFontFamily(const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefSetting_setSansSerifFontFamily(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string value);
        public void SetSansSerifFontFamily(string value)
        {
            CCefSetting_setSansSerifFontFamily(_native, value);
        }

        // Source: const std::string & sansSerifFontFamily()
        [DllImport("CCefView")]
        private static extern IntPtr CCefSetting_sansSerifFontFamily(IntPtr thiz);
        public string SansSerifFontFamily()
        {
            return Marshal.PtrToStringUTF8(CCefSetting_sansSerifFontFamily(_native));
        }

        // Source: void setCursiveFontFamily(const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefSetting_setCursiveFontFamily(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string value);
        public void SetCursiveFontFamily(string value)
        {
            CCefSetting_setCursiveFontFamily(_native, value);
        }

        // Source: const std::string & cursiveFontFamily()
        [DllImport("CCefView")]
        private static extern IntPtr CCefSetting_cursiveFontFamily(IntPtr thiz);
        public string CursiveFontFamily()
        {
            return Marshal.PtrToStringUTF8(CCefSetting_cursiveFontFamily(_native));
        }

        // Source: void setFantasyFontFamily(const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefSetting_setFantasyFontFamily(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string value);
        public void SetFantasyFontFamily(string value)
        {
            CCefSetting_setFantasyFontFamily(_native, value);
        }

        // Source: const std::string & fantasyFontFamily()
        [DllImport("CCefView")]
        private static extern IntPtr CCefSetting_fantasyFontFamily(IntPtr thiz);
        public string FantasyFontFamily()
        {
            return Marshal.PtrToStringUTF8(CCefSetting_fantasyFontFamily(_native));
        }

        // Source: void setDefaultEncoding(const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefSetting_setDefaultEncoding(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string value);
        public void SetDefaultEncoding(string value)
        {
            CCefSetting_setDefaultEncoding(_native, value);
        }

        // Source: const std::string & defaultEncoding()
        [DllImport("CCefView")]
        private static extern IntPtr CCefSetting_defaultEncoding(IntPtr thiz);
        public string DefaultEncoding()
        {
            return Marshal.PtrToStringUTF8(CCefSetting_defaultEncoding(_native));
        }

        // Source: void setAcceptLanguageList(const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefSetting_setAcceptLanguageList(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string value);
        public void SetAcceptLanguageList(string value)
        {
            CCefSetting_setAcceptLanguageList(_native, value);
        }

        // Source: const std::string & acceptLanguageList()
        [DllImport("CCefView")]
        private static extern IntPtr CCefSetting_acceptLanguageList(IntPtr thiz);
        public string AcceptLanguageList()
        {
            return Marshal.PtrToStringUTF8(CCefSetting_acceptLanguageList(_native));
        }

        // Source: void setWindowlessFrameRate(const int)
        [DllImport("CCefView")]
        private static extern void CCefSetting_setWindowlessFrameRate(IntPtr thiz, int value);
        public void SetWindowlessFrameRate(int value)
        {
            CCefSetting_setWindowlessFrameRate(_native, value);
        }

        // Source: int windowlessFrameRate()
        [DllImport("CCefView")]
        private static extern int CCefSetting_windowlessFrameRate(IntPtr thiz);
        public int WindowlessFrameRate()
        {
            return CCefSetting_windowlessFrameRate(_native);
        }

        // Source: void setDefaultFontSize(const int)
        [DllImport("CCefView")]
        private static extern void CCefSetting_setDefaultFontSize(IntPtr thiz, int value);
        public void SetDefaultFontSize(int value)
        {
            CCefSetting_setDefaultFontSize(_native, value);
        }

        // Source: int defaultFontSize()
        [DllImport("CCefView")]
        private static extern int CCefSetting_defaultFontSize(IntPtr thiz);
        public int DefaultFontSize()
        {
            return CCefSetting_defaultFontSize(_native);
        }

        // Source: void setDefaultFixedFontSize(const int)
        [DllImport("CCefView")]
        private static extern void CCefSetting_setDefaultFixedFontSize(IntPtr thiz, int value);
        public void SetDefaultFixedFontSize(int value)
        {
            CCefSetting_setDefaultFixedFontSize(_native, value);
        }

        // Source: int defaultFixedFontSize()
        [DllImport("CCefView")]
        private static extern int CCefSetting_defaultFixedFontSize(IntPtr thiz);
        public int DefaultFixedFontSize()
        {
            return CCefSetting_defaultFixedFontSize(_native);
        }

        // Source: void setMinimumFontSize(const int)
        [DllImport("CCefView")]
        private static extern void CCefSetting_setMinimumFontSize(IntPtr thiz, int value);
        public void SetMinimumFontSize(int value)
        {
            CCefSetting_setMinimumFontSize(_native, value);
        }

        // Source: int minimumFontSize()
        [DllImport("CCefView")]
        private static extern int CCefSetting_minimumFontSize(IntPtr thiz);
        public int MinimumFontSize()
        {
            return CCefSetting_minimumFontSize(_native);
        }

        // Source: void setMinimumLogicalFontSize(const int)
        [DllImport("CCefView")]
        private static extern void CCefSetting_setMinimumLogicalFontSize(IntPtr thiz, int value);
        public void SetMinimumLogicalFontSize(int value)
        {
            CCefSetting_setMinimumLogicalFontSize(_native, value);
        }

        // Source: int minimumLogicalFontSize()
        [DllImport("CCefView")]
        private static extern int CCefSetting_minimumLogicalFontSize(IntPtr thiz);
        public int MinimumLogicalFontSize()
        {
            return CCefSetting_minimumLogicalFontSize(_native);
        }

        // Source: void setRemoteFonts(CefViewPluingState)
        [DllImport("CCefView")]
        private static extern void CCefSetting_setRemoteFonts(IntPtr thiz, CefViewPluingState value);
        public void SetRemoteFonts(CefViewPluingState value)
        {
            CCefSetting_setRemoteFonts(_native, value);
        }

        // Source: CefViewPluingState remoteFonts()
        [DllImport("CCefView")]
        private static extern CefViewPluingState CCefSetting_remoteFonts(IntPtr thiz);
        public CefViewPluingState RemoteFonts()
        {
            return CCefSetting_remoteFonts(_native);
        }

        // Source: void setJavascript(CefViewPluingState)
        [DllImport("CCefView")]
        private static extern void CCefSetting_setJavascript(IntPtr thiz, CefViewPluingState value);
        public void SetJavascript(CefViewPluingState value)
        {
            CCefSetting_setJavascript(_native, value);
        }

        // Source: CefViewPluingState javascript()
        [DllImport("CCefView")]
        private static extern CefViewPluingState CCefSetting_javascript(IntPtr thiz);
        public CefViewPluingState Javascript()
        {
            return CCefSetting_javascript(_native);
        }

        // Source: void setJavascriptCloseWindows(CefViewPluingState)
        [DllImport("CCefView")]
        private static extern void CCefSetting_setJavascriptCloseWindows(IntPtr thiz, CefViewPluingState value);
        public void SetJavascriptCloseWindows(CefViewPluingState value)
        {
            CCefSetting_setJavascriptCloseWindows(_native, value);
        }

        // Source: CefViewPluingState javascriptCloseWindows()
        [DllImport("CCefView")]
        private static extern CefViewPluingState CCefSetting_javascriptCloseWindows(IntPtr thiz);
        public CefViewPluingState JavascriptCloseWindows()
        {
            return CCefSetting_javascriptCloseWindows(_native);
        }

        // Source: void setJavascriptAccessClipboard(CefViewPluingState)
        [DllImport("CCefView")]
        private static extern void CCefSetting_setJavascriptAccessClipboard(IntPtr thiz, CefViewPluingState value);
        public void SetJavascriptAccessClipboard(CefViewPluingState value)
        {
            CCefSetting_setJavascriptAccessClipboard(_native, value);
        }

        // Source: CefViewPluingState javascriptAccessClipboard()
        [DllImport("CCefView")]
        private static extern CefViewPluingState CCefSetting_javascriptAccessClipboard(IntPtr thiz);
        public CefViewPluingState JavascriptAccessClipboard()
        {
            return CCefSetting_javascriptAccessClipboard(_native);
        }

        // Source: void setJavascriptDomPaste(CefViewPluingState)
        [DllImport("CCefView")]
        private static extern void CCefSetting_setJavascriptDomPaste(IntPtr thiz, CefViewPluingState value);
        public void SetJavascriptDomPaste(CefViewPluingState value)
        {
            CCefSetting_setJavascriptDomPaste(_native, value);
        }

        // Source: CefViewPluingState javascriptDomPaste()
        [DllImport("CCefView")]
        private static extern CefViewPluingState CCefSetting_javascriptDomPaste(IntPtr thiz);
        public CefViewPluingState JavascriptDomPaste()
        {
            return CCefSetting_javascriptDomPaste(_native);
        }

        // Source: void setPlugins(CefViewPluingState)
        [DllImport("CCefView")]
        private static extern void CCefSetting_setPlugins(IntPtr thiz, CefViewPluingState value);
        public void SetPlugins(CefViewPluingState value)
        {
            CCefSetting_setPlugins(_native, value);
        }

        // Source: CefViewPluingState plugins()
        [DllImport("CCefView")]
        private static extern CefViewPluingState CCefSetting_plugins(IntPtr thiz);
        public CefViewPluingState Plugins()
        {
            return CCefSetting_plugins(_native);
        }

        // Source: void setImageLoading(CefViewPluingState)
        [DllImport("CCefView")]
        private static extern void CCefSetting_setImageLoading(IntPtr thiz, CefViewPluingState value);
        public void SetImageLoading(CefViewPluingState value)
        {
            CCefSetting_setImageLoading(_native, value);
        }

        // Source: CefViewPluingState imageLoading()
        [DllImport("CCefView")]
        private static extern CefViewPluingState CCefSetting_imageLoading(IntPtr thiz);
        public CefViewPluingState ImageLoading()
        {
            return CCefSetting_imageLoading(_native);
        }

        // Source: void setImageShrinkStandaloneToFit(CefViewPluingState)
        [DllImport("CCefView")]
        private static extern void CCefSetting_setImageShrinkStandaloneToFit(IntPtr thiz, CefViewPluingState value);
        public void SetImageShrinkStandaloneToFit(CefViewPluingState value)
        {
            CCefSetting_setImageShrinkStandaloneToFit(_native, value);
        }

        // Source: CefViewPluingState imageShrinkStandaloneToFit()
        [DllImport("CCefView")]
        private static extern CefViewPluingState CCefSetting_imageShrinkStandaloneToFit(IntPtr thiz);
        public CefViewPluingState ImageShrinkStandaloneToFit()
        {
            return CCefSetting_imageShrinkStandaloneToFit(_native);
        }

        // Source: void setTextAreaResize(CefViewPluingState)
        [DllImport("CCefView")]
        private static extern void CCefSetting_setTextAreaResize(IntPtr thiz, CefViewPluingState value);
        public void SetTextAreaResize(CefViewPluingState value)
        {
            CCefSetting_setTextAreaResize(_native, value);
        }

        // Source: CefViewPluingState textAreaResize()
        [DllImport("CCefView")]
        private static extern CefViewPluingState CCefSetting_textAreaResize(IntPtr thiz);
        public CefViewPluingState TextAreaResize()
        {
            return CCefSetting_textAreaResize(_native);
        }

        // Source: void setTabToLinks(CefViewPluingState)
        [DllImport("CCefView")]
        private static extern void CCefSetting_setTabToLinks(IntPtr thiz, CefViewPluingState value);
        public void SetTabToLinks(CefViewPluingState value)
        {
            CCefSetting_setTabToLinks(_native, value);
        }

        // Source: CefViewPluingState tabToLinks()
        [DllImport("CCefView")]
        private static extern CefViewPluingState CCefSetting_tabToLinks(IntPtr thiz);
        public CefViewPluingState TabToLinks()
        {
            return CCefSetting_tabToLinks(_native);
        }

        // Source: void setLocalStorage(CefViewPluingState)
        [DllImport("CCefView")]
        private static extern void CCefSetting_setLocalStorage(IntPtr thiz, CefViewPluingState value);
        public void SetLocalStorage(CefViewPluingState value)
        {
            CCefSetting_setLocalStorage(_native, value);
        }

        // Source: CefViewPluingState localStorage()
        [DllImport("CCefView")]
        private static extern CefViewPluingState CCefSetting_localStorage(IntPtr thiz);
        public CefViewPluingState LocalStorage()
        {
            return CCefSetting_localStorage(_native);
        }

        // Source: void setDatabases(CefViewPluingState)
        [DllImport("CCefView")]
        private static extern void CCefSetting_setDatabases(IntPtr thiz, CefViewPluingState value);
        public void SetDatabases(CefViewPluingState value)
        {
            CCefSetting_setDatabases(_native, value);
        }

        // Source: CefViewPluingState databases()
        [DllImport("CCefView")]
        private static extern CefViewPluingState CCefSetting_databases(IntPtr thiz);
        public CefViewPluingState Databases()
        {
            return CCefSetting_databases(_native);
        }

        // Source: void setWebGL(CefViewPluingState)
        [DllImport("CCefView")]
        private static extern void CCefSetting_setWebGL(IntPtr thiz, CefViewPluingState value);
        public void SetWebGL(CefViewPluingState value)
        {
            CCefSetting_setWebGL(_native, value);
        }

        // Source: CefViewPluingState webGL()
        [DllImport("CCefView")]
        private static extern CefViewPluingState CCefSetting_webGL(IntPtr thiz);
        public CefViewPluingState WebGL()
        {
            return CCefSetting_webGL(_native);
        }

        // Source: void setBackgroundColor(const uint32_t &)
        [DllImport("CCefView")]
        private static extern void CCefSetting_setBackgroundColor(IntPtr thiz, UInt32 value);
        public void SetBackgroundColor(UInt32 value)
        {
            CCefSetting_setBackgroundColor(_native, value);
        }

        // Source: uint32_t backgroundColor()
        [DllImport("CCefView")]
        private static extern UInt32 CCefSetting_backgroundColor(IntPtr thiz);
        public UInt32 BackgroundColor()
        {
            return CCefSetting_backgroundColor(_native);
        }

        // Source: void setHardwareAccelerationEnabled(bool)
        [DllImport("CCefView")]
        private static extern void CCefSetting_setHardwareAccelerationEnabled(IntPtr thiz, bool enabled);
        public void SetHardwareAccelerationEnabled(bool enabled)
        {
            CCefSetting_setHardwareAccelerationEnabled(_native, enabled);
        }

        // Source: bool hardwareAccelerationEnabled()
        [DllImport("CCefView")]
        private static extern bool CCefSetting_hardwareAccelerationEnabled(IntPtr thiz);
        public bool HardwareAccelerationEnabled()
        {
            return CCefSetting_hardwareAccelerationEnabled(_native);
        }

    }
}