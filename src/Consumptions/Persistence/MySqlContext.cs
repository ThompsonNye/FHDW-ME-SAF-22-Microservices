﻿using Microsoft.EntityFrameworkCore;

namespace Nuyken.Vegasco.Backend.Microservices.Consumptions.Persistence;

/// <summary>
/// The DbContext when using MySql / MariaDb.
/// </summary>
public class MySqlContext : ConsumptionContext
{
    private readonly IConfiguration _configuration;

    public MySqlContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    /// <summary>
    /// Configuring the db provider for MySql / MariaDb.
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