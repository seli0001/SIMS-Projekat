﻿using SIMS.Domain.Model;
using SIMS.Repository.GuideRepository;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Repository
{
    class BookedTourRepository
    {
        private const string _filePath = "../../../../SIMS/Resources/Data/BookedTours.csv";

        private readonly TourRepository _tourRepository;
        private readonly UserRepository _userRepository;
        private readonly CheckpointRepository _checkpointRepository;
        private readonly Serializer<BookedTour> _serializer;
        public BookedTourRepository()
        {

            _serializer = new Serializer<BookedTour>();
            _tourRepository = new TourRepository();
            _userRepository = new UserRepository();
            _checkpointRepository = new CheckpointRepository();
        }

        public List<BookedTour> GetAll()
        {
            List<BookedTour> bookedTours = _serializer.FromCSV(_filePath);

            foreach (BookedTour bookedTour in bookedTours)
            {
                bookedTour.Tour = _tourRepository.GetById(bookedTour.TourId);
                bookedTour.User = _userRepository.GetAllUsers().Find(u => u.Id == bookedTour.UserId);
                if (bookedTour.Checkpoint != null)
                {
                    bookedTour.Checkpoint = _checkpointRepository.GetAll().FirstOrDefault(c => c.Id == bookedTour.Checkpoint.Id);
                }
            }
            return bookedTours;
        }

        public List<BookedTour> GetAllByTourId(int tourId)
        {
            List<BookedTour> bookedTours = GetAll();
            return bookedTours.Where(bookedTour => bookedTour.Tour.Id == tourId).ToList();
        }
        
        public BookedTour GetById(int id)
        {
            List<BookedTour> bookedTours = GetAll();
            return bookedTours.Find(bookedTour => bookedTour.Id == id);
        }

        private int GenerateId()
        {
            List<BookedTour> bookedTours = GetAll();

            if (bookedTours.Count == 0)
            {
                return 0;
            }
            else
            {
                return bookedTours[bookedTours.Count - 1].Id + 1;
            }
        }

        public void Save(Tour tour, int iduser)
        {
            List<BookedTour> bookedTours = GetAll();

            BookedTour bookedTour = new BookedTour();
            bookedTour.Tour = tour;
            bookedTour.TourId = tour.Id;
            bookedTour.UserId = iduser;
            bookedTour.Id = GenerateId();

            bookedTours.Add(bookedTour);
            _serializer.ToCSV(_filePath, bookedTours);
        }

        public void Update(BookedTour bookedTour)
        {
            List<BookedTour> bookedTours = GetAll();
            bookedTours.RemoveAll(b => b.Id == bookedTour.Id);
            bookedTours.Add(bookedTour);
            _serializer.ToCSV(_filePath, bookedTours);
        }
    }
}
