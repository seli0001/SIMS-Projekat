using HarfBuzzSharp;
using LiveCharts;
using LiveCharts.Wpf;
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
    /// Interaction logic for TourRequestStatisticsView.xaml
    /// </summary>
    public partial class TourRequestStatisticsView : Window
    {
        private TourRequestService requestService;
        public TourRequestStatisticsView()
        {
            InitializeComponent();
            requestService = new TourRequestService();
            var result = requestService.GetAll().GroupBy(g => new { g.Location.City, g.StartDate.Date.Year }).Select(group => new { city = group.Key.City, year = group.Key.Year, number = group.Count()});
            foreach( var group in result)
            {
                text.Content += "Za lokaciju " + group.city + " je bilo: " + group.number + " zahteva u "+ group.year+" godini \n";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void izbor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(izbor.SelectedIndex == 0 && requestService != null)
            {
                text.Content = "";
                var result = requestService.GetAll().GroupBy(g => new { g.Location.City, g.StartDate.Date.Year }).Select(group => new { city = group.Key.City, year = group.Key.Year, number = group.Count() });
                foreach (var group in result)
                {
                    text.Content += "Za lokaciju " + group.city + " je bilo: " + group.number + " zahteva u " + group.year + " godini \n";
                }
            }
            else if ( requestService != null)
            {
                text.Content = "";
                var result = requestService.GetAll().GroupBy(g => new { g.Language, g.StartDate.Date.Year }).Select(group => new { language = group.Key.Language, year = group.Key.Year, number = group.Count() });
                foreach (var group in result)
                {
                    text.Content += "Za jezik " + group.language + " je bilo: " + group.number + " zahteva u " + group.year + " godini \n";
                }
            }
        }
    }
    
}
