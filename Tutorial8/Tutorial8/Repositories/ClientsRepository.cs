namespace Tutorial8.Repositories;

using Microsoft.Data.SqlClient;
using System.Data;
using Tutorial8.Models.DTOs;

public class ClientsRepository : IClientsRepository
{
    private readonly string _connectionString;

    public ClientsRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default");
    }
    
    public async Task<ClientDTO> GetClient(int clientId)
    {
        ClientDTO client = null;
        using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        const string command = @"
            SELECT IdClient, FirstName, LastName, Email, Telephone, Pesel
            FROM Client
            WHERE IdClient = @IdClient";
        using var cmd = new SqlCommand(command, conn);
        cmd.Parameters.AddWithValue("@IdClient", clientId);

        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            client = new ClientDTO
            {
                IdClient  = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName  = reader.GetString(2),
                Email     = reader.GetString(3),
                Telephone = reader.GetString(4),
                Pesel     = reader.GetString(5)
            };
        }
        return client;
    }

    public async Task<bool> HasTrips(int clientId)
    {
        using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        const string command = @"
            SELECT 1 FROM Client_Trip
            WHERE IdClient = @IdClient";
        using var cmd = new SqlCommand(command, conn);
        cmd.Parameters.AddWithValue("@IdClient", clientId);
        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync()) return true;
        return false;
    }
}