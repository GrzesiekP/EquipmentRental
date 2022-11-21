using Equipment.Models.ValueObjects;

namespace Equipment;

public interface IEquipmentService
{
    bool IsReservedFor(RentalPeriod rentalPeriod);
    void ReserveFor(RentalPeriod rentalPeriod);
}