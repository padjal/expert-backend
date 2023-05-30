using ExpertAdministration.Core.Models;
using ExpertAdministration.Server.Exceptions;

namespace ExpertAdministration.Server.Interfaces;

/// <summary>
/// Handles get and retrieve logic from the database.
/// </summary>
public interface IDatabaseService
{
    /// <summary>
    /// Returns all the offers from the database according to the specified limit. Offers are ordered by createdAt attribute
    /// by default.
    /// </summary>
    /// <param name="maxOffersLimit">The maximum number of offers returned from the database.</param>
    /// <param name="ct">The cancellation token, which keeps track of user cancellation intent.</param>
    /// <returns>A collection of offers or NULL if an error occurs.</returns>
    public Task<List<Offer>?> GetAllOffersAsync(CancellationToken ct, int maxOffersLimit, string status = "any");

    /// <summary>
    /// Returns an offer instance based on a specified id asynchronously.
    /// </summary>
    /// <param name="offerId">The specified id that should match the returned offer.</param>
    /// <param name="ct">The cancellation token, which keeps track of user cancellation intent.</param>
    /// <returns>An offer instance if found in the database or NULL if an error occurs.</returns>
    /// <exception cref="IdNotFoundException">Occurs when the specified id is not found.</exception>
    public Task<Offer?> GetOfferAsync(string offerId, CancellationToken ct);

    /// <summary>
    /// Get a user from the database by a specified id.
    /// </summary>
    /// <param name="userId">The specified id.</param>
    /// <param name="ct">The cancellation token, which keeps track of user cancellation intent.</param>
    /// <returns>A user instance if found in the database or NULL if an error occurs.</returns>
    /// /// <exception cref="IdNotFoundException">Occurs when the specified id is not found.</exception>
    public Task<User?> GetUserByIdAsync(string userId, CancellationToken ct);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="offerId"></param>
    /// <param name="field"></param>
    /// <param name="value"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public Task<bool> UpdateOfferFieldAsync(string offerId, string field, object value, CancellationToken ct);

    /// <summary>
    /// Deletes an offer with specified id from the database.
    /// </summary>
    /// <param name="offerId">The specified id of the offer being deleted.</param>
    /// <param name="ct">The cancellation token, which keeps track of user cancellation intent.</param>
    /// <returns>True if the deletion is successful.</returns>
    public Task<bool> DeleteOfferAsync(string offerId, CancellationToken ct);
}