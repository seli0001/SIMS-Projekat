using SIMS.Serializer;

namespace SIMS.Model.Guide
{
    public class CheckPoint : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Tour Tour { get; set; }

        public CheckPoint()
        {

        }

        public CheckPoint(int id, string name)
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