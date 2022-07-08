using System;
using System.Runtime.InteropServices;

namespace DNCef
{
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

        [DllImport("CCefView")]
        private static extern IntPtr CCefSetting_new0();

        [DllImport("CCefView")]
        private static extern void CCefSetting_setStandardFontFamily(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string value);
        public void SetStandardFontFamily(string value)
        {
            CCefSetting_setStandardFontFamily(_native, value);
        }

        [DllImport("CCefView")]
        private static extern IntPtr CCefSetting_standardFontFamily(IntPtr thiz);
        public string StandardFontFamily()
        {
            return Marshal.PtrToStringUTF8(CCefSetting_standardFontFamily(_native));
        }

        [DllImport("CCefView")]
        private static extern void CCefSetting_setFixedFontFamily(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string value);
        public void SetFixedFontFamily(string value)
        {
            CCefSetting_setFixedFontFamily(_native, value);
        }

        [DllImport("CCefView")]
        private static extern IntPtr CCefSetting_fixedFontFamily(IntPtr thiz);
        public string FixedFontFamily()
        {
            return Marshal.PtrToStringUTF8(CCefSetting_fixedFontFamily(_native));
        }

        [DllImport("CCefView")]
        private static extern void CCefSetting_setSerifFontFamily(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string value);
        public void SetSerifFontFamily(string value)
        {
            CCefSetting_setSerifFontFamily(_native, value);
        }

        [DllImport("CCefView")]
        private static extern IntPtr CCefSetting_serifFontFamily(IntPtr thiz);
        public string SerifFontFamily()
        {
            return Marshal.PtrToStringUTF8(CCefSetting_serifFontFamily(_native));
        }

        [DllImport("CCefView")]
        private static extern void CCefSetting_setSansSerifFontFamily(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string value);
        public void SetSansSerifFontFamily(string value)
        {
            CCefSetting_setSansSerifFontFamily(_native, value);
        }

        [DllImport("CCefView")]
        private static extern IntPtr CCefSetting_sansSerifFontFamily(IntPtr thiz);
        public string SansSerifFontFamily()
        {
            return Marshal.PtrToStringUTF8(CCefSetting_sansSerifFontFamily(_native));
        }

        [DllImport("CCefView")]
        private static extern void CCefSetting_setCursiveFontFamily(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string value);
        public void SetCursiveFontFamily(string value)
        {
            CCefSetting_setCursiveFontFamily(_native, value);
        }

        [DllImport("CCefView")]
        private static extern IntPtr CCefSetting_cursiveFontFamily(IntPtr thiz);
        public string CursiveFontFamily()
        {
            return Marshal.PtrToStringUTF8(CCefSetting_cursiveFontFamily(_native));
        }

        [DllImport("CCefView")]
        private static extern void CCefSetting_setFantasyFontFamily(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string value);
        public void SetFantasyFontFamily(string value)
        {
            CCefSetting_setFantasyFontFamily(_native, value);
        }

        [DllImport("CCefView")]
        private static extern IntPtr CCefSetting_fantasyFontFamily(IntPtr thiz);
        public string FantasyFontFamily()
        {
            return Marshal.PtrToStringUTF8(CCefSetting_fantasyFontFamily(_native));
        }

        [DllImport("CCefView")]
        private static extern void CCefSetting_setDefaultEncoding(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string value);
        public void SetDefaultEncoding(string value)
        {
            CCefSetting_setDefaultEncoding(_native, value);
        }

        [DllImport("CCefView")]
        private static extern IntPtr CCefSetting_defaultEncoding(IntPtr thiz);
        public string DefaultEncoding()
        {
            return Marshal.PtrToStringUTF8(CCefSetting_defaultEncoding(_native));
        }

        [DllImport("CCefView")]
        private static extern void CCefSetting_setAcceptLanguageList(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string value);
        public void SetAcceptLanguageList(string value)
        {
            CCefSetting_setAcceptLanguageList(_native, value);
        }

        [DllImport("CCefView")]
        private static extern IntPtr CCefSetting_acceptLanguageList(IntPtr thiz);
        public string AcceptLanguageList()
        {
            return Marshal.PtrToStringUTF8(CCefSetting_acceptLanguageList(_native));
        }

        [DllImport("CCefView")]
        private static extern void CCefSetting_setWindowlessFrameRate(IntPtr thiz, Int32 value);
        public void SetWindowlessFrameRate(Int32 value)
        {
            CCefSetting_setWindowlessFrameRate(_native, value);
        }

        [DllImport("CCefView")]
        private static extern Int32 CCefSetting_windowlessFrameRate(IntPtr thiz);
        public Int32 WindowlessFrameRate()
        {
            return CCefSetting_windowlessFrameRate(_native);
        }

