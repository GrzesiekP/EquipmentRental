using Core.Models;

namespace Equipment.Models.ValueObjects
{
    public record EquipmentType(string EquipmentTypeCode, Money RentalPrice);
}