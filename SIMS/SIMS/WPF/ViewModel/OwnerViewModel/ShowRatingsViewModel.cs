using SIMS.Domain.Model;
using SIMS.Repository;
using SIMS.Service.UseCases;
using SIMS.WPF.ViewModel.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace SIMS.WPF.ViewModel.OwnerViewModel
{
    public class ShowRatingsViewModel : ViewModelBase
    {
        private readonly OwnerRatingRepository _ownerRatingRepository;
        private readonly GuestRatingService _guestRatingService;
        public static ObservableCollection<OwnerRating> Ratings { get; set; }
        public OwnerRating SelectedRating { get; set; }
        public User Owner { get; set; }

        public ShowRatingsViewModel(User user)
        {
            _ownerRatingRepository = new OwnerRatingRepository();
            _guestRatingService = new GuestRatingService();

            Owner = user;
            Ratings = new ObservableCollection<OwnerRating>();
            UpdateRatings();
        }

        private void UpdateRatings()
        {
            //Prikazemo sve ocene iz OwnerRatings koje su za naseg ownera
            List<OwnerRating> allOwnerRatings = new List<OwnerRating>(_ownerRatingRepository.GetByOwnerId(Owner.Id));
            List<GuestRating> allGuestRatings = new List<GuestRating>(_guestRatingService.GetAll());

            foreach (OwnerRating rating in allOwnerRatings)
            {
                if (checkIfRated(rating, allGuestRatings))
                {
                    Ratings.Add(rating);
                }
            }
        }

        private bool checkIfRated(OwnerRating rating, List<GuestRating> allRatings)
        {
            foreach (GuestRating guestRating in allRatings)
            {
                if (guestRating.Reservation.Id == rating.Reservation.Id)
                {
                    return true;
                }
            }
            return false;
        }
        private void ShowRatingInfo(object sender, RoutedEventArgs e)
        {

        }
    }
}
