using ExpertAdministration.Core.Models;
using ExpertAdministration.Web.Interfaces;

namespace ExpertAdministration.Web.ViewModels;

public class OfferReviewViewModel : ViewModelBase
{
    private readonly IDatabaseService _databaseService;

    private Offer _offer;
    private bool _isProgressIndicatorActive = true;
    private string _offerStatus;
    private bool _offerStatusHasChanged;

    public event EventHandler<string> OnOfferStatusUpdated;


    public OfferReviewViewModel(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public Offer Offer
    {
        get => _offer;
        private set => SetField(ref _offer, value);
    }

    public bool IsProgressIndicatorActive
    {
        get => _isProgressIndicatorActive;
        private set => SetField(ref _isProgressIndicatorActive, value);
    }

    public string OfferStatus
    {
        get => _offerStatus;
        set => SetField(ref _offerStatus, value);
    }

    public bool OfferStatusHasChanged
    {
        get => _offerStatusHasChanged;
        set => SetField(ref _offerStatusHasChanged, value);
    }

    public async Task LoadOfferDataAsync(string offerId)
    {
        IsProgressIndicatorActive = true;

        Offer = await _databaseService.GetOfferAsync(offerId);

        OfferStatus = Offer.Status;

        IsProgressIndicatorActive = false;
    }

    public async Task ChangeStatus()
    {
        IsProgressIndicatorActive = true;

        var isUpdateSuccessful = await _databaseService.UpdateOfferStatusAsync(Offer.Id, OfferStatus);

        IsProgressIndicatorActive = false;

        if (isUpdateSuccessful)
        {
            OnOfferStatusUpdated(this, $"Successfully updated offer {Offer.Id} status to {OfferStatus}");

            OfferStatusHasChanged = true;
        }
        else
        {
            OnOfferStatusUpdated(this, $"Could not updat offer {Offer.Id} status");
        }
    }
}