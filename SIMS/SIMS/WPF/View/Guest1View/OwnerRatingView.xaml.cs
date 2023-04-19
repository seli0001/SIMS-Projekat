using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using SIMS.Domain.Model;
using SIMS.Repository;
using SIMS.WPF.ViewModel;
using SIMS.WPF.ViewModel.Guest1ViewModel;

namespace SIMS.View.FirstGuestView
{
    /// <summary>
    /// Interaction logic for OwnerRatingView.xaml
    /// </summary>
    public partial class OwnerRatingView : Window
    {
       
        public OwnerRatingView(Reservation reservation, User user)
        {
            InitializeComponent();
            OwnerRatingViewModel ownerRatingViewModel = new OwnerRatingViewModel(reservation, user);
            DataContext = ownerRatingViewModel;
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
