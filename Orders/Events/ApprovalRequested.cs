using System;
using Core.Domain;

namespace Orders.Events
{
    public class ApprovalRequested : IEvent
    {
        public ApprovalRequested(Guid orderId)
        {
            OrderId = orderId;
        }
        
        public Guid OrderId { get; }
    }
}