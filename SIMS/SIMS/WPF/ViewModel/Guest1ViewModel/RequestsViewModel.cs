using SIMS.Domain.Model;
using SIMS.Model;
using SIMS.Repository;
using SIMS.Service.Services;
using System.Collections.ObjectModel;

namespace SIMS.WPF.ViewModel.Guest1ViewModel
{
    public class RequestsViewModel
    {
        public User LoggedInUser { get; set; }

        private ReschedulingRequestsService _reschedulingRequestsService;
        public static ObservableCollection<ReschedulingRequests> ReschedulingRequests { get; set; }
        public ReschedulingRequests SelectedRequest { get; set; }

        public RequestsViewModel(User user)
        {
            _reschedulingRequestsService = new ReschedulingRequestsService();
            ReschedulingRequests = new ObservableCollection<ReschedulingRequests>(_reschedulingRequestsService.GetByUserId(user.Id));
            LoggedInUser = user;
        }
    }
}
