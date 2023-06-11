using SIMS.Domain.Model;
using SIMS.Model;
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
        private readonly SuperOwnerRepository _superOwnerRepository;

        public AccommodationRepository()
        {
            _serializer = new Serializer<Accommodation>();
            _accommodations = _serializer.FromCSV(_filePath);

            _locationRepository = new LocationRepository();
            _imageRepository = new ImageRepository();
            _superOwnerRepository = new SuperOwnerRepository();

        }

        public List<Accommodation> GetAll()
        {
            _accommodations = _serializer.FromCSV(_filePath);
            foreach (Accommodation accommodation in _accommodations)
            {
                accommodation.Location = _locationRepository.GetById(accommodation.Location.Id);
                foreach (Image image in _imageRepository.GetByAccommodation(accommodation))
                {
                    accommodation.Images.Add(image);
                }
            }
            return _accommodations;
        }

        public Accommodation GetById(int id)
        {
            _accommodations = GetAll();
            return _accommodations.Find(c => c.Id == id);
        }

        public List<Accommodation> GetForView()
        {
            _accommodations = GetAll();
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
            _accommodations = GetAll();
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
            _accommodations = GetAll();
            return _accommodations.FindAll(c => c.User.Id == user.Id);
        }
        public void makeSuperOwner(User user)
        {
            _accommodations = GetByUser(user);
            _superOwnerRepository.Save(user);
            foreach (Accommodation accommodation in _accommodations)
            {
                accommodation.Super = true;
              //  accommodation.Name = "*" + accommodation.Name;
            }
            _serializer.ToCSV(_filePath, _accommodations);
        }

        public void deleteSuperOwner(User user)
        {
            _accommodations = GetByUser(user);
            _superOwnerRepository.Delete(user);
            foreach (Accommodation accommodation in _accommodations)
            {
                accommodation.Super = false;
             //   accommodation.Name = accommodation.Name.Substring(1, accommodation.Name.Length - 1);
            }
            _serializer.ToCSV(_filePath, _accommodations);
        }

        public void MakeRenovated(Accommodation acc)
        {
            Accommodation accommodation = GetById(acc.Id);
            accommodation.Renovated = true;
            Update(accommodation);
        }

        public void RegulateRenovations()
        {
            _accommodations = GetAll();
            foreach(Accommodation acc in _accommodations)
            {
                if(acc.RenovatedOn < DateOnly.FromDateTime(DateTime.Today) && acc.RenovatedOn.AddYears(1) > DateOnly.FromDateTime(DateTime.Today))
                {
                    if(!acc.Renovated)
                    {
                        MakeRenovated(acc);
                    }
                }
                else
                {
                    if(acc.Renovated)
                    {
                        DeleteRenovation(acc);
                    }
                }
            }
        }

        public void Renovate(Accommodation acc, DateOnly date)
        {
            Accommodation accommodation = GetById(acc.Id);
            accommodation.RenovatedOn = date;
            Update(accommodation);
        }

        public void DeleteRenovation(Accommodation acc)
        {
            Accommodation accommodation = GetById(acc.Id);
            accommodation.RenovatedOn = DateOnly.FromDateTime(new DateTime(1970, 01, 01));
            if(accommodation.Renovated)
            {
                accommodation.Renovated = false;
            }
            Update(accommodation);
        }

    }
}
