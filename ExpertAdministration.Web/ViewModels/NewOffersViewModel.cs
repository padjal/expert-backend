using ExpertAdministration.Core.Models;
using ExpertAdministration.Web.Interfaces;

namespace ExpertAdministration.Web.ViewModels
{
    public class NewOffersViewModel : ViewModelBase
    {
        private readonly IDatabaseService _databaseService;
        private List<Offer> _offers;
        private bool _areOffersLoading;

        public NewOffersViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;

            GetOffersAsync();
        }

        public bool AreOffersLoading
        {
            get => _areOffersLoading;
            set => SetField(ref _areOffersLoading, value);
        }

        public List<Offer> Offers
        {
            get => _offers;
            set => SetField(ref _offers, value);
        }

        public async Task GetOffersAsync()
        {
            AreOffersLoading = true;

            Offers = await _databaseService.GetAllOffersForReviewAsync();

            AreOffersLoading = false;
        }
    }
}