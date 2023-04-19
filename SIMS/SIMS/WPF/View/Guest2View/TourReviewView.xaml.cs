using SIMS.Domain.Model;
using SIMS.Repository;
using SIMS.Service.Services;
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
        public TourReviewView(int tourId)
        {
            InitializeComponent();
            DataContext = this;
            _tourRatingService = new TourRatingService();
            SelectedTourRating = new TourRating();
            TourRatings = new ObservableCollection<TourRating>(_tourRatingService.GetAllByTourId(tourId));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TourRatingDetailsView tourRatingDetailsView = new TourRatingDetailsView(SelectedTourRating);
            tourRatingDetailsView.ShowDialog();
        }
    }
}
