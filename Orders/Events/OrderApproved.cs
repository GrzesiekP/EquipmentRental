using System;
using Core.Domain.Events;

namespace Orders.Events
{
    public class OrderApproved : IEvent
    {
        public OrderApproved(Guid orderId)
        {
            OrderId = orderId;
        }
        
        public Guid OrderId { get; }
    }
}