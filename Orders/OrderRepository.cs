using System;
using System.Threading.Tasks;
using Core;
using Marten;
using MediatR;

namespace Orders
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
            var events = order.DequeueUncommittedEvents();
            _documentSession.Events.Append(order.Id, events);

            await _documentSession.SaveChangesAsync();

            foreach (var @event in events)
            {
                await _mediator.Publish(@event);
            }
        }
    }
}