using Core.EventStore;
using Marten;
using MediatR;
using Orders.Aggregate;

namespace Orders.EventStore
{
    public class OrderRepository : AggregateRepository<Order>
    {
        public OrderRepository(IDocumentSession documentSession, IMediator mediator) : 
            base(documentSession, mediator) {}
    }
}