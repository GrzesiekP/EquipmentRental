// ReSharper disable ConvertToPrimaryConstructor
namespace Orders.ValueObjects
{
    public record EquipmentItem
    {
        public EquipmentItem(string equipmentTypeCode, decimal rentalPrice)
        {
            EquipmentTypeCode = equipmentTypeCode;
            RentalPrice = rentalPrice;
        }
        
        public string EquipmentTypeCode { get; }
        public decimal RentalPrice { get; }
    }
}