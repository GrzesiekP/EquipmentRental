using System;
using Core.Domain.Events;
using Orders.ValueObjects;

namespace Orders.Events
{
    public class OrderSubmitted : IEvent
    {
        public OrderSubmitted(Guid orderId, OrderData orderData, string clientEmail)
        {
            OrderId = orderId;
            OrderData = orderData;
            ClientEmail = clientEmail;
        }
        
        public Guid OrderId { get; }
        public string ClientEmail { get; }
        public OrderData OrderData { get; }
    }
}