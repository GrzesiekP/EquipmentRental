using System;
using System.Collections.Generic;

namespace Orders.Models.ValueObjects
{
    public record OrderData(
        IDictionary<string, int> EquipmentItems, 
        RentalPeriod RentalPeriod, 
        Money TotalPrice)
    {
        public int RentalDays()
        {
            return (int)Math.Ceiling(RentalPeriod.Value.TotalDays);
        }
    }
}