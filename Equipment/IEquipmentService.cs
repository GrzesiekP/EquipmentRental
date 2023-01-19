using Core.Models;

namespace Equipment;

public interface IEquipmentService
{
    bool IsReservedFor(RentalPeriod rentalPeriod);
    void ReserveFor(RentalPeriod rentalPeriod);
}