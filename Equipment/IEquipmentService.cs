namespace Equipemnts;

public interface IEquipmentService
{
    bool IsReservedFor(RentalPeriod rentalPeriod);
    void ReserveFor(RentalPeriod rentalPeriod);
}