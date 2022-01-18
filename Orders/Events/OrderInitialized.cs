using System;
using Core.Domain;

namespace Orders.Events
{
    public class OrderInitialized : IEvent
    {
        public OrderInitialized(Guid orderId)
        {
            OrderId = orderId;
        }
        
        public Guid OrderId { get; }
    }
}