using Microsoft.Data.SqlClient;

namespace Tutorial8.Repositories;

public class ClientTripRepository : IClientTripRepository
{
    private readonly string _connectionString;

    public ClientTripRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default");
    }

    public async Task<bool> AssignTripAsync(int clientId, int tripId)
    {
        await using var con = new SqlConnection(_connectionString);
        await con.OpenAsync();
        await using var transaction = con.BeginTransaction();
        try
        {
            var registeredAt = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
                    
            const string command = @"
                    INSERT INTO Client_Trip (IdClient, IdTrip, RegisteredAt)
                    VALUES(@IdClient, @IdTrip, @RegisteredAt)";
            await using SqlCommand cmd = new SqlCommand(command, con, transaction);
            cmd.Parameters.AddWithValue("@IdClient", clientId);
            cmd.Parameters.AddWithValue("@IdTrip", tripId);
            cmd.Parameters.AddWithValue("@RegisteredAt", registeredAt);
                        
            var rowsAffected = await cmd.ExecuteNonQueryAsync();
            if (rowsAffected == 0)
            {
                transaction.Rollback();
                return false;
            }
            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
    
    public async Task<bool> UnassignTripAsync(int clientId, int tripId)
    {
        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();
        await using var transaction = conn.BeginTransaction();
        try
        {
            const string sql = @"
                    DELETE FROM Client_Trip
                    WHERE IdClient = @IdClient
                      AND IdTrip   = @IdTrip";

            await using var cmd = new SqlCommand(sql, conn, transaction);
            cmd.Parameters.AddWithValue("@IdClient", clientId);
            cmd.Parameters.AddWithValue("@IdTrip",   tripId);

            var rowsAffected = await cmd.ExecuteNonQueryAsync();
            if (rowsAffected == 0)
            {
                transaction.Rollback();
                return false;
            }

            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<bool> RegistrationExistsAsync(int clientId, int tripId)
    {
        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        const string sql = @"
            SELECT 1
              FROM Client_Trip
             WHERE IdClient = @IdClient
               AND IdTrip   = @IdTrip";

        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@IdClient", clientId);
        cmd.Parameters.AddWithValue("@IdTrip",   tripId);

        await using var reader = await cmd.ExecuteReaderAsync();
        return await reader.ReadAsync();
    }
}
    