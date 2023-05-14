using ExpertAdministration.Core.Models;
using ExpertAdministration.Server.Exceptions;

namespace ExpertAdministration.Server.Interfaces;

/// <summary>
/// Handles get and retrieve logic from the database.
/// </summary>
public interface IDatabaseService
{
    public Task<List<Offer>> GetAllOffersAsync(CancellationToken ct);

    /// <summary>
    /// Returns an offer instance based on a specified id asynchronously.
    /// </summary>
    /// <param name="offerId">The specified id that should match the returned offer.</param>
    /// <param name="ct">The cancellation token, which keeps track of user cancellation intent.</param>
    /// <returns>An offer instance if found in the database or NULL if an error occurs.</returns>
    /// <exception cref="IdNotFoundException">Occurs when the specified id is not found.</exception>
    public Task<Offer?> GetOfferAsync(string offerId, CancellationToken ct);
}