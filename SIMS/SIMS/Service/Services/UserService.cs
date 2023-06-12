using SIMS.Domain.Model;
using SIMS.Repository;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Service.Services
{
    internal class UserService
    {
        private UserRepository _userRepository;

        public UserService()
        {
            _userRepository = new UserRepository();
        }
        public User GetUserByUsername(string username)
        {
            return _userRepository.GetUserByUsername(username);
        }

        public User GetById(int id)
        {
            return _userRepository.GetById(id);
        }


        public List<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public void Update(User updatedUser)
        {
            _userRepository.Update(updatedUser);    
        }
    }
}
