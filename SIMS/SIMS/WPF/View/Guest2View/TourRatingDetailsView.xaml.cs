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

namespace SIMS.WPF.View
{
    /// <summary>
    /// Interaction logic for TourRatingDetailsView.xaml
    /// </summary>
    public partial class TourRatingDetailsView : Window
    {
        private readonly TourRatingService _tourRatingService;
        private TourRating rating;
        public TourRatingDetailsView(TourRating tourRating)
        {
            InitializeComponent();
            _tourRatingService = new TourRatingService();
            guestNameLabel.Content += tourRating.bookedTour.User.Username;
            knowLabel.Content += tourRating.GuideKnown.ToString();
            languageLabel.Content += tourRating.GuideLanguage.ToString();
            tourLabel.Content += tourRating.TourReview.ToString();
            checkpointLabel.Content += tourRating.bookedTour.Checkpoint.Name;
            comment.Text += tourRating.Comment;
            rating = tourRating;
            if(!tourRating.Valid)
            {
                reportButton.IsEnabled = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Da li ste sigurni da zelite da prijavite recenziju?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _tourRatingService.ReportRating(rating.Id);
            }
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
