using SIMS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using SIMS.View.OwnerView;
using System.Windows;
using System.Windows.Input;
using SIMS.Domain.Model;
using SIMS.Service.UseCases;

namespace SIMS.WPF.ViewModel.OwnerViewModel
{
    public class ShowAccommodationViewModel : OwnerViewModelBase
    {
        public Accommodation SelectedAccommodation { get; set; }
        private Reservation _selectedReservation;

        private GuestRatingService _guestRatingService;

        private bool _isEnabled;
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }
        public Reservation SelectedReservation
        {
            get => _selectedReservation;

            set
            {
                if (value != null)
                {

                    DateTime today = DateTime.Today;

                    if (CompareDates(today, value.ToDate)) IsEnabled = true;
                    else IsEnabled = false;
                    _selectedReservation = value;
                }
            }
        }
        public static ObservableCollection<Reservation> Reservations { get; set; }
        public User LoggedInUser { get; set; }

        private ReservationRepository _reservationRepository;

        public ShowAccommodationViewModel(Accommodation selectedAccommodation, User user)
        {
            _reservationRepository = new ReservationRepository();
            _guestRatingService = new GuestRatingService();
            Reservations = new ObservableCollection<Reservation>();
            SelectedAccommodation = selectedAccommodation;
            UpdateReservations();
            LoggedInUser = user;
        }

        public void UpdateReservations()
        {
            List<Reservation> AllReservationsForAccommodation = new List<Reservation>(_reservationRepository.GetByAccommodationsId(SelectedAccommodation.Id));
            Reservations.Clear();
            foreach (Reservation res in AllReservationsForAccommodation)
            {
                if (!_guestRatingService.checkRatingForReservation(res))
                {
                    Reservations.Add(res);
                }
            }
        }

        private bool CompareDates(DateTime date1, DateTime date2)
        {
            if (date2 > date1) return false;
            TimeSpan difference = date1 - date2;
            int days = difference.Days;
            if (days > 5) return false;
            else return true;
        }

        private ICommand _rateGuestCommand;
        public ICommand RateGuestCommand
        {
            get
            {
                return _rateGuestCommand ?? (_rateGuestCommand = new CommandBase(() => ShowRatingForm(), true));
            }
        }
        private void ShowRatingForm()
        {
            GuestRatingView guestRatingView = new GuestRatingView(SelectedReservation, LoggedInUser);
            guestRatingView.Show();
        }

        
    }
}
