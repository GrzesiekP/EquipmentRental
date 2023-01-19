using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Domain.Events;
using MediatR;
using Orders.Commands;
using Orders.Events;

namespace Orders.EventHandlers
{
    public class OrderSubmittedEventHandler : IEventHandler<OrderSubmitted>
    {
        private readonly IMediator _mediator;
        
        public OrderSubmittedEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public Task Handle(OrderSubmitted eOrderSubmitted, CancellationToken cancellationToken)
        {
            Console.WriteLine($"{nameof(OrderSubmittedEventHandler)} handling {nameof(OrderSubmitted)}");
            var requestApproval = new RequestOrderApproval(eOrderSubmitted.OrderId);

            Console.WriteLine($"{nameof(OrderSubmittedEventHandler)} publishing {nameof(RequestOrderApproval)}");
            return _mediator.Send(requestApproval, cancellationToken);
        }
    }
}