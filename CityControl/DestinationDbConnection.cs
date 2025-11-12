using Dapper;
using Npgsql;

namespace CityControl;

public class DestinationDbConnection
{
    private string connectionString => "Host=127.0.0.1;Port=5472;Database=componentdb;Username=Del8a;Password=del8almond";

    public List<string> ReadCities()
    {
        try
        {
            using var connection = new NpgsqlConnection(connectionString);
            var query = "SELECT name FROM cities";
            var list = connection.Query<string>(query).ToList();
            return list;
        }
        catch (Exception ex)
        {
            MessageBox.Show("[ Error ] Ошибка чтения городов");
            return null;
        }
    }
}

