using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Model
{
    class OwnerRating : ISerializable
    {
        public int Id { get; set; }
        public int Cleanliness { get; set; }
        public int RulesRespect { get; set; }
        public string Comment { get; set; }
        public User User { get; set; }
        public Reservation Reservation { get; set; }
        //Images

        public OwnerRating()
        {

        }

        public OwnerRating(int cleanliness, int rulesRespect, string comment, User user, Reservation reservation)
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
            Id = Convert.ToInt32(values[0]);
            Cleanliness = Convert.ToInt32(values[1]);
            RulesRespect = Convert.ToInt32(values[2]);
            Comment = values[3];
            User = new User() { Id = Convert.ToInt32(values[4]) };
            Reservation = new Reservation() { Id = Convert.ToInt32(values[5]) };
        }
    }
}
