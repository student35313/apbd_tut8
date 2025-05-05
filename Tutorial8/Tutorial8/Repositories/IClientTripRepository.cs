namespace Tutorial8.Repositories;

public interface IClientTripRepository
{
    Task<bool> AssignTripAsync(int clientId, int tripId);
    Task<bool> UnassignTripAsync(int clientId, int tripId);
    Task<bool> RegistrationExistsAsync(int clientId, int tripId);
}