using SIMS.Domain.Model;
using SIMS.Repository;
using SIMS.Service.UseCases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.WPF.ViewModel
{
    public class ShowGuestRatingsViewModel
    {
        private readonly GuestRatingRepository _guestRatingRepository;
        private readonly OwnerRatingService _ownerRatingService;
        public static ObservableCollection<GuestRating> Ratings { get; set; }
        public GuestRating SelectedRating { get; set; }
        public User Owner { get; set; }

        public ShowGuestRatingsViewModel(User user)
        {
            _guestRatingRepository = new GuestRatingRepository();
            _ownerRatingService = new OwnerRatingService();

            Owner = user;
            Ratings = new ObservableCollection<GuestRating>();
            UpdateRatings();
        }

        private void UpdateRatings()
        {
            // Display all ratings from GuestRatings that are for our owner
            List<GuestRating> allGuestRatings = new List<GuestRating>(_guestRatingRepository.GetByGuestId(Owner.Id));
            List<OwnerRating> allOwnerRatings = new List<OwnerRating>(_ownerRatingService.GetAll());

            foreach (GuestRating rating in allGuestRatings)
            {
                if (CheckIfRated(rating, allOwnerRatings))
                {
                    Ratings.Add(rating);
                }
            }
        }

        private bool CheckIfRated(GuestRating rating, List<OwnerRating> allRatings)
        {
            foreach (OwnerRating ownerRating in allRatings)
            {
                if (ownerRating.Reservation.Id == rating.Reservation.Id)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
