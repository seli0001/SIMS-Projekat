using SIMS.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Threading;
using System.Windows;
using SIMS.View.OwnerView;
using System.Windows.Input;
using SIMS.Domain.Model;
using SIMS.Service.UseCases;

namespace SIMS.WPF.ViewModel.OwnerViewModel
{
    public class OwnerMainViewModel : OwnerViewModelBase
    {
        public static ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }

        private readonly GuestRatingService _guestRatingService;
        private readonly AccommodationService _accommodationService;

        private readonly OwnerRatingRepository _ownerRatingRepository;
        
        private Timer timer;

        public User LoggedInUser { get; set; }

        public OwnerMainViewModel(User user)
        {
            LoggedInUser = user;

            _guestRatingService = new GuestRatingService();
            _accommodationService = new AccommodationService();


            _ownerRatingRepository = new OwnerRatingRepository();
            Accommodations = new ObservableCollection<Accommodation>(_accommodationService.GetByUser(user));
            //timer = new Timer(CheckCondition, null, TimeSpan.Zero, TimeSpan.FromMinutes(2));
        }

        private void CheckCondition(object? state)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                //Unreviewed Notification
                if (_guestRatingService.checkIfNotRated())
                {
                    MessageBox.Show("You Have some Unreviewed reservations!");
                }

                //Super owner
                int temp = checkIsSuperOwner();
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
        private int checkIsSuperOwner()
        {
            List<OwnerRating> ratings = new List<OwnerRating>(_ownerRatingRepository.GetAll());
            SuperOwnerService superOwnerService = new SuperOwnerService();
            if (superOwnerService.CheckById(LoggedInUser.Id))
            {
                if (!(ratings.Count > 5 && checkRating(ratings)))
                    return 0;
            }
            else if (ratings.Count > 5 && checkRating(ratings))
                return 1;

            return 2;
        }

        private bool checkRating(List<OwnerRating> ratings)
        {
            double score = 0;
            foreach (OwnerRating rating in ratings)
            {
                score += (rating.RulesRespect + rating.Cleanliness) / 2.0;
            }
            score /= ratings.Count;
            if (score > 4.5) return true;
            else return false;
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

        private ICommand _showRatings;
        public ICommand ShowRatingsCommand
        {
            get
            {
                return _showRatings ?? (_showRatings = new CommandBase(() => ShowRatings(), true));
            }
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
        private void ShowRatings()
        {
            ShowRatingsView showRatings = new ShowRatingsView(LoggedInUser);
            showRatings.Show();
        }
    }
}
