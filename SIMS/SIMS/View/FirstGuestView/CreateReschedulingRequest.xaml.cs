using SIMS.Domain.Model;
using SIMS.Model;
using SIMS.Repository;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;


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


        private DateTime _fromDate = DateTime.Now;

        public DateTime FromDate
        {
            get => _fromDate;
            set
            {
                _fromDate = value;
            }
        }

        private DateTime _toDate = DateTime.Now;

        public DateTime ToDate
        {
            get => _toDate;
            set
            {
                _toDate = value;
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
            ReschedulingRequests reschedulingRequest = new ReschedulingRequests(SelectedReservation,DateOnly.FromDateTime(FromDate),DateOnly.FromDateTime(ToDate));
            _reschedulingRequestsRepository.Save(reschedulingRequest);
            MessageBox.Show("Request Sent");
            Close();
        }
    }
}
