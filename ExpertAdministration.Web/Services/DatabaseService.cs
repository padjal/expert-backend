using System.Net.Http.Json;
using ExpertAdministration.Core.Models;
using ExpertAdministration.Web.Common;
using ExpertAdministration.Web.Interfaces;

namespace ExpertAdministration.Web.Services
{
    public class DatabaseService : IDatabaseService
    {
        private HttpClient _httpClient;

        public DatabaseService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient(Constants.CustomWebApi);
        }

        public async Task<List<Offer>> GetAllOffersAsync()
        {
            //TODO: Check for any errors while fetching all offers.
            List<Offer> offers = new List<Offer>();

            var response = await _httpClient.GetFromJsonAsync<List<Offer>>("api/Offers");

            if (response != null)
            {
                offers.AddRange(response);
            }

            return offers;
        }

        public Task<List<Offer>> GetAllOffersForReviewAsync()
        {
            throw new NotImplementedException();
        }

        public bool UpdateOfferStatus(string offerId, string offerStatus)
        {
            throw new NotImplementedException();
        }

        public async Task<Offer> GetOfferAsync(string offerId)
        {
            //TODO: Check returned result
            return await _httpClient.GetFromJsonAsync<Offer>($"api/Offers/id/{offerId}");
        }
    }
}