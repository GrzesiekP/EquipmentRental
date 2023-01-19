using System;
using System.Collections.Generic;
using Core.Domain.Events;

namespace Core.Domain.Aggregates
{
    public abstract class Aggregate<T>: IAggregate<T> where T : notnull
    {
        public T Id { get; protected set; }
        public int Version { get; protected set; }
        
        [NonSerialized] private readonly Queue<IEvent> _uncommittedEvents = new();
        
        public IEvent[] DequeueUncommittedEvents()
        {
            var dequeuedEvents = _uncommittedEvents.ToArray();

            _uncommittedEvents.Clear();

            return dequeuedEvents;
        }

        private void Enqueue(IEvent eventToPublish)
        {
            _uncommittedEvents.Enqueue(eventToPublish);
        }
        
        protected void PublishEvent(IEvent eventToPublish)
        {
            Enqueue(eventToPublish);
        }
    }
}