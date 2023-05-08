using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpertAdministration.Core.Models;

namespace ExpertAdministration.Web.Interfaces
{
    public interface IDatabaseService
    {
        /// <summary>
        /// Gets all currently available offers in the product database.
        /// </summary>
        /// <returns>A list of offers. Null if database connection cannot be established.</returns>
        Task<List<Offer>> GetAllOffersAsync();

        /// <summary>
        /// Gets all currently available offers in the product database which have not been approved.
        /// </summary>
        /// <returns>A list of offers. Null if database connection cannot be established.</returns>
        Task<List<Offer>> GetAllOffersForReviewAsync();

        /// <summary>
        /// Updates the offer status of a specified offer.
        /// </summary>
        /// <param name="offerId">The id of the offer being updated.</param>
        /// <param name="offerStatus">The new status of the offer.</param>
        /// <returns>True if update completes successfully.</returns>
        bool UpdateOfferStatus(string offerId, string offerStatus);

        /// <summary>
        /// Gets information for a specified offer given an offer Id asynchronously.
        /// </summary>
        /// <param name="offerId">The offer Id used to filter the offers.</param>
        /// <returns>An offer object</returns>
        Task<Offer> GetOfferAsync(string offerId);
    }
}
