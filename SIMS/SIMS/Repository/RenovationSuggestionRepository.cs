using SIMS.Domain.Model;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Repository
{
    class RenovationSuggestionRepository
    {
        private const string _filePath = "../../../../SIMS/Resources/Data/RenovationSuggestion.csv";

        private readonly Serializer<RenovationSuggestion> _serializer;

        private readonly UserRepository _userRepository;
        private readonly ReservationRepository _reservationRepository;

        private List<RenovationSuggestion> _suggestions;

        public RenovationSuggestionRepository()
        {
            _serializer = new Serializer<RenovationSuggestion>();

            _suggestions = _serializer.FromCSV(_filePath);
            _userRepository = new UserRepository();
            _reservationRepository = new ReservationRepository();
        }

        public List<RenovationSuggestion> GetAll()
        {
            _suggestions = _serializer.FromCSV(_filePath);
            foreach (RenovationSuggestion suggestion in _suggestions)
            {
                suggestion.User = _userRepository.GetById(suggestion.User.Id);
                suggestion.Reservation = _reservationRepository.GetById(suggestion.Reservation.Id);
            }
            return _suggestions;
        }

        public RenovationSuggestion Save(RenovationSuggestion suggestion)
        {
            suggestion.Id = NextId();
            _suggestions = GetAll();
            _suggestions.Add(suggestion);
            _serializer.ToCSV(_filePath, _suggestions);
            return suggestion;
        }

        public int NextId()
        {
            _suggestions = GetAll();
            if (_suggestions.Count < 1)
            {
                return 1;
            }
            return _suggestions.Max(c => c.Id) + 1;
        }

        public void Delete(RenovationSuggestion suggestion)
        {
            _suggestions = GetAll();
            RenovationSuggestion founded = _suggestions.Find(r => r.Id == suggestion.Id);
            if (founded == null) return;
            _suggestions.Remove(founded);
            _serializer.ToCSV(_filePath, _suggestions);
        }

        public RenovationSuggestion Update(RenovationSuggestion suggestion)
        {
            _suggestions = GetAll();
            RenovationSuggestion current = _suggestions.Find(r => r.Id == suggestion.Id);
            if (current == null) return null;
            int index = _suggestions.IndexOf(current);
            _suggestions.Remove(current);
            _suggestions.Insert(index, suggestion);
            _serializer.ToCSV(_filePath, _suggestions);
            return suggestion;
        }

        public List<RenovationSuggestion> GetByReservation(Reservation reservation)
        {
            _suggestions = GetAll();
            return _suggestions.FindAll(c => c.Reservation.Id == reservation.Id);
        }

        public List<RenovationSuggestion> GetByUserId(int id)
        {
            _suggestions = GetAll();
            return _suggestions.FindAll(r => r.User.Id == id);
        }

    }
}
