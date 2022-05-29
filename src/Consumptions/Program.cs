using Microsoft.EntityFrameworkCore;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Extensions;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapControllers();

ApplyMigrations(app);

app.Run();

internal partial class Program
{
    private static void ApplyMigrations(IHost app)
    {
        using var scope = app.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>() as DbContext;
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        
        if (dbContext is null)
        {
            throw new InvalidOperationException("The application context cannot be retrieved for migrating.");
        }
        
        if (dbContext.Database.IsInMemory())
        {
            dbContext.Database.EnsureCreated();
            logger.LogInformation("Database is in memory, ensuring it is created");
            return;
        }
        
        if (!dbContext.Database.GetPendingMigrations().Any())
        {
            logger.LogInformation("No pending migrations");
            return;
        }
        
        dbContext.Database.Migrate();
        logger.LogInformation("Database migrated");
    }
}