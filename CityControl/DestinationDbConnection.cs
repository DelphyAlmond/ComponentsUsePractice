using System.Data;
using Dapper;
using Npgsql;

using Microsoft.Extensions.Configuration;

namespace CityControl;

public class DestinationDbConnection
{
    private readonly string _connectionString = "Host=127.0.0.1;Port=5472;Database=componentdb;Username=Del8a;Password=del8almond";

    public DestinationDbConnection()
    {
        _connectionString = LoadConnectionString();
    }

    private string LoadConnectionString()
    {
        try
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            var connString = config.GetConnectionString("DefaultConnection");

            if (!string.IsNullOrEmpty(connString))
                return connString;
        }
        catch
        {
            // Continue to default
        }

        return "Host=127.0.0.1;Port=5472;Database=componentdb;Username=Del8a;Password=del8almond";
    }

    public List<string> ReadCities()
    {
        try
        {
            using var connection = new NpgsqlConnection(_connectionString); // CHANGED
            var query = "SELECT name FROM cities";
            var list = connection.Query<string>(query).ToList();
            return list;
        }
        catch
        {
            MessageBox.Show("[ Error ] Ошибка чтения городов");
            return null;
        }
    }
}

