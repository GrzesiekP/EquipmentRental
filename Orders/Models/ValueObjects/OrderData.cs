using System;
using System.Collections.Generic;
using System.Linq;
using Core.Extensions;
using Core.Models;
using Equipment.Models.Entities;

namespace Orders.Models.ValueObjects
{
    public record OrderData
    {
        public OrderData(List<EquipmentItem> equipmentItems, RentalPeriod rentalPeriod)
        {
            EquipmentItems = equipmentItems.AssertNotNullOrEmpty(nameof(equipmentItems));
            RentalPeriod = rentalPeriod;
        }
        public List<EquipmentItem> EquipmentItems { get; }
        public RentalPeriod RentalPeriod { get; }
        
        public Money TotalPrice => new(EquipmentItems.Sum(i => i.Type.RentalPrice.Amount) * RentalDays);

        private int RentalDays => (int)Math.Ceiling(RentalPeriod.Value.TotalDays);
    }
}