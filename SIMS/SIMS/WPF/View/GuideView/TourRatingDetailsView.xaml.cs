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

namespace SIMS.WPF.View
{
    /// <summary>
    /// Interaction logic for TourRatingDetailsView.xaml
    /// </summary>
    public partial class TourRatingDetailsView : Window
    {
        private TourRatringDetailsViewModel tourRatringDetailsViewModel;
        public TourRatingDetailsView(TourRating tourRating)
        {
            InitializeComponent();
            tourRatringDetailsViewModel = new TourRatringDetailsViewModel(tourRating);
            DataContext = tourRatringDetailsViewModel;

            if (DataContext is IClose vm)
            {
                vm.Close += () =>
                {
                    this.Close();
                };
            }
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
            else if (e.Key == Key.F1)
            {
                if (tourRatringDetailsViewModel.IsEnabled) tourRatringDetailsViewModel.Report();
                else MessageBox.Show("Ne mozete prijaviti recenziju!");
            }
            else if (e.Key == Key.F2)
            {
                Close();
            }
        }
    }
}
