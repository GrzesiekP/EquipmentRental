using System;

namespace Core
{
    public interface IAggregate<out T>
    {
        T Id { get; }
    }

    public interface IAggregate : IAggregate<Guid>
    {
        
    }
}