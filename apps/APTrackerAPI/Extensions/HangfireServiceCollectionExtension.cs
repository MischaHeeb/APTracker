using APTrackerAPI.config;
using Hangfire;
using Hangfire.PostgreSql;

namespace APTrackerAPI.Extensions
{
    public static class HangfireServiceCollectionExtension
    {
        public static IServiceCollection AddHangfireConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configuration);

            var dbOptions = DatabaseConfigurationHelper.CreateFromConfiguration(configuration);
            
            services.AddHangfire(config =>
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UsePostgreSqlStorage(options =>
                        options.UseNpgsqlConnection(dbOptions.GetConnectionString()))
                );
            
            services.AddHangfireServer();
            
            return services;
        }
    }
}

