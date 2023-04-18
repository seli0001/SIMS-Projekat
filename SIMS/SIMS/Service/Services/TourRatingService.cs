using SIMS.Domain.Model;
using SIMS.Domain.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Service.Services
{
    internal class TourRatingService
    {
        private readonly ITourRatingRepository _tourRatingRepository;

        public TourRatingService()
        {
            _tourRatingRepository = Injector.Injector.CreateInstance<ITourRatingRepository>();
        }

        public List<TourRating> GetAll()
        {
            return _tourRatingRepository.GetAll();
        }

        public List<TourRating> GetAllByTourId(int tourId) 
        {
            return GetAll().Where(t => t.bookedTour.TourId == tourId).ToList();
        }

        public void SaveRating(TourRating rating)
        {
            _tourRatingRepository.Update(rating);
        }

        public void Save(BookedTour bookedTour, int idUser, int znanjeVodica, int jezikVodica, int zanimljivostTure, string com, List<string> images)
        {
            _tourRatingRepository.Save(bookedTour, idUser, znanjeVodica, jezikVodica, zanimljivostTure, com, images);
        }

        public void ReportRating(int ratingId)
        {
            TourRating reported = GetAll().FirstOrDefault(r => r.Id == ratingId);
            if(reported == null)
            {
                return;
            }
            reported.Valid = false;
            SaveRating(reported);
        }
    }
}
