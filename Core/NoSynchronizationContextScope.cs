#nullable enable
using System;
using System.Threading;

namespace Core
{
    public class NoSynchronizationContextScope
    {
        public static Disposable Enter()
        {
            var context = SynchronizationContext.Current;
            SynchronizationContext.SetSynchronizationContext(null);
            return new Disposable(context);
        }

        public struct Disposable: IDisposable
        {
            private readonly SynchronizationContext? _synchronizationContext;

            public Disposable(SynchronizationContext? synchronizationContext)
            {
                this._synchronizationContext = synchronizationContext;
            }

            public void Dispose() =>
                SynchronizationContext.SetSynchronizationContext(_synchronizationContext);
        }
    }
}