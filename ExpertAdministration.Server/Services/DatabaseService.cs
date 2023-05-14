using ExpertAdministration.Core.Models;
using ExpertAdministration.Server.Exceptions;
using ExpertAdministration.Server.Interfaces;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;

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
    
    public async Task<List<Offer>?> GetAllOffersAsync(CancellationToken ct, int maxOffersLimit)
    {
        var collection = FirestoreDb.Collection("offers").Limit(maxOffersLimit);

        var querySnapshot = await collection.GetSnapshotAsync(ct);

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
                _logger.LogError($"Offer from document{document.Id} does not comply with schema." +
                                 $"{e.Message}");
            }
        }

        return offers;
    }

    public async Task<Offer?> GetOfferAsync(string offerId, CancellationToken ct)
    {
        DocumentReference document;
        
        try
        {
            document = FirestoreDb.Document(offerId);
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

            var offer = new Offer()
            {
                Name = name,
                Id = id,
                CreatedAt = created,
                Description = description,
                Owner = owner,
                Categories = categories
            };

            return offer;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            
            return null;
        }
    }
}