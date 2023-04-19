using SIMS.Domain.Model;
using SIMS.Model;
using SIMS.Repository;
using SIMS.Service.Services;
using SIMS.WPF.ViewModel.ViewModel;
using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace SIMS.WPF.ViewModel.Guest1ViewModel
{
    public class CreateReschedulingRequestViewModel : ViewModelBase, IClose
    {
        public Reservation SelectedReservation { get; set; }
        public User LoggedInUser { get; set; }
        public Action Close { get; set; }

        private readonly ReschedulingRequestsService _reschedulingRequestsService;

        private readonly int[] validator;

        public CreateReschedulingRequestViewModel(Reservation selectedReservation, User user)
        {
            validator = new int[2];
            _reschedulingRequestsService = new ReschedulingRequestsService();
            SelectedReservation = selectedReservation;
            LoggedInUser = user;
            IsEnabled = true;
        }

        #region data
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

        #endregion


        #region commands
        private ICommand _requestRescheduleCommand;
        public ICommand RequestRescheduleCommand
        {
            get
            {
                return _requestRescheduleCommand ?? (_requestRescheduleCommand = new CommandBase(() => RequestReschedule(), true));
            }
        }
        #endregion

        private void ValidatorTest()
        {
            foreach (int validation in validator)
            {
                if (validation == 0)
                {
                    IsEnabled = false;
                }
            }
        }

        private void RequestReschedule()
        {
            /*Reservation reservation = new Reservation(FromDate, FromDate.AddDays(TimeOfStay), SelectedAccommodation, TimeOfStay, NumberOfGuests, LoggedInUser);
            if (_reservationRepository.AvailableAccommodation(reservation))
            {
                _reservationRepository.Save(reservation);
                MessageBox.Show("Accommodation " + SelectedAccommodation.Name + " successfully booked from " + FromDate.ToShortDateString() + " to " + FromDate.AddDays(TimeOfStay).ToShortDateString());
            }*/
            ReschedulingRequests reschedulingRequest = new ReschedulingRequests(SelectedReservation, DateOnly.FromDateTime(FromDate), DateOnly.FromDateTime(ToDate));
            _reschedulingRequestsService.Save(reschedulingRequest);
            MessageBox.Show("Request Sent");
            Close();
        }
    }
}
