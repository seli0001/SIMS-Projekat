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
    public class Guest1MainViewModel : ViewModelBase
    {
        public User LoggedInUser { get; set; }
        public RatingsViewModel RatingsVM { get; set; }
        public ReschedulingRequestViewModel ReschedulingRequestVM { get; set; }
        public UnratedReservationsViewModel UnratedReservationsVM { get; set; }
        public HomeViewModel HomeVM { get; set; }


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

        //private ICommand _homeViewCommand;
        //public ICommand HomeViewCommand
        //{
        //    get
        //    {
        //        return _homeViewCommand ?? (_homeViewCommand = new CommandBase(
        //            () => {
        //                HomeVM = new HomeViewModel(LoggedInUser, this);
        //                CurrentView = HomeVM;
        //            }, true));
        //    }
        //}

        //private ICommand _guestRatingsViewCommand;
        //public ICommand GuestRatingsViewCommand
        //{
        //    get
        //    {
        //        return _guestRatingsViewCommand ?? (_guestRatingsViewCommand = new CommandBase(
        //            () => {
        //                RatingsVM = new RatingsViewModel(LoggedInUser);
        //                CurrentView = RatingsVM;
        //            }, true));
        //    }
        //}

        //private ICommand _logOutCommand;
        //public ICommand LogOutCommand
        //{
        //    get
        //    {
        //        return _logOutCommand ?? (_logOutCommand = new CommandBase(() => Close(), true));
        //    }
        //}

        //private ICommand _newAccommodationCommand;
        //public ICommand NewAccommodationCommand
        //{
        //    get
        //    {
        //        return _newAccommodationCommand ?? (_newAccommodationCommand = new CommandBase(
        //            () => {
        //                newAccommodationVM = new NewAccommodationViewModel(LoggedInUser, this);
        //                CurrentView = newAccommodationVM;
        //            }, true));
        //    }
        //}

        //private ICommand _reschReqCommand;
        //public ICommand ReschedulingCommand
        //{
        //    get
        //    {
        //        return _reschReqCommand ?? (_reschReqCommand = new CommandBase(
        //            () =>
        //            {
        //                ReschedulingRequestVM = new ReschedulingRequestViewModel(LoggedInUser);
        //                CurrentView = ReschedulingRequestVM;
        //            }, true));
        //    }
        //}

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

            LoggedInUser = user;
            CurrentView = HomeVM;
        }
    }
}
