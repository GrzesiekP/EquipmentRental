// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace EquipmentRental.WebApi.Models
{
    public record EquipmentItem
    {
        public string EquipmentTypeCode { get; set; }
        public decimal RentalPrice { get; set; }
    }
}