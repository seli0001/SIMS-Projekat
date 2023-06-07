using Microsoft.Win32;
using SIMS.Domain.Model;
using SIMS.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows;
using Image = SIMS.Domain.Model.ImageTour;
using Aplikacija.Core;
using System.ComponentModel;
using SIMS.Core;
using HarfBuzzSharp;

namespace SIMS.WPF.ViewModel.GuideViewModel
{
    class TourRegistrationViewModel: BindableBase, IClose, INotifyPropertyChanged
    {
        private readonly TourRepository _tourRepository;
        private readonly User _guide;
        public ObservableCollection<Checkpoint> Checkpoints { get; set; }
        public ObservableCollection<BitmapImage> Images { get; set; }
        private Tour _tour;
        public Tour Tour
        {
            get { return _tour; }
            set
            {
                _tour = value;
                OnPropertyChanged("Tour");
            }
        }

        public string DescriptionError
        {
            get
            {
                if (Tour.ValidationErrors["Description"] != null)
                {
                    return Tour.ValidationErrors["Description"];
                }
                return string.Empty;
            }
        }

        public string NameError
        {
            get
            {
                if (Tour.ValidationErrors["Name"] != null)
                {
                    return Tour.ValidationErrors["Name"];
                }
                return string.Empty;
            }
        }

        public string LanguageError
        {
            get
            {
                if (Tour.ValidationErrors["Language"] != null)
                {
                    return Tour.ValidationErrors["Language"];
                }
                return string.Empty;
            }
        }

        public string MaxNumberOfPeopleError
        {
            get
            {
                if (Tour.ValidationErrors["MaxNumberOfPeople"] != null)
                {
                    return Tour.ValidationErrors["MaxNumberOfPeople"];
                }
                return string.Empty;
            }
        }

        public string DurationError
        {
            get
            {
                if (Tour.ValidationErrors["Duration"] != null)
                {
                    return Tour.ValidationErrors["Duration"];
                }
                return string.Empty;
            }
        }

        public string CheckpointsError
        {
            get
            {
                if (Tour.ValidationErrors["Checkpoints"] != null)
                {
                    return Tour.ValidationErrors["Checkpoints"];
                }
                return string.Empty;
            }
        }
        public string CityError
        {
            get
            {
                if (Tour.Location.ValidationErrors["City"] != null)
                {
                    return Tour.Location.ValidationErrors["City"];
                }
                return string.Empty;
            }
        }
        public string CountryError
        {
            get
            {
                if (Tour.Location.ValidationErrors["Country"] != null)
                {
                    return Tour.Location.ValidationErrors["Country"];
                }
                return string.Empty;
            }
        }

        public ObservableCollection<DateTime> StartDates { get; set; }
        private IMessageBoxService _messageBoxService;

        public int ImageSelectedIndex { get; set; }
        public int CheckpointSelectedIndex { get; set; }

        public ObservableCollection<string> Hours { get; set; }
        public ObservableCollection<string> Minutes { get; set; }
        public int DatesSelectedIndex { get; set; }
        public string HourSelectedItem { get; set; }
        private int _hourSelectedIndex;
        public int HourSelectedIndex
        {
            get { return _hourSelectedIndex; }
            set
            {
                _hourSelectedIndex = value;
                OnPropertyChanged("HourSelectedIndex");
            }
        }
        public string MinuteSelectedItem { get; set; }
        private int _minuteSelectedIndex;
        public int MinuteSelectedIndex
        {
            get { return _minuteSelectedIndex; }
            set
            {
                _minuteSelectedIndex = value;
                OnPropertyChanged("MinuteSelectedIndex");
            }
        }
        public DateTime SelectedDate { get; set; }

        private string _checkpointName;

        public string CheckpointName
        {
            get { return _checkpointName; }
            set
            {
                _checkpointName = value;
                OnPropertyChanged("CheckpointName");
            }
        }

        public TourRegistrationViewModel(User guide, Location location = null, string language = "")
        {
            _messageBoxService = Injector.Injector.CreateInstance<IMessageBoxService>();
            _tourRepository = new TourRepository();
            Tour = new Tour();
            Checkpoints = new ObservableCollection<Checkpoint>();
            Images = new ObservableCollection<BitmapImage>();
            StartDates = new ObservableCollection<DateTime>();
            Hours = new ObservableCollection<string>();
            Minutes = new ObservableCollection<string>();
            SelectedDate = DateTime.Today;
            if (location != null)
            {
                Tour.Location = location;
            }
            if (language != "")
            {
                Tour.Language = language;
            }
            PopulateComboBoxes();
            _guide = guide;
            ImageSelectedIndex = -1;
            CheckpointSelectedIndex = -1;
            DatesSelectedIndex = -1;
            HourSelectedIndex = -1;
            MinuteSelectedIndex = -1;

        }

        #region Commands

        ICommand _createTour;

        public ICommand CreateTour
        {
            get
            {
                if (_createTour == null)
                {
                    _createTour = new RelayCommand(param => RegisterTour());
                }
                return _createTour;
            }
        }

        ICommand _addImage;

        public ICommand AddImage
        {
            get
            {
                if (_addImage == null)
                {
                    _addImage = new RelayCommand(param => AddImageFromFileDialog());
                }
                return _addImage;
            }
        }

        ICommand _removeImage;

        public ICommand RemoveImageCommand
        {
            get
            {
                if (_removeImage == null)
                {
                    _removeImage = new RelayCommand(param => RemoveImage());
                }
                return _removeImage;
            }
        }

