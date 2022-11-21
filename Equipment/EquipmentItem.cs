// ReSharper disable ConvertToPrimaryConstructor

using Core.Domain.Models;
using Equipemnts;

namespace Equipment
{
    public class EquipmentItem : Entity
    {
        public EquipmentType Type { get; }
        public List<RentalPeriod> Reservations { get; private set; }
        public EquipmentStatus Status { get; private set; }

        public EquipmentItem(EquipmentType type)
        {
            Type = type;
            Reservations = new List<RentalPeriod>();
        }

        public void ReserveFor(RentalPeriod rentalPeriod)
        {
            Reservations.Add(rentalPeriod);
        }

        public void Rent()
        {
            Status = EquipmentStatus.Rent;
        }
        
        public void Release()
        {
            Status = EquipmentStatus.Available;
        }

        public bool IsAvailableFor(RentalPeriod rentalPeriod) => !IsReservedFor(rentalPeriod);
        public bool IsReservedFor(RentalPeriod rentalPeriod) => Reservations.Any(r => r.IntersectsWith(rentalPeriod));
    }
}