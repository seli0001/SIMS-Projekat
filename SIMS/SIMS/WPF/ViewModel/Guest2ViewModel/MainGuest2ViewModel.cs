using SIMS.Domain.Model;
using SIMS.Service.Services;
using SIMS.WPF.View;
using SIMS.WPF.ViewModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace SIMS.WPF.ViewModel.Guest2ViewModel
{
    public class MainGuest2ViewModel : ViewModelBase, IClose, INotifyPropertyChanged
    {
        private readonly TourService _tourService;
        public ObservableCollection<Tour> tours { get; set; }
        private readonly TourRequestService _tourRequestService;
        public Tour selectedTour { get; set; }
        public int userId;
        public Action Close { get; set; }

        public MainGuest2ViewModel(int userId)
        {
            this.userId = userId;
            _tourService = new TourService();
            _tourRequestService = new TourRequestService();
            tours = new ObservableCollection<Tour>(_tourService.GetNotStarted());
            CheckRequestNotify();


        }

        #region commands
        private ICommand _searchClickCommand;
        public ICommand SearchClickCommand
        {
            get
            {
                return _searchClickCommand ?? (_searchClickCommand = new CommandBase(() => SearchClick(), true));
            }
        }

        private ICommand _numberOfToursClickCommand;
        public ICommand NumberOfToursClickCommand
        {
            get
            {
                return _numberOfToursClickCommand ?? (_numberOfToursClickCommand = new CommandBase(() => NumberOfToursClick(), true));
            }
        }

        private ICommand _menuClickCommand;
        public ICommand MenuClickCommand
        {
            get
            {
                return _menuClickCommand ?? (_menuClickCommand = new CommandBase(() => MenuClick(), true));
            }
        }

        private string _tbDuration; 

        public string tbDuration
        {
            get => _tbDuration;
            set
            {
                if (value != _tbDuration)
                {
                    _tbDuration = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _tbLocation;
        public string tbLocation
        {
            get => _tbLocation;
            set
            {
                if (value != _tbLocation)
                {
                    _tbLocation = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _tbNumber;
        public string tbNumber
        {
            get => _tbNumber;
            set
            {
                if (value != _tbNumber)
                {
                    _tbNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _tbCountry;
        public string tbCountry
        {
            get => _tbCountry;
            set
            {
                if (value != _tbCountry)
                {
                    _tbCountry = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _tbLanguage;
        public string tbLanguage
        {
            get => _tbLanguage;
            set
            {
                if (value != _tbLanguage)
                {
                    _tbLanguage = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private List<Tour> _dgwTours;
        public List<Tour> dgwTours
        {
            get => _dgwTours;
            set
            {
                if (value != _dgwTours)
                {
                    _dgwTours = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SearchClick()
        {
            string duration = tbDuration;
            string language = tbLanguage;
            int numberOfPeople = 0;
            try
            {
                numberOfPeople = Convert.ToInt32(tbNumber);
            }
            catch
            {
            }
            string city = tbLocation;
            string country = tbCountry;
            List<Tour> tours = _tourService.GetAll();
            List<Tour> filtratedTours = new List<Tour>();
            filtratedTours = GetFiltratedTours(tours, city, country, duration, language, numberOfPeople);
            dgwTours = filtratedTours;
        }

        public List<Tour> GetFiltratedTours(List<Tour> tours, string city, string country, string duration, string language, int numberOfPeople)
        {
            if (duration != "")
            {
                tours = tours.Where(t => t.Language.StartsWith(language, StringComparison.OrdinalIgnoreCase)
                && t.Duration.ToString().Equals(duration)
                && (t.MaxNumberOfPeople >= numberOfPeople)
                && t.Location.City.StartsWith(city, StringComparison.OrdinalIgnoreCase)
                && t.Location.Country.StartsWith(country, StringComparison.OrdinalIgnoreCase)
                && t.Status.ToString().Equals("NOT_STARTED")
                ).ToList();
            }
            else
            {
                tours = tours.Where(t => t.Language.StartsWith(language, StringComparison.OrdinalIgnoreCase)
                && (t.MaxNumberOfPeople >= numberOfPeople)
                && t.Location.City.StartsWith(city, StringComparison.OrdinalIgnoreCase)
                && t.Location.Country.StartsWith(country, StringComparison.OrdinalIgnoreCase)
                && t.Status.ToString().Equals("NOT_STARTED")
                ).ToList();
            }
            return tours;
        }

        public  void CheckRequestNotify()
        {
            List<TourRequest> requests = _tourRequestService.GetByUser(userId);
            
            foreach (TourRequest t in requests)
            {
                if (t.Status == RequestStatus.Accepted && t.Notify==false)
                {
                    MessageBox.Show("Vodic je prihvatio vasu turu" + t.Location.City + "vreme pogledajte u prozoru zahtevi");
                    
                        t.Notify = true;
                    _tourRequestService.Update(t);
                    
                   
                }

                if ( (t.StartDate - DateTime.Now).TotalDays <= 2 && t.Status==RequestStatus.Waiting)
                {
                    t.Status = RequestStatus.Rejected;
                    _tourRequestService.Update(t);
                }

            }

        }
        private void NumberOfToursClick()
        {
            NumberOfTourGuestView numberOfTourGuestView = new NumberOfTourGuestView(selectedTour, userId, 100000);
            numberOfTourGuestView.Show();
            Close();
        }

        private void MenuClick()
        {
            MenuGuest2View menuGuest2 = new MenuGuest2View(userId);
            menuGuest2.Show();
            Close();
        }
    }
}