        ICommand _addDate;

        public ICommand AddDate
        {
            get
            {
                if (_addDate == null)
                {
                    _addDate = new RelayCommand(param => AddDateButton_Click());
                }
                return _addDate;
            }
        }

        ICommand _removeDate;

        public ICommand RemoveDateCommand
        {
            get
            {
                if (_removeDate == null)
                {
                    _removeDate = new RelayCommand(param => RemoveDate());
                }
                return _removeDate;
            }
        }

        ICommand _addCheckpoint;

        public ICommand AddCheckpoint
        {
            get
            {
                if (_addCheckpoint == null)
                {
                    _addCheckpoint = new RelayCommand(param => AddNewCheckpoint());
                }
                return _addCheckpoint;
            }
        }

        ICommand _removeCheckpoint;

        public ICommand RemoveCheckpointCommand
        {
            get
            {
                if (_removeCheckpoint == null)
                {
                    _removeCheckpoint = new RelayCommand(param => RemoveCheckpoint());
                }
                return _removeCheckpoint;
            }
        }

        

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new CommandBase(() => Close(), true));
            }
        }



        #endregion
        #region Methods
        public Action Close { get; set; }


        public void PopulateComboBoxes()
        {
            // add the hours to the list
            for (int i = 0; i < 24; i++)
            {
                Hours.Add(i.ToString("00"));
            }
            for (int i = 0; i < 60; i += 5)
            {
                Minutes.Add(i.ToString("00"));
            }
        }

        public bool CheckStartTime(string time)
        {
            string pattern = @"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$";
            return Regex.IsMatch(time, pattern);
        }


        private void RegisterTour()
        {
            //
            Tour.Validate();
            Tour.Location.Validate();
            if (Tour.IsValid && Tour.Location.IsValid)
            {
                if(StartDates.Count == 0)
                {
                    _messageBoxService.ShowMessage("Morate uneti bar jedan datum!");
                    return;
                }
                Tour.Guide = _guide;
                foreach (var date in StartDates)
                {
                    Tour.StartTime.Time = date;
                    _tourRepository.Save(Tour);
                }

                Close();
            }
            else
            {
                OnPropertyChanged("CheckpointsError");
                OnPropertyChanged("NameError");
                OnPropertyChanged("DescriptionError");
                OnPropertyChanged("LanguageError");
                OnPropertyChanged("MaxNumberOfPeopleError");
                OnPropertyChanged("DurationError");
                OnPropertyChanged("CityError");
                OnPropertyChanged("CountryError");
            }
        }

      
        private void AddImageFromFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                Tour.Images.Add(new Image() { Path = openFileDialog.FileName });
                LoadImageFromPath(openFileDialog.FileName);
            }
        }



        private void RemoveImage()
        {
            if (ImageSelectedIndex != -1)
            {
                Tour.Images.RemoveAt(ImageSelectedIndex);
                Images.RemoveAt(ImageSelectedIndex);
            }
            else
            {
                _messageBoxService.ShowMessage("Morate selektovati sliku koju zelite da obrisete!");
            }
        }

        public void LoadImageFromPath(string path)
        {
            // Convert the path to a URI format
            Uri uri = new Uri(path, UriKind.RelativeOrAbsolute);

            // Set the BitmapImage source to the URI
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.UriSource = uri;
            bitmapImage.EndInit();

            Images.Add(bitmapImage);
        }

        public int GetNextIndex()
        {
            return Checkpoints.Count == 0 ? 1 : Checkpoints.Max(checkpoint => checkpoint.Index) + 1;
        }

        private void AddNewCheckpoint()
        {
            if (CheckpointName != "")
            {
                var checkPoint = new Checkpoint() { Name = CheckpointName };
                checkPoint.Index = GetNextIndex();
                checkPoint.IsChecked = false;
                Tour.Checkpoints.Add(checkPoint);
                Checkpoints.Add(checkPoint);
                CheckpointName = "";
            }
        }

        private void RemoveCheckpoint()
        {
            if (CheckpointSelectedIndex != -1)
            {
                Tour.Checkpoints.RemoveAt(CheckpointSelectedIndex);
                Checkpoints.RemoveAt(CheckpointSelectedIndex);
            }
            else
            {
                _messageBoxService.ShowMessage("Morate selektovati checkpoint!");
            }
        }

        private void AddDateButton_Click()
        {
            string time = HourSelectedItem + ":" + MinuteSelectedItem;
            if (CheckStartTime(time))
            {
                string startTime = SelectedDate.Date.ToShortDateString() + ' ' + time;
                StartDates.Add(DateTime.Parse(startTime));
                HourSelectedIndex = -1;
                MinuteSelectedIndex = -1;
                SelectedDate = DateTime.Now;
                _messageBoxService.ShowMessage("Uspesno ste dodali datum");
            }
        }

        private void RemoveDate()
        {
            if (DatesSelectedIndex != -1)
            {
                StartDates.RemoveAt(DatesSelectedIndex);
            }
            else
            {
                _messageBoxService.ShowMessage("Morate selektovati datum!");
            }
        }

        private void date_SelectedDateChanged()
        {
            DateTime selectedDate = SelectedDate;

            // Disable selecting dates in the past
            if (selectedDate < DateTime.Today)
            {
                _messageBoxService.ShowMessage("Datum ne sme biti od pre!");
                SelectedDate = DateTime.Today;
            }
        }

        private void Button_Click()
        {
            Close();
        }
        #endregion
    }
}
