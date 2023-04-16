using SIMS.Model;
using SIMS.Model.AccommodationModel;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace SIMS.Repository
{
    class ReschedulingRequestsRepository
    {
        private const string _filePath = "../../../../SIMS/Resources/Data/ReschedulingRequests.csv";

        private readonly Serializer<ReschedulingRequests> _serializer;

        private readonly UserRepository _userRepository;
        private readonly ReservationRepository _reservationRepository;

        private List<ReschedulingRequests> _requests;

        public ReschedulingRequestsRepository()
        {
            _serializer = new Serializer<ReschedulingRequests>();

            _requests = _serializer.FromCSV(_filePath);
            _userRepository = new UserRepository();
            _reservationRepository = new ReservationRepository();
        }

        public List<ReschedulingRequests> GetAll()
        {
            return _serializer.FromCSV(_filePath);
        }

        public List<ReschedulingRequests> GetByUserId(int id)
        {
            List<ReschedulingRequests> reschedulingRequests = GetAll();
            List<Reservation> reservations = _reservationRepository.GetByUserId(id);
            foreach(Reservation reservation in reservations)
            {
                reschedulingRequests.Where(reschedulingRequest => reschedulingRequest.Reservation == reservation);
            }
            return reschedulingRequests;

        }

        

        public ReschedulingRequests Save(ReschedulingRequests request)
        {
            request.Id = NextId();
            _requests = _serializer.FromCSV(_filePath);
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
            _requests = _serializer.FromCSV(_filePath);
            ReschedulingRequests founded = _requests.Find(r => r.Id == request.Id);
            if (founded == null) return;
            _requests.Remove(founded);
            _serializer.ToCSV(_filePath, _requests);
        }

        public ReschedulingRequests Update(ReschedulingRequests request)
        {
            _requests = _serializer.FromCSV(_filePath);
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
