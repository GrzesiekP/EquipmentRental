using System.Threading.Tasks;

namespace Core
{
    public interface IMartenEventStoreRepository<T> where T : IAggregate
    {
        Task Add(T aggregate);
    }
}