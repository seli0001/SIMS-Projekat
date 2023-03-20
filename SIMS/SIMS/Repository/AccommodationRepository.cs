using SIMS.Model;
using SIMS.Model.AccommodationModel;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace SIMS.Repository
{
    public class AccommodationRepository
    {
        private const string _filePath = "../../../../SIMS/Resources/Data/Accommodations.csv";

        private readonly Serializer<Accommodation> _serializer;

        private List<Accommodation> _accommodations;

        private readonly LocationRepository _locationRepository;

        private readonly ImageRepository _imageRepository;

        public AccommodationRepository()
        {
            _serializer = new Serializer<Accommodation>();

            _accommodations = _serializer.FromCSV(_filePath);

            _locationRepository = new LocationRepository();

            _imageRepository = new ImageRepository();
        }

        public List<Accommodation> GetAll()
        {
            return _serializer.FromCSV(_filePath);
        }

        public List<Accommodation> GetForView()
        {
            _accommodations = _serializer.FromCSV(_filePath);
            foreach (Accommodation accommodation in _accommodations)
            {
                accommodation.Location = _locationRepository.GetById(accommodation.Location.Id);
            }
            return _accommodations;
        }

        public Accommodation Save(Accommodation accommodation)
        {
            accommodation.Id = NextId();
            _accommodations = _serializer.FromCSV(_filePath);
            _accommodations.Add(accommodation);
            _serializer.ToCSV(_filePath, _accommodations);
            return accommodation;
        }

        public int NextId()
        {
            _accommodations = _serializer.FromCSV(_filePath);
            if (_accommodations.Count < 1)
            {
                return 1;
            }
            return _accommodations.Max(c => c.Id) + 1;
        }

        public void Delete(Accommodation accommodation)
        {
            _accommodations = _serializer.FromCSV(_filePath);
            Accommodation founded = _accommodations.Find(c => c.Id == accommodation.Id);
            _accommodations.Remove(founded);
            _serializer.ToCSV(_filePath, _accommodations);
        }

        public Accommodation Update(Accommodation accommodation)
        {
            _accommodations = _serializer.FromCSV(_filePath);
            Accommodation current = _accommodations.Find(c => c.Id == accommodation.Id);
            int index = _accommodations.IndexOf(current);
            _accommodations.Remove(current);
            _accommodations.Insert(index, accommodation);       
            _serializer.ToCSV(_filePath, _accommodations);
            return accommodation;
        }

        public List<Accommodation> GetByUser(User user)
        {
            _accommodations = _serializer.FromCSV(_filePath);
            foreach(Accommodation accommodation in _accommodations)
            {
               accommodation.Location = _locationRepository.GetById(accommodation.Location.Id);
               foreach(Image image in _imageRepository.GetByAccommodationId(accommodation.Id))
                {
                    accommodation.Images.Add(image);
                }
            }
            return _accommodations.FindAll(c => c.User.Id == user.Id);
        }

    }
}
