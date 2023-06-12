using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Domain.Model
{
    public class OwnerComment : ISerializable
    {
        public int Id { get; set; }
        public Comment Comment { get; set; }
        public User Owner { get; set; }

        public OwnerComment()
        {

        }
        public OwnerComment(Comment comment, User owner)
        {
            Comment = comment;
            Owner = owner;  
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Comment.Id.ToString(),
                Owner.Id.ToString(),
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Comment = new Comment() { Id = Convert.ToInt32(values[1]) };
            Owner = new User() { Id = Convert.ToInt32(values[2]) };
        }
    }
}
