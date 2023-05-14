using SIMS.Domain.Model;
using SIMS.Repository;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Service.UseCases
{
    public class LocationService
    {
        private LocationRepository _locationRepository;

        public LocationService()
        {
            _locationRepository = new LocationRepository();
        }

        public List<Location> GetAll()
        {
            return _locationRepository.GetAll();
        }
        public Location GetById(int id)
        {
            return _locationRepository.GetById(id);
        }
       

        public int IsExists(Location location)
        {
            return _locationRepository.IsExists(location);
        }
        public Location Save(string country, string city)
        {
            return _locationRepository.Save(new Location(country, city));
        }
        public Location Save(Location location)
        {
            return _locationRepository.Save(location);
        }
    }
}
