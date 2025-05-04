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
    
    public async Task<ClientDTO> GetClientAsync(int clientId)
    {
        ClientDTO client = null;

        using (var conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();

            const string command = @"
            SELECT IdClient, FirstName, LastName, Email, Telephone, Pesel
            FROM Client
            WHERE IdClient = @IdClient";

            using (var cmd = new SqlCommand(command, conn))
            {
                cmd.Parameters.AddWithValue("@IdClient", clientId);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        client = new ClientDTO
                        {
                            IdClient  = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName  = reader.GetString(2),
                            Email     = reader.GetString(3),
                            Telephone = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Pesel     = reader.IsDBNull(5) ? null : reader.GetString(5)
                        };
                    }
                }
            }
        }

        return client;
    }

    public async Task<bool> HasTripsAsync(int clientId)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();

            const string command = @"
            SELECT 1 FROM Client_Trip
            WHERE IdClient = @IdClient";

            using (var cmd = new SqlCommand(command, conn))
            {
                cmd.Parameters.AddWithValue("@IdClient", clientId);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public async Task<int> CreateClientAsync(ClientCreationDTO client)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            using (SqlTransaction transaction = conn.BeginTransaction())
            {
                try
                {
                    string command = @"
                    INSERT INTO Client (FirstName, LastName, Email, Telephone, Pesel)
                    VALUES (@FirstName, @LastName, @Email, @Telephone, @Pesel);
                    SELECT SCOPE_IDENTITY();";
                    using (var cmd = new SqlCommand(command, conn, transaction))
                    {
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
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }

    public async Task<bool> ClientExistsAsync(int clientId)
    {
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                const string command = @"
            SELECT 1 FROM Client
            WHERE IdClient = @IdClient";

                using (var cmd = new SqlCommand(command, conn))
                {
                    cmd.Parameters.AddWithValue("@IdClient", clientId);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}