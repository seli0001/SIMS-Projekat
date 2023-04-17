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

        public void Save(BookedTour bookedTour, int idUser, int znanjeVodica, int jezikVodica, int zanimljivostTure, string com, List<string> images)
        {
            _tourRatingRepository.Save(bookedTour, idUser, znanjeVodica, jezikVodica, zanimljivostTure, com, images);
        }

    }
}
