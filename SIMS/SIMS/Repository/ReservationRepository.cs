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

    }
}
