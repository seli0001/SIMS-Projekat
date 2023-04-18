using SIMS.Domain.Model;
using SIMS.Domain.RepositoryInterface;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Repository
{
    public class TourRatingRepository : ITourRatingRepository
    {
        private const string _filePath = "../../../../SIMS/Resources/Data/TourRating.csv";

        private readonly Serializer<TourRating> _serializer;
        private readonly UserRepository _userRepository;
        private readonly BookedTourRepository _bookedTourRepository;

        public TourRatingRepository()
        {
            _serializer = new Serializer<TourRating>();
            _bookedTourRepository = new BookedTourRepository();
            _userRepository = new UserRepository();
        }

        public List<TourRating> GetAll()
        {
            List<TourRating> ratings = _serializer.FromCSV(_filePath);
            ratings.ForEach(r => r.bookedTour = _bookedTourRepository.GetById(r.bookedTour.Id));
            return ratings;
        }

        public int GenerateId()
        {


            List<TourRating> reviews = GetAll();

            if (reviews.Count == 0)
            {
                return 0;
            }
            else
            {
                return reviews[reviews.Count - 1].Id + 1;
            }
        }

        public void Update(TourRating rating)
        {
            List<TourRating> reviews = GetAll();
            int index = reviews.FindIndex(r => r.Id == rating.Id);
            if (index == -1)
            {
                return;
            }
            reviews[index] = rating;
            _serializer.ToCSV(_filePath, reviews);
        }

        public void Save(BookedTour bookedTour, int idUser, int znanjeVodica, int jezikVodica, int zanimljivostTure, string com, List<string> images)
        {
            List<TourRating> reviews = GetAll();
            TourRating guestTourReview = new TourRating();
            guestTourReview.Id = GenerateId();
            guestTourReview.GuideLanguage = jezikVodica;
            guestTourReview.GuideKnown = znanjeVodica;
            guestTourReview.TourReview = zanimljivostTure;
            guestTourReview.Comment = com;
            guestTourReview.bookedTour = bookedTour;
            guestTourReview.bookedTour.Id = bookedTour.Id;
            guestTourReview.GuideId = bookedTour.Tour.Guide.Id;
            guestTourReview.Images = images;
            reviews.Add(guestTourReview);
            _serializer.ToCSV(_filePath, reviews);
        }




    }
}
