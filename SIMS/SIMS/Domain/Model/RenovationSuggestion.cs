using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Domain.Model
{
    public class RenovationSuggestion:ISerializable
    {
        public int Id { get; set; }
        public int Urgency { get; set; }
        public string Suggestion { get; set; }
        public User User { get; set; }
        public Reservation Reservation { get; set; }

        public RenovationSuggestion()
        {

        }

        public RenovationSuggestion(int urgency, string suggestion, User user, Reservation reservation)
        {
            Urgency = urgency;
            Suggestion = suggestion;
            User = user;
            Reservation = reservation;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
             Id.ToString(),
             Urgency.ToString(),
             Suggestion,
             User.Id.ToString(),
             Reservation.Id.ToString(),
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Urgency = Convert.ToInt32(values[1]);
            Suggestion = values[2];
            User = new User() { Id = Convert.ToInt32(values[3]) };
            Reservation = new Reservation() { Id = Convert.ToInt32(values[4]) };
        }
    }
}
