using LiveCharts;
using SIMS.Domain.Model;
using SIMS.Service.Services;
using SIMS.WPF.ViewModel;
using SIMS.WPF.ViewModel.GuideViewModel;
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

namespace SIMS.WPF.View.GuideView
{
    /// <summary>
    /// Interaction logic for TourStatisticsView.xaml
    /// </summary>
    /// 
    
    public partial class TourStatisticsView : Window
    {

        public TourStatisticsView(Tour selectedTour)
        {
            InitializeComponent();
            TourStatisticsViewModel tourStatisticsViewModel = new TourStatisticsViewModel(selectedTour);
            DataContext = tourStatisticsViewModel;

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
