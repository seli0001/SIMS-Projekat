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
using System.Reflection.Emit;
using System.Windows.Threading;
using System.Threading;

namespace SIMS.View.OwnerView
{
    /// <summary>
    /// Interaction logic for OwnerMainView.xaml
    /// </summary>
    public partial class OwnerMainView : Window
    {
        public static ObservableCollection<Accommodation> Accommodations { get; set; }

        public Accommodation SelectedAccommodation { get; set; }

        public User LoggedInUser { get; set; }

        private readonly AccommodationRepository _repository;
        private readonly GuestRatingRepository _guestRatingRepository;

        private string DateL;
        private Timer timer;
        public OwnerMainView(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            _repository = new AccommodationRepository();
            _guestRatingRepository = new GuestRatingRepository();
            Accommodations = new ObservableCollection<Accommodation>(_repository.GetByUser(user));

            startClock();
            timer = new Timer(CheckCondition, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        }

        private void CheckCondition(object? state)
        {
            //Checking is there is any unreviewed reservation
            if (_guestRatingRepository.checkIfNotRated())
            {
                MessageBox.Show("You Have some Unreviewed reservations!");
            }

            //Checking for points (is it got above or below 4.5, for super owner)
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
    }
}
