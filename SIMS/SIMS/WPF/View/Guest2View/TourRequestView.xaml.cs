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

namespace SIMS.WPF.View.Guest2View
{
    /// <summary>
    /// Interaction logic for TourRequestView.xaml
    /// </summary>
    public partial class TourRequestView : Window
    {
        public TourRequestView(int userId)
        {
            InitializeComponent();
            TourRequestViewModel tourRequesViewModel = new TourRequestViewModel(userId);
            DataContext = tourRequesViewModel;
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
