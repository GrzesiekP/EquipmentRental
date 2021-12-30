using System.Threading.Tasks;

namespace Core
{
    public interface IEventStoreRepository<T> where T : IAggregate
    {
        Task Add(T aggregate);
    }
}