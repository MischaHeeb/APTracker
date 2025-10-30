using APTrackerAPI.config;
using APTrackerAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace APTrackerAPI.Extensions
{
    public static class DatabaseServiceCollectionExtension
    {
        private const string DbHostKey = "DB_HOST";
        private const string DbPortKey = "DB_PORT";
        private const string DbNameKey = "DB_NAME";
        private const string DbUserKey = "DB_USER";
        private const string DbPassKey = "DB_PASS";
        public static IServiceCollection AddDatabaseConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configuration);

            DatabaseOptions dbOptions = new DatabaseOptions
            {
                Host = GetRequiredConfig(configuration, DbHostKey),
                Port = GetRequiredConfig(configuration, DbPortKey),
                DbName = GetRequiredConfig(configuration, DbNameKey),
                User = GetRequiredConfig(configuration, DbUserKey),
                Password = GetRequiredConfig(configuration, DbPassKey)
            };

            ValidateConfiguration(dbOptions);

            services.AddDbContext<APTrackerDbContext>(options =>
                options.UseNpgsql(dbOptions.GetConnectionString()));

            return services;
        }

        /// <summary>
        /// Returns the value of the given key inside the configuration. If nothing was found, an error gets thrown, informing the user to set it.
        /// </summary>
        /// <param name="key">The key used inside the configuration file</param>
        /// <returns>Value of the key inside the configuration file</returns>
        /// <exception cref="InvalidOperationException">Nothing was found in the configuration. The user should add it.</exception>
        private static string GetRequiredConfig(IConfiguration config, string key)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            return config[key]
                ?? throw new InvalidOperationException($"The environment variable '{key}' is required but was not found in your configuration.");
        }

        /// <summary>
        /// Checks wether the given database configuration is valid.
        /// </summary>
        /// <param name="dbOptions"></param>
        /// <exception cref="ArgumentException">If the database configuration is invalid, we'll throw an exception.</exception>
        private static void ValidateConfiguration(DatabaseOptions dbOptions)
        {
            List<string> errors = new List<string>();
            if (string.IsNullOrWhiteSpace(dbOptions.Host))
            {
                errors.Add("Database host must be set in the configuration.");
            }

            if (string.IsNullOrWhiteSpace(dbOptions.DbName))
            {
                errors.Add("Database name must be set in the configuration.");
            }

            if (string.IsNullOrWhiteSpace(dbOptions.User))
            {
                errors.Add("Database user must be set in the configuration.");
            }

            if (!int.TryParse(dbOptions.Port, out int port) || port < 1 || port > 65535)
            {
               errors.Add("Database port must be a number between 1 and 65535.");
            }

            if (errors.Any())
            {
                throw new ArgumentException("There were errors during database configuration: " + string.Join(", ", errors));
            }
        }
    }
}
