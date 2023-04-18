using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Model
{
    public class ReschedulingRequests:ISerializable
    {
        public int Id { get; set; }
        public Reservation Reservation { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public ReschedulingRequests()
        {

        }

        public ReschedulingRequests(Reservation reservation, DateTime fromDate, DateTime toDate)
        {
            Reservation = reservation;
            FromDate = fromDate;
            ToDate = toDate;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Reservation.Id.ToString(),
                FromDate.ToString(),
                ToDate.ToString(),
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Reservation = new Reservation() { Id = Convert.ToInt32(values[1]) };
            FromDate = DateTime.Parse(values[2]);
            ToDate = DateTime.Parse(values[3]);
        }
    }
}
