using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Domain.Model
{
    public class ComplexTourRequest : ISerializable
    {
        public enum Status
        {
            Waiting,
            Accepted,
            Rejected
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public User user { get; set; }
        public List<int> TourRequestsId { get; set; }

        public Status status { get;set; }

        public ComplexTourRequest() {
            TourRequestsId = new List<int>();
            user=new User();
        }

        
        public ComplexTourRequest(int id,string name,List<int> requests,User user)
        {
            Id = id;
            Name = name;
            TourRequestsId = requests;
            status=Status.Waiting;
            this.user = user;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
            Id.ToString(),   
            Name,
            user.Id.ToString(),
            string.Join(",",TourRequestsId),
            status.ToString()
        };
            return csvValues;
        }

        public void FromCSV(string[] csv)
        {
            Id = int.Parse(csv[0]);
            Name = csv[1];
            user.Id = int.Parse(csv[2]);
            TourRequestsId = csv[3].Split(',').Select(int.Parse).ToList();
            status = (Status)Enum.Parse(typeof(Status), csv[4]);
        }
    }
}
