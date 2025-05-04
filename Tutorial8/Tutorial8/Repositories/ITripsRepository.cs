using Tutorial8.Models.DTOs;

namespace Tutorial8.Repositories;

public interface ITripsRepository
{
    Task<List<TripDTO>> GetTripsAsync(int clientId); 
    Task<bool> TripExistsAsync(int tripId);
    Task<bool> IsFullAsync(int tripId);
}