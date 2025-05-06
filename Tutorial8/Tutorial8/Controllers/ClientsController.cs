using Microsoft.AspNetCore.Mvc;
using Tutorial8.Exceptions;
using Tutorial8.Models.DTOs;
using Tutorial8.Services;

namespace Tutorial8.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly IClientsService _clientsService;

    public ClientsController(IClientsService clientsService)
    {
        _clientsService = clientsService;
    }

    // Returns a list of trips of the client with the given id
    [HttpGet("{id}/trips")]
    public async Task<IActionResult> GetClientTrips(int id)
    {
        try
        {
            if (id <= 0)
                return BadRequest(new { error = "Ids must be positive." });
            var trips = await _clientsService.GetClientTrips(id);
            return Ok(trips);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { error = "Internal server error" });
        }
    }

    // Adds a new client to the database from the special DTO
    [HttpPost]
    public async Task<IActionResult> AddClient(ClientCreationDTO client)
    {
        try
        {
            var created = await _clientsService.AddClient(client);
            return Created("", created);
        }
        catch (FormatException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { error = "Internal server error" });
        }
    }
    
}