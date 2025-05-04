using System.Data;
using Microsoft.Data.SqlClient;
using Tutorial8.Models.DTOs;
using Microsoft.Extensions.Configuration;
using Tutorial8.Repositories;

namespace Tutorial8.Services;

public class TripsService : ITripsService
{
    private readonly ITripsRepository _tripsRepository;

    public TripsService( ITripsRepository tripsRepository)
    {
        _tripsRepository = tripsRepository;
    }

    public async Task<List<TripDTO>> GetAllTrips()
    {
        return await _tripsRepository.GetTripsAsync(-1);
    }
}