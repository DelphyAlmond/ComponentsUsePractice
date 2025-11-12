using Dapper;
using Npgsql;
using OrderControl;

namespace ReportsControl;

public class ReportDbConnection
{
    private string connectionString => "Host=127.0.0.8;Port=5472;Database=postgres;Username=Del8a;Password=del8almond";

    public List<Order> GetEmployeesByPosition(string position)
    {
        try
        {
            using var connection = new NpgsqlConnection(connectionString);
            string query = "SELECT * FROM orders WHERE city=@Destination";
            var list = connection.Query<Order>(query, new { Position = position }).ToList();
            return list;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"[ Error ] Ошибка извлечения заказов: {ex.Message}");
            return null;
        }
    }

    public List<string> GetDestination()
    {
        try
        {
            using var connection = new NpgsqlConnection(connectionString);
            var query = "SELECT city FROM cities";
            var list = connection.Query<string>(query).ToList();
            return list;
        }
        catch (Exception ex)
        {
            MessageBox.Show("[ Error ] Ошибка чтения города");
            return null;
        }
    }
}
