using System;
using Core.Domain.Commands;
using Core.Extensions;
using Orders.Models.ValueObjects;

namespace Orders.Commands
{
    public class SubmitOrder : ICommand
    {
        public SubmitOrder(Guid orderId, OrderData orderData, string clientEmail)
        {
            if (orderId == Guid.Empty) throw new ArgumentNullException(nameof(orderId));

            OrderId = orderId;
            OrderData = orderData ?? throw new ArgumentNullException(nameof(orderData));
            ClientEmail = clientEmail.AssertIsValidEmail(nameof(clientEmail));
        }
        
        public Guid OrderId { get; }
        public string ClientEmail { get; }
        public OrderData OrderData { get; }
    }
}