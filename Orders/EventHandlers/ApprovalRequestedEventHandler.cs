using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Domain.Events;
using Orders.Events;

namespace Orders.EventHandlers
{
    public class ApprovalRequestedEventHandler : IEventHandler<ApprovalRequested>
    {
        public Task Handle(ApprovalRequested approvalRequested, CancellationToken cancellationToken)
        {
            Console.WriteLine($"{nameof(ApprovalRequestedEventHandler)} handling {nameof(ApprovalRequested)}");
            return Task.CompletedTask;
        }
    }
}