using System;
using Core.Domain.Events;

namespace Orders.Events;

public class EquipmentRent : IEvent
{
    public EquipmentRent(Guid orderId)
    {
        OrderId = orderId;
    }
        
    public Guid OrderId { get; }
}