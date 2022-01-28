using System;
using Core.Domain.Commands;

namespace Orders.Commands
{
    public class NotifyClient : ICommand
    {
        public NotifyClient(Guid orderId)
        {
            OrderId = orderId;
        }
        
        public Guid OrderId { get; }
    }
}