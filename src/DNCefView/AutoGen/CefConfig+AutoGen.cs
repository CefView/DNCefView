#pragma warning disable CS8603
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
            get { if (_native == IntPtr.Zero) throw new ObjectDisposedException(nameof(CefConfig)); return _native; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [DllImport("CCefView")]
        private static extern void CCefConfig_Delete(IntPtr p);
        protected virtual void Dispose([MarshalAs(UnmanagedType.I1)] bool disposing)
        {
            var native = System.Threading.Interlocked.Exchange(ref _native, IntPtr.Zero);
            if (native != IntPtr.Zero) CCefConfig_Delete(native);
        }

        // Source: CCefConfig()
        [DllImport("CCefView")]
        private static extern IntPtr CCefConfig_new0();

        // Source: void addCommandLineSwitch(const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefConfig_addCommandLineSwitch(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string smitch);
        public void AddCommandLineSwitch(string smitch)
        {
            CCefConfig_addCommandLineSwitch(NativeObject, smitch);
        }

        // Source: void addCommandLineSwitchWithValue(const std::string &, const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefConfig_addCommandLineSwitchWithValue(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string smitch, [MarshalAs(UnmanagedType.LPUTF8Str)] string v);
        public void AddCommandLineSwitchWithValue(string smitch, string v)
        {
            CCefConfig_addCommandLineSwitchWithValue(NativeObject, smitch, v);
        }

        // Source: void setCommandLinePassthroughDisabled(const bool)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setCommandLinePassthroughDisabled(IntPtr thiz, [MarshalAs(UnmanagedType.I1)] bool disabled);
        public void SetCommandLinePassthroughDisabled([MarshalAs(UnmanagedType.I1)] bool disabled)
        {
            CCefConfig_setCommandLinePassthroughDisabled(NativeObject, disabled);
        }

        // Source: bool commandLinePassthroughDisabled()
        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CCefConfig_commandLinePassthroughDisabled(IntPtr thiz);
        public bool CommandLinePassthroughDisabled()
        {
            return CCefConfig_commandLinePassthroughDisabled(NativeObject);
        }

        // Source: void setLogLevel(CefViewLogLevel)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setLogLevel(IntPtr thiz, CefViewLogLevel lvl);
        public void SetLogLevel(CefViewLogLevel lvl)
        {
            CCefConfig_setLogLevel(NativeObject, lvl);
        }

        // Source: CefViewLogLevel logLevel()
        [DllImport("CCefView")]
        private static extern CefViewLogLevel CCefConfig_logLevel(IntPtr thiz);
        public CefViewLogLevel LogLevel()
        {
            return CCefConfig_logLevel(NativeObject);
        }

        // Source: void setLocale(const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setLocale(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string locale);
        public void SetLocale(string locale)
        {
            CCefConfig_setLocale(NativeObject, locale);
        }

        // Source: const std::string & locale()
        [DllImport("CCefView")]
        private static extern IntPtr CCefConfig_locale(IntPtr thiz);
        public string Locale()
        {
            return Marshal.PtrToStringUTF8(CCefConfig_locale(NativeObject));
        }

        // Source: void setUserAgent(const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setUserAgent(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string agent);
        public void SetUserAgent(string agent)
        {
            CCefConfig_setUserAgent(NativeObject, agent);
        }

        // Source: const std::string & userAgent()
        [DllImport("CCefView")]
        private static extern IntPtr CCefConfig_userAgent(IntPtr thiz);
        public string UserAgent()
        {
            return Marshal.PtrToStringUTF8(CCefConfig_userAgent(NativeObject));
        }

        // Source: void setCachePath(const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setCachePath(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string path);
        public void SetCachePath(string path)
        {
            CCefConfig_setCachePath(NativeObject, path);
        }

        // Source: const std::string & cachePath()
        [DllImport("CCefView")]
        private static extern IntPtr CCefConfig_cachePath(IntPtr thiz);
        public string CachePath()
        {
            return Marshal.PtrToStringUTF8(CCefConfig_cachePath(NativeObject));
        }

        // Source: void setUserDataPath(const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setUserDataPath(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string path);
        public void SetUserDataPath(string path)
        {
            CCefConfig_setUserDataPath(NativeObject, path);
        }

        // Source: const std::string & userDataPath()
        [DllImport("CCefView")]
        private static extern IntPtr CCefConfig_userDataPath(IntPtr thiz);
        public string UserDataPath()
        {
            return Marshal.PtrToStringUTF8(CCefConfig_userDataPath(NativeObject));
        }

        // Source: void setRootCachePath(const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setRootCachePath(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string path);
        public void SetRootCachePath(string path)
        {
            CCefConfig_setRootCachePath(NativeObject, path);
        }

        // Source: const std::string & rootCachePath()
        [DllImport("CCefView")]
        private static extern IntPtr CCefConfig_rootCachePath(IntPtr thiz);
        public string RootCachePath()
        {
            return Marshal.PtrToStringUTF8(CCefConfig_rootCachePath(NativeObject));
        }

        // Source: void setBridgeObjectName(const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setBridgeObjectName(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);
        public void SetBridgeObjectName(string name)
        {
            CCefConfig_setBridgeObjectName(NativeObject, name);
        }

        // Source: const std::string & bridgeObjectName()
        [DllImport("CCefView")]
        private static extern IntPtr CCefConfig_bridgeObjectName(IntPtr thiz);
        public string BridgeObjectName()
        {
            return Marshal.PtrToStringUTF8(CCefConfig_bridgeObjectName(NativeObject));
        }

        // Source: void setBuiltinSchemaName(const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setBuiltinSchemaName(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);
        public void SetBuiltinSchemaName(string name)
        {
            CCefConfig_setBuiltinSchemaName(NativeObject, name);
        }

        // Source: const std::string & builtinSchemaName()
        [DllImport("CCefView")]
        private static extern IntPtr CCefConfig_builtinSchemaName(IntPtr thiz);
        public string BuiltinSchemaName()
        {
            return Marshal.PtrToStringUTF8(CCefConfig_builtinSchemaName(NativeObject));
        }

        // Source: void setBackgroundColor(uint32_t)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setBackgroundColor(IntPtr thiz, UInt32 color);
        public void SetBackgroundColor(UInt32 color)
        {
            CCefConfig_setBackgroundColor(NativeObject, color);
        }

        // Source: uint32_t backgroundColor()
        [DllImport("CCefView")]
        private static extern UInt32 CCefConfig_backgroundColor(IntPtr thiz);
        public UInt32 BackgroundColor()
        {
            return CCefConfig_backgroundColor(NativeObject);
        }

        // Source: void setAcceptLanguageList(const std::string &)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setAcceptLanguageList(IntPtr thiz, [MarshalAs(UnmanagedType.LPUTF8Str)] string languages);
        public void SetAcceptLanguageList(string languages)
        {
            CCefConfig_setAcceptLanguageList(NativeObject, languages);
        }

        // Source: const std::string & acceptLanguageList()
        [DllImport("CCefView")]
        private static extern IntPtr CCefConfig_acceptLanguageList(IntPtr thiz);
        public string AcceptLanguageList()
        {
            return Marshal.PtrToStringUTF8(CCefConfig_acceptLanguageList(NativeObject));
        }

        // Source: void setPersistSessionCookies(bool)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setPersistSessionCookies(IntPtr thiz, [MarshalAs(UnmanagedType.I1)] bool enabled);
        public void SetPersistSessionCookies([MarshalAs(UnmanagedType.I1)] bool enabled)
        {
            CCefConfig_setPersistSessionCookies(NativeObject, enabled);
        }

        // Source: bool persistSessionCookies()
        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CCefConfig_persistSessionCookies(IntPtr thiz);
        public bool PersistSessionCookies()
        {
            return CCefConfig_persistSessionCookies(NativeObject);
        }

        // Source: void setPersistUserPreferences(bool)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setPersistUserPreferences(IntPtr thiz, [MarshalAs(UnmanagedType.I1)] bool enabled);
        public void SetPersistUserPreferences([MarshalAs(UnmanagedType.I1)] bool enabled)
        {
            CCefConfig_setPersistUserPreferences(NativeObject, enabled);
        }

        // Source: bool persistUserPreferences()
        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CCefConfig_persistUserPreferences(IntPtr thiz);
        public bool PersistUserPreferences()
        {
            return CCefConfig_persistUserPreferences(NativeObject);
        }

        // Source: void setMultiThreadedMessageLoop(bool)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setMultiThreadedMessageLoop(IntPtr thiz, [MarshalAs(UnmanagedType.I1)] bool enable);
        public void SetMultiThreadedMessageLoop([MarshalAs(UnmanagedType.I1)] bool enable)
        {
            CCefConfig_setMultiThreadedMessageLoop(NativeObject, enable);
        }

        // Source: bool multiThreadedMessageLoop()
        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CCefConfig_multiThreadedMessageLoop(IntPtr thiz);
        public bool MultiThreadedMessageLoop()
        {
            return CCefConfig_multiThreadedMessageLoop(NativeObject);
        }

        // Source: void setRemoteDebuggingPort(short)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setRemoteDebuggingPort(IntPtr thiz, short port);
        public void SetRemoteDebuggingPort(short port)
        {
            CCefConfig_setRemoteDebuggingPort(NativeObject, port);
        }

        // Source: short remoteDebuggingPort()
        [DllImport("CCefView")]
        private static extern short CCefConfig_remoteDebuggingPort(IntPtr thiz);
        public short RemoteDebuggingPort()
        {
            return CCefConfig_remoteDebuggingPort(NativeObject);
        }

        // Source: void setWindowlessRendering(bool)
        [DllImport("CCefView")]
        private static extern void CCefConfig_setWindowlessRendering(IntPtr thiz, [MarshalAs(UnmanagedType.I1)] bool enable);
        public void SetWindowlessRendering([MarshalAs(UnmanagedType.I1)] bool enable)
        {
            CCefConfig_setWindowlessRendering(NativeObject, enable);
        }

        // Source: bool windowlessRendering()
        [DllImport("CCefView")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CCefConfig_windowlessRendering(IntPtr thiz);
        public bool WindowlessRendering()
        {
            return CCefConfig_windowlessRendering(NativeObject);
        }

    }
}
