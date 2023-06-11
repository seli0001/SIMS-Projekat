using SIMS.Domain.Model;
using SIMS.Domain.RepositoryInterface;
using SIMS.Model;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Repository
{
    public class ComplexTourRepository : IComplexTourRepository
    {
        private const string _filePath = "../../../../SIMS/Resources/Data/ComplexTourRequest.csv";

        private TourRequestRepository tourRequestRepository;

        private readonly Serializer<ComplexTourRequest> _serializer;


        public ComplexTourRepository ()
        {
            _serializer = new Serializer<ComplexTourRequest>();
            tourRequestRepository = new TourRequestRepository();

        }

        public void ChangeStatus(int userId)
        {
            List<ComplexTourRequest> requests = GetByUser(userId);

            foreach (ComplexTourRequest request in requests)
            {
                if (request.status == ComplexTourRequest.Status.Waiting)
                {
                    UpdateRequestStatus(request);
                }
            }

            _serializer.ToCSV(_filePath, requests);
        }

        private void UpdateRequestStatus(ComplexTourRequest request)
        {
            int accepted = 0;
            int rejected = 0;
            int waiting = 0;

            List<TourRequest> tourRequests = tourRequestRepository.GetRequestsById(request.TourRequestsId);

            foreach (TourRequest tourRequest in tourRequests)
            {
                if (tourRequest.Status == RequestStatus.Accepted)
                {
                    accepted++;
                }
                else if (tourRequest.Status == RequestStatus.Rejected)
                {
                    rejected++;
                }
                else if (tourRequest.Status == RequestStatus.Waiting)
                {
                    waiting++;
                }
            }

            if (accepted != 0 && waiting == 0 && rejected == 0)
            {
                request.status = ComplexTourRequest.Status.Accepted;
            }
            else if (accepted == 0 && waiting == 0 && rejected != 0)
            {
                request.status = ComplexTourRequest.Status.Rejected;
            }

            
            accepted = 0;
            rejected = 0;
            waiting = 0;
        }


        public void add(string name, List<TourRequest> tours,int userId)
        {
            List<int> ids = new List<int>();
           
            foreach(TourRequest t in tours)
            {
                TourRequest addedTour = tourRequestRepository.Save(t.Location, t.Description, t.Language, t.MaxNumberOfPeople, t.StartDate, t.EndDate, t.Status, t.User.Id);
                ids.Add(addedTour.Id);

            }

            ComplexTourRequest complexTourRequest = new ComplexTourRequest();
            complexTourRequest.Id = GenerateId();
            complexTourRequest.Name = name;
            complexTourRequest.TourRequestsId = ids;
            complexTourRequest.user.Id = userId;
            complexTourRequest.status = ComplexTourRequest.Status.Waiting;
            List<ComplexTourRequest> requests = GetAll();
            requests.Add(complexTourRequest);
            _serializer.ToCSV(_filePath, requests);
        }

        public int GenerateId()
        {
            List<ComplexTourRequest> requests = GetAll();

            if (requests.Count == 0)
            {
                return 0;
            }
            else
            {
                return requests[requests.Count - 1].Id + 1;
            }
        }

        public List<ComplexTourRequest> GetAll()
        {
            List<ComplexTourRequest> ComplexTourRequests = _serializer.FromCSV(_filePath);
            
            return ComplexTourRequests;
        }

        public List<ComplexTourRequest> GetByUser(int userId)
        {
            List<ComplexTourRequest> complexTourRequests = _serializer.FromCSV(_filePath);      
            return complexTourRequests.Where(t => t.user.Id == userId).ToList();
        }

    }
}
