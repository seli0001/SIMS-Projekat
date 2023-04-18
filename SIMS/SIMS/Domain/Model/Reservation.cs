using SIMS.Domain.Model;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Domain.Model
{
    public class Reservation : ISerializable
    {

        public int Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public Accommodation Accommodation { get; set; }
        public int TimeOfStay { get; set; }
        public int NumberOfGuests { get; set; }
        public User User { get; set; }

       // public User User { get; set; }





        public Reservation()
        {

        }

        public Reservation(DateTime fromDate, DateTime toDate, Accommodation accommodation, int timeOfStay, int numberOfGuests, User user)
        {
            FromDate = fromDate;
            ToDate = toDate;
            Accommodation = accommodation;
            TimeOfStay = timeOfStay;
            NumberOfGuests = numberOfGuests;
            User = user;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                    Id.ToString(),
                    FromDate.ToString(),
                    ToDate.ToString(),
                    Accommodation.Id.ToString(),
                    TimeOfStay.ToString(),
                    NumberOfGuests.ToString(),
                    User.Id.ToString(),
                };
            return csvValues;
        }

        public void FromCSV(string[] csvValues)
        {
            Id = int.Parse(csvValues[0]);
            FromDate = DateTime.Parse(csvValues[1]);
            ToDate = DateTime.Parse(csvValues[2]);
            Accommodation = new Accommodation() { Id = Convert.ToInt32(csvValues[3]) };
            TimeOfStay = Convert.ToInt32(csvValues[4]);
            NumberOfGuests = Convert.ToInt32(csvValues[5]);
            User = new User() { Id = Convert.ToInt32(csvValues[6]) };

        }

    }
}
