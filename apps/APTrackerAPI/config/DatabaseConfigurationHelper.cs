namespace APTrackerAPI.config
{
    public static class DatabaseConfigurationHelper
    {
        private const string DbHostKey = "DB_HOST";
        private const string DbPortKey = "DB_PORT";
        private const string DbNameKey = "DB_NAME";
        private const string DbUserKey = "DB_USER";
        private const string DbPassKey = "DB_PASS";

        /// <summary>
        /// Builds the database options from the configuration and returns it
        /// </summary>
        /// <param name="configuration">Configuration with given database parameters</param>
        /// <returns>DatabaseOptions object with the configuration inside</returns>
        public static DatabaseOptions CreateFromConfiguration(IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);
            
            var dbOptions = new DatabaseOptions
            {
                Host = GetRequiredConfig(configuration, DbHostKey),
                Port = GetRequiredConfig(configuration, DbPortKey),
                DbName = GetRequiredConfig(configuration, DbNameKey),
                User = GetRequiredConfig(configuration, DbUserKey),
                Password = GetRequiredConfig(configuration, DbPassKey)
            };
            
            ValidateConfiguration(dbOptions);

            return dbOptions;
        }
        
        /// <summary>
        /// Returns the value of the given key inside the configuration. If nothing was found, an error gets thrown, informing the user to set it.
        /// </summary>
        /// <param name="config">The configuration in which the key lies</param>
        /// <param name="key">The key used inside the configuration file</param>
        /// <returns>Value of the key inside the configuration file</returns>
        /// <exception cref="InvalidOperationException">Nothing was found in the configuration. The user should add it.</exception>
        private static string GetRequiredConfig(IConfiguration config, string key)
        {
            return config[key]
                   ?? throw new InvalidOperationException($"The environment variable '{key}' is required but was not found in your configuration.");
        }
        
        /// <summary>
        /// Checks whether the given database configuration is valid.
        /// </summary>
        /// <param name="dbOptions">Database options to validate</param>
        /// <exception cref="ArgumentException">If the database configuration is invalid, we'll throw an exception.</exception>
        private static void ValidateConfiguration(DatabaseOptions dbOptions)
        {
            List<string> errors = [];
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

            if (!int.TryParse(dbOptions.Port, out var port) || port < 1 || port > 65535)
            {
                errors.Add("Database port must be a number between 1 and 65535.");
            }

            if (errors.Count > 0)
            {
                throw new ArgumentException("There were errors during database configuration: " + string.Join(", ", errors));
            }
        }
    }
}

