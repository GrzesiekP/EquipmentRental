using System;
using System.Collections.Generic;

namespace Core.Domain
{
    public abstract class Aggregate<T>: IAggregate<T> where T : notnull
    {
        public T Id { get; protected set; } = default;
        
        [NonSerialized] private readonly Queue<IEvent> _uncommittedEvents = new Queue<IEvent>();
        
        public IEvent[] DequeueUncommittedEvents()
        {
            var dequeuedEvents = _uncommittedEvents.ToArray();

            _uncommittedEvents.Clear();

            return dequeuedEvents;
        }
        
        protected void Enqueue(IEvent eventToPublish)
        {
            _uncommittedEvents.Enqueue(eventToPublish);
        }
    }
}