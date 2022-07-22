using System;
using System.Collections.Generic;
using System.Linq;
using Core.Extensions;
using Orders.Models.Entities;

namespace Orders.Models.ValueObjects
{
    public record OrderData
    {
        public OrderData(List<EquipmentItem> equipmentItems, RentalPeriod rentalPeriod)
        {
            EquipmentItems = equipmentItems.AssertNotNullOrEmpty(nameof(equipmentItems));
        }
        
        public List<EquipmentItem> EquipmentItems { get; }
        public RentalPeriod RentalPeriod { get; }

        public Money CalculateTotalPrice()
        {
            var totalAmount = EquipmentItems.Sum(i => i.Type.RentalPrice.Amount);
            return new Money(totalAmount * RentalDays());
        }

        public int RentalDays()
        {
            return (int)Math.Ceiling(RentalPeriod.Value.TotalDays);
        }
    }
}