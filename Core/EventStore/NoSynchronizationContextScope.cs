﻿#nullable enable
using System;
using System.Threading;

namespace Core.EventStore
{
    public class NoSynchronizationContextScope
    {
        public static Disposable Enter()
        {
            var context = SynchronizationContext.Current;
            SynchronizationContext.SetSynchronizationContext(null);
            return new Disposable(context);
        }

        public readonly struct Disposable : IDisposable
        {
            private readonly SynchronizationContext? _synchronizationContext;

            public Disposable(SynchronizationContext? synchronizationContext)
            {
                _synchronizationContext = synchronizationContext;
            }

            public void Dispose() =>
                SynchronizationContext.SetSynchronizationContext(_synchronizationContext);
        }
    }
}