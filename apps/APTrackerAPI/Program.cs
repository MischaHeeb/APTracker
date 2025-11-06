using APTrackerAPI.Data;
using APTrackerAPI.Extensions;
using APTrackerAPI.Filters;
using Hangfire;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Build together Database config
builder.Services.AddDatabaseConfiguration(builder.Configuration);

// Setup Hangfire
builder.Services.AddHangfireConfiguration(builder.Configuration);

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


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = [new HangfireAuthFilter(app.Environment.IsDevelopment())]
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
