using SIMS.Model.AccommodationModel;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Model
{
    public class Image : ISerializable
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public Accommodation Accommodation { get; set; }

        public Image()
        {

        }

        public Image(int id, string path)
        {
            Id = id;
            Path = path;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
            Id.ToString(),
            Path,
            Accommodation.Id.ToString(),
        };
            return csvValues;
        }

        public void FromCSV(string[] csvValues)
        {
            Id = int.Parse(csvValues[0]);
            Path = csvValues[1];
            Accommodation = new Accommodation() { Id = int.Parse(csvValues[2]) };
        }
    }
}
