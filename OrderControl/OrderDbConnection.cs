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

    public Dictionary<string, List<(string Date, double Value)>> GetOrdersByCityAndDateWithString()
    {
        var result = new Dictionary<string, List<(string Date, double Value)>>();

        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                // VARCHAR 'YYYY-MM-DD', нужно преобразовать в DATE
                var query = @"
                    SELECT 
                        destination as City,
                        receivedate as OrderDate,  -- Уже строка в формате 'YYYY-MM-DD'
                        COUNT(*) as OrderCount
                    FROM orders 
                    WHERE destination IS NOT NULL 
                      AND receivedate IS NOT NULL
                      AND receivedate ~ '^\d{4}-\d{2}-\d{2}$' -- Проверяем формат даты
                    GROUP BY destination, receivedate
                    ORDER BY destination, receivedate";

                var data = connection.Query<(string City, string OrderDate, int OrderCount)>(query);

                foreach (var item in data)
                {
                    if (string.IsNullOrEmpty(item.City))
                        continue;

                    if (!result.ContainsKey(item.City))
                        result[item.City] = new List<(string Date, double Value)>();

                    // Преобразуем 'YYYY-MM-DD' в 'DD.MM.YYYY'
                    if (DateTime.TryParse(item.OrderDate, out DateTime date))
                    {
                        var formattedDate = date.ToString("dd.MM.yyyy");
                        result[item.City].Add((formattedDate, item.OrderCount));
                    }
                    else
                    {
                        // Если не удалось распарсить, оставляем как есть
                        result[item.City].Add((item.OrderDate, item.OrderCount));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
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
