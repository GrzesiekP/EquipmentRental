using System;
using Core;

namespace Orders.Events
{
    public class RequestApproved : IEvent
    {
        public RequestApproved(Guid orderId)
        {
            OrderId = orderId;
        }
        
        public Guid OrderId { get; }
    }
}