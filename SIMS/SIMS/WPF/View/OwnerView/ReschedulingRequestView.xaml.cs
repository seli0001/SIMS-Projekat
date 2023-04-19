using SIMS.Domain.Model;
using SIMS.WPF.ViewModel;
using SIMS.WPF.ViewModel.OwnerViewModel;
using System.Windows;

namespace SIMS.WPF.View.OwnerView
{
    /// <summary>
    /// Interaction logic for ReschedulingRequestView.xaml
    /// </summary>
    public partial class ReschedulingRequestView : Window
    {
        public ReschedulingRequestView(User user)
        {
            InitializeComponent();
            ReschedulingRequestViewModel reschedulingRequestVM = new ReschedulingRequestViewModel(user);
            DataContext = reschedulingRequestVM;

            if(DataContext is IClose vm)
            {
                vm.Close += () =>
                {
                    this.Close();
                };
            }
        }
    }
}
