using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS.Domain.Model;
using SIMS.Serializer;

namespace SIMS.Repository
{
    class OwnerRatingRepository
    {
        private const string _filePath = "../../../../SIMS/Resources/Data/OwnerRatings.csv";

        private readonly Serializer<OwnerRating> _serializer;

        private readonly UserRepository _userRepository;
        private readonly ReservationRepository _reservationRepository;

        private List<OwnerRating> _ratings;

        public OwnerRatingRepository()
        {
            _serializer = new Serializer<OwnerRating>();

            _ratings = _serializer.FromCSV(_filePath);
            _userRepository = new UserRepository();
            _reservationRepository = new ReservationRepository();
        }

        public List<OwnerRating> GetAll()
        {
            return _serializer.FromCSV(_filePath);
        }

        public OwnerRating Save(OwnerRating rating)
        {
            rating.Id = NextId();
            _ratings = _serializer.FromCSV(_filePath);
            _ratings.Add(rating);
            _serializer.ToCSV(_filePath, _ratings);
            return rating;
        }

        public int NextId()
        {
            _ratings = _serializer.FromCSV(_filePath);
            if (_ratings.Count < 1)
            {
                return 1;
            }
            return _ratings.Max(c => c.Id) + 1;
        }

        public void Delete(OwnerRating rating)
        {
            _ratings = _serializer.FromCSV(_filePath);
            OwnerRating founded = _ratings.Find(r => r.Id == rating.Id);
            if (founded == null) return;
            _ratings.Remove(founded);
            _serializer.ToCSV(_filePath, _ratings);
        }

        public OwnerRating Update(OwnerRating rating)
        {
            _ratings = _serializer.FromCSV(_filePath);
            OwnerRating current = _ratings.Find(r => r.Id == rating.Id);
            if (current == null) return null;
            int index = _ratings.IndexOf(current);
            _ratings.Remove(current);
            _ratings.Insert(index, rating);
            _serializer.ToCSV(_filePath, _ratings);
            return rating;
        }

        public List<OwnerRating> GetByReservation(Reservation reservation)
        {
            _ratings = _serializer.FromCSV(_filePath);
            foreach (OwnerRating rating in _ratings)
            {
                rating.User = _userRepository.GetById(rating.User.Id);
                rating.Reservation = _reservationRepository.GetById(rating.Reservation.Id);

            }
            return _ratings.FindAll(c => c.Reservation.Id == reservation.Id);
        }
    }
}
