using Dapper;
using Npgsql;

namespace OrderControl;

public class OrderDbConnection
{
    private string connectionString => "Host=127.0.0.1;Port=5472;Database=componentDBorders;Username=Del8a;Password=del8almond";

    public void AddOrder(Order order)
    {
        try
        {
            using var connection = new NpgsqlConnection(connectionString);
            const string sql = @"INSERT INTO orders (fio, movementnotes, destination, receivedate)
                                 VALUES(@FIO, @MovementNotes, @Destination, @ReceiveDate)";
            // execute [ ? ]
            connection.Execute(sql, order);
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
            using var connection = new NpgsqlConnection(connectionString);
            const string sql = @"UPDATE orders 
                                 SET fio = @FullName, movementnotes = @MovementNotes, destination = @Destination, receivedate = @ReceiveDate 
                                 WHERE id = @Id";
            connection.Execute(sql, order);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"[ Error ] updating order : {ex.Message}");
        }
    }

    public void DeleteOrder(int id)
    {
        try
        {
            using var connection = new NpgsqlConnection(connectionString);
            const string sql = "DELETE FROM orders WHERE id = @Id";
            // > saving from SQL ijection interfere:
            // object with field for dapper to relate
            connection.Execute(sql, new { Id = id });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"[ Error ] deleting order from DB : {ex.Message}");
        }
    }

    public Order GetOrder(int id)
    {
        try
        {
            using var connection = new NpgsqlConnection(connectionString);
            string query = "SELECT * FROM orders WHERE id = @Id";
            var order = connection.QueryFirstOrDefault<Order>(query, new { Id = id });
            if (order == null)
            {
                MessageBox.Show($"[ Error ] order with {id} not found");
            }
            return order;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"[ Error ] executing order : {ex.Message}");
            return null;
        }
    }

    public List<Order> GetOrders()
    {
        try
        {
            using var connection = new NpgsqlConnection(connectionString);
            string query = "SELECT * FROM orders";
            var list = connection.Query<Order>(query).ToList();
            return list;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"[ Error ] getting order list {ex.Message}");
            return null;
        }
    }

    public List<string> GetCities()
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
            MessageBox.Show("[ Error ] Reading cities");
            return null;
        }
    }
}
