using SIMS.Domain.Model;
using SIMS.Model;
using SIMS.Repository;
using System.Collections.ObjectModel;

using System.Windows;

namespace SIMS.View.FirstGuestView
{
    /// <summary>
    /// Interaction logic for RequestsView.xaml
    /// </summary>
    public partial class RequestsView : Window
    {
        public User LoggedInUser { get; set; }

        private ReschedulingRequestsRepository _reschedulingRequestsRepository;
        public static ObservableCollection<ReschedulingRequests> ReschedulingRequests { get; set; }
        public ReschedulingRequests SelectedRequest { get; set; }
        

        public RequestsView(User user)
        {
            InitializeComponent();
            DataContext = this;
            _reschedulingRequestsRepository = new ReschedulingRequestsRepository();
            ReschedulingRequests = new ObservableCollection<ReschedulingRequests>(_reschedulingRequestsRepository.GetByUserId(user.Id));
            LoggedInUser = user;

        }

        /*private bool CompareDates(DateTime date1, DateTime date2)
        {
            if (date2 > date1) return false;
            TimeSpan difference = date1 - date2;
            int days = difference.Days;
            if (days > 5) return false;
            else return true;
        }*/

        
    }
}
