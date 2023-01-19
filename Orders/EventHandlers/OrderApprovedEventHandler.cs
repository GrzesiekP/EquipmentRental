using System.Threading;
using System.Threading.Tasks;
using Core.Domain.Events;
using MediatR;
using Orders.Commands;
using Orders.Events;

namespace Orders.EventHandlers
{
    public class OrderApprovedEventHandler : IEventHandler<OrderApproved>
    {
        private readonly IMediator _mediator;
        
        public OrderApprovedEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public Task Handle(OrderApproved orderApproved, CancellationToken cancellationToken)
        {
            var notifyClient = new NotifyClient(orderApproved.OrderId);

            return _mediator.Send(notifyClient, cancellationToken);
        }
    }
}