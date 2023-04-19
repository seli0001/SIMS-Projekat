using SIMS.Domain.Model;
using SIMS.Model;
using SIMS.Repository;
using SIMS.WPF.ViewModel;
using SIMS.WPF.ViewModel.Guest1ViewModel;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;


namespace SIMS.View.FirstGuestView
{
    /// <summary>
    /// Interaction logic for CreateReschedulingRequest.xaml
    /// </summary>
    public partial class CreateReschedulingRequest : Window
    {
        public CreateReschedulingRequest(Reservation selectedReservation, User user)
        {
            InitializeComponent();
            CreateReschedulingRequestViewModel createReschedulingRequestViewModel = new CreateReschedulingRequestViewModel(selectedReservation, user);
            DataContext = createReschedulingRequestViewModel;

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
