using SIMS.Domain.Model;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace SIMS.Repository
{
    class ReservationRepository
    {
        private const string _filePath = "../../../../SIMS/Resources/Data/Reservations.csv";
        private readonly Serializer<Reservation> _serializer;

        private readonly AccommodationRepository _accommodationRepository;
        private readonly UserRepository _userRepository;

        private List<Reservation> _reservations;

        public ReservationRepository()
        {
            _serializer = new Serializer<Reservation>();
            _accommodationRepository = new AccommodationRepository();
            _userRepository = new UserRepository(); 
            _reservations = new List<Reservation>();
        }

        public int GetNextId(List<Reservation> reservations)
        {
            if (reservations.Count < 1)
            {
                return 0;
            }
            return reservations.Max(reservation => reservation.Id) + 1;
        }

        public List<Reservation> GetAll()
        {
            _reservations = _serializer.FromCSV(_filePath);
            foreach (Reservation res in _reservations)
            {
                res.Accommodation = _accommodationRepository.GetById(res.Accommodation.Id);
                res.User = _userRepository.GetById(res.User.Id);
                //Maybe res.User...
            }
            return _reservations;
        }

        public Reservation GetById(int id)
        {
            _reservations = GetAll();
            return _reservations.FirstOrDefault(reservation => reservation.Id == id);
        }

        public List<Reservation> GetByAccommodationsId(int id)
        {
            _reservations = GetAll();
            return _reservations.Where(reservation => reservation.Accommodation.Id == id).ToList();
        }
        public List<Reservation> GetByUserId(int id)
        {
            _reservations = GetAll();
            return _reservations.Where(reservation => reservation.User.Id == id).ToList();
        }

        public List<Reservation> GetByOwnerId(int id)
        {
            _reservations = GetAll();
            return _reservations.Where(reservation => reservation.Accommodation.User.Id == id).ToList();
        }



        public Reservation Save(Reservation reservation)
        {
            List<Reservation> reservations = GetAll();
            reservation.Id = GetNextId(reservations);

            reservations.Add(reservation);
            _serializer.ToCSV(_filePath, reservations);
            return reservation;
        }

        public Reservation Update(Reservation reservation)
        {
            _reservations = GetAll();
            Reservation current = _reservations.Find(r => r.Id == reservation.Id);
            if (current == null) return null;
            int index = _reservations.IndexOf(current);
            _reservations.Remove(current);
            _reservations.Insert(index, reservation);
            _serializer.ToCSV(_filePath, _reservations);
            return reservation;
        }

        public List<int> GetYearsForAccommodation(Accommodation accommodation)
        {
            List<int> years = new List<int>();
            _reservations = GetByAccommodationsId(accommodation.Id);
            
            foreach(Reservation res in _reservations)
            {
                if(!years.Contains(res.FromDate.Year))
                    years.Add(res.FromDate.Year);
            }

            return years;
        }


        private double getStatsForYear(Accommodation acc, int year)
        {
            List<Reservation> reservations = GetByAccommodationsId(acc.Id);
            double reservedDays = 0;
            foreach(Reservation res in reservations)
            {
                if(res.FromDate.Year == year)
                {
                    DateTime startDate = res.FromDate.ToDateTime(new TimeOnly());
                    DateTime endDate = res.ToDate.ToDateTime(new TimeOnly());

                    TimeSpan difference = endDate - startDate;
                    reservedDays += difference.Days;
                }
            }
            return (reservedDays / 365) * 100;
        }

        private double getStatsForMonth(Accommodation acc,int year, int month)
        {
            List<Reservation> reservations = GetByAccommodationsId(acc.Id);
            double reservedDays = 0;
            foreach (Reservation res in reservations)
            {
                if (res.FromDate.Year == year && res.FromDate.Month == month)
                {
                    DateTime startDate = res.FromDate.ToDateTime(new TimeOnly());
                    DateTime endDate = res.ToDate.ToDateTime(new TimeOnly());

                    TimeSpan difference = endDate - startDate;
                    reservedDays += difference.Days;
                }
            }
            return (reservedDays / 30) * 100;
        }
        

        public int GetBestYear(Accommodation accommodation)
        {
            List<int> years = new List<int>(GetYearsForAccommodation(accommodation));
            Dictionary<int, double> yearsDays = new Dictionary<int, double>();
            foreach(int year in years)
            {
                yearsDays[year] = getStatsForYear(accommodation, year);
            }

            return yearsDays.FirstOrDefault(x => x.Value == yearsDays.Values.Max()).Key;
        }

        public int GetBestMonth(Accommodation accommodation, int year)
        {
            List<int> months = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12};
            Dictionary<int, double> monthsDays = new Dictionary<int, double>();
            foreach (int month in months)
            {
                monthsDays[month] = getStatsForMonth(accommodation, year, month);
            }

            return monthsDays.FirstOrDefault(x => x.Value == monthsDays.Values.Max()).Key;
        }

       

        public double GetResNumForYear(int year, Accommodation accommodation)
        {
            double count = 0;
            List<Reservation> reservations = GetByAccommodationsId(accommodation.Id);
            foreach (Reservation res in reservations)
            {
                if (res.FromDate.Year == year)
                    count++;
            }

            return count;
        }
        public double GetResNumForMonth(int year, int month, Accommodation accommodation)
        {
            double count = 0;
            List<Reservation> reservations = GetByAccommodationsId(accommodation.Id);
            foreach (Reservation res in reservations)
            {
                if (res.FromDate.Year == year && res.FromDate.Month == month)
                    count++;
            }

            return count;
        }

        public int CountResForLocation(Location location)
        {
            _reservations = GetAll();
            int sum = 0;
            foreach(Reservation res in _reservations)
            {
                if(res.Accommodation.Location.Id == location.Id)
                {
                    sum++;
                }
            }

            return sum;
        }

    }
}
