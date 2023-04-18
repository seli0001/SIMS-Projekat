using SIMS.Domain.Model;
using SIMS.Repository;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Service.UseCases
{
    public class GuestRatingService
    {
        private List<GuestRating> _ratings;
        private readonly GuestRatingRepository _guestRatingRepository;

        public GuestRatingService()
        {
            _ratings = new List<GuestRating>();
            _guestRatingRepository = new GuestRatingRepository();
        }

        public List<GuestRating> GetAll()
        {
            return _guestRatingRepository.GetAll();
        }

        public GuestRating Save(GuestRating rating)
        {
            return _guestRatingRepository.Save(rating);
        }

        public void Delete(GuestRating rating)
        {
            _guestRatingRepository.Delete(rating);
        }       

        public GuestRating Update(GuestRating rating)
        {
            return _guestRatingRepository.Update(rating);
        }

        public List<GuestRating> GetByReservation(Reservation reservation)
        {
            _ratings = GetAll();
            return _ratings.FindAll(c => c.Reservation.Id == reservation.Id);
        }

        public bool checkIfNotRated()
        {
            _ratings = GetAll();
            ReservationRepository _reservationRepository = new ReservationRepository();
            List<Reservation> reservations = _reservationRepository.GetAll();

            foreach (Reservation reservation in reservations)
            {
                if (GetByReservation(reservation).Count == 0 && CompareDates(DateOnly.FromDateTime(DateTime.Today), reservation.ToDate))
                {
                    return true;
                }
            }
            return false;
        }
        private bool CompareDates(DateOnly date11, DateOnly date22)
        {
            DateTime date1 = date11.ToDateTime(new TimeOnly());
            DateTime date2 = date22.ToDateTime(new TimeOnly());

            if (date2 > date1) return false;
            TimeSpan difference = date1 - date2;
            int days = difference.Days;
            if (days > 5) return false;
            else return true;
        }

        public bool checkRatingForReservation(Reservation reservation)
        {
            _ratings = GetAll();
            foreach (GuestRating rating in _ratings)
            {
                if (rating.Reservation.Id == reservation.Id) return true;
            }
            return false;
        }
    }
}
