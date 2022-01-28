using System.Collections.Generic;

namespace Orders.ValueObjects
{
    public class OrderData
    {
        public List<EquipmentItem> Equipment { get; set; }
        
        // TODO:
        // public DateTime RentalDate { get; set; }
        // public DateTime ReturnDate { get; set; }
        // public Guid RentingUserId { get; set; }
        // public decimal CalculateTotalPrice()
    }
}