using System;
using System.Net.Mail;
using Core.Domain.Commands;
using Orders.ValueObjects;

namespace Orders.Commands
{
    public class SubmitOrder : ICommand
    {
        public SubmitOrder(Guid orderId, OrderData orderData, MailAddress clientEmail)
        {
            if (orderId == Guid.Empty) throw new ArgumentNullException(nameof(orderId));

            OrderId = orderId;
            OrderData = orderData ?? throw new ArgumentNullException(nameof(orderData));
            ClientEmail = clientEmail;
        }
        
        public Guid OrderId { get; }
        public MailAddress ClientEmail { get; }
        public OrderData OrderData { get; }
    }
}