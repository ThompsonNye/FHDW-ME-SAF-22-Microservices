using Microsoft.EntityFrameworkCore;

namespace Nuyken.Vegasco.Backend.Microservices.Consumptions.Persistence;

/// <summary>
/// The DbContext when using PostgreSQL.
/// </summary>
public class PostgreSqlContext : ConsumptionContext
{
    private readonly IConfiguration _configuration;

    public PostgreSqlContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Configuring the db provider for PostgreSQL.
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connString = _configuration.GetConnectionString("Default");
        optionsBuilder.UseNpgsql(connString, b =>
        {
            b.MigrationsAssembly(GetType().Assembly.FullName);
            b.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null);
        });
    }
}