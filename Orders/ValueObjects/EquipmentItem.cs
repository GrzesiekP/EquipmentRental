namespace Orders.ValueObjects
{
    public record EquipmentItem
    {
        public string EquipmentTypeCode { get; set; }
        public decimal RentalPrice { get; set; }
    }
}