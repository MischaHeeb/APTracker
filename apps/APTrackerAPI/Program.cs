using APTrackerAPI.Data;
using APTrackerAPI.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Build database configuration
builder.Services.AddDatabaseConfiguration(builder.Configuration);

// Add health checks
builder.Services.AddHealthChecks();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Initialize Database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("Initializing database");
        var context = services.GetRequiredService<APTrackerDbContext>();

        context.Database.Migrate();

        logger.LogInformation("Database successfully initialized!");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Database could not be initialized!");
        throw;
    }
}

// Map health check status to URI
app.MapHealthChecks("health");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
