using System;
using System.Threading.Tasks;
using Core.Domain.Aggregates;
using Marten;
using MediatR;

namespace Core.EventStore
{
    public class AggregateRepository<T> : IMartenEventStoreRepository<T> where T : class, IAggregate
    {
        private readonly IDocumentSession _documentSession;
        private readonly IMediator _mediator;

        protected AggregateRepository(IDocumentSession documentSession, IMediator mediator)
        {
            _documentSession = documentSession;
            _mediator = mediator;
        }
        
        public async Task<T> Find(Guid id)
        {
            return await _documentSession.Events.AggregateStreamAsync<T>(id);
        }

        public async Task Add(T aggregate)
        {
            await Store(aggregate);
        }

        public async Task Update(T aggregate)
        {
            await Store(aggregate);
        }
        
        private async Task Store(T order)
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