using SIMS.Serializer;
using System;

namespace SIMS.Domain.Model
{
    public class Renovation : ISerializable
    {
        public int Id { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Description { get; set; }
        public Accommodation Accommodation { get; set; }

        public Renovation()
        {

        }

        public Renovation(DateOnly startDate, DateOnly endDate, string description, Accommodation accommodation)
        {
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
            Accommodation = accommodation;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                   Id.ToString(),
                   StartDate.ToString(),
                   EndDate.ToString(),
                   Description,
                   Accommodation.Id.ToString(),
            };
            return csvValues;
        }

        public void FromCSV(string[] csvValues)
        {
            Id = int.Parse(csvValues[0]);
            StartDate = DateOnly.Parse(csvValues[1]);
            EndDate = DateOnly.Parse(csvValues[2]);
            Description = csvValues[3];
            Accommodation = new Accommodation() { Id = Convert.ToInt32(csvValues[4]) };
        }
    }
}
