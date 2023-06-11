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
        private readonly LocationRepository _locationRepository;
        private readonly Serializer<TourRequest> _serializer;
        public List<TourRequest> tourRequests;

        public TourRequestRepository()
        {
            _serializer = new Serializer<TourRequest>();
            _locationRepository = new LocationRepository();
            tourRequests = _serializer.FromCSV(_filePath);
        }

        public List<TourRequest> GetAll()
        {
            List<TourRequest> tourRequests = _serializer.FromCSV(_filePath);
            foreach( TourRequest t in tourRequests)
            {
                t.Location = _locationRepository.GetById(t.Location.Id);
            }
            return tourRequests;
        }

        public List<TourRequest> GetRequestsById(List<int> ids)
        {
            List<TourRequest> requests = GetAll();
            List<TourRequest> requestsInComplex = new List<TourRequest>();
            foreach (int id in ids)
                {
                    foreach( TourRequest t in requests)
                        {
                            if(id==t.Id)
                                {
                             requestsInComplex.Add(t);
                                }
                        }
                }

            return requestsInComplex.ToList();
        }

        public Location GetMostLocation()
        {
            var groupedRequests = GetAll().GroupBy(r => r.Location.City).OrderByDescending(g => g.Count()).FirstOrDefault();
            foreach ( TourRequest t in groupedRequests)
            {
                return t.Location;
            }
            return null;
        }

        public string GetMostLanguage()
        {
            var groupedRequests = GetAll().GroupBy(r => r.Language).OrderByDescending(g => g.Count()).FirstOrDefault();
            foreach (TourRequest t in groupedRequests)
            {
                return t.Language;
            }
            return "";
        }

        public TourRequest GetById(int id)
        {
            return GetAll().FirstOrDefault(t => t.Id == id);
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

        public int GenerateLocationId()
        {
            List<Location> locations = _locationRepository.GetAll();

            if (locations.Count == 0)
            {
                return 0;
            }
            else
            {
                return locations[locations.Count - 1].Id + 1;
            }
        }



        public TourRequest Save (Location location, string description, string language, int maxNumberOfPeople, DateTime startDate, DateTime endDate, RequestStatus status, int userId)
        {   
            List<TourRequest> requests = GetAll();
            TourRequest tourRequest = new TourRequest();
            tourRequest.Id = GenerateId();
            tourRequest.Location = new Location();
            tourRequest.Location.Id = GenerateLocationId();
            tourRequest.Location = location;
            _locationRepository.Save(location);
            tourRequest.StartDate = startDate;
            tourRequest.Language = language;
            tourRequest.EndDate = endDate;
            tourRequest.Description = description;
            tourRequest.Status = status;
            tourRequest.MaxNumberOfPeople = maxNumberOfPeople;
            tourRequest.User.Id = userId;
            requests.Add(tourRequest);
            _serializer.ToCSV(_filePath, requests);
            return tourRequest;
        }

        public void Update(TourRequest tourRequest)
        {
            List<TourRequest> tourRequests = GetAll();
            tourRequests.RemoveAll(b => b.Id == tourRequest.Id);
            tourRequests.Add(tourRequest);
            _serializer.ToCSV(_filePath, tourRequests);
        }

        public Dictionary<string, int> GetLanguageGraphData(int userId)
        {
            var languagesCount = new Dictionary<string, int>();
            List<TourRequest> requests = GetByUser(userId);

            foreach (var request in requests)
            {
                var language = request.Language;
                if (languagesCount.ContainsKey(language))
                {
                    languagesCount[language]++;
                }
                else
                {
                    languagesCount[language] = 1;
                }

            }

            return languagesCount;
        }

        public List<int> GetYearsOfRequests(int userId)
        {
            List<TourRequest> requests = GetByUser(userId);
            List<int> years = new List<int>();

            foreach (TourRequest request in requests)
            {
                if (request.Status == RequestStatus.Accepted || request.Status == RequestStatus.Rejected)
                {
                    years.Add(request.StartDate.Year);
                }


            }


            return years.Distinct().ToList();
        }



        public Dictionary<string, int> GetLocationGraphData(int userId)
        {
            var locationsCount = new Dictionary<string, int>();
            List<TourRequest> requests = GetByUser(userId);

            foreach (TourRequest request in requests)
            {
                string location = _locationRepository.GetById(request.Location.Id).City;
                if (locationsCount.ContainsKey(location))
                {
                    locationsCount[location]++;
                }
                else
                {
                    locationsCount[location] = 1;
                }

            }
            return locationsCount;
        }

    }
}
