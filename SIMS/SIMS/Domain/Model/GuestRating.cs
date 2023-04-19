using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SIMS.Domain.Model
{
    public class GuestRating : ISerializable
    {
        public int Id { get; set; }
        public int Cleanliness { get; set; }
        public int RulesRespect { get; set; }
        public string Comment { get; set; }
        public User User { get; set; }
        public Reservation Reservation { get; set; }

        public GuestRating()
        {

        }

        public GuestRating(int cleanliness, int rulesRespect, string comment, User user, Reservation reservation)
        {
            Cleanliness = cleanliness;
            RulesRespect = rulesRespect;
            Comment = comment;
            User = user;
            Reservation = reservation;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
             Id.ToString(),
             Cleanliness.ToString(),
             RulesRespect.ToString(),
             Comment,
             User.Id.ToString(),
             Reservation.Id.ToString(),
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            if(values.Length != 0)
            {
                Id = Convert.ToInt32(values[0]);
                Cleanliness = Convert.ToInt32(values[1]);
                RulesRespect = Convert.ToInt32(values[2]);
                Comment = values[3];
                User = new User() { Id = Convert.ToInt32(values[4]) };
                Reservation = new Reservation() { Id = Convert.ToInt32(values[5]) };
            }
        }

    }
}
