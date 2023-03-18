using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS.Model.Guide;
using SIMS.Serializer;

namespace SIMS.Repository.GuideRepository
{
    class TourRepository
    {
        private const string filePath = "../../../../SIMS/Resources/Data/Tours.csv";
        private readonly Serializer<Tour> serializer;
        private readonly LocationRepository locationRepository;

        private readonly StartTimeRepository startTimeRepository;
        private readonly ImageRepository imageRepository;
        private readonly CheckpointRepository checkpointRepository;
        public TourRepository()
        {
            serializer = new Serializer<Tour>();
            locationRepository = new LocationRepository();
            startTimeRepository = new StartTimeRepository();
            imageRepository = new ImageRepository();
            checkpointRepository = new CheckpointRepository();
        }
        
        public int GetNextId(List<Tour> tours)
        {
            if (tours.Count < 1)
            {
                return 0;
            } 
            return tours.Max(tour => tour.Id) + 1;
        }

        public Tour GetById(int id)
        {
            List<Tour> tours = GetAll();
            Tour tour = tours.FirstOrDefault(tour => tour.Id == id);
            if(tour != null){
                tour.Location = locationRepository.GetById(tour.Location.Id);
                tour.StartTime = startTimeRepository.GetById(tour.StartTime.Id);
                tour.Images = imageRepository.GetByTourId(tour.Id);
                tour.Checkpoints = checkpointRepository.GetByTourId(tour.Id);
            }

            return tour;
        }

        public List<Tour> GetAll()
        {
            List<Tour> tours = serializer.FromCSV(filePath);
            foreach (Tour tour in tours)
            {
                tour.Location = locationRepository.GetById(tour.Location.Id);
                tour.StartTime = startTimeRepository.GetById(tour.StartTime.Id);
                tour.Images = imageRepository.GetByTourId(tour.Id);
                tour.Checkpoints = checkpointRepository.GetByTourId(tour.Id);
            }

            return tours;
        }

        public void Save(Tour tour)
        {
            List<Tour> tours = GetAll();
            tour.Id = GetNextId(tours);
            
            foreach (var image in tour.Images)
            {
                image.Tour = tour;
            }
            foreach (var checkpoint in tour.Checkpoints)
            {
                checkpoint.Tour = tour;
            }
            
            locationRepository.Save(tour.Location);
            startTimeRepository.Save(tour.StartTime);
            imageRepository.SaveAll(tour.Images);
            checkpointRepository.SaveAll(tour.Checkpoints);
            
            tours.Add(tour);
            serializer.ToCSV( filePath, tours);
        }
    }
}
