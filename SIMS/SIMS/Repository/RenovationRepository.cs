using SIMS.Domain.Model;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Repository
{
    public class RenovationRepository
    {
        private const string _filePath = "../../../../SIMS/Resources/Data/Renovations.csv";
        private readonly Serializer<Renovation> _serializer;

        private readonly AccommodationRepository _accommodationRepository;
        private readonly ReservationRepository _reservationRepository;
        private List<Renovation> _renovations;

        public RenovationRepository()
        {
            _serializer = new Serializer<Renovation>();
            _accommodationRepository = new AccommodationRepository();
            _reservationRepository = new ReservationRepository();
            _renovations = new List<Renovation>();
        }

        public int GetNextId(List<Renovation> renovations)
        {
            if (renovations.Count < 1)
            {
                return 0;
            }
            return renovations.Max(renovation => renovation.Id) + 1;
        }

        public List<Renovation> GetAll()
        {
            _renovations = _serializer.FromCSV(_filePath);
            foreach (Renovation ren in _renovations)
            {
                ren.Accommodation = _accommodationRepository.GetById(ren.Accommodation.Id);
            }
            return _renovations;
        }

        public Renovation GetById(int id)
        {
            _renovations = GetAll();
            return _renovations.FirstOrDefault(renovation => renovation.Id == id);
        }

        public List<Renovation> GetByAccommodationsId(int id)
        {
            _renovations = GetAll();
            return _renovations.Where(renovation => renovation.Accommodation.Id == id).ToList();
        }
        public List<Renovation> GetByOwnerId(int id)
        {
            _renovations = GetAll();
            return _renovations.Where(renovation => renovation.Accommodation.User.Id == id).ToList();
        }

        public Renovation Save(Renovation renovation)
        {
            _renovations = GetAll();
            renovation.Id = GetNextId(_renovations);

            _accommodationRepository.Renovate(renovation.Accommodation, renovation.EndDate);

            _renovations.Add(renovation);
            _serializer.ToCSV(_filePath, _renovations);
            return renovation;
        }

        public Renovation Update(Renovation renovation)
        {
            _renovations = GetAll();
            Renovation current = _renovations.Find(r => r.Id == renovation.Id);
            if (current == null) return null;
            int index = _renovations.IndexOf(current);
            _renovations.Remove(current);
            _renovations.Insert(index, renovation);
            _serializer.ToCSV(_filePath, _renovations);
            return renovation;
        }

        public void Delete(Renovation renovation)
        {
            _renovations = GetAll();
            Renovation founded = _renovations.Find(c => c.Id == renovation.Id);
            _renovations.Remove(founded);

            _accommodationRepository.DeleteRenovation(renovation.Accommodation);

            _serializer.ToCSV(_filePath, _renovations);
        }

        public List<DatesDTO> AvailableDates(DateOnly startTime, DateOnly endTime, int daysStaying, Accommodation accommodation)
        {
            List<Reservation> reservations = new List<Reservation>(_reservationRepository.GetByAccommodationsId(accommodation.Id));
            List<Renovation> renovations = new List<Renovation>(GetByAccommodationsId(accommodation.Id));

            List<DatesDTO> dates = new List<DatesDTO>();
            while(startTime.AddDays(daysStaying - 1) <= endTime)
            {
                if(CheckDates(reservations, renovations, startTime, startTime.AddDays(daysStaying - 1)))
                {
                    DatesDTO date = new DatesDTO(startTime, startTime.AddDays(daysStaying - 1));
                    dates.Add(date);
                }
                startTime = startTime.AddDays(1);
            }
            return dates;
        }

        private bool CheckDates(List<Reservation> reservations, List<Renovation> renovations, DateOnly startDate, DateOnly endDate)
        {
            if(reservations.Count == 0)
            {
                return true;
            }
            if (!checkReservations(reservations, startDate, endDate)) return false;
            if (!checkRenovations(renovations, startDate, endDate)) return false;

            return true;
        }

        private bool checkReservations(List<Reservation> reservations, DateOnly startDate, DateOnly endDate)
        {
            foreach (Reservation res in reservations)
            {
                if ((startDate >= res.FromDate && startDate <= res.ToDate) || (endDate >= res.FromDate && endDate <= res.ToDate))
                    return false;
            }
            return true;
        }
        private bool checkRenovations(List<Renovation> renovations, DateOnly startDate, DateOnly endDate)
        {
            foreach (Renovation ren in renovations)
            {
                if ((startDate >= ren.StartDate && startDate <= ren.EndDate) || (endDate >= ren.StartDate && endDate <= ren.EndDate))
                    return false;
            }
            return true;
        }

        public List<Renovation> GetFromToDate(DateOnly startDate, DateOnly endDate)
        {
            _renovations = GetAll();
            List<Renovation> results = new List<Renovation>();
            foreach(Renovation ren in _renovations)
            {
                if(ren.StartDate > startDate && ren.StartDate < endDate && ren.EndDate > startDate && ren.EndDate < endDate)
                {
                    results.Add(ren);
                }
            }
            return results;
        }

    }

}
