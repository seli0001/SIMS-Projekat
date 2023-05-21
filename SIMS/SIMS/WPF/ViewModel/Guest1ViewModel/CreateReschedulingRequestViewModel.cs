using SIMS.Domain.Model;
using SIMS.Model;
using SIMS.Repository;
using SIMS.Service.Services;
using SIMS.WPF.ViewModel.ViewModel;
using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SIMS.Domain.Model;

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
            ReschedulingRequests reschedulingRequest = new ReschedulingRequests(SelectedReservation, FromDate, ToDate);
            if (_reservationRepository.AvailableReschedulingAccommodation(reschedulingRequest, _reservationRepository.GetAccommodationById(SelectedReservation.Id)))
            {
                _reschedulingRequestsRepository.Save(reschedulingRequest);
                MessageBox.Show("Rescheduling request has been sent");
            }
            else
            {
                MessageBox.Show("The accommodation is unavailable at the given time.");
            }
        }
    }
}
