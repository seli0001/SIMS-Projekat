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
        public ShowRatingsViewModel RatingsVM { get; set; }
        public ShowAccommodationViewModel UnratedReservationsVM { get; set; }

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

        public MainViewModel()
        {
           
            
        }

        public MainViewModel(User user)
        {
            HomeVM = new HomeViewModel(user);
            UnratedReservationsVM = new ShowAccommodationViewModel(user);
            RatingsVM = new ShowRatingsViewModel(user);
            LoggedInUser = user;
            CurrentView = HomeVM;
        }
    }
}
