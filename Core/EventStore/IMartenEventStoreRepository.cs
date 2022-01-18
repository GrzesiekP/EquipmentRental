using System;
using System.Threading.Tasks;
using Core.Domain;

namespace Core.EventStore
{
    public interface IMartenEventStoreRepository<T> where T : IAggregate
    {
        Task<T> Find(Guid id);
        Task Add(T aggregate);
        Task Update(T aggregate);
    }
}