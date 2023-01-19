using System;
using Core.Domain.Commands;

namespace Orders.Commands
{
    public class RequestOrderApproval : ICommand
    {
        public RequestOrderApproval(Guid orderId)
        {
            OrderId = orderId;
        }
        
        public Guid OrderId { get; }
    }
}