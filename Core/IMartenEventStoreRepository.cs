using System;
using System.Threading.Tasks;

namespace Core
{
    public interface IMartenEventStoreRepository<T> where T : IAggregate
    {
        Task<T> Find(Guid id);
        Task Add(T aggregate);
        Task Update(T aggregate);
    }
}