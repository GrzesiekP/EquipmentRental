using System.Threading.Tasks;
using Core;
using Marten;

namespace Orders
{
    public class OrderRepository : IMartenEventStoreRepository<Order>
    {
        private readonly IDocumentSession _documentSession;

        public OrderRepository(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public async Task Add(Order order)
        {
            await Store(order);
        }
        
        private async Task Store(Order order)
        {
            var events = order.DequeueUncommittedEvents();
            _documentSession.Events.Append(order.Id, events);

            await _documentSession.SaveChangesAsync();
            // publish events
        }
    }
}