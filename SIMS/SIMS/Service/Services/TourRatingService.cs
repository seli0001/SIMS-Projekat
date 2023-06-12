using HarfBuzzSharp;
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
            return _tourRatingRepository.GetAllByTourId(tourId);
        }

        public void SaveRating(TourRating rating)
        {
            _tourRatingRepository.Update(rating);
        }

        public void Save(BookedTour bookedTour, int idUser, int guideKnown, int guideLanguage, int tourReview, string comment, List<string> images)
        {
            _tourRatingRepository.Save(bookedTour, idUser, guideKnown, guideLanguage, tourReview, comment, images);
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

        public string CheckSuperGuide()
        {
            List<TourRating> tourRatings = GetAll();
            var result = tourRatings.GroupBy(tr => tr.bookedTour.Tour.Language).Select(group => new { Language = group.Key, AverageRating = group.Average(tour => ((double)tour.TourReview + (double)tour.GuideKnown + (double)tour.GuideLanguage)/3.0), counts = group.Count() });
            foreach(var item in result)
            {
                if (item.counts > 5 && item.AverageRating > 4.5)
                {
                    return item.Language;
                }
            }
            return null;
        }
    }
}
