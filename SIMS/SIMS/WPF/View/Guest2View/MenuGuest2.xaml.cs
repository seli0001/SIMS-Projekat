using SIMS.Domain.Model;
using SIMS.Service.Services;
using SIMS.WPF.ViewModel;
using SIMS.WPF.ViewModel.Guest2ViewModel;
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

namespace SIMS.WPF.View
{
    /// <summary>
    /// Interaction logic for MenuGuest2.xaml
    /// </summary>
    public partial class MenuGuest2 : Window
    {
       
        public MenuGuest2(int userId)
        {
            InitializeComponent();
            MenuGuest2ViewModel menuGuest2 = new MenuGuest2ViewModel(userId);
            DataContext = menuGuest2;
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
