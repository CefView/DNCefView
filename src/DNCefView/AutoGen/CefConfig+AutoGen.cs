using System;
using System.Runtime.InteropServices;

namespace DNCefView
{
    // Source: CCefConfig 
    public partial class CefConfig : IDisposable
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
        private static extern void CCefConfig_Delete(IntPtr p);
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // TODO: cleanup the managed resources
            }

            // cleanup unmanaged resources
            if (_native != IntPtr.Zero)
            {
                CCefConfig_Delete(_native);
                _native = IntPtr.Zero;
            }
        }

        // Source: CCefConfig()
        [DllImport("CCefView")]
        private static extern IntPtr CCefConfig_new0();

        // Source: void addCommandLineSwitch(const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefConfig_addCommandLineSwitch(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string smitch);
        public void AddCommandLineSwitch(string smitch)
        {
            CCefConfig_addCommandLineSwitch(_native, smitch);
        }

        // Source: void addCommandLineSwitchWithValue(const std::string &, const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefConfig_addCommandLineSwitchWithValue(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string smitch, [MarshalAs(UnmanagedType.LPUTF8Str)] string v);
        public void AddCommandLineSwitchWithValue(string smitch, string v)
        {
            CCefConfig_addCommandLineSwitchWithValue(_native, smitch, v);
        }

        // Source: void setCommandLinePassthroughDisabled(const bool)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setCommandLinePassthroughDisabled(IntPtr thiz, bool disabled);
        public void SetCommandLinePassthroughDisabled(bool disabled)
        {
            CCefConfig_setCommandLinePassthroughDisabled(_native, disabled);
        }

        // Source: bool commandLinePassthroughDisabled()
        [DllImport("CCefView")]
        private static extern bool CCefConfig_commandLinePassthroughDisabled(IntPtr thiz);
        public bool CommandLinePassthroughDisabled()
        {
            return CCefConfig_commandLinePassthroughDisabled(_native);
        }

        // Source: void setLogLevel(CefViewLogLevel)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setLogLevel(IntPtr thiz, CefViewLogLevel lvl);
        public void SetLogLevel(CefViewLogLevel lvl)
        {
            CCefConfig_setLogLevel(_native, lvl);
        }

        // Source: CefViewLogLevel logLevel()
        [DllImport("CCefView")]
        private static extern CefViewLogLevel CCefConfig_logLevel(IntPtr thiz);
        public CefViewLogLevel LogLevel()
        {
            return CCefConfig_logLevel(_native);
        }

        // Source: void setLocale(const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setLocale(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string locale);
        public void SetLocale(string locale)
        {
            CCefConfig_setLocale(_native, locale);
        }

        // Source: const std::string & locale()
        [DllImport("CCefView")]
        private static extern IntPtr CCefConfig_locale(IntPtr thiz);
        public string Locale()
        {
            return Marshal.PtrToStringUTF8(CCefConfig_locale(_native));
        }

        // Source: void setUserAgent(const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setUserAgent(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string agent);
        public void SetUserAgent(string agent)
        {
            CCefConfig_setUserAgent(_native, agent);
        }

        // Source: const std::string & userAgent()
        [DllImport("CCefView")]
        private static extern IntPtr CCefConfig_userAgent(IntPtr thiz);
        public string UserAgent()
        {
            return Marshal.PtrToStringUTF8(CCefConfig_userAgent(_native));
        }

        // Source: void setCachePath(const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setCachePath(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string path);
        public void SetCachePath(string path)
        {
            CCefConfig_setCachePath(_native, path);
        }

        // Source: const std::string & cachePath()
        [DllImport("CCefView")]
        private static extern IntPtr CCefConfig_cachePath(IntPtr thiz);
        public string CachePath()
        {
            return Marshal.PtrToStringUTF8(CCefConfig_cachePath(_native));
        }

        // Source: void setUserDataPath(const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setUserDataPath(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string path);
        public void SetUserDataPath(string path)
        {
            CCefConfig_setUserDataPath(_native, path);
        }

        // Source: const std::string & userDataPath()
        [DllImport("CCefView")]
        private static extern IntPtr CCefConfig_userDataPath(IntPtr thiz);
        public string UserDataPath()
        {
            return Marshal.PtrToStringUTF8(CCefConfig_userDataPath(_native));
        }

        // Source: void setBridgeObjectName(const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setBridgeObjectName(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);
        public void SetBridgeObjectName(string name)
        {
            CCefConfig_setBridgeObjectName(_native, name);
        }

        // Source: const std::string & bridgeObjectName()
        [DllImport("CCefView")]
        private static extern IntPtr CCefConfig_bridgeObjectName(IntPtr thiz);
        public string BridgeObjectName()
        {
            return Marshal.PtrToStringUTF8(CCefConfig_bridgeObjectName(_native));
        }

        // Source: void setBuiltinSchemaName(const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setBuiltinSchemaName(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);
        public void SetBuiltinSchemaName(string name)
        {
            CCefConfig_setBuiltinSchemaName(_native, name);
        }

        // Source: const std::string & builtinSchemaName()
        [DllImport("CCefView")]
        private static extern IntPtr CCefConfig_builtinSchemaName(IntPtr thiz);
        public string BuiltinSchemaName()
        {
            return Marshal.PtrToStringUTF8(CCefConfig_builtinSchemaName(_native));
        }

        // Source: void setBackgroundColor(uint32_t)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setBackgroundColor(IntPtr thiz, UInt32 color);
        public void SetBackgroundColor(UInt32 color)
        {
            CCefConfig_setBackgroundColor(_native, color);
        }

        // Source: uint32_t backgroundColor()
        [DllImport("CCefView")]
        private static extern UInt32 CCefConfig_backgroundColor(IntPtr thiz);
        public UInt32 BackgroundColor()
        {
            return CCefConfig_backgroundColor(_native);
        }

        // Source: void setAcceptLanguageList(const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setAcceptLanguageList(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string languages);
        public void SetAcceptLanguageList(string languages)
        {
            CCefConfig_setAcceptLanguageList(_native, languages);
        }

        // Source: const std::string & acceptLanguageList()
        [DllImport("CCefView")]
        private static extern IntPtr CCefConfig_acceptLanguageList(IntPtr thiz);
        public string AcceptLanguageList()
        {
            return Marshal.PtrToStringUTF8(CCefConfig_acceptLanguageList(_native));
        }

        // Source: void setPersistSessionCookies(bool)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setPersistSessionCookies(IntPtr thiz, bool enabled);
        public void SetPersistSessionCookies(bool enabled)
        {
            CCefConfig_setPersistSessionCookies(_native, enabled);
        }

        // Source: bool persistSessionCookies()
        [DllImport("CCefView")]
        private static extern bool CCefConfig_persistSessionCookies(IntPtr thiz);
        public bool PersistSessionCookies()
        {
            return CCefConfig_persistSessionCookies(_native);
        }

        // Source: void setPersistUserPreferences(bool)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setPersistUserPreferences(IntPtr thiz, bool enabled);
        public void SetPersistUserPreferences(bool enabled)
        {
            CCefConfig_setPersistUserPreferences(_native, enabled);
        }

        // Source: bool persistUserPreferences()
        [DllImport("CCefView")]
        private static extern bool CCefConfig_persistUserPreferences(IntPtr thiz);
        public bool PersistUserPreferences()
        {
            return CCefConfig_persistUserPreferences(_native);
        }

        // Source: void setMultiThreadedMessageLoop(bool)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setMultiThreadedMessageLoop(IntPtr thiz, bool enable);
        public void SetMultiThreadedMessageLoop(bool enable)
        {
            CCefConfig_setMultiThreadedMessageLoop(_native, enable);
        }

        // Source: bool multiThreadedMessageLoop()
        [DllImport("CCefView")]
        private static extern bool CCefConfig_multiThreadedMessageLoop(IntPtr thiz);
        public bool MultiThreadedMessageLoop()
        {
            return CCefConfig_multiThreadedMessageLoop(_native);
        }

        // Source: void setRemoteDebuggingPort(short)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setRemoteDebuggingPort(IntPtr thiz, short port);
        public void SetRemoteDebuggingPort(short port)
        {
            CCefConfig_setRemoteDebuggingPort(_native, port);
        }

        // Source: short remoteDebuggingPort()
        [DllImport("CCefView")]
        private static extern short CCefConfig_remoteDebuggingPort(IntPtr thiz);
        public short RemoteDebuggingPort()
        {
            return CCefConfig_remoteDebuggingPort(_native);
        }

        // Source: void setWindowlessRendering(bool)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setWindowlessRendering(IntPtr thiz, bool enable);
        public void SetWindowlessRendering(bool enable)
        {
            CCefConfig_setWindowlessRendering(_native, enable);
        }

        // Source: bool windowlessRendering()
        [DllImport("CCefView")]
        private static extern bool CCefConfig_windowlessRendering(IntPtr thiz);
        public bool WindowlessRendering()
        {
            return CCefConfig_windowlessRendering(_native);
        }

    }
}