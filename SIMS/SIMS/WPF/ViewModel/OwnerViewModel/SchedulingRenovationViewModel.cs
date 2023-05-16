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
        public DatesDTO SelectedDate { get; set; }

        private readonly RenovationService _renovationService;

        private MainViewModel mainViewModel;


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
        }

        #region data

        private bool _isEnabled;
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        private DateTime _fromDate = DateTime.Now;
        public DateTime FromDate
        {
            get => _fromDate;
            set
            {
                _fromDate = value;
            }
        }

        private DateTime _toDate = DateTime.Now;

        public DateTime ToDate
        {
            get => _toDate;
            set
            {
                _toDate = value;
            }
        }

        private int _numOfDays;
        public int NumOfDays
        {
            get => _numOfDays;
            set
            {
                _numOfDays = value;
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
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
