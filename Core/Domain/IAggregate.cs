using System;

namespace Core.Domain
{
    public interface IAggregate<out T>
    {
        T Id { get; }
    }

    public interface IAggregate : IAggregate<Guid>
    {
        
    }
}