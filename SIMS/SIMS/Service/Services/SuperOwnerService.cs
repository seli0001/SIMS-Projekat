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
    public class SuperOwnerService
    {
        private List<SuperOwner> _owners;
        private readonly SuperOwnerRepository _superOwnerRepository;

        public SuperOwnerService()
        {
            _owners = new List<SuperOwner>();
            _superOwnerRepository = new SuperOwnerRepository(); 
        }

        public List<SuperOwner> GetAll()
        {
            return _superOwnerRepository.GetAll();
        }
        public SuperOwner Save(User user)
        {
            return _superOwnerRepository.Save(user);
        }

        public void Delete(User user)
        {
            _superOwnerRepository.Delete(user);
        }

        public SuperOwner GetById(int id)
        {
           _owners = GetAll();
            return _owners.FirstOrDefault(owner => owner.Id == id);
        }

        public bool CheckById(int id)
        {
            _owners = GetAll();
            foreach (SuperOwner so in _owners)
            {
                if (so.Id == id) return true;
            }
            return false;
        }
    }
}
