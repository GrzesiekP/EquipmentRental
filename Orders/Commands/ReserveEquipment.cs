using System;
using Core.Domain.Commands;

namespace Orders.Commands
{
    public class ReserveEquipment : ICommand
    {
        public ReserveEquipment(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; }
    }
}