using System;

namespace Core.Domain.Aggregates
{
    public interface IAggregate<out T>
    {
        T Id { get; }
    }

    public interface IAggregate : IAggregate<Guid>
    {
        
    }
}