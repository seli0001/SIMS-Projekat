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
    class SuperGuestService
    {
        private List<SuperGuest> _guests;
        private readonly SuperGuestRepository _superGuestRepository;

        public SuperGuestService()
        {
            _guests = new List<SuperGuest>();
            _superGuestRepository = new SuperGuestRepository(); 
        }

        public List<SuperGuest> GetAll()
        {
            return _superGuestRepository.GetAll();
        }
        public SuperGuest Save(User user)
        {
            return _superGuestRepository.Save(user);
        }

        public void Delete(User user)
        {
            _superGuestRepository.Delete(user);
        }

        public SuperGuest GetById(int id)
        {
           _guests = GetAll();
            return _guests.FirstOrDefault(owner => owner.Id == id);
        }

        public bool CheckById(int id)
        {
            _guests = GetAll();
            foreach (SuperGuest so in _guests)
            {
                if (so.Id == id) return true;
            }
            return false;
        }
    }
}
