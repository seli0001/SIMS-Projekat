using SIMS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Domain.RepositoryInterface
{
    public interface IGuestRatingRepository
    {
        List<GuestRating> GetAll();
        GuestRating Save(GuestRating rating);
        void Delete(GuestRating rating);
        GuestRating Update(GuestRating rating);

    }
}
