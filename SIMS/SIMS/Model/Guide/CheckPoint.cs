using SIMS.Serializer;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SIMS.Model.Guide
{
    public class Checkpoint : ISerializable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private bool _isChecked;

        public bool IsChecked 
        {
            get {
                return _isChecked; 
            }

            set {
                _isChecked = value;
                OnPropertyChanged();
            }
        }
        public int Index { get; set; }
        public Tour Tour { get; set; }

        public Checkpoint()
        {

        }

        public Checkpoint(int id, string name)
        {
            Id = id;
            Name = name;
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
            Tour.Id.ToString(),
            IsChecked.ToString(),
            Index.ToString(),
        };
            return csvValues;
        }

        public void FromCSV(string[] csvValues)
        {
            Id = int.Parse(csvValues[0]);
            Name = csvValues[1];
            Tour = new Tour() { Id = int.Parse(csvValues[2])};
            IsChecked = bool.Parse(csvValues[3]);
            Index = int.Parse(csvValues[4]);
        }

    }
}