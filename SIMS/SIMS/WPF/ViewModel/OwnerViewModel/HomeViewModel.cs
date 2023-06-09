﻿using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using SIMS.View.OwnerView;
using System.Windows.Input;
using SIMS.Domain.Model;
using SIMS.Service.UseCases;
using SIMS.Service;
using SIMS.WPF.ViewModel.OwnerViewModel;
using SIMS.WPF.ViewModel.ViewModel;
using SIMS.WPF.View.OwnerView;
using SIMS.Repository;

namespace SIMS.WPF.ViewModel.OwnerViewModel
{
    class HomeViewModel : ViewModelBase, IClose
    {
        public static ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }

        private readonly GuestRatingService _guestRatingService;
        private readonly AccommodationService _accommodationService;
        private readonly OwnerMainService _ownerMainService;
        private readonly ForumRepository _forumRepository;


        private double _ownerRating;
        public string OwnerRating
        {
            get
            {

                MessageBox.Show(_ownerRating.ToString());
                return _ownerRating.ToString();
            
            } 
            set
            {
                if(Double.Parse(value) != _ownerRating)
                {
                    _ownerRating = Double.Parse(value);
                    OnPropertyChanged();
                }
            }
        }
        public Action Close { get; set; }
        
        private Timer timer;

        public User LoggedInUser { get; set; }
        MainViewModel mainViewModel;

        public HomeViewModel(User user, MainViewModel mvm)
        {
            LoggedInUser = user;

            _guestRatingService = new GuestRatingService();
            _accommodationService = new AccommodationService();
            _ownerMainService = new OwnerMainService();
            _forumRepository = new ForumRepository();

            Accommodations = new ObservableCollection<Accommodation>(_accommodationService.GetByUser(user));
            timer = new Timer(CheckCondition, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));
            mainViewModel = mvm;
            mainViewModel.setOwnerRating(_ownerMainService.getRating());
        }

        private void CheckCondition(object? state)
        {
            //Unreviewed Notification
            //App.Current.Dispatcher.Invoke((Action)delegate
            //{
            //    if (_guestRatingService.checkIfNotRated())
            //    {
            //        MessageBox.Show("You Have some Unreviewed reservations!");
            //    }
            //});


            //Super owner
            //App.Current.Dispatcher.Invoke((Action)delegate
            //{
            //    int temp = _ownerMainService.checkIsSuperOwner(LoggedInUser);

            //    mainViewModel.setOwnerRating(_ownerMainService.getRating());
            //    if (temp == 1)
            //    {
            //            _accommodationService.makeSuperOwner(LoggedInUser);
            //        MessageBox.Show("Congratulations You Have Became a SUPEROWNER!!!!");
            //            UpdateUI();
            //    }
            //    else if (temp == 0)
            //    {
            //            _accommodationService.deleteSuperOwner(LoggedInUser);
            //        MessageBox.Show("You have lost superowner title");
            //            UpdateUI();
            //    }
            //});

            //Renovation
            //App.Current.Dispatcher.Invoke((Action)delegate
            //{
            //    _accommodationService.RegulateRenovations();
            //    UpdateUI();
            //});



            //Forum
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                if (_forumRepository.CheckForOwner(LoggedInUser))
                    MessageBox.Show("There is new forum on one of your locations");

                _forumRepository.CheckSuperForum();
            });



        }

        private void UpdateUI()
        {
            Accommodations.Clear();
            foreach (Accommodation acc in _accommodationService.GetByUser(LoggedInUser))
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

        private ICommand _deleteAccommodationCommand;
        public ICommand DeleteAccommodationCommand
        {
            get
            {
                return _deleteAccommodationCommand ?? (_deleteAccommodationCommand = new CommandBase(() => DeleteAccommodationHandler(), true));
            }
        }

        private ICommand _schedulingRenovationCommand;
        public ICommand SchedulingRenovationCommand
        {
            get
            {
                return _schedulingRenovationCommand ?? (_schedulingRenovationCommand = new CommandBase(() => ShowSchedulingRenovationView(), true));
            }
        }

        private ICommand _statisticsCommand;
        public ICommand StatisticsCommand
        {
            get
            {
                return _statisticsCommand ?? (_statisticsCommand = new CommandBase(() => ShowStatisticsView(), true));
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
            /*
            ReschedulingRequestView reschedulingRequestView = new ReschedulingRequestView(LoggedInUser);
            reschedulingRequestView.Show(); */
        }
        private void ShowCreateAccommodationForm()
        {
            mainViewModel.NewAccommodationCommand.Execute(null);
        }

        private void ShowSchedulingRenovationView()
        {
            if(SelectedAccommodation != null)
            {
                mainViewModel.SelectedAccommodation = SelectedAccommodation;
                mainViewModel.SchedulingRenovationCommand.Execute(null);
            }
        }

        private void ShowStatisticsView()
        {
            if (SelectedAccommodation != null)
            {
                mainViewModel.SelectedAccommodation = SelectedAccommodation;
                mainViewModel.StatisticsComand.Execute(null);
            }
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
