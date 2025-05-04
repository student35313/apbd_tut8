namespace Tutorial8.Repositories;

public interface IClientTripRepository
{
    Task<bool> AssignTripAsync(int clientId, int tripId);
}