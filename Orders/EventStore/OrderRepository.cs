using System;
using System.Threading.Tasks;
using Core.EventStore;
using Marten;
using MediatR;
using Orders.Aggregate;

namespace Orders.EventStore
{
    public class OrderRepository : IMartenEventStoreRepository<Order>
    {
        private readonly IDocumentSession _documentSession;
        private readonly IMediator _mediator;

        public OrderRepository(IDocumentSession documentSession, IMediator mediator)
        {
            _documentSession = documentSession;
            _mediator = mediator;
        }

        public Task<Order> Find(Guid id)
        {
            return _documentSession.Events.AggregateStreamAsync<Order>(id);
        }

        public async Task Add(Order order)
        {
            await Store(order);
        }
        
        public async Task Update(Order order)
        {
            await Store(order);
        }
        
        private async Task Store(Order order)
        {
            var uncommittedEvents = order.DequeueUncommittedEvents();
            _documentSession.Events.Append(order.Id, uncommittedEvents);

            await _documentSession.SaveChangesAsync();

            foreach (var @event in uncommittedEvents)
            {
                await _mediator.Publish(@event);
            }
        }
    }
}