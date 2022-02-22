using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EquipmentRental.WebApi
{
    public class EquipmentRentalDbContext : IdentityDbContext<User>
    {
        public EquipmentRentalDbContext(DbContextOptions<EquipmentRentalDbContext> options) : base(options){}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}