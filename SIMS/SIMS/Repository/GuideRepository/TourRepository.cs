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
        private const string _filePath = "../../../../SIMS/Resources/Data/Tours.csv";
        private readonly Serializer<Tour> _serializer;
        private readonly LocationRepository _locationRepository;
        private readonly StartTimeRepository _startTimeRepository;
        private readonly ImageRepository _imageRepository;
        private readonly CheckpointRepository _checkpointRepository;
        public TourRepository()
        {
            _serializer = new Serializer<Tour>();
            _locationRepository = new LocationRepository();
            _startTimeRepository = new StartTimeRepository();
            _imageRepository = new ImageRepository();
            _checkpointRepository = new CheckpointRepository();
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
                tour.Location = _locationRepository.GetById(tour.Location.Id);
                tour.StartTime = _startTimeRepository.GetById(tour.StartTime.Id);
                tour.Images = _imageRepository.GetByTourId(tour.Id);
                tour.Checkpoints = _checkpointRepository.GetByTourId(tour.Id);
            }

            return tour;
        }

        public List<Tour> GetAlternative(Tour tour)
        {
            List<Tour> tours = GetAll();
            List<Tour> alternativeTours = new List<Tour>();
            //List<Tour> alternativeTour=GetAlternativeTours(tours,tour);

            return GetAlternativeTours(tours, alternativeTours, tour);
        }

        public List<Tour> GetAlternativeTours(List<Tour> tours, List<Tour> alternativeTours, Tour tour)
        {
            foreach (Tour t in tours)
            {
                if (tour.Location.Id == t.Location.Id && tour.Id != t.Id)
                {
                    alternativeTours.Add(t);
                }
            }
            return alternativeTours;
        }

        public void UpdateNumberOfPeople(Tour tour, int id)
        {
            List<Tour> tours = GetAll();

            foreach (Tour foreachTour in tours)
            {
                if (foreachTour.Id == id)
                {
                    foreachTour.NumberOfPeople =  tour.NumberOfPeople;

                }
            }
            _serializer.ToCSV(_filePath, tours);
        }



        public List<Tour> GetAll()
        {
            List<Tour> tours = _serializer.FromCSV(_filePath);
            foreach (Tour tour in tours)
            {
                tour.Location = _locationRepository.GetById(tour.Location.Id);
                tour.StartTime = _startTimeRepository.GetById(tour.StartTime.Id);
                tour.Images = _imageRepository.GetByTourId(tour.Id);
                tour.Checkpoints = _checkpointRepository.GetByTourId(tour.Id);
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
            
            _locationRepository.Save(tour.Location);
            _startTimeRepository.Save(tour.StartTime);
            _imageRepository.SaveAll(tour.Images);
            _checkpointRepository.SaveAll(tour.Checkpoints);
            
            tours.Add(tour);
            _serializer.ToCSV( _filePath, tours);
        }
    }
}
