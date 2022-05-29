using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Abstractions;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Persistence;

namespace Nuyken.Vegasco.Backend.Microservices.Consumptions.Extensions;

public static class DbExtensions
{
    private const string DbTypeConfigurationKey = "Database:Type";
    
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var dbType = configuration.GetValue<string>(DbTypeConfigurationKey);

        if (IsMySql(dbType))
        {
            services.AddDbContext<MySqlContext>();
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<MySqlContext>());
            return;
        }

        if (IsPostgres(dbType))
        {
            services.AddDbContext<PostgreSqlContext>();
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<PostgreSqlContext>());
            return;
        }

        services.AddDbContext<ConsumptionContext>();
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ConsumptionContext>());
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