using SIMS.Domain.Model;
using SIMS.Model;
using SIMS.Repository;
using SIMS.WPF.ViewModel.OwnerViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Service.Services
{
    class CancelingRequestService
    {
        private readonly CancelingRequestsRepository _cancelingRequestsRepository;


        public CancelingRequestService()
        {
            _cancelingRequestsRepository = new CancelingRequestsRepository();
        }

        public List<CancelingRequests> GetAll()
        {
            return _cancelingRequestsRepository.GetAll();
        }

        public List<CancelingRequests> GetByUserId(int id)
        {
            return _cancelingRequestsRepository.GetByUserId(id);
        }

        public CancelingRequests Save(CancelingRequests request)
        {
            return _cancelingRequestsRepository.Save(request);
        }

        public void Delete(CancelingRequests request)
        {
            _cancelingRequestsRepository.Delete(request);
        }

        public CancelingRequests Update(CancelingRequests request)
        {
            return _cancelingRequestsRepository.Update(request);
        }

     
        public double GetCanceledResNumForYear(int year, Accommodation accommodation)
        {
            return _cancelingRequestsRepository.GetCanceledResNumForYear(year, accommodation);
        }

        public double GetCanceledResNumForMonth(int year, int month, Accommodation accommodation)
        {
            return _cancelingRequestsRepository.GetCanceledResNumForMonth(year, month, accommodation);
        }

        
    }
}
