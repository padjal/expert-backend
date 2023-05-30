using ExpertAdministration.Core.Models;
using ExpertAdministration.Server.Exceptions;
using ExpertAdministration.Server.Interfaces;
using Google.Cloud.Firestore;

namespace ExpertAdministration.Server.Services;

public class DatabaseService : IDatabaseService
{
    private static readonly string KeyFilePath =
        $@".{Path.DirectorySeparatorChar}Shared{Path.DirectorySeparatorChar}firebase-key.json";

    private static readonly string ProjectId = "maistor-29821";
    private static readonly FirestoreDb FirestoreDb;

    private readonly ILogger _logger;

    static DatabaseService()
    {
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", KeyFilePath);

        FirestoreDb = FirestoreDb.Create(ProjectId);
    }

    public DatabaseService(ILogger<IDatabaseService> logger)
    {
        _logger = logger;
    }

    public async Task<List<Offer>?> GetAllOffersAsync(CancellationToken ct, int maxOffersLimit, string status = "any")
    {
        QuerySnapshot querySnapshot;

        try
        {
            var collection = FirestoreDb.Collection("offers")
                .OrderBy("created")
                .Limit(maxOffersLimit);

            if (status != "any")
            {
                collection = collection.WhereEqualTo("status", status);
            }

            querySnapshot = await collection.GetSnapshotAsync(ct);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);

            return null;
        }

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
                var offerStatus = document.GetValue<string>("status");
                var images = document.GetValue<List<string>>("images");


                var offer = new Offer()
                {
                    Name = name,
                    Id = id,
                    CreatedAt = created,
                    Description = description,
                    Owner = owner,
                    Categories = categories,
                    Status = offerStatus,
                    ImageUrls = images
                };

                offers.Add(offer);
            }
            catch (Exception e)
            {
                _logger.LogError($"Offer from document{document.Id} does not comply with schema." +
                                 $"{e.Message}");

                //TODO: Change offer status to Archived and add offerId if not present
                await UpdateFailingOfferDetails(document.Id);
            }
        }

        return offers;
    }

    public async Task<Offer?> GetOfferAsync(string offerId, CancellationToken ct)
    {
        DocumentReference document;

        try
        {
            document = FirestoreDb.Document($"offers/{offerId}");
        }
        catch
        {
            throw new IdNotFoundException(offerId, "Could not find specified offer in database.");
        }

        var docSnapshot = await document.GetSnapshotAsync(ct);

        try
        {
            var name = docSnapshot.GetValue<string>("name");
            var id = docSnapshot.GetValue<string>("id");
            var created = docSnapshot.GetValue<DateTime>("created");
            var description = docSnapshot.GetValue<string>("description");
            var owner = docSnapshot.GetValue<string>("owner");
            var categories = docSnapshot.GetValue<List<string>>("categories");
            var status = docSnapshot.GetValue<string>("status");
            var images = docSnapshot.GetValue<List<string>>("images");

            var offer = new Offer()
            {
                Name = name,
                Id = id,
                CreatedAt = created,
                Description = description,
                Owner = owner,
                Categories = categories,
                Status = status,
                ImageUrls = images
            };

            return offer;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);

            return null;
        }
    }

    public async Task<User> GetUserByIdAsync(string userId, CancellationToken ct)
    {
        //TODO: Add userId field validation
        DocumentReference document;

        try
        {
            document = FirestoreDb.Document($"users/{userId}");
        }
        catch
        {
            throw new IdNotFoundException(userId, "Could not find specified user in database.");
        }

        var docSnapshot = await document.GetSnapshotAsync(ct);

        try
        {
            var name = docSnapshot.GetValue<string>("name");
            var email = docSnapshot.GetValue<string>("email");
            var phone = docSnapshot.GetValue<string>("phone");
            var photoUrl = docSnapshot.GetValue<string>("photo");
            var region = docSnapshot.GetValue<string>("region");

            var user = new User()
            {
                Name = name,
                Id = userId,
                Email = email,
                Phone = phone,
                ProfilePictureUrl = photoUrl,
                Region = region
            };

            return user;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);

            return null;
        }
    }

    public async Task<bool> UpdateOfferFieldAsync(string offerId, string field, object value, CancellationToken ct)
    {
        DocumentReference document;

        try
        {
            document = FirestoreDb.Document($"offers/{offerId}");
        }
        catch
        {
            throw new IdNotFoundException(offerId, "Could not find specified offer in database.");
        }

        try
        {
            Dictionary<FieldPath, object> updates = new Dictionary<FieldPath, object>
            {
                { new FieldPath(field), value }
            };

            await document.UpdateAsync(updates);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);

            return false;
        }

        return true;
    }

    public async Task<bool> DeleteOfferAsync(string offerId, CancellationToken ct)
    {
        DocumentReference document;

        try
        {
            document = FirestoreDb.Document($"offers/{offerId}");
        }
        catch
        {
            throw new IdNotFoundException(offerId, "Could not find specified offer in database.");
        }

        try
        {
            await document.DeleteAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);

            return false;
        }

        return true;
    }

    private async Task UpdateFailingOfferDetails(string offerId)
    {
        DocumentReference document;

        try
        {
            document = FirestoreDb.Document($"offers/{offerId}");
        }
        catch
        {
            _logger.LogError($"Cannot find offer document with id:{offerId}");
            return;
        }

        try
        {
            Dictionary<FieldPath, object> updates = new Dictionary<FieldPath, object>
            {
                { new FieldPath("status"), "Review" },
                { new FieldPath("id"), offerId }
            };

            await document.UpdateAsync(updates);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return;
        }

        _logger.LogInformation($"Successfully updated id and status of offer {offerId}");
    }
}