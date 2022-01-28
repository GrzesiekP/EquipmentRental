using System;
using Core.Domain.Events;
using Orders.ValueObjects;

namespace Orders.Events
{
    public class OrderSubmitted : IEvent
    {
        public OrderSubmitted(Guid orderId, OrderData orderData)
        {
            OrderId = orderId;
            OrderData = orderData;
        }
        
        public Guid OrderId { get; }
        public OrderData OrderData { get; }
    }
}