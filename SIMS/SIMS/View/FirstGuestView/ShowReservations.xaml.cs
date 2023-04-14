using SIMS.Domain.Model;
using SIMS.Domain.Model.AccommodationModel;
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
            _reservationRepository = new ReservationRepository();
            Reservations = new ObservableCollection<Reservation>(_reservationRepository.GetByUserId(user.Id));
            LoggedInUser = user;
        }

        //Proverava da li je od date1 do date2 proslo vise od 5 dana
        private bool CompareDates(DateTime date1, DateTime date2)
        {
            if (date2 > date1) return false;
            TimeSpan difference = date1 - date2;
            int days = difference.Days;
            if (days > 5) return false;
            else return true;
        }

        /*private void ShowRatingForm(object sender, RoutedEventArgs e)
        {
            OwnerRatingView ownerRatingView = new OwnerRatingView(LoggedInUser);
            ownerRatingView.Show();
        }*/

        

        private void RateButton_Click_2(object sender, RoutedEventArgs e)
        {
            OwnerRatingView ownerRatingView = new OwnerRatingView(SelectedReservation, LoggedInUser);
            ownerRatingView.Show();
        }
    }
}
