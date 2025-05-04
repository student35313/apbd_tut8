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
        using (SqlConnection con = new SqlConnection(_connectionString))
        {
            await con.OpenAsync();
            using (SqlTransaction transaction = con.BeginTransaction())
            {
                try
                {
                    int registeredAt = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
                    
                    string command = @"
                    INSERT INTO Client_Trip (IdClient, IdTrip, RegisteredAt)
                    VALUES(@IdClient, @IdTrip, @RegisteredAt)";
                    using (SqlCommand cmd = new SqlCommand(command, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@IdClient", clientId);
                        cmd.Parameters.AddWithValue("@IdTrip", tripId);
                        cmd.Parameters.AddWithValue("@RegisteredAt", registeredAt);
                        
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        if (rowsAffected == 0)
                        {
                            transaction.Rollback();
                            return false;
                        }
                        transaction.Commit();
                        return true;
                    }
                    
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
    
}
    