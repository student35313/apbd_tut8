using Tutorial8.Exceptions;
using Tutorial8.Models.DTOs;
using Tutorial8.Repositories;

namespace Tutorial8.Services;

public class ClientsService : IClientsService
{
    private readonly ITripsRepository _tripsRepository;
    private readonly IClientsRepository _clientsRepository;

    public ClientsService(ITripsRepository tripsRepository, IClientsRepository clientsRepository)
    {
        _tripsRepository = tripsRepository;
        _clientsRepository = clientsRepository;
    }

    public async Task<List<TripDTO>> GetClientTrips(int clientId)
    { 
        if (! await _clientsRepository.ClientExistsAsync(clientId))
        {
           throw new NotFoundException("Client not found with this id");
        }
        if (! await _clientsRepository.HasTripsAsync(clientId))
        {
            return new List<TripDTO>();
        }
        return await _tripsRepository.GetTripsAsync(clientId);
            
    }

    public async Task<int> AddClient(ClientCreationDTO client)
    {
        
        return  await _clientsRepository.CreateClientAsync(client);
        
    }
}