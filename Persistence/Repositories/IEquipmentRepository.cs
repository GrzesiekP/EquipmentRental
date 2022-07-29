using Persistence.Data;

namespace Persistence.Repositories;

public interface IEquipmentRepository
{
    List<EquipmentType> GetEquipmentTypes();
}