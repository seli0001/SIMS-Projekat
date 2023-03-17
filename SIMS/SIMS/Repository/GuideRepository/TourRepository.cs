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
        private readonly CheckPointRepository checkPointRepository;
        public TourRepository()
        {
            serializer = new Serializer<Tour>();
            locationRepository = new LocationRepository();
            startTimeRepository = new StartTimeRepository();
            imageRepository = new ImageRepository();
            checkPointRepository = new CheckPointRepository();
        }

        public Tour GetTourById(int id)
        {
            List<Tour> tours = GetAll();
            Tour tour = tours.FirstOrDefault(tour => tour.Id == id);
            if(tour != null){
                tour.Location = locationRepository.GetById(tour.Location.Id);
                tour.StartTime = startTimeRepository.GetById(tour.StartTime.Id);
                tour.Images = imageRepository.GetByTourId(tour.Id);
                tour.CheckPoints = checkPointRepository.GetByTourId(tour.Id);
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
                tour.CheckPoints = checkPointRepository.GetByTourId(tour.Id);
            }

            return tours;
        }

        public void Save(Tour tour)
        {
            List<Tour> tours = GetAll();
            if(tours == null)
            {
                tours = new List<Tour>();
                tour.Id = 0;
            }
            else
            {
                tour.Id = tours.Max(tour => tour.Id) + 1;
            }

            tours.Add(tour);
            serializer.ToCSV( filePath, tours);
        }
    }
}
