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
using System.Collections.ObjectModel;
using SIMS.Domain.Model;
using SIMS.Repository;
using SIMS.Service.Services;
using SIMS.WPF.View;
using SIMS.WPF.View.GuideView;

namespace SIMS.View.GuideView
{
    /// <summary>
    /// Interaction logic for MainGuideView.xaml
    /// </summary>
    public partial class MainGuideView : Window
    {
        private readonly User _guide;
        private readonly TourService _tourService;
        private readonly BookedTourService _bookedTourService;
        private readonly VoucherService _voucherService;
        public ObservableCollection<Tour> Tours { get; set; }
        public ObservableCollection<Tour> AllTours { get; set; }
        public Tour SelectedTour { get; set; }
        public MainGuideView(User guide)
        {
            InitializeComponent();
            _guide = guide;
            _tourService = new TourService();
            _bookedTourService = new BookedTourService();
            _voucherService = new VoucherService();
            Tours = new ObservableCollection<Tour>(GetTours());
            AllTours = new ObservableCollection<Tour>(_tourService.GetAll());
            SelectedTour = new Tour();
            StartTourButton.IsEnabled = !HasTourInProgress();
            ViewStartedTourButton.IsEnabled = HasTourInProgress();
            DataContext = this;
        }
        
        public List<Tour> GetTours()
        {
            return _tourService.GetAllByGuideId(_guide.Id).Where(t => DateTime.Now.Date.Equals(t.StartTime.Time.Date)).ToList();
        }

        private void RegisterTour(object sender, RoutedEventArgs e)
        {
            TourRegistrationView tourRegistrationView = new TourRegistrationView(_guide);
            tourRegistrationView.ShowDialog();
            UpdateDataGrid();
            UpdateAllTours();
        }

        public void UpdateDataGrid()
        {
            Tours.Clear();
            foreach (var tour in GetTours())
            {
                Tours.Add(tour);
            }
        }

        public void UpdateAllTours()
        {
            AllTours.Clear();
            foreach(var tour in _tourService.GetAll())
            {
                AllTours.Add(tour);
            }
        }

        public bool HasTourInProgress()
        {
            return Tours.FirstOrDefault(t => t.Status == TourStatus.STARTED) != null;
        }

        private void dataGridTours_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridTours.SelectedIndex == -1)
                return;
            if(Tours.FirstOrDefault(t => t.Status == TourStatus.STARTED) != null)
            {
                StartTourButton.IsEnabled = false;
            }
            else if(SelectedTour.Status == TourStatus.NOT_STARTED)
            {
                StartTourButton.IsEnabled = true;
            }
            else
            {
                StartTourButton.IsEnabled = false;
            }
        }

        private void StartTourButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedTour.Status = TourStatus.STARTED;
            StartTourButton.IsEnabled = false;
            _tourService.Update(SelectedTour);
            LiveTourTrackingView liveTourTrackingView = new LiveTourTrackingView(SelectedTour);
            liveTourTrackingView.ShowDialog();
            ViewStartedTourButton.IsEnabled = HasTourInProgress();
        }

        private void ViewStartedTourButton_Click(object sender, RoutedEventArgs e)
        {
            Tour tourInProgress = Tours.FirstOrDefault(t => t.Status == TourStatus.STARTED);
            if (tourInProgress != null)
            {
                ViewStartedTourButton.IsEnabled = false;
                LiveTourTrackingView liveTourTrackingView = new LiveTourTrackingView(tourInProgress);
                liveTourTrackingView.ShowDialog();
                
            }
            ViewStartedTourButton.IsEnabled = HasTourInProgress();
        }

        private void tourRatingButton_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedTour.Status == TourStatus.FINISHED)
            {
                TourReviewView tourReviewView = new TourReviewView(SelectedTour.Id);
                tourReviewView.ShowDialog();
            }
        }
        
        private void cancelTourButton_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedTour.Status == TourStatus.NOT_STARTED && SelectedTour.StartTime.Time >= DateTime.Now.AddDays(2))
            {
                _tourService.CancelTour(SelectedTour);
                AllTours.Remove(SelectedTour);
                MessageBox.Show("Uspesno ste otkazali turu");
            }
        }

        private void tourStatisticButton_Click(object sender, RoutedEventArgs e)
        {
            TourStatisticsView tourStatisticsView = new TourStatisticsView(SelectedTour);
            tourStatisticsView.ShowDialog();
        }

        private void mostVisitedButton_Click(object sender, RoutedEventArgs e)
        {
            TourStatisticsView tourStatisticsView = new TourStatisticsView(_tourService.GetById(_bookedTourService.GetMostVisited()));
            tourStatisticsView.ShowDialog();
        }
    }
}
