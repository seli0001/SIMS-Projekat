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
        public int GenerateId();
        public void Save(Location location, string description, string language, int maxNumberOfPeople, DateTime startDate, DateTime endDate, RequestStatus status,int userId);
        public List<TourRequest> GetByUser(int userId);
        public void Update(TourRequest tourRequest);
    }
}
