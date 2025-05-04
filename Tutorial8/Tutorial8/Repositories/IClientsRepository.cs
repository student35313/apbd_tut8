using Tutorial8.Models;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Repositories;

public interface IClientsRepository
{
    Task<ClientDTO> GetClientAsync(int clientId);
    Task<bool> HasTripsAsync(int clientId);
    Task<int> CreateClientAsync(ClientCreationDTO client);
    Task<bool> ClientExistsAsync(int clientId);
}