﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SIMS.Domain.Model;
using SIMS.Serializer;

namespace SIMS.Repository
{
    class OwnerRatingRepository
    {
        private const string _filePath = "../../../../SIMS/Resources/Data/OwnerRatings.csv";

        private readonly Serializer<OwnerRating> _serializer;

        private readonly UserRepository _userRepository;
        private readonly ReservationRepository _reservationRepository;

        private List<OwnerRating> _ratings;

        public OwnerRatingRepository()
        {
            _serializer = new Serializer<OwnerRating>();

            _ratings = _serializer.FromCSV(_filePath);
            _userRepository = new UserRepository();
            _reservationRepository = new ReservationRepository();
        }

        public List<OwnerRating> GetAll()
        {
            _ratings = _serializer.FromCSV(_filePath);
            foreach (OwnerRating rating in _ratings)
            {
                rating.User = _userRepository.GetById(rating.User.Id);
                rating.Reservation = _reservationRepository.GetById(rating.Reservation.Id);
            }
            return _ratings;
        }

        public OwnerRating Save(OwnerRating rating)
        {
            rating.Id = NextId();
            _ratings = GetAll();
            _ratings.Add(rating);
            _serializer.ToCSV(_filePath, _ratings);
            return rating;
        }

        public int NextId()
        {
            _ratings = GetAll();
            if (_ratings.Count < 1)
            {
                return 1;
            }
            return _ratings.Max(c => c.Id) + 1;
        }

        public void Delete(OwnerRating rating)
        {
            _ratings = GetAll();
            OwnerRating founded = _ratings.Find(r => r.Id == rating.Id);
            if (founded == null) return;
            _ratings.Remove(founded);
            _serializer.ToCSV(_filePath, _ratings);
        }

        public OwnerRating Update(OwnerRating rating)
        {
            _ratings = GetAll();
            OwnerRating current = _ratings.Find(r => r.Id == rating.Id);
            if (current == null) return null;
            int index = _ratings.IndexOf(current);
            _ratings.Remove(current);
            _ratings.Insert(index, rating);
            _serializer.ToCSV(_filePath, _ratings);
            return rating;
        }

        public List<OwnerRating> GetByReservation(Reservation reservation)
        {
            _ratings = GetAll();
            return _ratings.FindAll(c => c.Reservation.Id == reservation.Id);
        }

        public List<OwnerRating> GetByOwnerId(int id)
        {
            _ratings = GetAll();
            return _ratings.FindAll(r => r.Reservation.Accommodation.User.Id == id);
        }

        public List<OwnerRating> GetByUserId(int id)
        {
            List<OwnerRating> ownerRatings = GetAll();
            List<Reservation> reservations = _reservationRepository.GetByUserId(id);
            foreach (Reservation reservation in reservations)
            {
                ownerRatings.Where(ownerRating => ownerRating.Reservation == reservation);
            }
            return ownerRatings;
        }
    }
}
