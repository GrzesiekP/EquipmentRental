using System;
using System.Net.Mail;
using Core.Domain.Events;
using Orders.ValueObjects;

namespace Orders.Events
{
    public class OrderSubmitted : IEvent
    {
        public OrderSubmitted(Guid orderId, OrderData orderData, MailAddress clientEmail)
        {
            OrderId = orderId;
            OrderData = orderData;
            ClientEmail = clientEmail;
        }
        
        public Guid OrderId { get; }
        public MailAddress ClientEmail { get; }
        public OrderData OrderData { get; }
    }
}