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

        public ReservationRepository()
        {
            _serializer = new Serializer<Reservation>();
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
            return reservations.FirstOrDefault(reservation => reservation.Id == id);
        }

        public List<Reservation> GetByAccommodationId(int id)
        {
            List<Reservation> reservations = GetAll();
            return reservations.Where(reservation => reservation.Accommodation.Id == id).ToList();
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

        public Boolean AvailableAccommodation(Reservation reservation)
        {
            List<Reservation> bookedReservations = GetByAccommodationId(reservation.Accommodation.Id);
            foreach (Reservation bookedReservation in bookedReservations)
            {
                if (reservation.FromDate >= bookedReservation.FromDate && reservation.FromDate < bookedReservation.ToDate)
                {
                    MessageBox.Show("Unfortunately, we cannot book this for you because it is already booked in the given time. The first available day is " + bookedReservation.ToDate);
                    return false;
                }
            }
            return true;
        }


        public DateTime GetFirstAvailableDate(Reservation reservation)
        {
            List<Reservation> bookedReservations = GetByAccommodationId(reservation.Accommodation.Id);

            bookedReservations.Sort((r1, r2) => r1.FromDate.CompareTo(r2.FromDate));

            DateTime availableDate = reservation.FromDate;

            foreach (Reservation bookedReservation in bookedReservations)
            {
                if (availableDate < bookedReservation.FromDate)
                {
                    return availableDate;
                }

                availableDate = bookedReservation.ToDate;
            }
            return availableDate;
        }

    }
}
