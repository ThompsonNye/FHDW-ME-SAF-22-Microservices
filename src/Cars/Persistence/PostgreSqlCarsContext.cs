using Microsoft.EntityFrameworkCore;

namespace Nuyken.Vegasco.Backend.Microservices.Cars.Persistence;

public class PostgreSqlCarsContext : CarsContext
{
    private readonly IConfiguration _configuration;

    public PostgreSqlCarsContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

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