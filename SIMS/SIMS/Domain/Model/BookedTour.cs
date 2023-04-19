using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SIMS.Repository;
using SIMS.Serializer;

namespace SIMS.Domain.Model
{
    public enum Notify
    {
        Accepted,
        Reject,
        NoAnswer
    }

    public class BookedTour : ISerializable, INotifyPropertyChanged
    {
        public int Id { get; set; }

        public int TourId { get; set; }
        public Tour Tour { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }
        public bool Review { get; set; }

        public bool UsedVoucher { get; set; }
        private Checkpoint _checkpoint;
        public Notify Notify;

        public Checkpoint Checkpoint
        {
            get { return _checkpoint; }
            set { _checkpoint = value; OnPropertyChanged(); }
        }


        public BookedTour()
        {
            Tour = new Tour();
            User = new User();
            Review = false;
            UsedVoucher = false;
            Notify = Notify.NoAnswer;
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
            TourId.ToString(),
            UserId.ToString(),
            Checkpoint != null ? Checkpoint.Id.ToString() : "",
            Review.ToString(),
            Notify.ToString(),
            UsedVoucher.ToString()
        };
            return csvValues;
        }

        public void FromCSV(string[] csvValues)
        {
            Id = int.Parse(csvValues[0]);
            TourId = int.Parse(csvValues[1]);
            UserId = int.Parse(csvValues[2]);
            Checkpoint = csvValues[3] == "" ? null : new Checkpoint() { Id = int.Parse(csvValues[3]) };
            Review = bool.Parse(csvValues[4]);
            Notify = (Notify)Enum.Parse(typeof(Notify), csvValues[5]);
            UsedVoucher = bool.Parse(csvValues[6]);
        }
    }
}
