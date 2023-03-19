using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS.Model.Guide;
using SIMS.Serializer;

namespace SIMS.Model.Guest2
{
    public  class BookedTour : ISerializable
    {
        public int Id { get; set; }

        public int TourId { get; set; }
        public Tour Tour { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }
        public BookedTour()
        {
            Tour = new Tour();
            User = new User();
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
            Id.ToString(),
            TourId.ToString(),
            UserId.ToString()
        };
            return csvValues;
        }

        public void FromCSV(string[] csvValues)
        {
            Id = int.Parse(csvValues[0]);
            TourId = int.Parse(csvValues[1]);
            UserId = int.Parse(csvValues[2]);
        }
    }
}
