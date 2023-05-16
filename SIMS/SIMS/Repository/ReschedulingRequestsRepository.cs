using SIMS.Domain.Model;
using SIMS.Model;
using SIMS.Serializer;
using System.Collections.Generic;
using System.Linq;

namespace SIMS.Repository
{
    class ReschedulingRequestsRepository
    {
        private const string _filePath = "../../../../SIMS/Resources/Data/ReschedulingRequests.csv";

        private readonly Serializer<ReschedulingRequests> _serializer;
        private readonly ReservationRepository _reservationRepository;

        private List<ReschedulingRequests> _requests;

        public ReschedulingRequestsRepository()
        {
            _serializer = new Serializer<ReschedulingRequests>();

            _requests = _serializer.FromCSV(_filePath);
            _reservationRepository = new ReservationRepository();
        }

        public List<ReschedulingRequests> GetAll()
        {
            _requests = _serializer.FromCSV(_filePath);
            foreach (ReschedulingRequests req in _requests)
            {
                req.Reservation = _reservationRepository.GetById(req.Reservation.Id);
            }
            return _requests;
        }

        public List<ReschedulingRequests> GetByUserId(int id)
        {
            List<ReschedulingRequests> reschedulingRequests = GetAll();
            List<Reservation> reservations = _reservationRepository.GetByUserId(id);
            foreach (Reservation reservation in reservations)
            {
                reschedulingRequests.Where(reschedulingRequest => reschedulingRequest.Reservation == reservation);
            }
            return reschedulingRequests;

        }

        public List<ReschedulingRequests> GetByOwnerId(int id)
        {
            _requests = GetAll();
            List<ReschedulingRequests> allRequests = new List<ReschedulingRequests>();
            foreach(ReschedulingRequests req in _requests)
            {
                if (req.Reservation.Accommodation.User.Id == id && req.Status == Status.WAITING) allRequests.Add(req);
            }
            return allRequests;
        }
        public ReschedulingRequests Save(ReschedulingRequests request)
        {
            request.Id = NextId();
            _requests = GetAll();
            _requests.Add(request);
            _serializer.ToCSV(_filePath, _requests);
            return request;
        }

        public int NextId()
        {
            _requests = _serializer.FromCSV(_filePath);
            if (_requests.Count < 1)
            {
                return 1;
            }
            return _requests.Max(c => c.Id) + 1;
        }

        public void Delete(ReschedulingRequests request)
        {
            _requests = GetAll();
            ReschedulingRequests founded = _requests.Find(r => r.Id == request.Id);
            if (founded == null) return;
            _requests.Remove(founded);
            _serializer.ToCSV(_filePath, _requests);
        }

        public ReschedulingRequests Update(ReschedulingRequests request)
        {
            _requests = GetAll();
            ReschedulingRequests current = _requests.Find(r => r.Id == request.Id);
            if (current == null) return null;
            int index = _requests.IndexOf(current);
            _requests.Remove(current);
            _requests.Insert(index, request);
            _serializer.ToCSV(_filePath, _requests);
            return request;
        }

        /*public List<ReschedulingRequests> GetByReservation(Reservation reservation)
        {
            _requests = _serializer.FromCSV(_filePath);
            foreach (ReschedulingRequests rating in _requests)
            {
                rating.User = _userRepository.GetById(rating.User.Id);
                rating.Reservation = _reservationRepository.GetById(rating.Reservation.Id);

            }
            return _requests.FindAll(c => c.Reservation.Id == reservation.Id);
        }*/
    }
}
