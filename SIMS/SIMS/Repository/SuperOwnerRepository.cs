using SIMS.Domain.Model;
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

        public List<SuperOwner> GetAll()
        {
            return _serializer.FromCSV(_filePath);
        }
        public SuperOwner Save(User user)
        {
            SuperOwner superOwner = new SuperOwner();
            superOwner.Id = user.Id;
            _owners = GetAll();
            _owners.Add(superOwner);
            _serializer.ToCSV(_filePath, _owners);
            return superOwner;
        }
        public void Delete(User user)
        {
            _owners = GetAll();
            SuperOwner founded = _owners.Find(c => c.Id == user.Id);
            _owners.Remove(founded);
            _serializer.ToCSV(_filePath, _owners);
        }
        public int NextId()
        {
            _owners = GetAll();
            if (_owners.Count < 1)
            {
                return 1;
            }
            return _owners.Max(c => c.Id) + 1;
        }
    }
}
