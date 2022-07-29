using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence;

public class EquipmentRentalDbContext : DbContext
{
    public EquipmentRentalDbContext(DbContextOptions<EquipmentRentalDbContext> options) : base(options)
    {
    }
    
    public DbSet<EquipmentType> EquipmentTypes { get; set; }
}





