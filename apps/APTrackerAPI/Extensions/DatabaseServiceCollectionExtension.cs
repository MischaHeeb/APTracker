using APTrackerAPI.config;
using APTrackerAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace APTrackerAPI.Extensions
{
    public static class DatabaseServiceCollectionExtension
    {
        public static IServiceCollection AddDatabaseConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configuration);

            var dbOptions = DatabaseConfigurationHelper.CreateFromConfiguration(configuration);

            services.AddDbContext<APTrackerDbContext>(options =>
                options.UseNpgsql(dbOptions.GetConnectionString()));

            return services;
        }
    }
}
