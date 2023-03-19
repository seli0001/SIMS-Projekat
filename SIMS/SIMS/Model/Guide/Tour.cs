using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using SIMS.Serializer;
using SIMS.Model;

namespace SIMS.Model.Guide
{
    public enum TourStatus
    {
        NOT_STARTED,
        STARTED,
        FINISHED
    }
    public class Tour : ISerializable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int MaxNumberOfPeople { get; set; }
        public int NumberOfPeople { get; set; }
        public int Duration { get; set; }
        public StartTime StartTime { get; set; }
        public Location Location { get; set; }
        public List<Image> Images { get; set; }
        public List<Checkpoint> Checkpoints { get; set; }
        public User Guide { get; set; }
        public TourStatus Status { get; set; }

        public Tour()
        {
            StartTime = new StartTime();
            Location = new Location();
            Images = new List<Image>();
            Checkpoints = new List<Checkpoint>();
            NumberOfPeople = 0;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
            Id.ToString(),
            Name,
            Description,
            Language,
            MaxNumberOfPeople.ToString(),
            Duration.ToString(),
            StartTime.Id.ToString(),
            Location.Id.ToString(),
            NumberOfPeople.ToString(),
            Guide.Id.ToString(),
            Status.ToString(),
        };
            return csvValues;
        }

        public void FromCSV(string[] csvValues)
        {
            Id = int.Parse(csvValues[0]);
            Name = csvValues[1];
            Description = csvValues[2];
            Language = csvValues[3];
            MaxNumberOfPeople = int.Parse(csvValues[4]);
            Duration = int.Parse(csvValues[5]);
            StartTime = new StartTime() { Id = int.Parse(csvValues[6]) };
            Location = new Location() { Id = int.Parse(csvValues[7]) };
            NumberOfPeople = int.Parse(csvValues[8]);
            Guide = new User() { Id = int.Parse(csvValues[9]) };
            Status = (TourStatus) Enum.Parse(typeof(TourStatus), csvValues[10]);
        }


    }
}