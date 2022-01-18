using System.Threading.Tasks;
using Core.Domain;

namespace Core.EventStore
{
    public interface IEventStoreRepository<T> where T : IAggregate
    {
        Task Add(T aggregate);
    }
}