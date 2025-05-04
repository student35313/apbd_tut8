namespace Tutorial8.Services;

public interface IClientTripService
{
    Task<bool> AssignTrip(int clientId, int tripId);
}