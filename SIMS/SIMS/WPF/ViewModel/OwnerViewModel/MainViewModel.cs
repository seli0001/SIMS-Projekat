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
        public RenovationsViewModel RenovationsVM { get;  set; }
        public AccommodationStatisticsViewModel AccommodationStatisticsVM { get; set; }
        public SystemProposalViewModel SystemProposalVM { get; set; }
        public TutorialViewModel TutorialVM { get; set; }

        public ForumsViewModel ForumsVM { get; set; }
        public ForumViewModel ForumVM { get; set; }
        public Accommodation SelectedAccommodation { get; set; }

        public Forum SelectedForum { get; set; }

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

        #region commands

        private ICommand _unratedReservationsCommand;
        public ICommand UnratedReservationsCommand
        {
            get
            {
                return _unratedReservationsCommand ?? (_unratedReservationsCommand = new CommandBase(
                    () => {
                        UnratedReservationsVM = new UnratedReservationsViewModel(LoggedInUser);
                        CurrentView = UnratedReservationsVM; 
                    }, true));
            }
        }

        private ICommand _homeViewCommand;
        public ICommand HomeViewCommand
        {
            get
            {
                return _homeViewCommand ?? (_homeViewCommand = new CommandBase(
                    () => {
                        HomeVM = new HomeViewModel(LoggedInUser, this);
                        CurrentView = HomeVM; 
                    }, true));
            }
        }

        private ICommand _guestRatingsViewCommand;
        public ICommand GuestRatingsViewCommand
        {
            get
            {
                return _guestRatingsViewCommand ?? (_guestRatingsViewCommand = new CommandBase(
                    () => {
                        RatingsVM = new RatingsViewModel(LoggedInUser);
                        CurrentView = RatingsVM; 
                    }, true));
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
                return _newAccommodationCommand ?? (_newAccommodationCommand = new CommandBase(
                    () => {
                        newAccommodationVM = new NewAccommodationViewModel(LoggedInUser, this);
                        CurrentView = newAccommodationVM; 
                    }, true));
            }
        }

        private ICommand _reschReqCommand;
        public ICommand ReschedulingCommand
        {
            get
            {
                return _reschReqCommand ?? (_reschReqCommand = new CommandBase(
                    () => 
                    {
                        ReschedulingRequestVM = new ReschedulingRequestViewModel(LoggedInUser);
                        CurrentView = ReschedulingRequestVM; 
                    }, true));
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

        private ICommand _renovationsCommand;
        public ICommand RenovationsCommand
        {
            get
            {
                return _renovationsCommand ??
                    (_renovationsCommand = new CommandBase(
                        () => {
                            RenovationsVM = new RenovationsViewModel(LoggedInUser, this);
                            CurrentView = RenovationsVM;
                        }, true));
            }
        }

        private ICommand _statisticsComand;
        public ICommand StatisticsComand
        {
            get
            {
                return _statisticsComand ??
                    (_statisticsComand = new CommandBase(
                        () => {
                            AccommodationStatisticsVM = new AccommodationStatisticsViewModel(LoggedInUser, this, SelectedAccommodation);
                            CurrentView = AccommodationStatisticsVM;
                        }, true));
            }
        }

        private ICommand _proposalCommand;
        public ICommand ProposalCommand
        {
            get
            {
                return _proposalCommand ??
                    (_proposalCommand = new CommandBase(
                        () => {
                            SystemProposalVM = new SystemProposalViewModel(LoggedInUser);
                            CurrentView = SystemProposalVM;
                        }, true));
            }
        }

        private ICommand _tutorialCommand;
        public ICommand TutorialCommand
        {
            get
            {
                return _tutorialCommand ??
                    (_tutorialCommand = new CommandBase(
                        () => {
                            TutorialVM = new TutorialViewModel();
                            CurrentView = TutorialVM;
                        }, true));
            }
        }


        private ICommand _forumsCommand;
        public ICommand ForumsCommand
        {
            get
            {
                return _forumsCommand ??
                    (_forumsCommand = new CommandBase(
                        () => {
                            ForumsVM = new ForumsViewModel(LoggedInUser, this);
                            CurrentView = ForumsVM;
                        }, true));
            }
        }

        private ICommand _forumCommand;
        public ICommand ForumCommand
        {
            get
            {
                return _forumCommand ??
                    (_forumCommand = new CommandBase(
                        () => {
                            ForumVM = new ForumViewModel(LoggedInUser, SelectedForum);
                            CurrentView = ForumVM;
                        }, true));
            }
        }



        #endregion

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
            RenovationsVM = new RenovationsViewModel(user, this);
            TutorialVM = new TutorialViewModel();

            LoggedInUser = user;
            CurrentView = HomeVM;
        }
    }
}
