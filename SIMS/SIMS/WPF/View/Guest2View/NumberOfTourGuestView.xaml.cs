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
    /// Interaction logic for NumberOfTourGuestView.xaml
    /// </summary>
    public partial class NumberOfTourGuestView : Window
    {
        
        public NumberOfTourGuestView(Tour selectedtour, int userid, int voucherId)
        {
            InitializeComponent();
            NumberOfTourGuestViewModel numberOfTourGuestViewModel = new NumberOfTourGuestViewModel( selectedtour, userid,voucherId);
            DataContext = numberOfTourGuestViewModel;
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
