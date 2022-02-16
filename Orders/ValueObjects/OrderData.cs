using System;
using System.Collections.Generic;
using System.Linq;
using Core.Extensions;

namespace Orders.ValueObjects
{
    public record OrderData
    {
        public OrderData(List<EquipmentItem> equipmentItems, DateTime rentalDate, DateTime returnDate, Guid rentingUserId)
        {
            EquipmentItems = equipmentItems.AssertNotNullOrEmpty(nameof(equipmentItems));
            if (rentalDate > returnDate)
            {
                throw new ArgumentException(
                    $"Rental date {rentalDate.ToShortDateString()} cannot be after return date {returnDate.ToShortDateString()}");
            }
            RentalDate = rentalDate;
            ReturnDate = returnDate;
            RentingUserId = rentingUserId.AssertIsNotEmpty(nameof(rentingUserId));
        }
        
        public List<EquipmentItem> EquipmentItems { get; }
        public DateTime RentalDate { get; }
        public DateTime ReturnDate { get; }
        public Guid RentingUserId { get; }

        public decimal CalculateTotalPrice()
        {
            return EquipmentItems.Sum(i => i.RentalPrice) * RentalDays();
        }

        public int RentalDays()
        {
            return (int)Math.Ceiling((ReturnDate - RentalDate).TotalDays);
        }
    }
}