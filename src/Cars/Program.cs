var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddOpenApiDocument();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseReDoc(o => o.Path = "/redoc");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();