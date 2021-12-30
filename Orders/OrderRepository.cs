using System.Linq;
using System.Threading.Tasks;
using Core;
using EventStore.Client;

namespace Orders
{
    public class OrderRepository : IEventStoreRepository<Order>
    {
        private readonly EventStoreClient _eventStoreClient;

        public OrderRepository(EventStoreClient eventStoreClient)
        {
            _eventStoreClient = eventStoreClient;
        }

        public async Task Add(Order order)
        {
            await Store(order);
        }
        
        private async Task Store(Order order)
        {
            var events = order.DequeueUncommittedEvents();

            var eventData = events.Select(EventStoreSerializer.ToJsonEventData);

            var streamName = StreamNameMapper.ToStreamId<Order>(order.Id);
            
            await _eventStoreClient.AppendToStreamAsync(
                streamName,
                StreamState.Any,
                eventData);
        }
    }
}