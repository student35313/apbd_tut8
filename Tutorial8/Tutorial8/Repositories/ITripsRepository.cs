using Tutorial8.Models.DTOs;

namespace Tutorial8.Repositories;

public interface ITripsRepository
{
    Task<List<TripDTO>> GetTrips(int clientId); 
}