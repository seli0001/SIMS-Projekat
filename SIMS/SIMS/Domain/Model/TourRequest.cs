using SIMS.Model;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Xml.Linq;

namespace SIMS.Domain.Model
{
    public enum RequestStatus
    {
        Waiting,
        Accepted,
        Rejected
    }
    public class TourRequest : ISerializable
    {
        public int Id { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }

        public string Language { get; set; }

        public int MaxNumberOfPeople { get; set; }

        public DateTime StartDate {  get; set; }

        public DateTime EndDate { get; set; }  

        public RequestStatus Status { get; set; }

        public DateTime TourTime { get; set;}

        public bool Notify { get; set; }
        public User User { get; set; }
        public TourRequest() {
            User = new User();
            Notify = false;
            TourTime = new DateTime();
        }
        public TourRequest(int id, Location location, string description, string language, int maxNumberOfPeople, DateTime startDate, DateTime endDate, RequestStatus status,int userId)
        {
            Id = id;
            Location = location;
            Description = description;
            Language = language;
            MaxNumberOfPeople = maxNumberOfPeople;
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
            User.Id = userId;
            Notify = false;
            TourTime = DateTime.MinValue;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
            Id.ToString(),
            Description,
            Language,
            MaxNumberOfPeople.ToString(),
            Location.Id.ToString(),
            StartDate.ToString(),
            EndDate.ToString(),
            Status.ToString(),
            User.Id.ToString(),
            Notify.ToString(),
            TourTime.ToString(),
        };
            return csvValues;
        }

        public void FromCSV(string[] csvValues)
        {
            Id = int.Parse(csvValues[0]);
            Description = csvValues[1];
            Language = csvValues[2];
            MaxNumberOfPeople = int.Parse(csvValues[3]);
            Location = new Location() { Id = int.Parse(csvValues[4]) };
            StartDate = DateTime.Parse(csvValues[5]);
            EndDate = DateTime.Parse(csvValues[6]);
            Status = (RequestStatus)Enum.Parse(typeof(RequestStatus), csvValues[7]);
            User.Id= int.Parse(csvValues[8]);
            Notify = bool.Parse(csvValues[9]);
            TourTime = DateTime.Parse(csvValues[10]);

        }


    }
}
