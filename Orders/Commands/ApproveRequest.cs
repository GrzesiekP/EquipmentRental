using System;
using Core.Domain.Commands;

namespace Orders.Commands
{
    public class ApproveRequest : ICommand
    {
        public ApproveRequest(Guid orderId)
        {
            OrderId = orderId;
        }
        
        public Guid OrderId { get; }
    }
}