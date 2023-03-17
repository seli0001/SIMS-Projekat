using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS.Serializer;

namespace SIMS.Model.Guide
{
    public class Tour : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int MaxNumberOfPeople { get; set; }
        public int Duration { get; set; }
        public StartTime StartTime { get; set; }
        public Location Location { get; set; }
        public List<Image> Images { get; set; }
        public List<CheckPoint> CheckPoints { get; set; }

        public Tour()
        {

        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
            Id.ToString(),
            Name,
            Description,
            Language,
            MaxNumberOfPeople.ToString(),
            Duration.ToString(),
            StartTime.Id.ToString(),
            Location.Id.ToString(),
        };
            return csvValues;
        }

        public void FromCSV(string[] csvValues)
        {
            Id = int.Parse(csvValues[0]);
            Name = csvValues[1];
            Description = csvValues[2];
            Language = csvValues[3];
            MaxNumberOfPeople = int.Parse(csvValues[4]);
            Duration = int.Parse(csvValues[5]);
            StartTime = new StartTime() { Id = int.Parse(csvValues[6]) };
            Location = new Location() { Id = int.Parse(csvValues[7]) };
        }
    }
}