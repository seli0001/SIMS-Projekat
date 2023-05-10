using SIMS.Repository;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using SIMS.Domain.Model;

namespace SIMS.View.FirstGuestView
{
    /// <summary>
    /// Interaction logic for FirstGuestBookingView.xaml
    /// </summary>
    public partial class FirstGuestBookingView : Window
    {

        public Accommodation SelectedAccommodation { get; set; }

        public User LoggedInUser { get; set; }

        private readonly AccommodationRepository _repository;
        private readonly ReservationRepository _reservationRepository;
      //  private User LoggedInUser { get; set; }

        private readonly int[] validator;

        public FirstGuestBookingView(Accommodation selectedAccommodation, User user)
        {
            InitializeComponent();
            Title = "Accommodation reservation";
            DataContext = this;
            validator = new int[2];
            _repository = new AccommodationRepository();
            _reservationRepository = new ReservationRepository();
            SelectedAccommodation = selectedAccommodation;
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

        private int _timeOfStay = 1;
        /*public int TimeOfStay
        {
            get => _timeOfStay;
            set
            {
                if (value != _timeOfStay)
                {
                    if (value < SelectedAccommodation.MinBookingDays)
                    {
                        TimeOfStayValidator.Content = "Number of days must be at least " + SelectedAccommodation.MinBookingDays;
                        TimeOfStayValidator.Visibility = Visibility.Visible;
                        validator[0] = 0;
                    }
                    else
                    {
                        TimeOfStayValidator.Visibility = Visibility.Hidden;
                        validator[0] = 1;
                    }
                    BtnBook.IsEnabled = true;

                    _timeOfStay = value;
                    OnPropertyChanged();
                }

            }
        }*/

        public int TimeOfStay
        {
            get { return _timeOfStay; }
            set
            {
                _timeOfStay = value;
                ValidateTimeOfStay();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TimeOfStay"));
            }
        }

        private void ValidateTimeOfStay()
        {
            if (FromDate != null && ToDate != null && TimeOfStay <= 0)
            {
                TimeSpan diff = ToDate - FromDate;
                TimeOfStay = diff.Days;
            }

            if (TimeOfStay <= SelectedAccommodation.MinBookingDays)
            {
                TimeOfStayValidator.Content = "Time of stay must be at least " + SelectedAccommodation.MinBookingDays + " days.";
                TimeOfStayValidator.Visibility = Visibility.Visible;
            }
            else
            {
                TimeOfStayValidator.Visibility = Visibility.Hidden;
            }
        }


        private int _numberOfGuests = 1;

        public int NumberOfGuests
        {
            get { return _numberOfGuests; }
            set
            {
                _numberOfGuests = value;
                ValidateMaxGuestNum();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NumberOfGuests"));
            }
        }

        private void ValidateMaxGuestNum()
        {
            if (NumberOfGuests > SelectedAccommodation.MaxGuestsNumber)
            {
                MaxGuestNumValidator.Content = "Max guests is " + SelectedAccommodation.MaxGuestsNumber;
                MaxGuestNumValidator.Visibility = Visibility.Visible;
            }
            else
            {
                MaxGuestNumValidator.Visibility = Visibility.Hidden;
            }
        }








        /*public int NumberOfGuests
        {
            get => _numberOfGuests;
            set
            {
                if (value != _numberOfGuests)
                {
                    if (value > SelectedAccommodation.MaxGuestsNumber)
                    {
                        MaxGuestNumValidator.Content = "Number of days must be at most " + SelectedAccommodation.MaxGuestsNumber;
                        MaxGuestNumValidator.Visibility = Visibility.Visible;
                        validator[1] = 0;
                    }
                    else
                    {
                        MaxGuestNumValidator.Visibility = Visibility.Hidden;
                        validator[1] = 1;
                    }
                    BtnBook.IsEnabled = true;

                    //ValidatorTest();
                    _numberOfGuests = value;
                    OnPropertyChanged();
                }

            }
        }*/

        /*private void ValidatorTest()
        {
            foreach (int validation in validator)
            {
                if (validation == 0)
                {
                    BtnBook.IsEnabled = false;
                }
            }
        }*/

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void BtnBook_Click(object sender, RoutedEventArgs e)
        {
            if (TimeOfStayValidator.Visibility == Visibility.Hidden && MaxGuestNumValidator.Visibility == Visibility.Hidden)
            {
                Reservation reservation = new Reservation(FromDate, FromDate.AddDays(TimeOfStay), SelectedAccommodation, TimeOfStay, NumberOfGuests, LoggedInUser);
                if (_reservationRepository.AvailableAccommodation(reservation))
                {
                    _reservationRepository.Save(reservation);
                    MessageBox.Show("Accommodation " + SelectedAccommodation.Name + " successfully booked from " + FromDate.ToShortDateString() + " to " + FromDate.AddDays(TimeOfStay).ToShortDateString());
                }
            }
            else
            {
                MessageBox.Show("Check inputs");
            }
            

        }
    }
}
