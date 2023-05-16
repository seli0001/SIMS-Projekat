using SIMS.Domain.Model;
using SIMS.WPF.ViewModel.ViewModel;
using System;
using System.Windows.Input;

namespace SIMS.WPF.ViewModel.OwnerViewModel
{
    class MainViewModel : ViewModelBase, IClose
    {
        public User LoggedInUser { get; set; }
        public HomeViewModel HomeVM { get; set; }
        public RatingsViewModel RatingsVM { get; set; }
        public ReschedulingRequestViewModel ReschedulingRequestVM { get; set; }
        public SchedulingRenovationViewModel SchedulingRenovationVM { get; set; }   
        public NewAccommodationViewModel newAccommodationVM { get; set; }
        public UnratedReservationsViewModel UnratedReservationsVM { get; set; }
        public Accommodation SelectedAccommodation { get; set; }

        private double _ownerRating;
        public string OwnerRating
        {
            get
            {
                 return Math.Round(_ownerRating, 2).ToString();

            }
            set
            {
                if (Double.Parse(value) != _ownerRating)
                {
                    _ownerRating = Double.Parse(value);
                    OnPropertyChanged();
                }
            }
        }



        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public Action Close { get; set; }

        private ICommand _unratedReservationsCommand;
        public ICommand UnratedReservationsCommand
        {
            get
            {
                return _unratedReservationsCommand ?? (_unratedReservationsCommand = new CommandBase(() => { CurrentView = UnratedReservationsVM; }, true));
            }
        }

        private ICommand _homeViewCommand;
        public ICommand HomeViewCommand
        {
            get
            {
                return _homeViewCommand ?? (_homeViewCommand = new CommandBase(() => { CurrentView = HomeVM; }, true));
            }
        }

        private ICommand _guestRatingsViewCommand;
        public ICommand GuestRatingsViewCommand
        {
            get
            {
                return _guestRatingsViewCommand ?? (_guestRatingsViewCommand = new CommandBase(() => { CurrentView = RatingsVM; }, true));
            }
        }

        private ICommand _logOutCommand;
        public ICommand LogOutCommand
        {
            get
            {
                return _logOutCommand ?? (_logOutCommand = new CommandBase(() => Close(), true));
            }
        }

        private ICommand _newAccommodationCommand;
        public ICommand NewAccommodationCommand
        {
            get
            {
                return _newAccommodationCommand ?? (_newAccommodationCommand = new CommandBase(() => { CurrentView = newAccommodationVM; }, true));
            }
        }

        private ICommand _reschReqCommand;
        public ICommand ReschedulingCommand
        {
            get
            {
                return _reschReqCommand ?? (_reschReqCommand = new CommandBase(() => { CurrentView = ReschedulingRequestVM; }, true));
            }
        }

        private ICommand _schedulingRenovationCommand;
        public ICommand SchedulingRenovationCommand
        {
            get
            {
                return _schedulingRenovationCommand ?? 
                    (_schedulingRenovationCommand = new CommandBase(
                        () => { SchedulingRenovationVM = new SchedulingRenovationViewModel(LoggedInUser, this, SelectedAccommodation);
                    CurrentView = SchedulingRenovationVM; }, true));
            }
        }

        public void setOwnerRating(double value)
        {
            OwnerRating = value.ToString();
        }

        public MainViewModel()
        {
           
            
        }

        public MainViewModel(User user)
        {
            HomeVM = new HomeViewModel(user, this);
            newAccommodationVM = new NewAccommodationViewModel(user, this);
            UnratedReservationsVM = new UnratedReservationsViewModel(user);
            RatingsVM = new RatingsViewModel(user);
            ReschedulingRequestVM = new ReschedulingRequestViewModel(user);

            LoggedInUser = user;
            CurrentView = HomeVM;
        }
    }
}
