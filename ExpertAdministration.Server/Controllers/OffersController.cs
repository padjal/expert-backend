using System.Net;
using System.Text.RegularExpressions;
using ExpertAdministration.Core.Models;
using ExpertAdministration.Server.Exceptions;
using ExpertAdministration.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExpertAdministration.Server.Controllers
{
    /// <summary>
    /// The controller responsible for interaction with the offers of the application.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OffersController : ControllerBase
    {
        private readonly IDatabaseService _databaseService;

        private readonly ILogger _logger;

        public OffersController(ILogger<OffersController> logger,
            IDatabaseService databaseService)
        {
            _logger = logger;
            _databaseService = databaseService;
        }

        /// <summary>
        /// Gets all the offers from the database according to the specified limit.
        /// </summary>
        /// <remarks>Offers are ordered by createdAt attribute by default.</remarks>
        /// <param name="ct">The cancellation token used to indicate user cancellation intents.</param>
        /// <param name="maxOffersLimit">The maximum number of offers returned from the database. 1000 by default</param>
        /// <returns>An action result from the operation.</returns>
        /// <response code="500">Returned if there is a database issue while fetching the offers.</response>
        [HttpGet("{maxOffersLimit:int?}")]
        public async Task<ActionResult<List<Offer>>> Get(CancellationToken ct, int maxOffersLimit = 1000)
        {
            var offers = await _databaseService.GetAllOffersAsync(ct, maxOffersLimit);

            if (offers == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }

            return Ok(offers);
        }

        /// <summary>
        /// Gets an offer instance based on a specified  asynchronously.
        /// </summary>
        /// <param name="id">The specified id of the offer being seeked.</param>
        /// <param name="ct">The cancellation token used to indicate user cancellation intents.</param>
        /// <returns>An action result from the operation.</returns>
        /// <response code="200">Returned if offer is found successfully.</response>
        /// <response code="400">Returned if offer id does not match schema.</response>
        /// <response code="404">Returned if offer id is not found.</response>
        /// <response code="500">Returned if offer does not follow application schema.</response>
        [HttpGet("id/{id}")]
        public async Task<ActionResult<Offer>> Get(string id, CancellationToken
            ct)
        {
            var idFormat = new Regex(@"^\w{20}$");

            if (!idFormat.IsMatch(id))
            {
                return BadRequest("Id does not match schema. Please verify that the given id is in the format ^\\w{20}$");
            }

            Offer? offer;

            try
            {
                offer = await _databaseService.GetOfferAsync(id, ct);
            }
            catch (IdNotFoundException notFoundException)
            {
                _logger.LogError(notFoundException.Message);

                return NotFound(notFoundException.Message);
            }

            if (offer == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }

            return Ok(offer);
        }

        /// <summary>
        /// Updates a value of an offer with specified offer id with a passed value.
        /// </summary>
        /// <remarks>Can only be used to update simple field types. No lists are supported.</remarks>
        /// <param name="id">The specified id of the offer being updated.</param>
        /// <param name="field">The name of the field being updated.</param>
        /// <param name="value">The value of the field being updated.</param>
        [HttpPatch("{id}/{field}/{value}")]
        public async Task<ActionResult> Patch(string id, string field, string value, CancellationToken ct)
        {
            if (await _databaseService.UpdateOfferFieldAsync(id, field, value, ct))
            {
                return Ok();
            }

            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}