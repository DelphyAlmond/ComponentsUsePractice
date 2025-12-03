using System.Data;
using Dapper;
using Npgsql;

using Microsoft.Extensions.Configuration;

namespace OrderControl;

public class OrderDbConnection
{
    private readonly string _connectionString = "Host=127.0.0.1;Port=5472;Database=componentdb;Username=Del8a;Password=del8almond";

    // [ ! ] Constructor that loads connection string
    public OrderDbConnection()
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

        // Default fallback
        return "Host=127.0.0.1;Port=5472;Database=componentdb;Username=Del8a;Password=del8almond";
    }

    // ALL YOUR EXISTING METHODS - change to use _connectionString instead of static property
    public void AddOrder(Order order)
    {
        try
        {
            using var connection = new NpgsqlConnection(_connectionString); // CHANGED
            const string sql = @"INSERT INTO orders (fio, movementnotes, destination, receivedate)
                                 VALUES(@FIO, @MovementNotes, @Destination, @ReceiveDate)
                                 RETURNING id";
            var generatedId = connection.ExecuteScalar<Guid>(sql, order);
            order.Id = generatedId;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"[ Error ] inserting order to DB : {ex.Message}");
        }
    }

    public void UpdateOrder(Order order)
    {
        try
        {
            using var connection = new NpgsqlConnection(_connectionString); // CHANGED
            const string sql = @"UPDATE orders 
                                 SET fio = @FullName, movementnotes = @MovementNotes, 
                                     destination = @Destination, receivedate = @ReceiveDate 
                                 WHERE id = @Id::uuid";
            connection.Execute(sql, order);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"[ Error ] updating order : {ex.Message}");
        }
    }

    public void DeleteOrder(Guid id)
    {
        try
        {
            using var connection = new NpgsqlConnection(_connectionString); // CHANGED
            const string sql = "DELETE FROM orders WHERE id = @Id::uuid";
            connection.Execute(sql, new { Id = id.ToString() });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"[ Error ] deleting order from DB : {ex.Message}");
        }
    }

    // Note: GetOrder is static but needs connection string
    public static Order GetOrder(Guid id)
    {
        try
        {
            // Create temporary instance to get connection string
            var tempDb = new OrderDbConnection();
            using var connection = new NpgsqlConnection(tempDb._connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Guid);

            string query = "SELECT * FROM orders WHERE id = @Id::uuid";
            var order = connection.QueryFirstOrDefault<Order>(query, parameters);

            if (order == null)
                throw new Exception($"[ Error ] order with {id} not found");

            return order;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return null;
        }
    }

    public List<Order>? GetOrders()
    {
        try
        {
            using var connection = new NpgsqlConnection(_connectionString); // CHANGED
            string query = "SELECT * FROM orders";
            var list = connection.Query<Order>(query).ToList();
            return list;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"[ Error ] getting order list : {ex.Message}");
            return null;
        }
    }

    public List<string> GetCities()
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
            MessageBox.Show("[ Error ] Reading cities");
            return null;
        }
    }

    public Dictionary<string, List<(int Parameter, double Value)>> GetOrdersByCityAndDate()
    {
        var result = new Dictionary<string, List<(int Parameter, double Value)>>(); // CHANGE

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            var query = @"
            SELECT destination, 
                   EXTRACT(DAY FROM receivedate) as day,
                   COUNT(*) as count
            FROM orders 
            WHERE receivedate >= CURRENT_DATE - INTERVAL '30 days'
            GROUP BY destination, EXTRACT(DAY FROM receivedate)
            ORDER BY destination, day";

            var data = connection.Query<(string City, int Day, int Count)>(query);

            foreach (var item in data)
            {
                if (!result.ContainsKey(item.City))
                    result[item.City] = new List<(int Parameter, double Value)>();

                result[item.City].Add((item.Day, item.Count));
                // >> tuple names don't matter for assignment
            }
        }

        return result;
    }

    public List<OrderReportDto> GetOrdersForExcelReport()
    {
        using (var connection = new NpgsqlConnection(_connectionString)) // CHANGED
        {
            var query = @"
                SELECT id as Id, 
                       fio as CustomerName,
                       destination as City,
                       receivedate as ReceiveDate,
                       movementnotes as MovementNotes
                FROM orders 
                ORDER BY receivedate DESC";

            return connection.Query<OrderReportDto>(query).ToList();
        }
    }
}

public class OrderReportDto
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; }
    public string City { get; set; }
    public string ReceiveDate { get; set; }
    public string MovementNotes { get; set; }
}
