using ExpertAdministration.Core.Models;
using ExpertAdministration.Web.Interfaces;
using Radzen;

namespace ExpertAdministration.Web.ViewModels;

public class OfferReviewViewModel : ViewModelBase
{
    private readonly IDatabaseService _databaseService;

    private Offer _offer;
    private bool _isProgressIndicatorActive = true;
    private string _offerStatus;
    private bool _offerStatusHasChanged;
    private User _user;
    private string _previousPage;

    public delegate void OfferStatusHandler(object sender, NotificationMessage message);

    public event OfferStatusHandler OnOfferStatusUpdated;

    public OfferReviewViewModel(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public Offer Offer
    {
        get => _offer;
        private set => SetField(ref _offer, value);
    }

    public User User
    {
        get => _user;
        private set => SetField(ref _user, value);
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

    public string PreviousPage
    {
        get => _previousPage;
        set => SetField(ref _previousPage, value);
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

        User = await _databaseService.GetUserByIdAsync(Offer.Owner);

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
            OnOfferStatusUpdated(this,
                new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Successful update",
                    Detail = $"Successfully updated offer {Offer.Id} status to {OfferStatus}"
                }
            );

            OfferStatusHasChanged = true;
        }
        else
        {
            OnOfferStatusUpdated(this,
                new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Unsuccessful update",
                    Detail = $"Could not updat offer {Offer.Id} status"
                }
            );
        }
    }

    public async Task DeleteOffer()
    {
        IsProgressIndicatorActive = true;

        var isDeleteSuccessful = await _databaseService.DeleteOfferAsync(Offer.Id);

        IsProgressIndicatorActive = false;

        if (isDeleteSuccessful)
        {
            OnOfferStatusUpdated(this,
                new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Delete result",
                    Detail = $"Successfully deleted offer {Offer.Id}"
                }
            );

            OfferStatusHasChanged = true;
        }
        else
        {
            OnOfferStatusUpdated(this,
                new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Delete result",
                    Detail = $"Could not delete offer {Offer.Id}"
                }
            );
        }
    }
}