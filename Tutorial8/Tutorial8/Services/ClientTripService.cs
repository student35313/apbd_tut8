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
        {
            throw new NotFoundException("Client not found with this id");
        }
        if (! await _tripsRepository.TripExistsAsync(tripId))
        {
            throw new NotFoundException("Trip not found with this id");
        }

        if (!await _tripsRepository.IsFullAsync(tripId))
        {
            throw new BadHttpRequestException("Trip is full");
        }
        return await _clientTripRepository.AssignTripAsync(clientId, tripId);
    }
}