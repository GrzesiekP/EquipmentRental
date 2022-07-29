using Persistence.Data;

namespace Persistence.Repositories;

public class EquipmentRepository : IEquipmentRepository
{
    private readonly EquipmentRentalDbContext _context;

    public EquipmentRepository(EquipmentRentalDbContext context)
    {
        _context = context;
    }
    public List<EquipmentType> GetEquipmentTypes()
    {
        return _context.EquipmentTypes.ToList();
    }
}