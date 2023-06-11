using SIMS.Domain.Model;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Domain.RepositoryInterface
{
    internal interface ITourRequestRepository
    {
        public List<TourRequest> GetAll();
        public List<TourRequest> GetRequestsById(List<int> ids);

        public int GenerateId();
        public TourRequest Save(Location location, string description, string language, int maxNumberOfPeople, DateTime startDate, DateTime endDate, RequestStatus status,int userId);
        public List<TourRequest> GetByUser(int userId);
        public void Update(TourRequest tourRequest);
        public Dictionary<string, int> GetLanguageGraphData(int userId);
        public Dictionary<string, int> GetLocationGraphData(int userId);
        public List<int> GetYearsOfRequests(int userId);

        public Location GetMostLocation();
        public string GetMostLanguage();
    }
}
