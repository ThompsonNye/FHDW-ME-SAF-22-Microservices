using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Eureka;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile($"ocelot.{builder.Environment}.json", optional: true, reloadOnChange: true);

builder.Services
    .AddOcelot(builder.Configuration)
    .AddEureka();

// Add services to the container.
var app = builder.Build();

// Configure the HTTP request pipeline.
await app.UseOcelot();

app.MapControllers();

app.Run();
