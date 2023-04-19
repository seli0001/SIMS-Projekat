using SIMS.Domain.Model;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS.Repository
{
    class ReservationRepository
    {
        private const string _filePath = "../../../../SIMS/Resources/Data/Reservations.csv";
        private readonly Serializer<Reservation> _serializer;
        private readonly AccommodationRepository _accommodationRepository;
        private List<Reservation> _reservations;

        public ReservationRepository()
        {
            _serializer = new Serializer<Reservation>();
            _accommodationRepository = new AccommodationRepository();
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

        public Reservation Save(Reservation reservation)
        {
            List<Reservation> reservations = GetAll();
            reservation.Id = GetNextId(reservations);

            reservations.Add(reservation);
            _serializer.ToCSV(_filePath, reservations);
            return reservation;
        }

        public bool AvailableAccommodation(Reservation reservation)
        {
            List<Reservation> bookedReservations = GetByAccommodationsId(reservation.Accommodation.Id);

            bookedReservations.Sort((r1, r2) => r1.FromDate.CompareTo(r2.FromDate));
            Reservation lastReservation = new Reservation() { FromDate = DateOnly.MinValue, ToDate = DateOnly.MinValue }; 

            foreach (Reservation bookedReservation in bookedReservations)
            {
                if (reservation.FromDate >= bookedReservation.FromDate && reservation.FromDate < bookedReservation.ToDate)
                {
                    
                    MessageBox.Show("Unfortunately, we cannot book this for you because it is already booked in the given time. The first available day is " + GetFirstAvailableDate(reservation).ToString());
                    return false;
                }
                foreach (Reservation reservation1 in bookedReservations)
                {
                    if (reservation.FromDate.AddDays(reservation.TimeOfStay) > reservation1.FromDate && reservation.FromDate.AddDays(reservation.TimeOfStay) < reservation1.ToDate)
                    {
                        MessageBox.Show("Unfortunately, we cannot book this for you because it is already booked in the given time. The first available day is " + GetFirstAvailableDate(reservation).ToString());
                        return false;
                    }
                }
            }

            return true;
        }

        public DateOnly GetFirstAvailableDate(Reservation reservation)
        {
            List<Reservation> bookedReservations = GetByAccommodationsId(reservation.Accommodation.Id);

            bookedReservations.Sort((r1, r2) => r1.FromDate.CompareTo(r2.FromDate));

            DateOnly availableDateFrom = reservation.FromDate;
            DateOnly availableDateTo= reservation.ToDate;

            foreach (Reservation bookedReservation in bookedReservations)
            {
                
                if (availableDateFrom < bookedReservation.FromDate && availableDateFrom.AddDays(reservation.TimeOfStay) < bookedReservation.FromDate)
                {
                    return availableDateFrom;
                }

                availableDateFrom = bookedReservation.ToDate;
            }
            return availableDateFrom;
        }

        public bool checkAvailabilityForAcc(Reservation reservation, DateOnly fromDate, DateOnly toDate)
        {
            _reservations = GetByAccommodationsId(reservation.Accommodation.Id);
            foreach(Reservation res in _reservations)
            {
                bool inBetween = (fromDate > res.FromDate && fromDate < res.ToDate) || (toDate > res.FromDate && toDate < res.ToDate);
                if (res.Id != reservation.Id  &&  (inBetween))
                {
                    return false;
                }
            }

            return true;
        }

    }
}
