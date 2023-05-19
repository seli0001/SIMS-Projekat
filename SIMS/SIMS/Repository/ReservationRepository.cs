using SIMS.Domain.Model;
using SIMS.Model;
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

        public ReservationRepository()
        {
            _serializer = new Serializer<Reservation>();
            _accommodationRepository = new AccommodationRepository();
        }

        public int GetNextId(List<Reservation> reservations)
        {
            if (reservations.Count < 1)
            {
                return 0;
            }
            return reservations.Max(reservation => reservation.Id) + 1;
        }

        public Reservation GetById(int id)
        {
            List<Reservation> reservations = GetAll();
            foreach(Reservation res in reservations)
            {
                res.Accommodation = _accommodationRepository.GetById(res.Accommodation.Id);
            }
            return reservations.FirstOrDefault(reservation => reservation.Id == id);
        }

        public List<Reservation> GetByAccommodationsId(int id)
        {
            List<Reservation> reservations = GetAll();
            return reservations.Where(reservation => reservation.Accommodation.Id == id).ToList();
        }
        public List<Reservation> GetByUserId(int id)
        {
            List<Reservation> reservations = GetAll();
            return reservations.Where(reservation => reservation.User.Id == id).ToList();
        }

        public List<Reservation> GetAll()
        {
            return _serializer.FromCSV(_filePath);
        }

        public Reservation Save(Reservation reservation)
        {
            List<Reservation> reservations = GetAll();
            reservation.Id = GetNextId(reservations);

            reservations.Add(reservation);
            _serializer.ToCSV(_filePath, reservations);
            return reservation;
        }

        public Accommodation GetAccommodationById(int id)
        {
            Reservation reservation = GetById(id);
            return reservation.Accommodation;
        }

        public bool AvailableAccommodation(Reservation reservation)
        {
            List<Reservation> bookedReservations = GetByAccommodationsId(reservation.Accommodation.Id);

            bookedReservations.Sort((r1, r2) => r1.FromDate.CompareTo(r2.FromDate));
            Reservation lastReservation = new Reservation() { FromDate = DateTime.MinValue, ToDate = DateTime.MinValue }; 

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

        public bool AvailableReschedulingAccommodation(ReschedulingRequests reservation, Accommodation accommodation)
        {
            List<Reservation> bookedReservations = GetByAccommodationsId(accommodation.Id);
            int timeOfStay = ((int)(reservation.ToDate - reservation.FromDate).TotalDays); 

            bookedReservations.Sort((r1, r2) => r1.FromDate.CompareTo(r2.FromDate));
            Reservation lastReservation = new Reservation() { FromDate = DateTime.MinValue, ToDate = DateTime.MinValue };

            foreach (Reservation bookedReservation in bookedReservations)
            {
                if (reservation.FromDate >= bookedReservation.FromDate && reservation.FromDate < bookedReservation.ToDate)
                {

                    MessageBox.Show("Unfortunately, we cannot book this for you because it is already booked in the given time." );
                    return false;
                }
                foreach (Reservation reservation1 in bookedReservations)
                {
                    if (reservation.FromDate.AddDays(timeOfStay) > reservation1.FromDate && reservation.FromDate.AddDays(timeOfStay) < reservation1.ToDate)
                    {
                        MessageBox.Show("Unfortunately, we cannot book this for you because it is already booked in the given time.");
                        return false;
                    }
                }
            }

            return true;
        }


        public DateTime GetFirstAvailableDate(Reservation reservation)
        {
            List<Reservation> bookedReservations = GetByAccommodationsId(reservation.Accommodation.Id);

            bookedReservations.Sort((r1, r2) => r1.FromDate.CompareTo(r2.FromDate));

            DateTime availableDateFrom = reservation.FromDate;
            DateTime availableDateTo= reservation.ToDate;

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

    }
}
