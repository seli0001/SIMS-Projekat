using SIMS.Model;
using SIMS.Model.AccommodationModel;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Repository
{
    internal class SuperOwnerRepository
    {
        private const string _filePath = "../../../../SIMS/Resources/Data/SuperOwners.csv";
        private readonly Serializer<SuperOwner> _serializer;
        private List<SuperOwner> _owners;

        public SuperOwnerRepository()
        {
            _serializer = new Serializer<SuperOwner>();
        }
        public SuperOwner Save(User user)
        {
            SuperOwner superOwner = new SuperOwner();
            superOwner.Id = user.Id;
            _owners = _serializer.FromCSV(_filePath);
            _owners.Add(superOwner);
            _serializer.ToCSV(_filePath, _owners);
            return superOwner;
        }
        public void Delete(User user)
        {
            _owners = _serializer.FromCSV(_filePath);
            SuperOwner founded = _owners.Find(c => c.Id == user.Id);
            _owners.Remove(founded);
            _serializer.ToCSV(_filePath, _owners);
        }
        public int NextId()
        {
            _owners = _serializer.FromCSV(_filePath);
            if (_owners.Count < 1)
            {
                return 1;
            }
            return _owners.Max(c => c.Id) + 1;
        }
        public SuperOwner GetById(int id)
        {
            List<SuperOwner> owners = GetAll();
            return owners.FirstOrDefault(owner => owner.Id == id);
        }

        public bool CheckById(int id)
        {
            List<SuperOwner> owners = GetAll();
            foreach(SuperOwner so in owners)
            {
                if(so.Id == id) return true;
            }
            return false;
        }

        public List<SuperOwner> GetAll()
        {
            return _serializer.FromCSV(_filePath);
        }
    }
}
