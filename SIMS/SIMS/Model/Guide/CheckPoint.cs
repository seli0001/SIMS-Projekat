using SIMS.Serializer;
using System.ComponentModel;

namespace SIMS.Model.Guide
{
    public class Checkpoint : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Tour Tour { get; set; }

        public Checkpoint()
        {

        }

        public Checkpoint(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
            Id.ToString(),
            Name,
            Tour.Id.ToString(),
        };
            return csvValues;
        }

        public void FromCSV(string[] csvValues)
        {
            Id = int.Parse(csvValues[0]);
            Name = csvValues[1];
            Tour = new Tour() { Id = int.Parse(csvValues[2])};
        }

    }
}