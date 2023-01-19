using System;
using Core.Domain.Commands;

namespace Orders.Commands
{
    public class ConfirmEquipmentReturned: ICommand
    {
        public ConfirmEquipmentReturned(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; }
    }
}