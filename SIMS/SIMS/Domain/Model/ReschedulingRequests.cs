using SIMS.Domain.Model;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Model
{
    public enum Status
    {
        WAITING,
        APPROVED,
        REJECTED
    }

    public class ReschedulingRequests:ISerializable
    {
        public int Id { get; set; }
        public Reservation Reservation { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public Status Status { get; set; }

        public ReschedulingRequests()
        {

        }

        public ReschedulingRequests(Reservation reservation, DateOnly fromDate, DateOnly toDate)
        {
            Reservation = reservation;
            FromDate = fromDate;
            ToDate = toDate;
            Status = Status.WAITING;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Reservation.Id.ToString(),
                FromDate.ToString("dd-MM-yyyy"),
                ToDate.ToString("dd-MM-yyyy"),
                Status.ToString(),
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Reservation = new Reservation() { Id = Convert.ToInt32(values[1]) };
            FromDate = DateOnly.Parse(values[2]);
            ToDate = DateOnly.Parse(values[3]);
            Status = (Status)Enum.Parse(typeof(Status), values[4]);
        }
    }
}
