using System.Windows;
using SIMS.WPF.ViewModel.OwnerViewModel;
using SIMS.Domain.Model;
using SIMS.WPF.ViewModel;

namespace SIMS.View.OwnerView
{
    /// <summary>
    /// Interaction logic for OwnerMainView.xaml
    /// </summary>
    public partial class OwnerMainView : Window
    {
        public OwnerMainView(User user)
        {
            InitializeComponent();
            HomeViewModel ownerMainViewModel = new HomeViewModel(user);
            DataContext = ownerMainViewModel;
            if (DataContext is IClose vm)
            {
                vm.Close += () =>
                {
                    this.Close();
                };
            }
        }

    }
}
