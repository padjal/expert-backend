using System.Net;
using ExpertAdministration.Core.Models;
using ExpertAdministration.Server.Exceptions;
using ExpertAdministration.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExpertAdministration.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IDatabaseService _databaseService;

    private readonly ILogger _logger;

    public UsersController(ILogger<OffersController> logger,
        IDatabaseService databaseService)
    {
        _logger = logger;
        _databaseService = databaseService;
    }

    [Route("{id}")]
    [HttpGet]
    public async Task<ActionResult<User>> GetUserById(string id, CancellationToken ct)
    {
        User? user;

        try
        {
            user = await _databaseService.GetUserByIdAsync(id, ct);
        }
        catch (IdNotFoundException notFoundException)
        {
            _logger.LogError(notFoundException.Message);

            return NotFound(notFoundException.Message);
        }

        if (user == null)
        {
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }

        return Ok(user);
    }
}