using SIMS.Domain.Model;
using SIMS.Service;
using SIMS.Service.UseCases;
using SIMS.WPF.ViewModel.OwnerViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.WPF.ViewModel.Guest1ViewModel
{
    public class HomeViewModel
    {
        public static ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }

        private readonly GuestRatingService _guestRatingService;
        private readonly AccommodationService _accommodationService;

        public User LoggedInUser { get; set; }

        Guest1MainViewModel guest1MainViewModel;

        public HomeViewModel(User user, Guest1MainViewModel mvm)
        {
            LoggedInUser = user;

            _guestRatingService = new GuestRatingService();
            _accommodationService = new AccommodationService();

            Accommodations = new ObservableCollection<Accommodation>(_accommodationService.GetByUser(user));
            guest1MainViewModel = mvm;
        }
    }
}
