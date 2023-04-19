using SIMS.Domain.Model;
using SIMS.Repository;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SIMS.Service.Services
{
    public class ReservationService
    {
        private List<Reservation> _reservations;
        private readonly ReservationRepository _reservationRepository;

        public ReservationService()
        {
            _reservations = new List<Reservation>();   
            _reservationRepository = new ReservationRepository();
        }

        public List<Reservation> GetAll()
        {
            return _reservationRepository.GetAll();
        }

        public List<Reservation> GetByAccommodationsId(int id)
        {
            return _reservationRepository.GetByAccommodationsId(id);
        }
        public List<Reservation> GetByUserId(int id)
        {
            return _reservationRepository.GetByUserId(id);
        }

        public Reservation Save(Reservation reservation)
        {
            return _reservationRepository.Save(reservation);
        }

        public Reservation Update(Reservation reservation)
        {
            return _reservationRepository.Update(reservation);
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
                if (checkEndDate(bookedReservations, reservation)) return false;
            }

            return true;
        }


        private bool checkEndDate(List<Reservation> bookedReservations, Reservation reservation)
        {
            foreach (Reservation reservation1 in bookedReservations)
            {
                if (reservation.FromDate.AddDays(reservation.TimeOfStay) > reservation1.FromDate && reservation.FromDate.AddDays(reservation.TimeOfStay) < reservation1.ToDate)
                {
                    MessageBox.Show("Unfortunately, we cannot book this for you because it is already booked in the given time. The first available day is " + GetFirstAvailableDate(reservation).ToString());
                    return true;
                }
            }
            return false;
        }

        public DateOnly GetFirstAvailableDate(Reservation reservation)
        {
            List<Reservation> bookedReservations = GetByAccommodationsId(reservation.Accommodation.Id);

            bookedReservations.Sort((r1, r2) => r1.FromDate.CompareTo(r2.FromDate));

            DateOnly availableDateFrom = reservation.FromDate;
            DateOnly availableDateTo = reservation.ToDate;

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
            foreach (Reservation res in _reservations)
            {
                bool inBetween = (fromDate > res.FromDate && fromDate < res.ToDate) || (toDate > res.FromDate && toDate < res.ToDate);
                if (res.Id != reservation.Id && (inBetween))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
