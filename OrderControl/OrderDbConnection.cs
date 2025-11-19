using System.Data;
using Dapper;
using Npgsql;

namespace OrderControl;

public class OrderDbConnection
{
    private static string connectionString => "Host=127.0.0.1;Port=5472;Database=componentdb;Username=Del8a;Password=del8almond";

    public void AddOrder(Order order)
    {
        try
        {
            using var connection = new NpgsqlConnection(connectionString);
            const string sql = @"INSERT INTO orders (fio, movementnotes, destination, receivedate)
                                 VALUES(@FIO, @MovementNotes, @Destination, @ReceiveDate)
                                 RETURNING id";
            // execute [ ? ]
            // > and get the generated ID back
            var generatedId = connection.ExecuteScalar<Guid>(sql, order);

            // Update the order object with the database-generated ID
            order.Id = generatedId;

            // connection.Execute(sql, order);
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
            using var connection = new NpgsqlConnection(connectionString);
            const string sql = "DELETE FROM orders WHERE id = @Id::uuid";
            // > saving from SQL ijection interfere:
            // object with field for dapper to relate
            connection.Execute(sql, new { Id = id.ToString() });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"[ Error ] deleting order from DB : {ex.Message}");
        }
    }

    public static Order GetOrder(Guid id)
    {
        try
        {
            using var connection = new NpgsqlConnection(connectionString);

            /*
            // ✅ explicit UUID casting in SQL
            string query = "SELECT * FROM orders WHERE id = @Id::uuid";
            var order = connection.QueryFirstOrDefault<Order>(query, new { Id = id });
             */

            // > Ensure Npgsql knows this is a GUID/UUID
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Guid);

            string query = "SELECT * FROM orders WHERE id = @Id::uuid";
            var order = connection.QueryFirstOrDefault<Order>(query, parameters);

            if (order == null)
            {
                throw new Exception($"[ Error ] order with {id} not found");
            }
            return order;

        } catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return null;
        }
    }

    public List<Order>? GetOrders()
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
            MessageBox.Show($"[ Error ] getting order list : {ex.Message}");
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
        catch
        {
            MessageBox.Show("[ Error ] Reading cities");
            return null;
        }
    }
}
