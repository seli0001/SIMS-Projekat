using SIMS.Model;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Domain.Model
{
    public class Forum : ISerializable
    {
        public int Id { get; set; }
        public Accommodation Accommodation { get; set; }
        public DateOnly FromDate { get; set; }
        public List<string> Comments { get; set; }
        public User ForumOwner { get; set; }
        public bool IsOpen { get; set; }


        public Forum()
        {

        }

        public Forum(Accommodation accommodation, DateOnly fromDate, string comment, User forumOwner)
        {
            Accommodation = accommodation;
            FromDate = fromDate;
            Comments = new List<string>();
            Comments.Add(comment);
            ForumOwner = forumOwner;
            IsOpen = true;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Accommodation.Id.ToString(),
                FromDate.ToString("dd-MM-yyyy"),
                string.Join(",", Comments),
                ForumOwner.Id.ToString(),
                IsOpen.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Accommodation = new Accommodation() { Id = Convert.ToInt32(values[1]) };
            FromDate = DateOnly.Parse(values[2]);
            Comments = values[3].Split(',').ToList();
            ForumOwner = new User() { Id = Convert.ToInt32(values[4]) };
            IsOpen = Convert.ToBoolean(values[5]);
        }
    }
}
