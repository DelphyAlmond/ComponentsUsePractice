namespace FormComponentDisplay;

using Microsoft.Extensions.Configuration;

public static class DbConnectionString
{
    private static string _connectionString;

    static DbConnectionString()
    {
        LoadConnectionString();
    }

    private static void LoadConnectionString()
    {
        try
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            _connectionString = config.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(_connectionString))
            {
                // Fallback to default
                _connectionString = "Host=127.0.0.1;Port=5472;Database=componentdb;Username=Del8a;Password=del8almond";
                Console.WriteLine("Using default connection string");
            }
        }
        catch
        {
            // Fallback to default
            _connectionString = "Host=127.0.0.1;Port=5472;Database=componentdb;Username=Del8a;Password=del8almond";
            Console.WriteLine("Using default connection string (config error)");
        }
    }

    public static string Get() => _connectionString;
}
