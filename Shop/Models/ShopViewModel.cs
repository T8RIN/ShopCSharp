using Npgsql;

namespace Shop.Models;

public class ShopViewModel(IConfiguration configuration) {
    private readonly string? _connectionString = configuration.GetConnectionString("DefaultConnection");
    public List<Customer> Customers { get; } = [];
    public string CurrentSort { get; private set; } = "id";

    public async Task LoadCustomers(string sortBy = "id") {
        CurrentSort = sortBy;
        await using var conn = new NpgsqlConnection(_connectionString);
        conn.Open();
        await using var cmd = new NpgsqlCommand($"SELECT * FROM customers ORDER BY {sortBy}", conn);
        await using var reader = await cmd.ExecuteReaderAsync();
        Customers.Clear();
        while (await reader.ReadAsync()) {
            Customers.Add(
                new Customer(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4)
                )
            );
        }
    }
}

public class Customer(string id, string name, string email, string phone, string address) {
    public string Id { get; } = id;
    public string Name { get; } = name;
    public string Email { get; } = email;
    public string Phone { get; } = phone;
    public string Address { get; } = address;

    public override string ToString() {
        return "Id: " + Id + " Name: " + Name + " Email: " + Email + " Phone: " + Phone + " Address: " + Address;
    }
}