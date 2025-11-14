using APTrackerAPI.config;
using APTrackerAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace APTrackerAPI.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring database services in the dependency injection container.
    /// </summary>
    public static class DatabaseServiceCollectionExtension
    {
        /// <summary>
        /// Adds the database context and configuration to the service collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="configuration">The application configuration containing database connection settings.</param>
        /// <returns>The <see cref="IServiceCollection"/> for method chaining.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="services"/> or <paramref name="configuration"/> is null.
        /// </exception>
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
