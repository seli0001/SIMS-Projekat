using SIMS.Domain.Model;
using SIMS.Model;
using SIMS.Service.Services;
using SIMS.WPF.View.OwnerView;
using SIMS.WPF.ViewModel.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace SIMS.WPF.ViewModel.OwnerViewModel
{
    public enum Available
    {
        AVAILABLE,
        OCCUPIED
    }

    public class ReschedulingRequestViewModel : ViewModelBase, IClose
    {
        private User Owner;

        private readonly ReschedulingRequestsService _requestService;

        public static ObservableCollection<ReschedulingRequests> Requests { get; set; }

        public static ObservableCollection<Available> ReservationStatus { get; set; }

        public ReschedulingRequests SelectedRequests { get; set; }
        public ReschedulingRequestViewModel(User user)
        {
            Owner = user;
            _requestService = new ReschedulingRequestsService();
            Requests = new ObservableCollection<ReschedulingRequests>(_requestService.GetByOwnerId(user.Id));
            ReservationStatus = new ObservableCollection<Available>(_requestService.checkAvailability(user.Id));
        }

        private string _rejectMessage = "";
        public string RejectMessage
        {
            get => _rejectMessage;
            set
            {
                _rejectMessage = value;
                OnPropertyChanged();
            }
        }

        #region commands
        private ICommand _acceptRequestCommand;
        public ICommand AcceptRequestCommand
        {
            get
            {
                return _acceptRequestCommand ?? (_acceptRequestCommand = new CommandBase(() => AcceptRequest(), true));
            }
        }

        private ICommand _showRejectWindowComman;
        public ICommand ShowRejectWindowCommand
        {
            get
            {
                return _showRejectWindowComman ?? (_showRejectWindowComman = new CommandBase(() => ShowRejectWindow(), true));
            }
        }

        private ICommand _rejectCommand;
        public ICommand RejectCommand
        {
            get
            {
                return _rejectCommand ?? (_rejectCommand = new CommandBase(() => RejectRequest(), true));
            }
        }

        #endregion
        public Action Close { get; set; }

        private void AcceptRequest()
        {
            if(SelectedRequests != null)
            {
                _requestService.AcceptRequest(SelectedRequests);
                ReservationStatus.RemoveAt(Requests.IndexOf(SelectedRequests));
                Requests.Remove(SelectedRequests);

                MessageBox.Show("Request succesffully approved!");
            }
        }

        private void ShowRejectWindow()
        {
            if (SelectedRequests != null)
            {
                RejectReason rejectReason = new RejectReason(this);
                rejectReason.Show();
            }
        }

        private void RejectRequest()
        {
           if (SelectedRequests != null)
           {
                Close();
                _requestService.RejectRequest(SelectedRequests);
                ReservationStatus.RemoveAt(Requests.IndexOf(SelectedRequests));
                Requests.Remove(SelectedRequests);
                MessageBox.Show($"Request rejected! \n Message sent: {RejectMessage}");
           }
        }

    }
}
