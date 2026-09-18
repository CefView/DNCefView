using System;
using System.Threading;

namespace DNCefView
{
    public partial class CefBrowser
    {
        private volatile bool _created, _closed, _closeRequested;
        private int _disposeQueued, _deleteQueued;
        private readonly object _callGate = new object();
        private int _activeCalls;
        private bool _deleting;
        public bool IsDisposed => Interlocked.CompareExchange(ref _native, IntPtr.Zero, IntPtr.Zero) == IntPtr.Zero;

        private CallLease AcquireCall()
        {
            lock (_callGate)
            {
                if (_deleting || _native == IntPtr.Zero) throw new ObjectDisposedException(nameof(CefBrowser));
                _activeCalls++;
                return new CallLease(this);
            }
        }
        private readonly struct CallLease : IDisposable
        {
            private readonly CefBrowser _owner;
            public CallLease(CefBrowser owner) { _owner = owner; }
            public void Dispose()
            {
                lock (_owner._callGate)
                {
                    _owner._activeCalls--;
                    Monitor.PulseAll(_owner._callGate);
                }
            }
        }

        private void RequestDispose()
        {
            if (Interlocked.Exchange(ref _disposeQueued, 1) != 0) return;
            _closeRequested = true;
            if (_created && !_closed) CloseBrowser(true);
            if (_closed) QueueNativeDelete();
        }
        private void QueueNativeDelete()
        {
            if (Interlocked.Exchange(ref _deleteQueued, 1) != 0) return;
            // The native UI-thread barrier executes only after this callback returns.
            ThreadPool.QueueUserWorkItem(_ =>
            {
                // Block new calls and drain callers before queuing native deletion.
                // Never hold this gate while waiting for the CEF UI thread.
                lock (_callGate)
                {
                    _deleting = true;
                    while (_activeCalls != 0) Monitor.Wait(_callGate);
                }
                CCefBrowser_Delete(_native);
                Interlocked.Exchange(ref _native, IntPtr.Zero);
                lock (LiveInstances) LiveInstances.Remove(this);
            });
        }
    }
}
