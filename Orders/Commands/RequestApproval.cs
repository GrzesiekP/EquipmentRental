using System;
using Core.Domain;

namespace Orders.Commands
{
    public class RequestApproval : ICommand
    {
        public RequestApproval(Guid orderId)
        {
            OrderId = orderId;
        }
        
        public Guid OrderId { get; }
    }
}