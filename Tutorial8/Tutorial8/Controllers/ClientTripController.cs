using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tutorial8.Exceptions;
using Tutorial8.Services;

namespace Tutorial8.Controllers;

[Route("api/clients/{clientId}/trips")]
[ApiController]
public class ClientTripController : ControllerBase
{
    private readonly IClientTripService _clientTripService;

    public ClientTripController(IClientTripService clientTripService)
    {
        _clientTripService = clientTripService;
    }

    // Assigns a client to a trip in a Client_Trip table
    [HttpPut("{tripId}")]
    public async Task<IActionResult> AssignTrip(int clientId, int tripId)
    {
        try
        {
            if (clientId <= 0 || tripId <= 0)
                return BadRequest(new { error = "Ids must be positive." });
            var success = await _clientTripService.AssignTrip(clientId, tripId);
            if (!success)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { error = "Could not assign client to trip" });

            return Ok();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (ConflictException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Internal server error" });
        }
    }
    
    // Unassigns a client from a trip in a Client_Trip table(delete a record)
    [HttpDelete("{tripId}")]
    public async Task<IActionResult> UnassignTrip(int clientId, int tripId)
    {
        try
        {
            if (clientId <= 0 || tripId <= 0)
                return BadRequest(new { error = "Ids must be positive." });
            var success = await _clientTripService.UnassignTrip(clientId, tripId);
            if (!success)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { error = "Could not unassign client from trip" });
            return Ok();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error" });
        }
    }
    
    
}