using LiveCharts;
using SIMS.Domain.Model;
using SIMS.Service.Services;
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

        public SeriesCollection SeriesCollectionAge { get; set; }
        public SeriesCollection SeriesCollectionVoucher { get; set; }
        private readonly BookedTourService _bookedTourService;
        public TourStatisticsView(Tour selectedTour)
        {
            InitializeComponent();
            _bookedTourService = new BookedTourService();
            SeriesCollectionAge = _bookedTourService.getDataForChartByAge(selectedTour);
            SeriesCollectionVoucher = _bookedTourService.getDataForChartByVoucher(selectedTour);
            DataContext = this;

            tourNameLabel.Content += selectedTour.Name;
            cityLabel.Content += selectedTour.Location.City;
            countryLabel.Content += selectedTour.Location.Country;
            dateLabel.Content += selectedTour.StartTime.Time.ToString();
        }

        private void closeWindowButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
    
}
