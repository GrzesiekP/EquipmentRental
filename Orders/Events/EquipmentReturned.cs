using System;
using Core.Domain.Events;

namespace Orders.Events;

public class EquipmentReturned : IEvent
{
    public EquipmentReturned(Guid orderId)
    {
        OrderId = orderId;
    }
        
    public Guid OrderId { get; }
}