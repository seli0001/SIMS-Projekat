using SIMS.Domain.Model;
using SIMS.Repository;
using System;
using System.Collections.Generic;

namespace SIMS.Service
{
    public class RenovationService
    {
        private List<Renovation> _renovations;
        private readonly RenovationRepository _renovationRepository;

        public RenovationService()
        {
            _renovations = new List<Renovation>();
            _renovationRepository = new RenovationRepository();
        }

        public List<Renovation> GetAll()
        {
            return _renovationRepository.GetAll();
        }

        public List<Renovation> GetByAccommodationsId(int id)
        {
            return _renovationRepository.GetByAccommodationsId(id);
        }

        public Renovation Save(Renovation renovation)
        {
            return _renovationRepository.Save(renovation);
        }

        public Renovation Update(Renovation renovation)
        {
            return _renovationRepository.Update(renovation);
        }

        public List<DatesDTO> AvailableDates(DateOnly startTime, DateOnly endTime, int daysStaying, Accommodation accommodation)
        {
            return _renovationRepository.AvailableDates(startTime, endTime, daysStaying, accommodation);
        }

    }
}
