using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using SIMS.Core;
using SIMS.Serializer;

namespace SIMS.Domain.Model
{
    public enum TourStatus
    {
        NOT_STARTED,
        STARTED,
        FINISHED
    }
    public class Tour : ValidationBase, ISerializable, INotifyPropertyChanged
    {
        private TourStatus _status;

        public int Id { get; set; }
        private string _name;
        public string Name { get => _name; set 
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            } 
        }
        public string Description { get; set; }
        public string Language { get; set; }
        public int MaxNumberOfPeople { get; set; }
        public int NumberOfPeople { get; set; }
        public int Duration { get; set; }
        public StartTime StartTime { get; set; }
        public Location Location { get; set; }
        public List<ImageTour> Images { get; set; }
        public List<Checkpoint> Checkpoints { get; set; }
        public User Guide { get; set; }
        public TourStatus Status { get => _status; set { _status = value; OnPropertyChanged(); } }

        public Tour()
        {
            StartTime = new StartTime();
            Location = new Location();
            Images = new List<ImageTour>();
            Checkpoints = new List<Checkpoint>();
            NumberOfPeople = 0;
            MaxNumberOfPeople = 1;
            Duration = 1;
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
            Status.ToString()
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
            Status = (TourStatus)Enum.Parse(typeof(TourStatus), csvValues[10]);
        }

        protected override void ValidateSelf()
        {
            if (string.IsNullOrWhiteSpace(this.Name))
            {
                this.ValidationErrors["Name"] = "Name is required.";
            }
            if (string.IsNullOrWhiteSpace(this.Description))
            {
                this.ValidationErrors["Description"] = "Description cannot be empty.";
            }
            if (string.IsNullOrWhiteSpace(this.Language))
            {
                this.ValidationErrors["Language"] = "Language cannot be empty.";
            }
            if (this.MaxNumberOfPeople < 1)
            {
                this.ValidationErrors["MaxNumberOfPeople"] = "Max number of people must be greater than 0.";
            }
            if (this.Duration < 1)
            {
                this.ValidationErrors["Duration"] = "Duration must be greater than 0.";
            }
            if (this.Checkpoints.Count < 2)
            {
                this.ValidationErrors["Checkpoints"] = "Tour must have at least two checkpoint.";
            }
        }

    }
}