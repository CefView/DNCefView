using Avalonia.Threading;
using System;

namespace DNCefView.Avalonia
{
    public partial class CefView
    {
        void RunInUIThread(Action action, bool block = true)
        {
            if (Dispatcher.UIThread.CheckAccess())
            {
                // invoke directly
                action();
            }
            else if (block)
            {
                // invoke and block 
                Dispatcher.UIThread.Invoke(action);
            }
            else
            {
                // invoke asynchronously
                Dispatcher.UIThread.InvokeAsync(action);
            }
        }
    }
}
