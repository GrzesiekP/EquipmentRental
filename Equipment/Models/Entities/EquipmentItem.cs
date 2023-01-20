// ReSharper disable ConvertToPrimaryConstructor

using Core.Domain.Models;
using Core.Models;
using Equipment.Models.ValueObjects;

namespace Equipment.Models.Entities
{
    public class EquipmentItem : Entity
    {
        public EquipmentType Type { get; }
        private List<RentalPeriod> Reservations { get; }
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
        
        public void Return()
        {
            Status = EquipmentStatus.Available;
        }

        public bool IsAvailableFor(RentalPeriod rentalPeriod) => !IsReservedFor(rentalPeriod);
        public bool IsReservedFor(RentalPeriod rentalPeriod) => Reservations.Any(r => r.IntersectsWith(rentalPeriod));
    }
}