using SIMS.Repository;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using SIMS.View.OwnerView;
using System.Windows.Input;
using SIMS.Domain.Model;
using SIMS.Service.UseCases;
using SIMS.Service;
using SIMS.WPF.ViewModel.ViewModel;
using SIMS.WPF.View.OwnerView;

namespace SIMS.WPF.ViewModel.OwnerViewModel
{
    class HomeViewModel : ViewModelBase, IClose
    {
        public static ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }

        private readonly GuestRatingService _guestRatingService;
        private readonly AccommodationService _accommodationService;
        private readonly OwnerMainService _ownerMainService;

        private readonly OwnerRatingRepository _ownerRatingRepository;

        public Action Close { get; set; }
        
        private Timer timer;

        public User LoggedInUser { get; set; }

        public HomeViewModel(User user)
        {
            LoggedInUser = user;

            _guestRatingService = new GuestRatingService();
            _accommodationService = new AccommodationService();
            _ownerMainService = new OwnerMainService();

            _ownerRatingRepository = new OwnerRatingRepository();
            Accommodations = new ObservableCollection<Accommodation>(_accommodationService.GetByUser(user));
            timer = new Timer(CheckCondition, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));
        }

        private void CheckCondition(object? state)
        {
            //Unreviewed Notification
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                if (_guestRatingService.checkIfNotRated())
                {
                    MessageBox.Show("You Have some Unreviewed reservations!");
                }
            });


            //Super owner
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                int temp = _ownerMainService.checkIsSuperOwner(LoggedInUser);
                if (temp == 1)
                {
                        _accommodationService.makeSuperOwner(LoggedInUser);
                        MessageBox.Show("Congratulations You Have Became a SUPEROWNER!!!!");
                        UpdateUI();
                }
                else if (temp == 0)
                {
                        _accommodationService.deleteSuperOwner(LoggedInUser);
                        MessageBox.Show("You have lost superowner title");
                        UpdateUI();
                }
            });

        }

        private void UpdateUI()
        {
            Accommodations.Clear();
            foreach (Accommodation acc in _accommodationService.GetAll())
            {
                Accommodations.Add(acc);
            }
        }
        #region commands
        
        private ICommand _showRequestsCommand;
        public ICommand ShowRequestsCommand
        {
            get
            {
                return _showRequestsCommand ?? (_showRequestsCommand = new CommandBase(() => ShowRequestView(), true));
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

        private ICommand _createAccommodationCommand;
        public ICommand CreateAccommodationCommand
        {
            get
            {
                return _createAccommodationCommand ?? (_createAccommodationCommand = new CommandBase(() => ShowCreateAccommodationForm(), true));
            }
        }

        private ICommand _updateAccommodationCommand;
        public ICommand UpdateAccommodationCommand
        {
            get
            {
                return _updateAccommodationCommand ?? (_updateAccommodationCommand = new CommandBase(() => ShowUpdateAccommodationForm(), true));
            }
        }

        private ICommand _deleteAccommodationCommand;
        public ICommand DeleteAccommodationCommand
        {
            get
            {
                return _deleteAccommodationCommand ?? (_deleteAccommodationCommand = new CommandBase(() => DeleteAccommodationHandler(), true));
            }
        }

        //private ICommand _showRatings;
        //public ICommand ShowRatingsCommand
        //{
        //    get
        //    {
        //        return _showRatings ?? (_showRatings = new CommandBase(() => ShowRatings(), true));
        //    }
        //}

        #endregion

        private void ShowRequestView()
        {
            ReschedulingRequestView reschedulingRequestView = new ReschedulingRequestView(LoggedInUser);
            reschedulingRequestView.Show();
        }
        private void ShowCreateAccommodationForm()
        {
            AccommondationRegistration createAccommondationForm = new AccommondationRegistration(LoggedInUser);
            createAccommondationForm.Show();
        }

        private void ShowUpdateAccommodationForm()
        {
            ShowAccommodationView showAccommodation = new ShowAccommodationView(SelectedAccommodation, LoggedInUser);
            showAccommodation.Show();
        }

        private void DeleteAccommodationHandler()
        {
            if (SelectedAccommodation != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure?", "Delete comment",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _accommodationService.Delete(SelectedAccommodation);
                    Accommodations.Remove(SelectedAccommodation);
                }
            }
        }

    }
}
