using System;
using Core.Models;
using IEvent = Core.Domain.Events.IEvent;

namespace Orders.Events;

public class EquipmentReserved : IEvent
{
    public EquipmentReserved(Guid orderId, RentalPeriod period)
    {
        OrderId = orderId;
        Period = period;
    }
        
    public Guid OrderId { get; }

    public RentalPeriod Period {get; }
}