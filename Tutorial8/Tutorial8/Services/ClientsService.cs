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
        var cient = await _clientsRepository.GetClient(clientId);
        if (cient == null)
        {
           throw new NotFoundException("Client not found with this id");
        }
        if (!await _clientsRepository.HasTrips(clientId))
        {
            return new List<TripDTO>();
        }
        return await _tripsRepository.GetTrips(clientId);
            
    }
}