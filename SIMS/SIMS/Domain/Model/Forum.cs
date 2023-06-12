using PdfSharp.Pdf.Content.Objects;
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
        public Location Location { get; set; }
        public DateOnly FromDate { get; set; }
        public List<Comment> Comments { get; set; }
        public User ForumOwner { get; set; }
        public bool IsOpen { get; set; }


        public Forum()
        {
            Comments = new List<Comment>();
        }

        public Forum(Location location, DateOnly fromDate, List<Comment> coments, User forumOwner)
        {
            Location = location;
            FromDate = fromDate;
            Comments = coments;
            ForumOwner = forumOwner;
            IsOpen = true;
        }

        public string[] ToCSV()
        { 
            string[] csvValues =
            {
                Id.ToString(),
                Location.Id.ToString(),
                FromDate.ToString("dd-MM-yyyy"),
                ForumOwner.Id.ToString(),
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Location = new Location() { Id = Convert.ToInt32(values[1]) };
            FromDate = DateOnly.Parse(values[2]);
            ForumOwner = new User() { Id = Convert.ToInt32(values[3]) };
        }
    }
}
