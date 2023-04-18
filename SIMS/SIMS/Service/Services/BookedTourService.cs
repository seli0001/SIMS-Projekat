using SIMS.Domain.Model.Guide;
using SIMS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS.Repository;

namespace SIMS.Service.Services
{
    internal class BookedTourService
    {
        private readonly BookedTourRepository _bookedToursRepository;

        public BookedTourService()
        {
            _bookedToursRepository=new BookedTourRepository();
        }

        public List<BookedTour> GetByUser(int userId)
        {
            return _bookedToursRepository.GetAll().Where(u => userId == u.UserId).ToList();
        }

        public void Save(Tour tour,int userId)
        {
            _bookedToursRepository.Save(tour, userId);
        }
        public void Update(BookedTour bookedTour)
        {
            _bookedToursRepository.Update(bookedTour);
        }

        public List<BookedTour> GetUserFinished(int userId)
        {
            return _bookedToursRepository.GetAll().Where(t => t.UserId == userId && t.Tour.Status.ToString().Equals("FINISHED") && t.Review == false && t.Checkpoint != null && t.Notify.ToString().Equals("Accepted")).ToList();
        }

        public List<BookedTour> GetUserActive(int userId)
        {
            return _bookedToursRepository.GetAll().Where(t => t.UserId == userId && t.Tour.Status.ToString().Equals("STARTED") && t.Checkpoint != null && t.Notify.ToString().Equals("Accepted")).ToList();
        }


    }
}
