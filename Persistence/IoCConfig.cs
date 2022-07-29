using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;

namespace Persistence;

public static class IoCConfig
{
    public static IServiceCollection RegisterPersistenceModule(this IServiceCollection services, PersistenceConfig config)
    {
        services.AddDbContext<EquipmentRentalDbContext>(
            opt =>
            {
                opt.UseSqlServer(
                    config.SqlServerConnectionString,
                    b => b.MigrationsAssembly("Persistence"));
            }
        );
        
        services.AddScoped<IEquipmentRepository, EquipmentRepository>();

        return services;
    }
}