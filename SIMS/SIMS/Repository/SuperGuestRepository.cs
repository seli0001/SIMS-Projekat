using SIMS.Domain.Model;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Repository
{
    internal class SuperGuestRepository
    {
        private const string _filePath = "../../../../SIMS/Resources/Data/SuperGuests.csv";
        private readonly Serializer<SuperGuest> _serializer;
        private List<SuperGuest> _guests;

        public SuperGuestRepository()
        {
            _serializer = new Serializer<SuperGuest>();
        }

        public List<SuperGuest> GetAll()
        {
            return _serializer.FromCSV(_filePath);
        }
        public SuperGuest Save(User user)
        {
            SuperGuest superGuest = new SuperGuest();
            superGuest.Id = user.Id;
            _guests = GetAll();
            _guests.Add(superGuest);
            _serializer.ToCSV(_filePath, _guests);
            return superGuest;
        }
        public void Delete(User user)
        {
            _guests = GetAll();
            SuperGuest founded = _guests.Find(c => c.Id == user.Id);
            _guests.Remove(founded);
            _serializer.ToCSV(_filePath, _guests);
        }
        public int NextId()
        {
            _guests = GetAll();
            if (_guests.Count < 1)
            {
                return 1;
            }
            return _guests.Max(c => c.Id) + 1;
        }
    }
}
