using SIMS.Domain.Model;
using SIMS.Repository;
using SIMS.Service.Services;
using SIMS.View.GuideView;
using SIMS.WPF.ViewModel;
using SIMS.WPF.ViewModel.GuideViewModel;
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
    /// Interaction logic for TourReviewWindows.xaml
    /// </summary>
    public partial class TourReviewView : Window
    {
        private readonly TourRatingService _tourRatingService;
        public TourRating SelectedTourRating { get; set; }
        public ObservableCollection<TourRating> TourRatings { get; set; }
        private TourReviewViewModel tourReviewViewModel;
        public TourReviewView(int tourId)
        {
            InitializeComponent();
            tourReviewViewModel = new TourReviewViewModel(tourId);

            DataContext = tourReviewViewModel;
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
                if (reviewDataGrid.SelectedIndex != -1) tourReviewViewModel.RatingDeatails();
                else MessageBox.Show("Morate izabrati recenziju!");
            }
            else if (e.Key == Key.F2)
            {
                Close();
            }
        }
    }
}
