using System;
using Core.Domain;

namespace Orders.Events
{
    public class OrderSubmitted : IEvent
    {
        public OrderSubmitted(Guid orderId)
        {
            OrderId = orderId;
        }
        
        public Guid OrderId { get; }
    }
}