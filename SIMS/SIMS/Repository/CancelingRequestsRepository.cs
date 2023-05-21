using SIMS.Domain.Model;
using SIMS.Model;
using SIMS.Serializer;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using SIMS.Domain.Model;

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
            _requests = _serializer.FromCSV(_filePath);
            foreach(CancelingRequests req in _requests)
            {
                req.Reservation = _reservationRepository.GetById(req.Reservation.Id);
            }
            return _requests;
        }

        public List<CancelingRequests> GetByUserId(int id)
        {
            _requests = GetAll();
            return _requests.Where(req => req.Reservation.User.Id == id).ToList();
        }

        public List<CancelingRequests> GetByAccommodationsId(int id)
        {
            _requests = GetAll();
            return _requests.Where(request => request.Reservation.Accommodation.Id == id).ToList();
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
            _requests = GetAll();
            CancelingRequests founded = _requests.Find(r => r.Id == request.Id);
            if (founded == null) return;
            _requests.Remove(founded);
            _serializer.ToCSV(_filePath, _requests);
        }

        public CancelingRequests Update(CancelingRequests request)
        {
            _requests = GetAll();
            CancelingRequests current = _requests.Find(r => r.Id == request.Id);
            if (current == null) return null;
            int index = _requests.IndexOf(current);
            _requests.Remove(current);
            _requests.Insert(index, request);
            _serializer.ToCSV(_filePath, _requests);
            return request;
        }

        public double GetCanceledResNumForYear(int year, Accommodation accommodation)
        {
            double count = 0;
            List<CancelingRequests> requests = GetByAccommodationsId(accommodation.Id);
            foreach (CancelingRequests req in requests)
            {
                if (req.Reservation.FromDate.Year == year)
                    count++;
            }

            return count;
        }

        public double GetCanceledResNumForMonth(int year, int month, Accommodation accommodation)
        {
            double count = 0;
            List<CancelingRequests> requests = GetByAccommodationsId(accommodation.Id);
            foreach (CancelingRequests req in requests)
            {
                if (req.Reservation.FromDate.Year == year && req.Reservation.FromDate.Month == month)
                    count++;
            }

            return count;
        }
    }
}
