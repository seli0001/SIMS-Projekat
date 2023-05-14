using SIMS.Domain.Model;
using SIMS.Domain.RepositoryInterface;
using SIMS.Injector;
using SIMS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Service.Services
{
    public class TourRequestService
    {
        private readonly ITourRequestRepository _tourRequestRepository;
    
        public TourRequestService()
        {
            _tourRequestRepository = Injector.Injector.CreateInstance<ITourRequestRepository>();

        }

        public void Save(Location location, string description, string language, int maxNumberOfPeople, DateTime startDate, DateTime endDate, RequestStatus status,int userId)
        {
            _tourRequestRepository.Save(location,description,language, maxNumberOfPeople,startDate,endDate, status,userId);
        }
       
        public List<TourRequest> GetByUser(int userId)
        {
           return  _tourRequestRepository.GetByUser(userId);
        }

        public void Update(TourRequest tourRequest)
        {
            _tourRequestRepository.Update(tourRequest);
        }

    }
}
