using APTrackerAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

// Define default values for DB config
const string defaultDBHost = "db";
const string defaultDBName = "aptracker";
const string defaultDBUser = "postgres";
const string defaultDBPort = "5432";

// Read DB data from config
string dbHost = configuration["DB_HOST"] ?? defaultDBHost;
string dbName = configuration["DB_NAME"] ?? defaultDBName;
string dbUser = configuration["DB_USER"] ?? defaultDBUser;
string dbPass = configuration["DB_PASS"] ?? throw new InvalidOperationException("The environement variable \"DB_PASS\" must be set!");
string dbPort = configuration["DB_PORT"] ?? defaultDBPort;

string connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPass};Include Error Detail=true";

builder.Services.AddDbContext<APTrackerDbContext>(options =>
    options.UseNpgsql(connectionString)
);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
