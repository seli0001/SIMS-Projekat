using SIMS.Domain.Model;
using SIMS.Model;
using SIMS.Repository;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Service.Services
{
    public class ReschedulingRequestsService
    {
        private readonly ReschedulingRequestsRepository _reschedulingRequestsRepository;
        private List<ReschedulingRequests> _requests;
        public ReschedulingRequestsService()
        {
            _reschedulingRequestsRepository = new ReschedulingRequestsRepository();
            _requests = new List<ReschedulingRequests>();
        }

        public List<ReschedulingRequests> GetAll()
        {
            return _reschedulingRequestsRepository.GetAll();
        }

        public List<ReschedulingRequests> GetByUserId(int id)
        {
            return _reschedulingRequestsRepository.GetByUserId(id);
        }

        public ReschedulingRequests Save(ReschedulingRequests request)
        {
            return _reschedulingRequestsRepository.Save(request);
        }

        public void Delete(ReschedulingRequests request)
        {
            _reschedulingRequestsRepository.Delete(request);
        }

        public ReschedulingRequests Update(ReschedulingRequests request)
        {
            return _reschedulingRequestsRepository.Update(request);
        }

        public void AcceptRequest(ReschedulingRequests request, User owner)
        {

        }
    }
}
