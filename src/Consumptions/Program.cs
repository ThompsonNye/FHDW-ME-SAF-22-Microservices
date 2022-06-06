using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Extensions;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Abstractions;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Configuration;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Services;
using Nuyken.Vegasco.Backend.Microservices.Shared.Kafka.Extensions;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext(builder.Configuration);

builder.Services.AddOpenApiDocument();

builder.Services.AddDiscoveryClient(builder.Configuration);
builder.Services.AddServiceDiscovery(options =>
{
    options.UseEureka();
});

builder.Services.Configure<ValidationOptions>(builder.Configuration.GetSection(nameof(ValidationOptions)));

builder.Services.AddHttpContextAccessor();
builder.Services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddControllers()
    .AddFluentValidation(config => config.LocalizationEnabled = false);

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

builder.Services.AddKafka();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseOpenApi();
app.UseSwaggerUi3();
app.UseReDoc(options => options.Path = "/redoc");

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
