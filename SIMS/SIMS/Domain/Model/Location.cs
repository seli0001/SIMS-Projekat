using SIMS.Core;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Domain.Model
{
    public class Location : ValidationBase, ISerializable
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        public Location()
        {

        }

        public Location(string country, string city)
        {
            Country = country;
            City = city;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
            Id.ToString(),
            Country,
            City,
        };
            return csvValues;
        }

        public void FromCSV(string[] csvValues)
        {
            Id = int.Parse(csvValues[0]);
            Country = csvValues[1];
            City = csvValues[2];
        }

        protected override void ValidateSelf()
        {
            if (string.IsNullOrWhiteSpace(this.Country))
            {
                this.ValidationErrors["Country"] = "Country is required.";
            }
            if (string.IsNullOrWhiteSpace(this.City))
            {
                this.ValidationErrors["City"] = "City cannot be empty.";
            }
        }
    }
}
