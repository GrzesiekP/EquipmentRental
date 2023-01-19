using System;
using Core.Domain.Commands;

namespace Orders.Commands
{
    public class ApproveOrder : ICommand
    {
        public ApproveOrder(Guid orderId)
        {
            OrderId = orderId;
        }
        
        public Guid OrderId { get; }
    }
}