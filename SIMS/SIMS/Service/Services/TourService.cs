using SIMS.Domain.Model.Guide;
using SIMS.Repository.GuideRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Service.Services
{
    internal class TourService
    {
        private readonly TourRepository _tourRepository;

        public TourService() {
            _tourRepository = new TourRepository();
        }

        public List<Tour> GetNotStarted()
        {
            return _tourRepository.GetAll().Where(t => t.Status.ToString().Equals("NOT_STARTED")).ToList();
        }

        public List<Tour> GetAll()
        {
            return _tourRepository.GetAll();
        }

        public List<Tour> GetAlternative(Tour tour)
        {
            List<Tour> tours = GetAll();
            List<Tour> alternativeTours = new List<Tour>();
            return _tourRepository.GetAlternativeTours(tours, alternativeTours, tour);
        }

        public void UpdateNumberOfPeople(Tour tour, int tourid)
        {
         _tourRepository.UpdateNumberOfPeople(tour, tourid);
        }

    }
}
