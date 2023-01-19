using System;
using Core.Domain.Commands;
using Core.Models;

namespace Orders.Commands
{
    public class ConfirmOrderPayment : ICommand
    {
        public ConfirmOrderPayment(Guid orderId, Money amount)
        {
            OrderId = orderId;
            Amount = amount;
        }
        
        public Guid OrderId { get; }
        public Money Amount { get; }
    }
}