using SIMS.Domain.Model;
using SIMS.Repository;
using SIMS.Service.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SIMS.WPF.ViewModel.Guest1ViewModel
{
    public class ReservationViewModel
    {
        public Accommodation SelectedAccommodation { get; set; }

        private Reservation _selectedReservation;

        public User LoggedInUser { get; set; }

        private ReservationService _reservationService;
        private CancelingRequestsRepository _cancelingRequestsRepository;

        Guest1MainViewModel guest1MainViewModel;

        public ReservationViewModel()
        {
        }

        public ReservationViewModel(User user, Guest1MainViewModel mvm)
        {
            _cancelingRequestsRepository = new CancelingRequestsRepository();
            _reservationService = new ReservationService();
            Reservations = new ObservableCollection<Reservation>(_reservationService.GetByUserId(user.Id));

            guest1MainViewModel = mvm;
            LoggedInUser = user;
        }

        public static ObservableCollection<Reservation> Reservations { get; set; }
        public Reservation SelectedReservation
        {
            get => _selectedReservation;

            set
            {
                DateOnly today = DateOnly.FromDateTime(DateTime.Today);

                //if (CompareDates(today, value.ToDate)) RateButton.IsEnabled = true;
                //else RateButton.IsEnabled = false;

                _selectedReservation = value;
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


        private ICommand _requestCommand;
        public ICommand RequestCommand
        {
            get
            {
                return _requestCommand ?? (_requestCommand = new CommandBase(() => Reservation(), true));
            }
        }

        private void Reservation()
        {
            if (SelectedAccommodation != null)
            {
                CreateReschedulingRequestViewModel CRRVM = new CreateReschedulingRequestViewModel(SelectedReservation, LoggedInUser);
                guest1MainViewModel.CurrentView = CRRVM;
            }
        }


    }
}