        [DllImport("CCefView")]
        private static extern void CCefSetting_setDefaultFontSize(IntPtr thiz, Int32 value);
        public void SetDefaultFontSize(Int32 value)
        {
            CCefSetting_setDefaultFontSize(_native, value);
        }

        [DllImport("CCefView")]
        private static extern Int32 CCefSetting_defaultFontSize(IntPtr thiz);
        public Int32 DefaultFontSize()
        {
            return CCefSetting_defaultFontSize(_native);
        }

        [DllImport("CCefView")]
        private static extern void CCefSetting_setDefaultFixedFontSize(IntPtr thiz, Int32 value);
        public void SetDefaultFixedFontSize(Int32 value)
        {
            CCefSetting_setDefaultFixedFontSize(_native, value);
        }

        [DllImport("CCefView")]
        private static extern Int32 CCefSetting_defaultFixedFontSize(IntPtr thiz);
        public Int32 DefaultFixedFontSize()
        {
            return CCefSetting_defaultFixedFontSize(_native);
        }

        [DllImport("CCefView")]
        private static extern void CCefSetting_setMinimumFontSize(IntPtr thiz, Int32 value);
        public void SetMinimumFontSize(Int32 value)
        {
            CCefSetting_setMinimumFontSize(_native, value);
        }

        [DllImport("CCefView")]
        private static extern Int32 CCefSetting_minimumFontSize(IntPtr thiz);
        public Int32 MinimumFontSize()
        {
            return CCefSetting_minimumFontSize(_native);
        }

        [DllImport("CCefView")]
        private static extern void CCefSetting_setMinimumLogicalFontSize(IntPtr thiz, Int32 value);
        public void SetMinimumLogicalFontSize(Int32 value)
        {
            CCefSetting_setMinimumLogicalFontSize(_native, value);
        }

        [DllImport("CCefView")]
        private static extern Int32 CCefSetting_minimumLogicalFontSize(IntPtr thiz);
        public Int32 MinimumLogicalFontSize()
        {
            return CCefSetting_minimumLogicalFontSize(_native);
        }

        [DllImport("CCefView")]
        private static extern void CCefSetting_setRemoteFonts(IntPtr thiz, CefViewPluingState value);
        public void SetRemoteFonts(CefViewPluingState value)
        {
            CCefSetting_setRemoteFonts(_native, value);
        }

        [DllImport("CCefView")]
        private static extern CefViewPluingState CCefSetting_remoteFonts(IntPtr thiz);
        public CefViewPluingState RemoteFonts()
        {
            return CCefSetting_remoteFonts(_native);
        }

        [DllImport("CCefView")]
        private static extern void CCefSetting_setJavascript(IntPtr thiz, CefViewPluingState value);
        public void SetJavascript(CefViewPluingState value)
        {
            CCefSetting_setJavascript(_native, value);
        }

        [DllImport("CCefView")]
        private static extern CefViewPluingState CCefSetting_javascript(IntPtr thiz);
        public CefViewPluingState Javascript()
        {
            return CCefSetting_javascript(_native);
        }

        [DllImport("CCefView")]
        private static extern void CCefSetting_setJavascriptCloseWindows(IntPtr thiz, CefViewPluingState value);
        public void SetJavascriptCloseWindows(CefViewPluingState value)
        {
            CCefSetting_setJavascriptCloseWindows(_native, value);
        }

        [DllImport("CCefView")]
        private static extern CefViewPluingState CCefSetting_javascriptCloseWindows(IntPtr thiz);
        public CefViewPluingState JavascriptCloseWindows()
        {
            return CCefSetting_javascriptCloseWindows(_native);
        }

        [DllImport("CCefView")]
        private static extern void CCefSetting_setJavascriptAccessClipboard(IntPtr thiz, CefViewPluingState value);
        public void SetJavascriptAccessClipboard(CefViewPluingState value)
        {
            CCefSetting_setJavascriptAccessClipboard(_native, value);
        }

        [DllImport("CCefView")]
        private static extern CefViewPluingState CCefSetting_javascriptAccessClipboard(IntPtr thiz);
        public CefViewPluingState JavascriptAccessClipboard()
        {
            return CCefSetting_javascriptAccessClipboard(_native);
        }

        [DllImport("CCefView")]
        private static extern void CCefSetting_setJavascriptDomPaste(IntPtr thiz, CefViewPluingState value);
        public void SetJavascriptDomPaste(CefViewPluingState value)
        {
            CCefSetting_setJavascriptDomPaste(_native, value);
        }

