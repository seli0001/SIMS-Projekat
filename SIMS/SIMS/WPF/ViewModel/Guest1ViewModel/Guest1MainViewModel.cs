using SIMS.Domain.Model;
using SIMS.WPF.ViewModel.OwnerViewModel;
using SIMS.WPF.ViewModel.Guest1ViewModel;
using SIMS.WPF.ViewModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SIMS.WPF.ViewModel.Guest1ViewModel
{
    public class Guest1MainViewModel : ViewModelBase, IClose
    {
        public User LoggedInUser { get; set; }
        public ReschedulingRequestViewModel ReschedulingRequestVM { get; set; }
        public UnratedReservationsViewModel UnratedReservationsVM { get; set; }
        public CreateReschedulingRequestViewModel CreateReschedulingRequestVM { get; set; }
        public RequestViewModel RequestsVM { get; set; }
        public ReservationViewModel ReservationVM { get; set; }
        public RatingsViewModel RatingsVM { get; set; }
        public HomeViewModel HomeVM { get; set; }
        public BookingViewModel BookVM { get; set; }


        public Action Close { get; set; }

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


        //#region commands

        private ICommand _homeCommand;
        public ICommand HomeCommand
        {
            get
            {
                return _homeCommand ?? (_homeCommand = new CommandBase(
                    () =>
                    {
                        HomeVM = new HomeViewModel(LoggedInUser, this);
                        CurrentView = HomeVM;
                    }, true));
            }
        }

        private ICommand _requestsCommand;
        public ICommand RequestsCommand
        {
            get
            {
                return _requestsCommand ?? (_requestsCommand = new CommandBase(
                    () =>
                    {
                        RequestsVM = new RequestViewModel(LoggedInUser);
                        CurrentView = RequestsVM;
                    }, true));
            }
        }

        private ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new CommandBase(() => Close(), true));
            }
        }



        private ICommand _reservationCommand;
        public ICommand ReservationCommand
        {
            get
            {
                return _reservationCommand ?? (_reservationCommand = new CommandBase(
                    () =>
                    {
                        ReservationVM = new ReservationViewModel(LoggedInUser, this);
                        CurrentView = ReservationVM;
                    }, true));
            }
        }

        private ICommand _profileCommand;
        public ICommand ProfileCommand
        {
            get
            {
                return _profileCommand ?? (_profileCommand = new CommandBase(
                    () =>
                    {
                        RatingsVM = new RatingsViewModel(LoggedInUser, this);
                        CurrentView = RatingsVM;
                    }, true));
            }
        }



        //#endregion

        public Guest1MainViewModel()
        {

        }
         
        public Guest1MainViewModel(User user)
        {
            HomeVM = new HomeViewModel(user, this);
            UnratedReservationsVM = new UnratedReservationsViewModel(user);
            RatingsVM = new RatingsViewModel(user, this);
            ReschedulingRequestVM = new ReschedulingRequestViewModel(user);
            ReservationVM = new ReservationViewModel(user, this);
            RequestsVM = new RequestViewModel(user);
            //BookVM = new BookingViewModel(selectedAccommodation, user);

            LoggedInUser = user;
            CurrentView = HomeVM;
        }
    }
}
