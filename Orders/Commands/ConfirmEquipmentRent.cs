using System;
using Core.Domain.Commands;

namespace Orders.Commands
{
    public class ConfirmEquipmentRent : ICommand
    {
        public ConfirmEquipmentRent(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; }
    }
}