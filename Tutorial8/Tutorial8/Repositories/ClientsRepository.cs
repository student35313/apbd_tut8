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
    
    // Returns a client with the given id
    public async Task<ClientDTO> GetClientAsync(int clientId)
    {
        ClientDTO client = null;

        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();
        
        const string command = @"
            SELECT *
            FROM Client
            WHERE IdClient = @IdClient";

        await using var cmd = new SqlCommand(command, conn);
        cmd.Parameters.AddWithValue("@IdClient", clientId);

        await using (var reader = await cmd.ExecuteReaderAsync()){
            if (await reader.ReadAsync())
            {
                client = new ClientDTO
                {
                    IdClient = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Email = reader.GetString(3),
                    Telephone = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Pesel = reader.IsDBNull(5) ? null : reader.GetString(5)
                };
            }
        }
        return client;
    }
    
    // Checks if the client has any trips assigned
    public async Task<bool> HasTripsAsync(int clientId)
    {
        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        const string command = @"
            SELECT 1 FROM Client_Trip
            WHERE IdClient = @IdClient";

        await using var cmd = new SqlCommand(command, conn);
        cmd.Parameters.AddWithValue("@IdClient", clientId);

        await using var reader = await cmd.ExecuteReaderAsync();
        return await reader.ReadAsync();
    }

    // Creates a new client in the database
    public async Task<int> CreateClientAsync(ClientCreationDTO client)
    {
        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();
        await using var transaction = conn.BeginTransaction();
        try
        {
            const string command = @"
                    INSERT INTO Client (FirstName, LastName, Email, Telephone, Pesel)
                    VALUES (@FirstName, @LastName, @Email, @Telephone, @Pesel);
                    SELECT SCOPE_IDENTITY();";
            await using var cmd = new SqlCommand(command, conn, transaction);
            cmd.Parameters.AddWithValue("@FirstName", client.FirstName);
            cmd.Parameters.AddWithValue("@LastName", client.LastName);
            cmd.Parameters.AddWithValue("@Email", client.Email);
            cmd.Parameters.AddWithValue("@Telephone",
                client.Telephone == null ? DBNull.Value : client.Telephone);
            cmd.Parameters.AddWithValue("@Pesel", client.Pesel == null ? DBNull.Value : client.Pesel);

            var result = await cmd.ExecuteScalarAsync();
            var newId = Convert.ToInt32(result);

            transaction.Commit();
            return newId;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }

    // Checks if the client with the given id exists in the database
    public async Task<bool> ClientExistsAsync(int clientId)
    {
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            const string command = @"
            SELECT 1 FROM Client
            WHERE IdClient = @IdClient";

            await using var cmd = new SqlCommand(command, conn);
            cmd.Parameters.AddWithValue("@IdClient", clientId);

            await using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync();
        }
    }
}