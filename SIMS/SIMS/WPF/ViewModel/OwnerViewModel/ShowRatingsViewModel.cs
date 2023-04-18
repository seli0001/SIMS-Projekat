using SIMS.Domain.Model;
using SIMS.Repository;
using SIMS.Service.UseCases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS.WPF.ViewModel.OwnerViewModel
{
    public class ShowRatingsViewModel : OwnerViewModelBase
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
            List<OwnerRating> allRatings = new List<OwnerRating>(_ownerRatingRepository.GetByOwnerId(Owner.Id));

            foreach (OwnerRating rating in allRatings)
            {
                if (checkIfRated(rating))
                {
                    Ratings.Add(rating);
                }
            }
        }

        private bool checkIfRated(OwnerRating rating)
        {
            List<GuestRating> allRatings = new List<GuestRating>(_guestRatingService.GetAll());
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
