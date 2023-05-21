using SIMS.Domain.Model;
using SIMS.WPF.ViewModel.Guest1ViewModel;

using System.Windows;

namespace SIMS.View.FirstGuestView
{
    public partial class RequestsView : Window
    {
        public RequestsView(User user)
        {
            InitializeComponent();
            RequestsViewModel requestsViewModel = new RequestsViewModel(user);
            DataContext = requestsViewModel;
        }
    }
}
