using System.Net;
using System.Net.Http.Json;
using ExpertAdministration.Core.Models;
using ExpertAdministration.Web.Common;
using ExpertAdministration.Web.Interfaces;

namespace ExpertAdministration.Web.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public DatabaseService(IHttpClientFactory clientFactory, ILogger<DatabaseService> logger)
        {
            _logger = logger;
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

        public async Task<bool> UpdateOfferStatusAsync(string offerId, string offerStatus)
        {
            var result = await _httpClient.PatchAsync($"api/offers/{offerId}/status/{offerStatus}", null);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                _logger.LogInformation($"Successfully changed value for offer {offerId}: status -> {offerStatus}");
                return true;
            }

            _logger.LogError($"Failed to change offer status for offer {offerId}");

            return false;
        }

        public async Task<Offer> GetOfferAsync(string offerId)
        {
            //TODO: Check returned result
            return await _httpClient.GetFromJsonAsync<Offer>($"api/Offers/id/{offerId}");
        }

        public async Task<bool> DeleteOfferAsync(string offerId)
        {
            var result = await _httpClient.DeleteAsync($"api/offers/{offerId}");

            if (result.StatusCode == HttpStatusCode.OK)
            {
                _logger.LogInformation($"Successfully deleted offer {offerId}");
                return true;
            }

            _logger.LogError($"Failed to delete offer {offerId}");

            return false;
        }
    }
}