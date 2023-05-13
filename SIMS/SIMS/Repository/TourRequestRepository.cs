using SIMS.Domain.Model;
using SIMS.Domain.RepositoryInterface;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Repository
{
    class TourRequestRepository : ITourRequestRepository
    {
        private const string _filePath = "../../../../SIMS/Resources/Data/TourRequest.csv";
        private readonly Serializer<TourRequest> _serializer;
        public List<TourRequest> tourRequests;

        public TourRequestRepository()
        {
            _serializer = new Serializer<TourRequest>();
            tourRequests = _serializer.FromCSV(_filePath);
        }

        public List<TourRequest> GetAll()
        {
           return _serializer.FromCSV(_filePath);
        }

        public List<TourRequest> GetByUser(int userId)
        {
            List<TourRequest> requests = _serializer.FromCSV(_filePath);
            return requests.Where(r => r.User.Id == userId).ToList();
        
        }
        public int GenerateId()
        {
            List<TourRequest> requests = GetAll();

            if (requests.Count == 0)
            {
                return 0;
            }
            else
            {
                return requests[requests.Count - 1].Id + 1;
            }
        }

        public void Save (Location location, string description, string language, int maxNumberOfPeople, DateTime startDate, DateTime endDate, RequestStatus status, int userId)
        {
            List<TourRequest> requests = GetAll();
            TourRequest tourRequest = new TourRequest();
            tourRequest.Id = GenerateId();
            tourRequest.Location = location;
            tourRequest.StartDate = startDate;
            tourRequest.Language = language;
            tourRequest.EndDate = endDate;
            tourRequest.Description = description;
            tourRequest.Status = status;
            tourRequest.MaxNumberOfPeople = maxNumberOfPeople;
            tourRequest.User.Id = userId;
            requests.Add(tourRequest);
            _serializer.ToCSV(_filePath, requests);
        }

        public void Update(TourRequest tourRequest)
        {
            List<TourRequest> tourRequests = GetAll();
            tourRequests.RemoveAll(b => b.Id == tourRequest.Id);
            tourRequests.Add(tourRequest);
            _serializer.ToCSV(_filePath, tourRequests);
        }

    }
}
