using SIMS.Domain.Model;
using SIMS.Domain.RepositoryInterface;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Repository
{
    public class TourRatingRepository : ITourRatingRepository
    {
        private const string _filePath = "../../../../SIMS/Resources/Data/TourRating.csv";

        private readonly Serializer<TourRating> _serializer;

        public TourRatingRepository()
        {
            _serializer = new Serializer<TourRating>();
        }

        public List<TourRating> GetAll()
        {
            return _serializer.FromCSV(_filePath);
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


        public void Save(BookedTour bookedTour, int idUser, int znanjeVodica, int jezikVodica, int zanimljivostTure, string com, List<string> images)
        {
            List<TourRating> reviews = GetAll();
            TourRating guestTourReview = new TourRating();
            guestTourReview.Id = GenerateId();
            guestTourReview.GuideLanguage = jezikVodica;
            guestTourReview.GuideKnown = znanjeVodica;
            guestTourReview.TourReview = zanimljivostTure;
            guestTourReview.Comment = com;
            guestTourReview.GuestId = idUser;
            guestTourReview.bookedTour = bookedTour;
            guestTourReview.bookedTourId = bookedTour.Id;
            guestTourReview.GuideId = bookedTour.Tour.Guide.Id;
            guestTourReview.Images = images;
            reviews.Add(guestTourReview);
            _serializer.ToCSV(_filePath, reviews);
        }




    }
}
