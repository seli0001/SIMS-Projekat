using SIMS.Domain.Model;
using SIMS.Service.Services;
using SIMS.WPF.ViewModel;
using SIMS.WPF.ViewModel.Guest2ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for MainGuest2View.xaml
    /// </summary>
    public partial class MainGuest2View : Window
    {
        public MainGuest2View(int userid)
        {
            InitializeComponent();
            MainGuest2ViewModel mainGuest2ViewModel = new MainGuest2ViewModel(userid);
            DataContext = mainGuest2ViewModel;
            
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
