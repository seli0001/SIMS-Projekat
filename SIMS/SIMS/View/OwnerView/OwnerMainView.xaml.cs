using SIMS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using SIMS.Model.AccommodationModel;
using SIMS.Repository;
using System.Threading;
using System.Windows.Threading;

namespace SIMS.View.OwnerView
{
    /// <summary>
    /// Interaction logic for OwnerMainView.xaml
    /// </summary>
    public partial class OwnerMainView : Window
    {
        public static ObservableCollection<Accommodation> Accommodations { get; set; }

        public Accommodation SelectedAccommodation { get; set; }
        private readonly GuestRatingRepository _guestRatingRepository;
        private readonly OwnerRatingRepository _ownerRatingRepository;

        private readonly AccommodationRepository _accommodationRepository;
        private string DateL;
        private Timer timer;

        public User LoggedInUser { get; set; }

        private readonly AccommodationRepository _repository;
        public OwnerMainView(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            _repository = new AccommodationRepository();
            _guestRatingRepository = new GuestRatingRepository();
            _ownerRatingRepository = new OwnerRatingRepository();
            _accommodationRepository = new AccommodationRepository();
            Accommodations = new ObservableCollection<Accommodation>(_repository.GetByUser(user));

            startClock();
            timer = new Timer(CheckCondition, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        }

        private void CheckCondition(object? state)
        {
            //Unreviewed Notification
            if (_guestRatingRepository.checkIfNotRated())
            {
                MessageBox.Show("You Have some Unreviewed reservations!");
            }

            //Super owner
            if (checkIsSuperOwner())
            {
                _accommodationRepository.makeSuperOwner(LoggedInUser);
                MessageBox.Show("Congratulations You Have Became a SUPEROWNER!!!!");
            }
        }

        private bool checkIsSuperOwner()
        {
            List<OwnerRating> ratings = new List<OwnerRating>(_ownerRatingRepository.GetAll());
            return ratings.Count > 5 && checkRating(ratings);
        }

        private bool checkRating(List <OwnerRating> ratings)
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



        private void startClock()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += tickevent;
            timer.Start();
        }
        private void tickevent(object sender, EventArgs e)
        {
            DateL = DateTime.Now.ToString("h:m dd.MM.yyyy");
            DateLabel.Content = DateL;
        }

        private void ShowCreateAccommodationForm(object sender, RoutedEventArgs e)
        {
            AccommondationRegistration createAccommondationForm = new AccommondationRegistration(LoggedInUser);
            createAccommondationForm.Show();
        }

        private void ShowUpdateAccommodationForm(object sender, RoutedEventArgs e)
        {
            ShowAccommodation showAccommodation = new ShowAccommodation(SelectedAccommodation, LoggedInUser);
            showAccommodation.Show();
        }

        private void DeleteAccommodationHandler(object sender, RoutedEventArgs e)
        {
            if (SelectedAccommodation != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure?", "Delete comment",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _repository.Delete(SelectedAccommodation);
                    Accommodations.Remove(SelectedAccommodation);
                }
            }
        }
        private void ShowRatings(object sender, RoutedEventArgs e)
        {
            ShowRatings showRatings = new ShowRatings(LoggedInUser);
            showRatings.Show();
        }
    }
}
