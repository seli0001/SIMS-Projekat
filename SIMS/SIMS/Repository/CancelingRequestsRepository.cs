using SIMS.Domain.Model;
using SIMS.Model;
using SIMS.Model;
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
    class CancelingRequestsRepository
    {
        private const string _filePath = "../../../../SIMS/Resources/Data/CancelingRequests.csv";

        private readonly Serializer<CancelingRequests> _serializer;

        private readonly UserRepository _userRepository;
        private readonly ReservationRepository _reservationRepository;

        private List<CancelingRequests> _requests;

        public CancelingRequestsRepository()
        {
            _serializer = new Serializer<CancelingRequests>();

            _requests = _serializer.FromCSV(_filePath);
            _userRepository = new UserRepository();
            _reservationRepository = new ReservationRepository();
        }

        public List<CancelingRequests> GetAll()
        {
            return _serializer.FromCSV(_filePath);
        }

        public List<CancelingRequests> GetByUserId(int id)
        {
            List<CancelingRequests> reschedulingRequests = GetAll();
            List<Reservation> reservations = _reservationRepository.GetByUserId(id);
            foreach (Reservation reservation in reservations)
            {
                reschedulingRequests.Where(reschedulingRequest => reschedulingRequest.Reservation == reservation);
            }
            return reschedulingRequests;

        }
        public CancelingRequests Save(CancelingRequests request)
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

        public void Delete(CancelingRequests request)
        {
            _requests = _serializer.FromCSV(_filePath);
            CancelingRequests founded = _requests.Find(r => r.Id == request.Id);
            if (founded == null) return;
            _requests.Remove(founded);
            _serializer.ToCSV(_filePath, _requests);
        }

        public CancelingRequests Update(CancelingRequests request)
        {
            _requests = _serializer.FromCSV(_filePath);
            CancelingRequests current = _requests.Find(r => r.Id == request.Id);
            if (current == null) return null;
            int index = _requests.IndexOf(current);
            _requests.Remove(current);
            _requests.Insert(index, request);
            _serializer.ToCSV(_filePath, _requests);
            return request;
        }
    }
}
