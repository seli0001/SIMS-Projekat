using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS.Serializer;


namespace SIMS.Domain.Model
{
    public class Voucher : ISerializable
    {
        public int Id { get; set; }
        public DateTime ValidUntil { get; set; }

        public string Name { get; set; }

        public int IdUser { get; set; }

        public User User { get; set; }

        public bool Status { get; set; }
        public Voucher()
        {
            User = new User();
            Status = false;
        }
        public Voucher(DateTime ValidUntil, string Name, int IdUser)
        {
            this.ValidUntil = ValidUntil;
            this.Name = Name;
            this.IdUser = IdUser;
            Status = false;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
            Id.ToString(),
            IdUser.ToString(),
            Name,
            ValidUntil.ToString(),
            Status.ToString()
        };
            return csvValues;
        }

        public void FromCSV(string[] csvValues)
        {
            Id = int.Parse(csvValues[0]);
            IdUser = int.Parse(csvValues[1]);
            Name = csvValues[2];
            ValidUntil = DateTime.Parse(csvValues[3]);
            Status = bool.Parse(csvValues[4]);

        }

    }
}
