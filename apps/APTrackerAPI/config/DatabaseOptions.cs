namespace APTrackerAPI.config
{
    public class DatabaseOptions
    {
        public string Host { get; set; } = string.Empty;
        public string Port { get; set; } = "5432";
        public string DbName { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Builds the whole connection string together using the given database host, port, database name, user and password.
        /// </summary>
        /// <returns>Connection string that can be used to link a database to this program</returns>
        public string GetConnectionString()
        {
            return $"Host={Host};Port={Port};Database={DbName};Username={User};Password={Password};Include Error Detail=true";
        }
    }
}
