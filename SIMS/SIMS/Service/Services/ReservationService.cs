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
        private readonly RenovationRepository _renovationRepository;

        public ReservationService()
        {
            _reservations = new List<Reservation>();   
            _reservationRepository = new ReservationRepository();
            _renovationRepository = new RenovationRepository();
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
            List<Reservation> allReservations = GetByAccommodationsId(reservation.Accommodation.Id);
            List<Renovation> allRenovations = _renovationRepository.GetByAccommodationsId(reservation.Accommodation.Id);

            //Start and End dates of reservation we are trying to make
            DatesDTO newResDate = new DatesDTO(reservation.FromDate, reservation.ToDate);

            if (checkIfReserved(allReservations, newResDate)) return false;
            if(checkIfRenovated(allRenovations, newResDate)) return false;

            return true;
        }

        private bool checkIfReserved(List<Reservation> allReservationsForAcc, DatesDTO newReservationDate)
        {
            foreach (Reservation currentReservation in allReservationsForAcc)
            {
                //Start and End dates of every one of the reservations in the list
                DatesDTO currentResDate = new DatesDTO(currentReservation.FromDate, currentReservation.ToDate);
                if (!checkDate(newReservationDate, currentResDate))
                {
                    MessageBox.Show("Unfortunately, we cannot book this for you because it is already booked in the given time. The first available day is " + GetFirstAvailableDate(currentReservation).ToString());
                    return true;
                }
            }
            return false;
        }

        private bool checkIfRenovated(List<Renovation> allRenovationsForAcc, DatesDTO newReservationDate)
        {
            foreach (Renovation currentRenovation in allRenovationsForAcc)
            {
                DatesDTO currentResDate = new DatesDTO(currentRenovation.StartDate, currentRenovation.EndDate);
                if (!checkDate(newReservationDate, currentResDate))
                {
                    MessageBox.Show("Unfortunately, we cannot book this for you because it is under construction");
                    return true;
                }
            }
            return false;
        }
        private bool checkDate(DatesDTO newDate, DatesDTO currentDate)
        {
            if ((newDate.StartDate >= currentDate.StartDate && newDate.StartDate <= currentDate.EndDate) ||
                 (newDate.EndDate >= currentDate.StartDate && newDate.EndDate <= currentDate.EndDate))
                return false;

            return true;
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
