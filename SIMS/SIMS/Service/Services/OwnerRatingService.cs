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
    public class OwnerRatingService
    {
        private List<OwnerRating> _ratings;
        private readonly OwnerRatingRepository _ownerRatingRepository;

        public OwnerRatingService()
        {
            _ratings = new List<OwnerRating>();
            _ownerRatingRepository = new OwnerRatingRepository();
        }

        public List<OwnerRating> GetAll()
        {
            return _ownerRatingRepository.GetAll();
        }

        public OwnerRating Save(OwnerRating rating)
        {
            return _ownerRatingRepository.Save(rating);
        }

        public void Delete(OwnerRating rating)
        {
            _ownerRatingRepository.Delete(rating);
        }

        public OwnerRating Update(OwnerRating rating)
        {
            return _ownerRatingRepository.Update(rating);
        }

        public List<OwnerRating> GetByReservation(Reservation reservation)
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
                if (GetByReservation(reservation).Count == 0 && CompareDates(DateTime.Today, reservation.ToDate))
                {
                    return true;
                }
            }
            return false;
        }

        private bool CompareDates(DateTime date1, DateTime date2)
        {
            if (date2 > date1) return false;
            TimeSpan difference = date1 - date2;
            int days = difference.Days;
            if (days > 5) return false;
            else return true;
        }

        public bool checkRatingForReservation(Reservation reservation)
        {
            _ratings = GetAll();
            foreach (OwnerRating rating in _ratings)
            {
                if (rating.Reservation.Id == reservation.Id) return true;
            }
            return false;
        }
    }
}
