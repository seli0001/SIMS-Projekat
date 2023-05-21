using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Domain.Model
{
    class SuperGuest : ISerializable
    {
        public int Id { get; set; }

        public SuperGuest()
        {

        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
            };
            return csvValues;
        }

        public void FromCSV(string[] csvValues)
        {
            Id = int.Parse(csvValues[0]);
        }
    }
}
