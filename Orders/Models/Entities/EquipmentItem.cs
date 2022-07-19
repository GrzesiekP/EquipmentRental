// ReSharper disable ConvertToPrimaryConstructor

using Core.Domain.Models;
using Orders.Models.ValueObjects;

namespace Orders.Models.Entities
{
    public class EquipmentItem : Entity
    {
        public EquipmentType Type { get; }

        public EquipmentItem(EquipmentType type)
        {
            Type = type;
        }
    }
}