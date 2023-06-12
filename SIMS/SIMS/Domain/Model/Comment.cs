using SIMS.Model;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Domain.Model
{
    public class Comment : ISerializable
    {
        public int Id { get; set; }
        public User Author { get; set; }
        public string Text { get; set; }
        public string Role { get; set; }
        public int ReportNumber { get; set; }
        public Forum Forum { get; set; }
        public Comment()
        {

        }

        public Comment(User author, string text, string role, int reportNumber, Forum forum)
        {
            Author = author;
            Text = text;
            Role = role;   
            Forum = forum;
            ReportNumber = reportNumber;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Author.Id.ToString(),
                Text,
                Role,
                ReportNumber.ToString(),
                Forum.Id.ToString(),
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Author = new User() { Id = Convert.ToInt32(values[1]) };
            Text = values[2];
            Role = values[3];
            ReportNumber = Convert.ToInt32(values[4]);
            Forum = new Forum() { Id = Convert.ToInt32(values[5]) };
        }
    }
}
