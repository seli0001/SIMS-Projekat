using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS.Serializer;
using SIMS.Domain.Model;


namespace SIMS.Model
{
    public class CancelingRequests: ISerializable
    {
        public int Id { get; set; }
        public Reservation Reservation { get; set; }

        public CancelingRequests()
        {

        }

        public CancelingRequests(Reservation reservation)
        {
            Reservation = reservation;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Reservation.Id.ToString(),
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Reservation = new Reservation() { Id = Convert.ToInt32(values[1]) };
        }
    }
}
