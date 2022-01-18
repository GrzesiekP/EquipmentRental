using System.Threading;
using System.Threading.Tasks;
using Core;
using MediatR;
using Orders.Commands;
using Orders.Events;

namespace Orders.EventHandlers
{
    public class RequestApprovedEventHandler : IEventHandler<RequestApproved>
    {
        private readonly IMediator _mediator;
        
        public RequestApprovedEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public Task Handle(RequestApproved requestApproved, CancellationToken cancellationToken)
        {
            var notifyClient = new NotifyClient(requestApproved.OrderId);

            return _mediator.Send(notifyClient, cancellationToken);
        }
    }
}