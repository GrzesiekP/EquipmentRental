using System;
using IEvent = Core.Domain.Events.IEvent;

namespace Orders.Events;

public class EquipmentReserved : IEvent
{
    public EquipmentReserved(Guid orderId)
    {
        OrderId = orderId;
    }
        
    public Guid OrderId { get; }
}