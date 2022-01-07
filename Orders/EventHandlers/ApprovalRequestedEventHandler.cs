using System;
using System.Threading;
using System.Threading.Tasks;
using Core;
using MediatR;
using Orders.Events;

namespace Orders.EventHandlers
{
    public class ApprovalRequestedEventHandler : IEventHandler<ApprovalRequested>
    {
        private readonly IMediator _mediator;
        
        public ApprovalRequestedEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public Task Handle(ApprovalRequested approvalRequested, CancellationToken cancellationToken)
        {
            Console.WriteLine($"{nameof(ApprovalRequested)}:{approvalRequested.OrderId}");

            return Task.CompletedTask;
        }
    }
}