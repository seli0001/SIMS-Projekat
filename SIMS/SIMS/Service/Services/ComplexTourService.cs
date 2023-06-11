using SIMS.Domain.Model;
using SIMS.Domain.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Service.Services
{
    internal class ComplexTourService
    {
        private IComplexTourRepository _complexTourRepository;
        public ComplexTourService()
        {
            _complexTourRepository = Injector.Injector.CreateInstance<IComplexTourRepository>();
        }

        public void ChangeStatus( int userId)
        {
            _complexTourRepository.ChangeStatus(userId);
        }

        public void add(string name,List<TourRequest> requests, int userId)
        {
            _complexTourRepository.add(name,requests,userId);
        }
  

       public List<ComplexTourRequest> GetByUser( int userId)
        {
            return _complexTourRepository.GetByUser(userId);
        }


    }
}
