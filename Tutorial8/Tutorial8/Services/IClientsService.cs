using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public interface IClientsService
{
    Task<List<TripDTO>> GetClientTrips(int clientId);
    Task<int> AddClient(ClientCreationDTO client);
}