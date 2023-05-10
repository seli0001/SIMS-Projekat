using SIMS.Model;
using SIMS.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using SIMS.Domain.Model;

namespace SIMS.View.FirstGuestView
{
    /// <summary>
    /// Interaction logic for CreateReschedulingRequest.xaml
    /// </summary>
    public partial class CreateReschedulingRequest : Window
    {
        

        public Reservation SelectedReservation { get; set; }

        public User LoggedInUser { get; set; }

        private readonly ReservationRepository _reservationRepository;
        private readonly ReschedulingRequestsRepository _reschedulingRequestsRepository;

        private readonly int[] validator;

        public CreateReschedulingRequest(Reservation selectedReservation, User user)
        {
            InitializeComponent();
            DataContext = this;
            validator = new int[2];
            _reservationRepository = new ReservationRepository();
            _reschedulingRequestsRepository = new ReschedulingRequestsRepository();
            SelectedReservation = selectedReservation;
            LoggedInUser = user;
        }


        private DateTime fromDate = DateTime.Now;
        public DateTime FromDate
        {
            get { return fromDate; }
            set
            {
                if (value >= DateTime.Today)
                {
                    fromDate = value;
                    OnPropertyChanged(nameof(FromDate));
                }
                else
                {
                    MessageBox.Show("Please enter a valid check-in date.");
                }
            }
        }

        private DateTime toDate = DateTime.Now.AddDays(1);
        public DateTime ToDate
        {
            get { return toDate; }
            set
            {
                if (value > fromDate)
                {
                    toDate = value;
                    OnPropertyChanged(nameof(ToDate));
                }
                else
                {
                    MessageBox.Show("Please enter a valid check-out date.");
                }
            }
        }





        private void ValidatorTest()
        {
            foreach (int validation in validator)
            {
                if (validation == 0)
                {
                    BtnRequest.IsEnabled = false;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void BtnRequest_Click(object sender, RoutedEventArgs e)
        {
            /*Reservation reservation = new Reservation(FromDate, FromDate.AddDays(TimeOfStay), SelectedAccommodation, TimeOfStay, NumberOfGuests, LoggedInUser);
            if (_reservationRepository.AvailableAccommodation(reservation))
            {
                _reservationRepository.Save(reservation);
                MessageBox.Show("Accommodation " + SelectedAccommodation.Name + " successfully booked from " + FromDate.ToShortDateString() + " to " + FromDate.AddDays(TimeOfStay).ToShortDateString());
            }*/
            ReschedulingRequests reschedulingRequest = new ReschedulingRequests(SelectedReservation, FromDate, ToDate);
            _reschedulingRequestsRepository.Save(reschedulingRequest);
            MessageBox.Show("Request Sent");
            Close();
        }
    }
}
