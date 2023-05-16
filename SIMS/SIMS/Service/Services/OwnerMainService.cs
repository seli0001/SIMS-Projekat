using SIMS.Domain.Model;
using SIMS.Repository;
using SIMS.Service.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS.Service
{
    public class OwnerMainService
    {
        private readonly OwnerRatingRepository _ownerRatingRepository;


        public OwnerMainService()
        {
            _ownerRatingRepository = new OwnerRatingRepository();
        }

        public double getRating()
        {
            List<OwnerRating> ratings = new List<OwnerRating>(_ownerRatingRepository.GetAll());
            double score = 0;
            foreach (OwnerRating rating in ratings)
            {
                score += (rating.RulesRespect + rating.Cleanliness) / 2.0;
            }
            score /= ratings.Count;
            return score;
        }
        public int checkIsSuperOwner(User LoggedInUser)
        {
            List<OwnerRating> ratings = new List<OwnerRating>(_ownerRatingRepository.GetAll());
            SuperOwnerService superOwnerService = new SuperOwnerService();
            if (superOwnerService.CheckById(LoggedInUser.Id))
            {
                if (!(ratings.Count > 5 && checkRating(ratings)))
                    return 0;
            }
            else if (ratings.Count > 5 && checkRating(ratings))
                return 1;

            return 2;
        }

        public bool checkRating(List<OwnerRating> ratings)
        {
            double score = 0;
            foreach (OwnerRating rating in ratings)
            {
                score += (rating.RulesRespect + rating.Cleanliness) / 2.0;
            }
            score /= ratings.Count;
            if (score > 4.5) return true;
            else return false;
        }
    }
}
