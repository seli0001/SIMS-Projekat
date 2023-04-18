using System.Windows;
using SIMS.WPF.ViewModel.OwnerViewModel;
using SIMS.Domain.Model;
using SIMS.WPF.ViewModel;

namespace SIMS.View.OwnerView
{
    /// <summary>
    /// Interaction logic for AccommondationRegistration.xaml
    /// </summary>
    public partial class AccommondationRegistration : Window
    {
      
        public AccommondationRegistration(User user)
        {
            InitializeComponent();
            Title = "Create new accommondation";
            AccommodationRegistrationViewModel accommodationRegistrationViewModel = new AccommodationRegistrationViewModel(user);
            DataContext = accommodationRegistrationViewModel;
            
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
