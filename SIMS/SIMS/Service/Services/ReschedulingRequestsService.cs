using SIMS.Domain.Model;
using SIMS.Model;
using SIMS.Repository;
using SIMS.Serializer;
using SIMS.WPF.ViewModel.OwnerViewModel;
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
        private readonly ReservationRepository _reservationRepository;

        private List<ReschedulingRequests> _requests;

        public ReschedulingRequestsService()
        {
            _reschedulingRequestsRepository = new ReschedulingRequestsRepository();
            _reservationRepository = new ReservationRepository();

            _requests = new List<ReschedulingRequests>();
        }

        public List<ReschedulingRequests> GetAll()
        {
            return _reschedulingRequestsRepository.GetAll();
        }

        public List<ReschedulingRequests> GetByOwnerId(int id)
        {
            return _reschedulingRequestsRepository.GetByOwnerId(id);
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

        public List<Available> checkAvailability(int id)
        {
            _requests = GetByOwnerId(id);
            List<Available> final = new List<Available>();
            foreach(ReschedulingRequests request in _requests)
            {
                if (checkIfAvailable(request)) final.Add(Available.AVAILABLE);
                else final.Add(Available.OCCUPIED);
            }

            return final;
        }

        private bool checkIfAvailable(ReschedulingRequests request)
        {
            DateOnly newStartDate = request.FromDate;
            DateOnly newEndDate = request.ToDate;

            return _reservationRepository.checkAvailabilityForAcc(request.Reservation, newStartDate, newEndDate);

        }

        public ReschedulingRequests AcceptRequest(ReschedulingRequests request)
        {
            request.Reservation.FromDate = request.FromDate;
            request.Reservation.ToDate = request.ToDate;
            request.Status = Status.APPROVED;
            _reservationRepository.Update(request.Reservation);
            request = _reschedulingRequestsRepository.Update(request);
            return request;
        }
    }
}
