using Microsoft.VisualBasic;
using SIMS.Model;
using SIMS.Model.AccommodationModel;
using SIMS.Repository;
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

namespace SIMS.View.OwnerView
{
    /// <summary>
    /// Interaction logic for ShowAccommodation.xaml
    /// </summary>
    public partial class ShowAccommodation : Window
    {
        public Accommodation SelectedAccommodation { get; set; }
        private Reservation _selectedReservation;

        private GuestRatingRepository _guestRatingRepository;
        public Reservation SelectedReservation
        {
            get => _selectedReservation;

            set
            {
                if(value != null)
                {

                DateTime today = DateTime.Today;

                if (CompareDates(today, value.ToDate)) RateButton.IsEnabled = true;
                else RateButton.IsEnabled = false;
                   _selectedReservation = value;
                }
            }
        }

        public static ObservableCollection<Reservation> Reservations { get; set; }

        public User LoggedInUser { get; set; }

        private ReservationRepository _reservationRepository;
        public ShowAccommodation(Accommodation selectedAccommodation, User user)
        {
            InitializeComponent();
            DataContext = this;
            _reservationRepository = new ReservationRepository();
            _guestRatingRepository = new GuestRatingRepository();
            Reservations = new ObservableCollection<Reservation>();
            SelectedAccommodation = selectedAccommodation;
            UpdateReservations();
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

        private void ShowRatingForm(object sender, RoutedEventArgs e)
        {
            GuestRatingView guestRatingView = new GuestRatingView(SelectedReservation, LoggedInUser);
            guestRatingView.Show();
        }

        public void UpdateReservations()
        {
            List<Reservation> AllReservationsForAccommodation = new List<Reservation>(_reservationRepository.GetByAccommodationsId(SelectedAccommodation.Id));
            Reservations.Clear();
            foreach(Reservation res in AllReservationsForAccommodation)
            {
                if (!_guestRatingRepository.checkRatingForReservation(res))
                {
                    Reservations.Add(res);
                }
            }

        }
    }
}
