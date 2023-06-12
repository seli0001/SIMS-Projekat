using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS.Domain.Model;
using SIMS.Serializer;

namespace SIMS.Repository
{
    internal class UserRepository
    {

        private const string filePath = "../../../../SIMS/Resources/Data/Users.csv";
        private readonly Serializer<User> serializer;

        
        public UserRepository()
        {
            serializer = new Serializer<User>();
        }
        
        public User GetUserByUsername(string username)
        {
            List<User> users = GetAllUsers();
            return users.FirstOrDefault(user => user.Username == username);
        }

        public User GetById(int id)
        {
            List<User> users = GetAllUsers();
            return users.FirstOrDefault(user => user.Id == id);
        }


        public List<User> GetAllUsers()
        {
            return serializer.FromCSV(filePath);
        }

        public void Update(User updatedUser)
        {
            List<User> users = GetAllUsers();
            users.RemoveAll(u => u.Id == updatedUser.Id);
            users.Add(updatedUser);
            serializer.ToCSV(filePath, users);
        }        
    }
}
