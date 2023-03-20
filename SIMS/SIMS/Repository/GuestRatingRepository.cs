using SIMS.Model.AccommodationModel;
using SIMS.Model;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS.Repository.GuideRepository;

namespace SIMS.Repository
{
    public class GuestRatingRepository
    {
        private const string _filePath = "../../../../SIMS/Resources/Data/GuestRatings.csv";

        private readonly Serializer<GuestRating> _serializer;

        private readonly UserRepository _userRepository;
        private readonly ReservationRepository _reservationRepository;

        private List<GuestRating> _ratings;

       public GuestRatingRepository()
        {
            _serializer = new Serializer<GuestRating>();

            _ratings = _serializer.FromCSV(_filePath);
            _userRepository = new UserRepository();
            _reservationRepository = new ReservationRepository();
        }

        public List<GuestRating> GetAll()
        {
            return _serializer.FromCSV(_filePath);
        }

        public GuestRating Save(GuestRating rating)
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

        public void Delete(GuestRating rating)
        {
            _ratings = _serializer.FromCSV(_filePath);
            GuestRating founded = _ratings.Find(r => r.Id == rating.Id);
            if (founded == null) return;
            _ratings.Remove(founded);
            _serializer.ToCSV(_filePath, _ratings);
        }

        public GuestRating Update(GuestRating rating)
        {
            _ratings = _serializer.FromCSV(_filePath);
            GuestRating current = _ratings.Find(r => r.Id == rating.Id);
            if (current == null) return null;
            int index = _ratings.IndexOf(current);
            _ratings.Remove(current);
            _ratings.Insert(index, rating);
            _serializer.ToCSV(_filePath, _ratings);
            return rating;
        }

        public List<GuestRating> GetByReservation(Reservation reservation)
        {
            _ratings = _serializer.FromCSV(_filePath);
            foreach (GuestRating rating in _ratings)
            {
                rating.User = _userRepository.GetById(rating.User.Id);
                rating.Reservation = _reservationRepository.GetById(rating.Reservation.Id);
               
            }
            return _ratings.FindAll(c => c.Reservation.Id == reservation.Id);
        }
    }
}
