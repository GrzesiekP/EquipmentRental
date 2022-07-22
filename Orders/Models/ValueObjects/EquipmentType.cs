namespace Orders.Models.ValueObjects
{
    public record EquipmentType
    {
        public EquipmentType(string equipmentTypeCode, Money rentalPrice)
        {
            EquipmentTypeCode = equipmentTypeCode;
            RentalPrice = rentalPrice;
        }
        
        public string EquipmentTypeCode { get; }
        public Money RentalPrice { get; }
    }
}