using SIMS.Repository.GuideRepository;
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
using SIMS.Domain.Model.Guide;
using SIMS.Domain.Model;

namespace SIMS.View.GuideView
{
    /// <summary>
    /// Interaction logic for MainGuideView.xaml
    /// </summary>
    public partial class MainGuideView : Window
    {
        private readonly User _guide;
        private readonly TourRepository _tourRepository;
        public ObservableCollection<Tour> Tours { get; set; }
        public Tour SelectedTour { get; set; }
        public MainGuideView(User guide)
        {
            InitializeComponent();
            _guide = guide;
            _tourRepository = new TourRepository();
            Tours = new ObservableCollection<Tour>(GetTours());
            SelectedTour = new Tour();
            StartTourButton.IsEnabled = !HasTourInProgress();
            ViewStartedTourButton.IsEnabled = HasTourInProgress();
            DataContext = this;
        }
        
        public List<Tour> GetTours()
        {
            return _tourRepository.GetAllByGuideId(_guide.Id).Where(t => DateTime.Now.Date.Equals(t.StartTime.Time.Date)).ToList();
        }

        private void RegisterTour(object sender, RoutedEventArgs e)
        {
            TourRegistrationView tourRegistrationView = new TourRegistrationView(_guide);
            tourRegistrationView.ShowDialog();
            UpdateDataGrid();
        }

        public void UpdateDataGrid()
        {
            Tours.Clear();
            foreach (var tour in GetTours())
            {
                Tours.Add(tour);
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
            _tourRepository.Update(SelectedTour);
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
    }
}
