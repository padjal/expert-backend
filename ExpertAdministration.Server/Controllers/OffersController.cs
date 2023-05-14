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

        [HttpGet]
        public Task<List<Offer>> Get(CancellationToken ct)
        {
            return _databaseService.GetAllOffersAsync(ct);
        }

        /// <summary>
        /// Gets an offer instance based on a specified  asynchronously.
        /// </summary>
        /// <param name="id">The specified id of the offer being seeked.</param>
        /// <param name="ct">The cancellation token used to indicate user cancellation intents.</param>
        /// <returns></returns>
        /// <response code="200">Returned if offer is found successfully.</response>
        /// <response code="400">Returned if offer id does not match schema.</response>
        /// <response code="404">Returned if offer id is not found.</response>
        /// <response code="500">Returned if offer does not follow application schema.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Offer>> Get(string id, CancellationToken
         ct)
        {
            var idFormat = new Regex(@"^\w{13}$");

            if (!idFormat.IsMatch(id))
            {
                return BadRequest("Id does not match schema. Please verify that the given id is in the format \"\\w{13}\"");
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

        // POST api/<OffersController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<OffersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OffersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
