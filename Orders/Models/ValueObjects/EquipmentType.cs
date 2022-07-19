namespace Orders.Models.ValueObjects
{
    public record EquipmentType
    {
        public EquipmentType(string equipmentTypeCode, decimal rentalPrice)
        {
            EquipmentTypeCode = equipmentTypeCode;
            RentalPrice = rentalPrice;
        }
        
        public string EquipmentTypeCode { get; }
        public decimal RentalPrice { get; }
    }
}