using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS.Model;
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
        
        public List<User> GetAllUsers()
        {
            return serializer.FromCSV(filePath);
        }
    }
}
