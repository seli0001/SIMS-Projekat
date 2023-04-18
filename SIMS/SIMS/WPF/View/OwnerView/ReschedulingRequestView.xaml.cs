using SIMS.Domain.Model;
using SIMS.WPF.ViewModel;
using SIMS.WPF.ViewModel.OwnerViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
