using System;
using Core.Domain;

namespace Orders.Commands
{
    public class SubmitOrder : ICommand
    {
        public SubmitOrder(Guid orderId)
        {
            OrderId = orderId;
        }
        
        public Guid OrderId { get; }
    }
}