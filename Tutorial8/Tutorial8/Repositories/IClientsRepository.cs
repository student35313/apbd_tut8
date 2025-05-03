using Tutorial8.Models;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Repositories;

public interface IClientsRepository
{
    Task<ClientDTO> GetClient(int clientId);
    Task<bool> HasTrips(int clientId);
}