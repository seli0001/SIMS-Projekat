using SIMS.Model;
using SIMS.Model.AccommodationModel;
using SIMS.Repository;
using SIMS.View.OwnerView;
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

        private ReservationRepository _reservationRepository;
        private CancelingRequestsRepository _cancelingRequestsRepository;

        public static ObservableCollection<Reservation> Reservations { get; set; }
        public Reservation SelectedReservation
        {
            get => _selectedReservation;

            set
            {
                DateTime today = DateTime.Today;

                if (CompareDates(today, value.ToDate)) RateButton.IsEnabled = true;
                else RateButton.IsEnabled = false;

                _selectedReservation = value;
            }
        }

        public ShowReservations(User user)
        {
            InitializeComponent();
            DataContext = this;
            _cancelingRequestsRepository = new CancelingRequestsRepository();
            _reservationRepository = new ReservationRepository();
            Reservations = new ObservableCollection<Reservation>(_reservationRepository.GetByUserId(user.Id));
            LoggedInUser = user;
        }

        private bool CompareDates(DateTime date1, DateTime date2)
        {
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
            CancelingRequests cancelingRequest = new CancelingRequests(SelectedReservation);
            _cancelingRequestsRepository.Save(cancelingRequest);
            MessageBox.Show("Request Sent");
            Close();
        }
    }
}
