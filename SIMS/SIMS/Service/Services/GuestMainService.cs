using SIMS.Domain.Model;
using SIMS.Repository;
using SIMS.Service.UseCases;
using System.Collections.Generic;

namespace SIMS.Service.Services
{
    class GuestMainService
    {

        private readonly GuestRatingRepository _guestRatingRepository;
        private readonly ReservationRepository _reservationRepository;

        public GuestMainService()
        {
            _guestRatingRepository = new GuestRatingRepository();
            _reservationRepository = new ReservationRepository();
        }

        public double getRating()
        {
            List<GuestRating> ratings = new List<GuestRating>(_guestRatingRepository.GetAll());
            double score = 0;
            foreach (GuestRating rating in ratings)
            {
                score += (rating.RulesRespect + rating.Cleanliness) / 2.0;
            }
            score /= ratings.Count;
            return score;
        }
        public int checkIsSuperGuest(User LoggedInUser)
        {
            List<Reservation> reservationsInPastYear = new List<Reservation>(_reservationRepository.GetRecentForGuest(LoggedInUser));
            SuperGuestService superGuestService = new SuperGuestService();
            if (superGuestService.CheckById(LoggedInUser.Id))
            {
                if (!(reservationsInPastYear.Count > 5))
                    return 0;
            }
            else if (reservationsInPastYear.Count > 5)
                return 1;

            return 2;
        }

        public bool checkRating(List<GuestRating> ratings)
        {
            double score = 0;
            foreach (GuestRating rating in ratings)
            {
                score += (rating.RulesRespect + rating.Cleanliness) / 2.0;
            }
            score /= ratings.Count;
            if (score > 4.5) return true;
            else return false;
        }

    }



}