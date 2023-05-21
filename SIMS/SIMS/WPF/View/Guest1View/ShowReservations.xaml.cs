using SIMS.Domain.Model;
using SIMS.Model;
using SIMS.Repository;
using SIMS.Service.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using SIMS.Model;
using System.Linq;

namespace SIMS.View.FirstGuestView
{
    /// <summary>
    /// Interaction logic for ShowReservations.xaml
    /// </summary>
    public partial class ShowReservations : Window
    {

        public Accommodation SelectedAccommodation { get; set; }

        private Reservation _selectedReservation;

        public User LoggedInUser { get; set; }

        private ReservationService _reservationService;
        private CancelingRequestsRepository _cancelingRequestsRepository;
        private AccommodationRepository _accomodationRepository;

        public static ObservableCollection<Reservation> Reservations { get; set; }
        public static ObservableCollection<Accommodation> Accommodations { get; set; }
        public Reservation SelectedReservation
        {
            get => _selectedReservation;

            set
            {
                DateOnly today = DateOnly.FromDateTime(DateTime.Today);

                if (CompareDates(today, value.ToDate)) RateButton.IsEnabled = true;
                else RateButton.IsEnabled = false;

                _selectedReservation = value;
            }
        }

        public ShowReservations(User user)
        {
            InitializeComponent();
            DataContext = this;
            RateButton.IsEnabled = false;
            _cancelingRequestsRepository = new CancelingRequestsRepository();
            _reservationRepository = new ReservationRepository();
            _accomodationRepository = new AccommodationRepository();
            Reservations = new ObservableCollection<Reservation>(_reservationRepository.GetByUserId(user.Id));
            Accommodations = new ObservableCollection<Accommodation>(_accomodationRepository.GetAll());
            PopulateAccommodationNames();
            LoggedInUser = user;
        }

        private void PopulateAccommodationNames()
        {
            foreach (var reservation in Reservations)
            {
                reservation.Accommodation = Accommodations.FirstOrDefault(a => a.Id == reservation.Accommodation.Id);
            }
        }

        private bool CompareDates(DateTime date1, DateTime date2)
        {
            DateTime date1 = date11.ToDateTime(new TimeOnly());
            DateTime date2 = date22.ToDateTime(new TimeOnly());
            if (date2 > date1) return false;
            TimeSpan difference = date1 - date2;
            int days = difference.Days;
            if (days > 5) return false;
            else return true;
        }

        private void RateButton_Click_2(object sender, RoutedEventArgs e)
        {
            OwnerRatingView ownerRatingView = new OwnerRatingView(SelectedReservation, LoggedInUser);
            ownerRatingView.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RequestsView requestsView = new RequestsView(LoggedInUser);
            requestsView.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CreateReschedulingRequest createReschedulingRequest = new CreateReschedulingRequest(SelectedReservation, LoggedInUser);
            createReschedulingRequest.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (SelectedReservation == null)
            {
                MessageBox.Show("You have to choose the reservation to cancel");
            }
            else if (SelectedReservation.FromDate < DateTime.Today.AddDays(1)) 
            {
                MessageBox.Show("Youare late with your canceling request");
            }
            else
            {
                CancelingRequests cancelingRequest = new CancelingRequests(SelectedReservation);
                _cancelingRequestsRepository.Save(cancelingRequest);
                MessageBox.Show("Request Sent");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ShowRatings showRatings = new ShowRatings(LoggedInUser);
            showRatings.Show();
        }
    }
}
