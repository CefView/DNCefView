using Avalonia.Logging;
using System.Runtime.CompilerServices;

namespace DNCefView.Avalonia;

internal static class CefViewLogExt
{
    /// <summary>
    /// 
    /// </summary>
    static readonly string LogArea = global::Avalonia.Logging.LogArea.Control;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="this"></param>
    /// <param name="message"></param>
    public static void LogD(this object @this, string message) =>
        Logger.TryGet(LogEventLevel.Debug, LogArea)?.Log(@this, message);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="this"></param>
    /// <param name="message"></param>
    public static void LogI(this object @this, string message) =>
        Logger.TryGet(LogEventLevel.Information, LogArea)?.Log(@this, message);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="this"></param>
    /// <param name="message"></param>
    public static void LogW(this object @this, string message) =>
        Logger.TryGet(LogEventLevel.Warning, LogArea)?.Log(@this, message);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="this"></param>
    /// <param name="message"></param>
    public static void LogE(this object @this, string message) =>
        Logger.TryGet(LogEventLevel.Error, LogArea)?.Log(@this, message);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="this"></param>
    /// <param name="message"></param>
    public static void LogF(this object @this, string message) =>
        Logger.TryGet(LogEventLevel.Fatal, LogArea)?.Log(@this, message);

    /// <summary>
    /// 
    /// </summary>
    private struct MethodScopeLogger : IDisposable
    {
        private readonly object _caller;

        private readonly string _source;

        public MethodScopeLogger(object caller, string member, string message)
        {
            _caller = caller;
            _source = $"{caller.GetType().Name}.{member}";

            _caller.LogD($"+++++++ {_source}: {message}");
        }

        public void Dispose()
        {
            _caller.LogD($"------- {_source}");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="this"></param>
    /// <param name="message"></param>
    /// <param name="member"></param>
    /// <returns></returns>
    public static IDisposable LogM(this object @this, string message = "", [CallerMemberName] string member = "")
    {
        return new MethodScopeLogger(@this, member, message);
    }
}