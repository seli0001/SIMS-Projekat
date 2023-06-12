using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS.Serializer;

namespace SIMS.Domain.Model
{
    public enum ROLE
    {
        Owner,
        Guest1,
        Guest2,
        Guide,
    }

    public class User : ISerializable
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public ROLE Role { get; set; }
        public bool Active { get; set; }

        public User()
        {

        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Username,
                Password,
                Role.ToString(),
                Age.ToString(),
                Active.ToString(),
            };
            return csvValues;
        }

        public void FromCSV(string[] csvValues)
        {
            Id = int.Parse(csvValues[0]);
            Username = csvValues[1];
            Password = csvValues[2];
            Role = (ROLE)Enum.Parse(typeof(ROLE), csvValues[3]);
            Age = int.Parse(csvValues[4]);
            Active = bool.Parse(csvValues[5]);
        }
    }
}
