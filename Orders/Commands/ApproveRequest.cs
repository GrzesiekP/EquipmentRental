using System;
using Core;

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