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
        public string Comment { get; set; }
        public User ForumOwner { get; set; }

        //neki state nam treba tipa open ili closed da znamo da li mogu idalje da se pisu komentari, samo vlasnik moze da zatvori ovaj forum

        public Forum()
        {

        }

        public Forum(Accommodation accommodation, DateOnly fromDate, string comment, User forumOwner)
        {
            Accommodation = accommodation;
            FromDate = fromDate;
            Comment = comment;
            ForumOwner = forumOwner;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Accommodation.Id.ToString(),
                FromDate.ToString("dd-MM-yyyy"),
                Comment.ToString(),
                ForumOwner.Id.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Accommodation = new Accommodation() { Id = Convert.ToInt32(values[1]) };
            FromDate = DateOnly.Parse(values[2]);
            Comment = values[3];
            ForumOwner = new User() { Id = Convert.ToInt32(values[4]) };
        }
    }
}
