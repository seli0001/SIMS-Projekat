using SIMS.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SIMS.View.OwnerView;
using System.Windows.Input;
using SIMS.Domain.Model;
using SIMS.Service.UseCases;
using SIMS.WPF.View;
using SIMS.WPF.ViewModel.ViewModel;

namespace SIMS.WPF.ViewModel.OwnerViewModel
{
    public class ShowAccommodationViewModel : ViewModelBase
    {
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

                    DateOnly today = DateOnly.FromDateTime(DateTime.Today);

                    if (CompareDates(today, value.ToDate)) IsEnabled = true;
                    else IsEnabled = false;
                    _selectedReservation = value;
                }
            }
        }
        public static ObservableCollection<Reservation> Reservations { get; set; }
        public User LoggedInUser { get; set; }

        private ReservationRepository _reservationRepository;

        public ShowAccommodationViewModel(User user)
        {
            _reservationRepository = new ReservationRepository();
            _guestRatingService = new GuestRatingService();
            Reservations = new ObservableCollection<Reservation>();
            LoggedInUser = user;
            UpdateReservations();
        }

        public void UpdateReservations()
        {
            List<Reservation> AllReservationsForUser = new List<Reservation>(_reservationRepository.GetByOwnerId(LoggedInUser.Id));
            Reservations.Clear();
            foreach (Reservation res in AllReservationsForUser)
            {
                if (!_guestRatingService.checkRatingForReservation(res))
                {
                    Reservations.Add(res);
                }
            }
        }

        private bool CompareDates(DateOnly date11, DateOnly date22)
        {
            DateTime date1 = date11.ToDateTime(new TimeOnly());
            DateTime date2 = date22.ToDateTime(new TimeOnly());
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
