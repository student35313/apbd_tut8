using Tutorial8.Exceptions;
using Tutorial8.Repositories;

namespace Tutorial8.Services;

public class ClientTripService : IClientTripService
{
    private readonly ITripsRepository _tripsRepository;
    private readonly IClientsRepository _clientsRepository;
    private readonly IClientTripRepository _clientTripRepository;

    public ClientTripService(ITripsRepository tripsRepository,
        IClientsRepository clientsRepository, IClientTripRepository clientTripRepository)
    {
        _tripsRepository = tripsRepository;
        _clientsRepository = clientsRepository;
        _clientTripRepository = clientTripRepository;
    }

    public async Task<bool> AssignTrip(int clientId, int tripId)
    {
        if (! await _clientsRepository.ClientExistsAsync(clientId))
            throw new NotFoundException("Client not found with this id");
        
        if (! await _tripsRepository.TripExistsAsync(tripId))
            throw new NotFoundException("Trip not found with this id");

        if (await _clientTripRepository.RegistrationExistsAsync(clientId, tripId))
            throw new ConflictException("Client already registered for this trip");
        
        if (!await _tripsRepository.IsFullAsync(tripId))
            throw new ConflictException("Trip is full");
        return await _clientTripRepository.AssignTripAsync(clientId, tripId);
    }
    
    public async Task<bool> UnassignTrip(int clientId, int tripId)
    {
        if (!await _clientsRepository.ClientExistsAsync(clientId))
            throw new NotFoundException("Client not found with this id");
        
        if (!await _tripsRepository.TripExistsAsync(tripId))
            throw new NotFoundException("Trip not found with this id");
        
        if (!await _clientTripRepository.RegistrationExistsAsync(clientId, tripId))
            throw new NotFoundException("Registration not found");
        
        return await _clientTripRepository.UnassignTripAsync(clientId, tripId);
    }
    
}