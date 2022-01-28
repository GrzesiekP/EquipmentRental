using System;
using Core.Domain.Commands;
using Orders.ValueObjects;

namespace Orders.Commands
{
    public class SubmitOrder : ICommand
    {
        public SubmitOrder(Guid orderId, OrderData orderData)
        {
            if (orderId == Guid.Empty) throw new ArgumentNullException(nameof(orderId));

            OrderId = orderId;
            OrderData = orderData ?? throw new ArgumentNullException(nameof(orderData));
        }
        
        public Guid OrderId { get; }
        public OrderData OrderData { get; }
    }
}