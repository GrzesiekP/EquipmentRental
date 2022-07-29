using Microsoft.Extensions.Configuration;

namespace Persistence;

public class PersistenceConfig
{
    public string SqlServerConnectionString { get;}
    
    public PersistenceConfig(IConfiguration configuration)
    {
        SqlServerConnectionString = configuration["SqlServerConnectionString"];
    }
}