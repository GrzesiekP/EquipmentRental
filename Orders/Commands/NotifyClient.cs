using System;
using Core.Domain;

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