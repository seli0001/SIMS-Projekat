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
        private const string filePath = "../../../../SIMS/Resources/Data/Locations.csv";
        private readonly Serializer<Location> serializer;

        public LocationRepository()
        {
            serializer = new Serializer<Location>();
        }
        
        public Location GetById(int id)
        {
            List<Location> locations = GetAll();
            return locations.FirstOrDefault(location => location.Id == id);
        }
        
        public List<Location> GetAll()
        {
            return serializer.FromCSV(filePath);
        }
        
        public void Save(Location location)
        {
            List<Location> locations = GetAll();
            if (locations == null)
            {
                locations = new List<Location>();
                location.Id = 0;
            }
            else
            {
                location.Id = locations.Max(loc => loc.Id) + 1;
            }
            
            locations.Add(location);
            serializer.ToCSV(filePath, locations);
        }
    }
}
