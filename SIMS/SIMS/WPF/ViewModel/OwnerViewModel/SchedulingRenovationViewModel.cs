using SIMS.Domain.Model;
using SIMS.Model;
using SIMS.Service;
using SIMS.Service.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SIMS.WPF.ViewModel.OwnerViewModel
{
    class SchedulingRenovationViewModel : MainViewModel
    {
        public Accommodation SelectedAccommodation { get; set; }
        public User LoggedInUser { get; set; }
        public static ObservableCollection<DatesDTO> AvailableDates { get; set; }
        private DatesDTO _selectedDate;
        public DatesDTO SelectedDate {

            get => _selectedDate;
                
            set {
                _selectedDate = value;
                validator2[1] = 1;
                IsEnabled = true;
                ValidatorTest();
                OnPropertyChanged();
            } 
            
        }

        private readonly RenovationService _renovationService;

        private MainViewModel mainViewModel;

        private readonly int[] validator;
        private readonly int[] validator2;


        public SchedulingRenovationViewModel()
        {

        }

        public SchedulingRenovationViewModel(User user, MainViewModel mvm, Accommodation selectedAccommodation)
        {
            _renovationService = new RenovationService();
            AvailableDates = new ObservableCollection<DatesDTO>();
            SelectedAccommodation = selectedAccommodation;
            mainViewModel = mvm;
            LoggedInUser = user;
            validator = new int[3];
            validator2 = new int[2];

        }

        #region data

        private bool _isEnabled=false;
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }
        private bool _isEnabled2 = false;
        public bool IsEnabled2
        {
            get => _isEnabled2;
            set
            {
                _isEnabled2 = value;
                OnPropertyChanged();
            }
        }


        private DateTime _fromDate = DateTime.Now;
        public DateTime FromDate
        {
            get => _fromDate;
            set
            {
                if(value < ToDate || ToDate.Date == DateTime.Now.Date)
                {
                    _fromDate = value;
                    validator[0] = 1;
                }
                IsEnabled = true;
                ValidatorTest();
                OnPropertyChanged();
            }
        }

        private DateTime _toDate = DateTime.Now;

        public DateTime ToDate
        {
            get => _toDate;
            set
            {
                if (value > FromDate || FromDate.Date == DateTime.Now.Date)
                {
                    _toDate = value;
                    validator[1] = 1;
                }
                IsEnabled = true;
                ValidatorTest();
                OnPropertyChanged();
            }
        }

        private int _numOfDays;
        public int NumOfDays
        {
            get => _numOfDays;
            set { 
            
                if(value != _numOfDays && value > 0)
                {
                    _numOfDays = value;
                    validator[2] = 1;
                }
                IsEnabled = true;
                ValidatorTest();
                OnPropertyChanged();
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if(value != "")
                {
                _description = value;
                validator2[0] = 1;

                }
                else
                {
                    validator2[0] = 0;
                }
                IsEnabled2 = true;
                ValidatorTest2();
                OnPropertyChanged();
            }
        }

        #endregion

        #region commands
        private ICommand _searchForDatesCommand;
        public ICommand SearchForDatesCommand
        {
            get
            {
                return _searchForDatesCommand ?? (_searchForDatesCommand = new CommandBase(() => SearchDates(), true));
            }
        }

        private ICommand _scheduleCommand;
        public ICommand ScheduleCommand
        {
            get
            {
                return _scheduleCommand ?? (_scheduleCommand = new CommandBase(() => Schedule(), true));
            }
        }

        #endregion

        private void ValidatorTest()
        {
            foreach (int kon in validator)
            {
                if (kon == 0)
                {
                    IsEnabled = false;
                }
            }
        }

        private void ValidatorTest2()
        {
            foreach (int kon in validator2)
            {
                if (kon == 0)
                {
                    IsEnabled2 = false;
                }
            }
        }

        private void SearchDates()
        {
            List<DatesDTO> dates = new List<DatesDTO>();
            dates = _renovationService.AvailableDates(DateOnly.FromDateTime(FromDate),DateOnly.FromDateTime(ToDate), NumOfDays, SelectedAccommodation);
            AvailableDates.Clear();
            foreach(DatesDTO date in dates)
            {
                AvailableDates.Add(date);
            }
        }

        private void Schedule()
        {
            if(SelectedDate != null)
            {
                _renovationService.Save(new Renovation(SelectedDate.StartDate, SelectedDate.EndDate, Description, SelectedAccommodation));
                mainViewModel.HomeViewCommand.Execute(null);
            }
        }


    }
}
