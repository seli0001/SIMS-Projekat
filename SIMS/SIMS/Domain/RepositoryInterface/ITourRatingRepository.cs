using SIMS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Domain.RepositoryInterface
{
    internal interface ITourRatingRepository
    {
        List<TourRating> GetAll();
        int GenerateId();
        void Save(BookedTour bookedTour, int idUser, int znanjeVodica, int jezikVodica, int zanimljivostTure, string com, List<string> images);
        void Update(TourRating rating);
    }
}