        [DllImport("CCefView")]
        private static extern CefViewPluingState CCefSetting_javascriptDomPaste(IntPtr thiz);
        public CefViewPluingState JavascriptDomPaste()
        {
            return CCefSetting_javascriptDomPaste(_native);
        }

        [DllImport("CCefView")]
        private static extern void CCefSetting_setPlugins(IntPtr thiz, CefViewPluingState value);
        public void SetPlugins(CefViewPluingState value)
        {
            CCefSetting_setPlugins(_native, value);
        }

        [DllImport("CCefView")]
        private static extern CefViewPluingState CCefSetting_plugins(IntPtr thiz);
        public CefViewPluingState Plugins()
        {
            return CCefSetting_plugins(_native);
        }

        [DllImport("CCefView")]
        private static extern void CCefSetting_setImageLoading(IntPtr thiz, CefViewPluingState value);
        public void SetImageLoading(CefViewPluingState value)
        {
            CCefSetting_setImageLoading(_native, value);
        }

        [DllImport("CCefView")]
        private static extern CefViewPluingState CCefSetting_imageLoading(IntPtr thiz);
        public CefViewPluingState ImageLoading()
        {
            return CCefSetting_imageLoading(_native);
        }

        [DllImport("CCefView")]
        private static extern void CCefSetting_setImageShrinkStandaloneToFit(IntPtr thiz, CefViewPluingState value);
        public void SetImageShrinkStandaloneToFit(CefViewPluingState value)
        {
            CCefSetting_setImageShrinkStandaloneToFit(_native, value);
        }

        [DllImport("CCefView")]
        private static extern CefViewPluingState CCefSetting_imageShrinkStandaloneToFit(IntPtr thiz);
        public CefViewPluingState ImageShrinkStandaloneToFit()
        {
            return CCefSetting_imageShrinkStandaloneToFit(_native);
        }

        [DllImport("CCefView")]
        private static extern void CCefSetting_setTextAreaResize(IntPtr thiz, CefViewPluingState value);
        public void SetTextAreaResize(CefViewPluingState value)
        {
            CCefSetting_setTextAreaResize(_native, value);
        }

        [DllImport("CCefView")]
        private static extern CefViewPluingState CCefSetting_textAreaResize(IntPtr thiz);
        public CefViewPluingState TextAreaResize()
        {
            return CCefSetting_textAreaResize(_native);
        }

        [DllImport("CCefView")]
        private static extern void CCefSetting_setTabToLinks(IntPtr thiz, CefViewPluingState value);
        public void SetTabToLinks(CefViewPluingState value)
        {
            CCefSetting_setTabToLinks(_native, value);
        }

        [DllImport("CCefView")]
        private static extern CefViewPluingState CCefSetting_tabToLinks(IntPtr thiz);
        public CefViewPluingState TabToLinks()
        {
            return CCefSetting_tabToLinks(_native);
        }

        [DllImport("CCefView")]
        private static extern void CCefSetting_setLocalStorage(IntPtr thiz, CefViewPluingState value);
        public void SetLocalStorage(CefViewPluingState value)
        {
            CCefSetting_setLocalStorage(_native, value);
        }

        [DllImport("CCefView")]
        private static extern CefViewPluingState CCefSetting_localStorage(IntPtr thiz);
        public CefViewPluingState LocalStorage()
        {
            return CCefSetting_localStorage(_native);
        }

        [DllImport("CCefView")]
        private static extern void CCefSetting_setDatabases(IntPtr thiz, CefViewPluingState value);
        public void SetDatabases(CefViewPluingState value)
        {
            CCefSetting_setDatabases(_native, value);
        }

        [DllImport("CCefView")]
        private static extern CefViewPluingState CCefSetting_databases(IntPtr thiz);
        public CefViewPluingState Databases()
        {
            return CCefSetting_databases(_native);
        }

        [DllImport("CCefView")]
        private static extern void CCefSetting_setWebGL(IntPtr thiz, CefViewPluingState value);
        public void SetWebGL(CefViewPluingState value)
        {
            CCefSetting_setWebGL(_native, value);
        }

        [DllImport("CCefView")]
        private static extern CefViewPluingState CCefSetting_webGL(IntPtr thiz);
        public CefViewPluingState WebGL()
        {
            return CCefSetting_webGL(_native);
        }

        [DllImport("CCefView")]
        private static extern void CCefSetting_setBackgroundColor(IntPtr thiz, UInt32 value);
        public void SetBackgroundColor(UInt32 value)
        {
            CCefSetting_setBackgroundColor(_native, value);
        }

        [DllImport("CCefView")]
        private static extern UInt32 CCefSetting_backgroundColor(IntPtr thiz);
        public UInt32 BackgroundColor()
        {
            return CCefSetting_backgroundColor(_native);
        }

    }
}