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

    [HttpPut("{tripId}")]
    public async Task<IActionResult> AssignTrip(int clientId, int tripId)
    {
        try
        {
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
        catch (BadHttpRequestException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Internal server error" });
        }
    }

    [HttpDelete("{tripId}")]
    public async Task<IActionResult> UnassignTrip(int clientId, int tripId)
    {
        try
        {
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