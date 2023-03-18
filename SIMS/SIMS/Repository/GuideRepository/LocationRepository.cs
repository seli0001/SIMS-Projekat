using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS.Model.Guide;
using SIMS.Serializer;

namespace SIMS.Repository.GuideRepository
{
    class LocationRepository
    {
        private const string _filePath = "../../../../SIMS/Resources/Data/Locations.csv";
        private readonly Serializer<Location> _serializer;

        public LocationRepository()
        {
            _serializer = new Serializer<Location>();
        }
        
        public int GetNextId(List<Location> locations)
        {
            if (locations.Count < 1)
            {
                return 0;
            } 
            return locations.Max(location => location.Id) + 1;
        }
        
        public Location GetById(int id)
        {
            List<Location> locations = GetAll();
            return locations.FirstOrDefault(location => location.Id == id);
        }
        
        public List<Location> GetAll()
        {
            return _serializer.FromCSV(_filePath);
        }
        
        public void Save(Location location)
        {
            List<Location> locations = GetAll();
            location.Id = GetNextId(locations);
            
            locations.Add(location);
            _serializer.ToCSV(_filePath, locations);
        }
    }
}
