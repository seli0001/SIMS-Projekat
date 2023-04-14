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

        public void Save(Tour tour,int userId)
        {
            _bookedToursRepository.Save(tour, userId);
        }
    }
}
