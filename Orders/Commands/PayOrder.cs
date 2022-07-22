using System;
using Core.Domain.Commands;
using Orders.Models.ValueObjects;

namespace Orders.Commands
{
    public class PayOrder : ICommand
    {
        public PayOrder(Guid orderId, Money amount)
        {
            OrderId = orderId;
            Amount = amount;
        }
        
        public Guid OrderId { get; }
        public Money Amount { get; private set; }
    }
}