using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS.Serializer;

namespace SIMS.Domain.Model
{
    public class ImageTour : ISerializable
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public Tour Tour { get; set; }

        public ImageTour()
        {

        }

        public ImageTour(int id, string path)
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
            Tour.Id.ToString(),
        };
            return csvValues;
        }

        public void FromCSV(string[] csvValues)
        {
            Id = int.Parse(csvValues[0]);
            Path = csvValues[1];
            Tour = new Tour() { Id = int.Parse(csvValues[2]) };
        }
    }
}