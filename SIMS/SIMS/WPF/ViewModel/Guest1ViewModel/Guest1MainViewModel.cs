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
        public RatingsViewModel RatingsVM { get; set; }
        public ReschedulingRequestViewModel ReschedulingRequestVM { get; set; }
        public UnratedReservationsViewModel UnratedReservationsVM { get; set; }
        public CreateReschedulingRequestViewModel CreateReschedulingRequestVM { get; set; }
        public RequestsViewModel RequestsVM { get; set; }
        public ReservationViewModel ReservationVM { get; set; }
        public HomeViewModel HomeVM { get; set; }


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

        //private ICommand _unratedReservationsCommand;
        //public ICommand UnratedReservationsCommand
        //{
        //    get
        //    {
        //        return _unratedReservationsCommand ?? (_unratedReservationsCommand = new CommandBase(
        //            () => {
        //                UnratedReservationsVM = new UnratedReservationsViewModel(LoggedInUser);
        //                CurrentView = UnratedReservationsVM;
        //            }, true));
        //    }
        //}

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
                        RequestsVM = new RequestsViewModel(LoggedInUser);
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
                        ReservationVM = new ReservationViewModel(LoggedInUser);
                        CurrentView = ReservationVM;
                    }, true));
            }
        }

        //private ICommand _schedulingRenovationCommand;
        //public ICommand SchedulingRenovationCommand
        //{
        //    get
        //    {
        //        return _schedulingRenovationCommand ??
        //            (_schedulingRenovationCommand = new CommandBase(
        //                () => {
        //                    SchedulingRenovationVM = new SchedulingRenovationViewModel(LoggedInUser, this, SelectedAccommodation);
        //                    CurrentView = SchedulingRenovationVM;
        //                }, true));
        //    }
        //}

        //private ICommand _renovationsCommand;
        //public ICommand RenovationsCommand
        //{
        //    get
        //    {
        //        return _renovationsCommand ??
        //            (_renovationsCommand = new CommandBase(
        //                () => {
        //                    RenovationsVM = new RenovationsViewModel(LoggedInUser, this);
        //                    CurrentView = RenovationsVM;
        //                }, true));
        //    }
        //}

        //private ICommand _statisticsComand;
        //public ICommand StatisticsComand
        //{
        //    get
        //    {
        //        return _statisticsComand ??
        //            (_statisticsComand = new CommandBase(
        //                () => {
        //                    AccommodationStatisticsVM = new AccommodationStatisticsViewModel(LoggedInUser, this, SelectedAccommodation);
        //                    CurrentView = AccommodationStatisticsVM;
        //                }, true));
        //    }
        //}

        //private ICommand _proposalCommand;
        //public ICommand ProposalCommand
        //{
        //    get
        //    {
        //        return _proposalCommand ??
        //            (_proposalCommand = new CommandBase(
        //                () => {
        //                    SystemProposalVM = new SystemProposalViewModel(LoggedInUser);
        //                    CurrentView = SystemProposalVM;
        //                }, true));
        //    }
        //}



        //#endregion

        public Guest1MainViewModel()
        {

        }
         
        public Guest1MainViewModel(User user)
        {
            HomeVM = new HomeViewModel(user, this);
            UnratedReservationsVM = new UnratedReservationsViewModel(user);
            RatingsVM = new RatingsViewModel(user);
            ReschedulingRequestVM = new ReschedulingRequestViewModel(user);
            ReservationVM = new ReservationViewModel(user);
            RequestsVM = new RequestsViewModel(user);

            LoggedInUser = user;
            CurrentView = HomeVM;
        }
    }
}
