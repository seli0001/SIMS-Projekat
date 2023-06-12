using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Domain.Model
{
    internal class SuperGuide: ISerializable
    {
        public int Id { get; set; }
        public string Language { get; set; }

        public SuperGuide()
        {

        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Language,
            };
            return csvValues;
        }

        public void FromCSV(string[] csvValues)
        {
            Id = int.Parse(csvValues[0]);
            Language = csvValues[1];
        }
    }
}
