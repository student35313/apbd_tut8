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

    public async Task<List<TripDTO>> GetTrips(int clientId = -1)
    {
        var trips = new List<TripDTO>();

        using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();
        
        string tripsCommand;
        if (clientId > 0)
        {
            tripsCommand = @"
                SELECT t.IdTrip, t.Name, t.Description, t.DateFrom, t.DateTo, t.MaxPeople
                FROM Trip t
                JOIN Client_Trip ct ON t.IdTrip = ct.IdTrip
                WHERE ct.IdClient = @IdClient";
        }
        else
        {
            tripsCommand = @"
                SELECT IdTrip, Name, Description, DateFrom, DateTo, MaxPeople
                FROM Trip";
        }
        using (var cmd = new SqlCommand(tripsCommand, conn))
        {
            if (clientId > 0)
            {
                cmd.Parameters.AddWithValue("@IdClient", clientId);
            }
        using (var reader = await cmd.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                trips.Add(new TripDTO
                {
                    IdTrip      = reader.GetInt32(0),
                    Name        = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                    DateFrom    = reader.GetDateTime(3),
                    DateTo      = reader.GetDateTime(4),
                    MaxPeople   = reader.GetInt32(5),
                });
            }
        }
        }
        
        string countriesCommand = @"SELECT c.Name 
                                    FROM Country_Trip ct 
                                    JOIN Country c ON c.IdCountry = ct.IdCountry    
                                    WHERE ct.IdTrip = @IdTrip";
        using var countryCmd = new SqlCommand(countriesCommand, conn);
        countryCmd.Parameters.Add("@IdTrip", SqlDbType.Int);

        foreach (var trip in trips)
        {
            countryCmd.Parameters["@IdTrip"].Value = trip.IdTrip;
            using var rdr = await countryCmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                trip.Countries.Add(rdr.GetString(0));
            }
            await rdr.CloseAsync();
        }

        return trips;
    }
}