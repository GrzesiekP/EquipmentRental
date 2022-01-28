using System;
using Core.Domain.Events;

namespace Orders.Events
{
    public class RequestApproved : IEvent
    {
        public RequestApproved(Guid orderId)
        {
            OrderId = orderId;
        }
        
        public Guid OrderId { get; }
    }
}