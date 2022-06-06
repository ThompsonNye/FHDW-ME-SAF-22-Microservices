using Nuyken.Vegasco.Backend.Microservices.Cars.Models.Abstractions;
using Nuyken.Vegasco.Backend.Microservices.Cars.Persistence;

namespace Nuyken.Vegasco.Backend.Microservices.Cars.Extensions;

public static class DbExtensions
{
    private const string DbTypeConfigurationKey = "Database:Type";
    
    /// <summary>
    /// Adds a database context to the services depending on the configured database type with key <see cref="DbTypeConfigurationKey"/>.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var dbType = configuration.GetValue<string>(DbTypeConfigurationKey);

        if (IsMySql(dbType))
        {
            services.AddDbContext<MySqlCarsContext>();
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<MySqlCarsContext>());
            return;
        }

        if (IsPostgres(dbType))
        {
            services.AddDbContext<PostgreSqlCarsContext>();
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<PostgreSqlCarsContext>());
            return;
        }

        services.AddDbContext<CarsContext>();
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<CarsContext>());
    }
    
    private static bool IsMySql(string? dbType)
    {
        return dbType is not null
               && (dbType.Equals("mariadb", StringComparison.OrdinalIgnoreCase) 
                   || dbType.Equals("mysql", StringComparison.OrdinalIgnoreCase));
    }
    
    private static bool IsPostgres(string? dbType)
    {
        return dbType is not null
               && (dbType.Equals("postgres", StringComparison.OrdinalIgnoreCase) 
                   || dbType.Equals("postgresql", StringComparison.OrdinalIgnoreCase));
    }
}