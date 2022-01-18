using System;
using Core;

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