using System;
using Core.Domain.Events;

namespace Core.Domain.Aggregates
{
    public interface IAggregate<out T>
    {
        T Id { get; }
    }

    public interface IAggregate : IAggregate<Guid>
    {
        IEvent[] DequeueUncommittedEvents();
    }
}