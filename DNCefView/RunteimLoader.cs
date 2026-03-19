using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DNCefView
{
#pragma warning disable CA2255
    internal static class RunteimLoader
    {
        const string CCEFVIEW_DLL_NAME = "CCefView";

        [ModuleInitializer]
        internal static void SetCCefViewResolver()
        {
            NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), (name, asm, paths) =>
            {
                var handle = IntPtr.Zero;

                if (name != CCEFVIEW_DLL_NAME)
                {
                    return handle;
                }

                var arc = RuntimeInformation.ProcessArchitecture switch
                {
                    Architecture.Arm64 => "arm64",

                    Architecture.X64 => "x64",

                    _ => throw new PlatformNotSupportedException(
                            $"Unsupported architecture: {RuntimeInformation.ProcessArchitecture}")
                };

                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    var binaryPath = Path.Combine(
                        AppContext.BaseDirectory,
                        "runtimes",
                        $"osx-{arc}",
                        "native",
                        "CCefView.framework",
                        "CCefView"
                        );

                    NativeLibrary.TryLoad(binaryPath, out handle);
                }

                return handle;
            });
        }
    }
#pragma warning restore CA2255
}

