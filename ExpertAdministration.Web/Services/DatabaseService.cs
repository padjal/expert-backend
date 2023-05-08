using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ExpertAdministration.Web.Interfaces;
using ExpertAdministration.Core.Models;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;

namespace ExpertAdministration.Web.Services
{
    public class DatabaseService : IDatabaseService
    {
        private HttpClient _httpClient;

        public DatabaseService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7269/") };
        }
        

        public async Task<List<Offer>> GetAllOffersAsync()
        {
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
            try
            {
                var test = await _httpClient.GetFromJsonAsync<Offer>($"api/Offers/{offerId}");

            }
            catch (Exception e)
            {

            }
            return await _httpClient.GetFromJsonAsync<Offer>($"api/Offers/{offerId}");
        }
    }
}
