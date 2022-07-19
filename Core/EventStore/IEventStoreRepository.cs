using System;
using System.Collections.Generic;
using Core.Domain.Aggregates;
using Core.Domain.Events;

namespace Core.EventStore
{
    public interface IEventStoreRepository<T> where T : Aggregate<Guid>
    {
        T GetAggregate(Guid streamId);
        void SaveEvents(Guid streamId, IEnumerable<IEvent> events);
        void SaveAggregate(T aggregate);
    }
}