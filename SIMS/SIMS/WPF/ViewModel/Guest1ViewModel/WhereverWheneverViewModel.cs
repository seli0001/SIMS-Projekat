using SIMS.Domain.Model;
using SIMS.Repository;
using SIMS.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SIMS.WPF.ViewModel.Guest1ViewModel
{
    public class WhereverWheneverViewModel
    {
        public User LoggedInUser { get; set; }
        private AccommodationRepository _accommodationRepository { get; set; }
        public ReservationService _reservationService { get; set; }
        Random random = new Random();
        public WhereverWheneverViewModel(User user)
        {
            _accommodationRepository = new AccommodationRepository();
            _reservationService = new ReservationService();
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

        private int _timeOfStay = 1;
        public int TimeOfStay
        {
            get => _timeOfStay;
            set
            {
                if (value != _timeOfStay)
                {
                    _timeOfStay = value;
                }

            }
        }

        private int _numberOfGuests = 1;
        public int NumberOfGuests
        {
            get => _numberOfGuests;
            set
            {
                if (value != _numberOfGuests)
                {
                    _numberOfGuests = value;
                }

            }
        }

        private ICommand _bookWhereverCommand;
        public ICommand BookWhereverCommand
        {
            get
            {
                return _bookWhereverCommand ?? (_bookWhereverCommand = new CommandBase(() => SaveBooking(), true));
            }
        }

        private void SaveBooking()
        {
            if(FromDate.Date == ToDate.Date)
            {

                int randomNumber = random.Next(1, _accommodationRepository.GetAccommodationCount());
                Accommodation accommodation = _accommodationRepository.GetById(randomNumber);
                Reservation _reservation = new Reservation(DateOnly.FromDateTime(FromDate), DateOnly.FromDateTime(FromDate.AddDays(TimeOfStay)), accommodation, TimeOfStay, NumberOfGuests, LoggedInUser);

                MessageBoxResult result = MessageBox.Show(
                    "Are you happy with this date " + _reservationService.GetFirstAvailableDate(_reservation).ToString(),
                    "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    _reservationService.Save(_reservation);
                }
                else
                {
                    MessageBox.Show("Change the dates");
                }
                return;

            }
            int _randomNumber = random.Next(1, _accommodationRepository.GetAccommodationCount());
            Accommodation _accommodation = _accommodationRepository.GetById(_randomNumber);
            Reservation reservation = new Reservation(DateOnly.FromDateTime(FromDate), DateOnly.FromDateTime(FromDate.AddDays(TimeOfStay)), _accommodation, TimeOfStay, NumberOfGuests, LoggedInUser);
            MessageBoxResult resultYes = MessageBox.Show(
                    "Are you happy with this date " + _reservationService.GetFirstAvailableDate(reservation).ToString(),
                    "Confirmation", MessageBoxButton.YesNo);
            if (resultYes == MessageBoxResult.Yes)
            {
                _reservationService.Save(reservation);
            }
            else
            {
                MessageBox.Show("Change the dates");
            }
            return;
        }

    }
}
