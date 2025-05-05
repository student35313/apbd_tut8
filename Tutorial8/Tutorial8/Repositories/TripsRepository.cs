using System.Data;
using Microsoft.Data.SqlClient;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Repositories;

public class TripsRepository : ITripsRepository
{
    private readonly string _connectionString;

    public TripsRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default");
    }

    public async Task<List<TripDTO>> GetTripsAsync(int clientId = -1)
    {
        var trips = new List<TripDTO>();

        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        var tripsCommand = clientId > 0 ? @"
                SELECT t.IdTrip, t.Name, t.Description, t.DateFrom, t.DateTo, t.MaxPeople
                FROM Trip t
                JOIN Client_Trip ct ON t.IdTrip = ct.IdTrip
                WHERE ct.IdClient = @IdClient" : @"
                SELECT IdTrip, Name, Description, DateFrom, DateTo, MaxPeople
                FROM Trip";

        await using var cmd = new SqlCommand(tripsCommand, conn);
        
            if (clientId > 0)
            {
                cmd.Parameters.AddWithValue("@IdClient", clientId);
            }

            await using (var reader = await cmd.ExecuteReaderAsync())
            {

                while (await reader.ReadAsync())
                {
                    trips.Add(new TripDTO
                    {
                        IdTrip = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                        DateFrom = reader.GetDateTime(3),
                        DateTo = reader.GetDateTime(4),
                        MaxPeople = reader.GetInt32(5),
                    });
                }
            }

            const string countriesCommand = @"SELECT c.Name 
                                    FROM Country_Trip ct 
                                    JOIN Country c ON c.IdCountry = ct.IdCountry    
                                    WHERE ct.IdTrip = @IdTrip";
        await using var countryCmd = new SqlCommand(countriesCommand, conn);
        countryCmd.Parameters.Add("@IdTrip", SqlDbType.Int);

        foreach (var trip in trips)
        {
            countryCmd.Parameters["@IdTrip"].Value = trip.IdTrip;
            await using var rdr = await countryCmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                trip.Countries.Add(rdr.GetString(0));
            }

            await rdr.CloseAsync();
        }

        return trips;
    }

    public async Task<bool> TripExistsAsync(int tripId)
    {
        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        const string command = @"
            SELECT 1 FROM Trip
            WHERE IdTrip = @IdTrip";

        await using var cmd = new SqlCommand(command, conn);
        cmd.Parameters.AddWithValue("@IdTrip", tripId);

        await using var reader = await cmd.ExecuteReaderAsync();
        return await reader.ReadAsync();
    }

    public async Task<bool> IsFullAsync(int tripId)
    {
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"
            SELECT 
                t.MaxPeople,
                (SELECT COUNT(*) FROM Client_Trip WHERE IdTrip = @IdTrip) AS CurrentCount
            FROM Trip t
            WHERE t.IdTrip = @IdTrip";

            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@IdTrip", tripId);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var maxPeople = reader.GetInt32(0);
                var currentCount = reader.GetInt32(1);

                return currentCount < maxPeople;
            }
        }
        return false;
    }
}