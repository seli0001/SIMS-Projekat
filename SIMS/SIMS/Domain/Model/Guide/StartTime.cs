using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS.Serializer;

namespace SIMS.Domain.Model.Guide
{
    public class StartTime : ISerializable
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }

        public StartTime()
        {

        }

        public StartTime(int id, DateTime time)
        {
            Id = id;
            Time = time;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
            Id.ToString(),
            Time.ToString(),
        };
            return csvValues;
        }

        public void FromCSV(string[] csvValues)
        {
            Id = int.Parse(csvValues[0]);
            Time = DateTime.Parse(csvValues[1]);
        }
    }
}