using ExpertAdministration.Core.Models;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExpertAdministration.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffersController : ControllerBase
    {
        private string _keyFilePath =
            @"C:\Users\pavel\source\repos\ExpertAdministration\ExpertAdministration.Server\Secret\firebase-key.json";
        private string _projectId = "maistor-29821";
        private readonly ILogger _logger;

        public OffersController(ILogger<OffersController> logger)
        {
            _logger = logger;

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", _keyFilePath);
        }

        // GET: api/<OffersController>
        [HttpGet]
        public async Task<IEnumerable<Offer>> Get()
        {
            FirestoreDb db = FirestoreDb.Create(_projectId);

            CollectionReference collection = db.Collection("offers");

            var querySnapshot = await collection.GetSnapshotAsync();

            var offers = new List<Offer>();

            foreach (var document in querySnapshot)
            {
                try
                {
                    var name = document.GetValue<string>("name");
                    var id = document.GetValue<string>("id");
                    var created = document.GetValue<DateTime>("created");
                    var description = document.GetValue<string>("description");
                    var owner = document.GetValue<string>("owner");
                    var categories = document.GetValue<List<string>>("categories");

                    var offer = new Offer()
                    {
                        Name = name,
                        Id = id,
                        CreatedAt = created,
                        Description = description,
                        Owner = owner,
                        Categories = categories
                    };

                    offers.Add(offer);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                }
            }

            return offers;
        }

        // GET api/<OffersController>/lksjdf32r44j
        [HttpGet("{id}")]
        public async Task<Offer> Get(string id)
        {
            FirestoreDb db = FirestoreDb.Create(_projectId);

            CollectionReference collection = db.Collection("offers");

            var filter = collection.WhereEqualTo("id", id);

            var querySnapshot = await filter.GetSnapshotAsync();

            var offers = new List<Offer>();

            foreach (var document in querySnapshot)
            {
                var name = document.GetValue<string>("name");
                var offerId = document.GetValue<string>("id");
                var created = document.GetValue<DateTime>("created");
                var description = document.GetValue<string>("description");
                var owner = document.GetValue<string>("owner");
                var categories = document.GetValue<List<string>>("categories");


                var offer = new Offer()
                {
                    Name = name,
                    Id = offerId,
                    CreatedAt = created,
                    Description = description,
                    Owner = owner,
                    Categories = categories
                };

                offers.Add(offer);
            }

            if (offers.Count == 1)
            {
                return offers[0];
            }
            else
            {
                return null;
            }
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
