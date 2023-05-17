using SIMS.Domain.Model;
using SIMS.WPF.ViewModel.OwnerViewModel;
using SIMS.WPF.ViewModel;
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
    /// Interaction logic for OwnerMain.xaml
    /// </summary>
    public partial class OwnerMain : Window
    {
        public OwnerMain()
        {

        }
        public OwnerMain(User user)
        {
            InitializeComponent();
            MainViewModel ownerMainViewModel = new MainViewModel(user);
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
