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
using SIMS.WPF.ViewModel;

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
            StartTourButton.IsEnabled = !HasTourInProgress() && Tours.Count > 0 && Tours.FirstOrDefault(t => t.Status == TourStatus.NOT_STARTED) != null;
            ViewStartedTourButton.IsEnabled = HasTourInProgress();
            mostVisitedButton.IsEnabled = AllTours.FirstOrDefault(t => t.Status == TourStatus.FINISHED) != null;
            cancelTourButton.IsEnabled = false;
            tourRatingButton.IsEnabled = false;
            tourStatisticButton.IsEnabled = false;
            DataContext = this;
        }
        
        public List<Tour> GetTours()
        {
            return _tourService.GetAllByGuideId(_guide.Id).Where(t => DateTime.Now.Date.Equals(t.StartTime.Time.Date)).ToList();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                if(TabControl.SelectedIndex == 0)
                    RegisterTour();
                else if(TabControl.SelectedIndex == 1)
                    if (cancelTourButton.IsEnabled)
                        cancelTourButton_Click(null, null);
                    else
                        MessageBox.Show("Morate odabrati turu koja moze da se otkaze!");
            }
            if (e.Key == Key.F2)
            {
                if (TabControl.SelectedIndex == 0)
                {
                    if (StartTourButton.IsEnabled) StartTourButtonClick();
                    else MessageBox.Show("Morate odabrati turu koju zelite da zapocnete!");
                }
                else if (TabControl.SelectedIndex == 1)
                {
                    if (tourRatingButton.IsEnabled) tourRatingButton_Click(null, null);
                    else MessageBox.Show("Morate odabrati turu koju zelite da ocenite!");
                }
            }
            if (e.Key == Key.F3)
            {
                if ( TabControl.SelectedIndex == 0)
                {
                    if (ViewStartedTourButton.IsEnabled) ViewStartedTourButtonClick();
                    else MessageBox.Show("Nemate zapocetu turu!");
                }
                else if (TabControl.SelectedIndex == 1)
                {
                    if (tourStatisticButton.IsEnabled) tourStatisticButton_Click(null, null);
                    else MessageBox.Show("Morate odabrati turu za koju zelite da vidite statistiku!");
                }
                
            }
            if (e.Key == Key.F4)
            {
                if (TabControl.SelectedIndex == 1)
                {
                    if (mostVisitedButton.IsEnabled) mostVisitedButton_Click(null, null);
                    else MessageBox.Show("Nemate zavrsenu turu!");
                }
            }
            if (e.Key == Key.D1 && Keyboard.Modifiers == ModifierKeys.Control)
            {
                TabControl.SelectedIndex = 0;
            }
            if (e.Key == Key.D2 && Keyboard.Modifiers == ModifierKeys.Control)
            {
                TabControl.SelectedIndex = 1;
            }
            if (e.Key == Key.D3 && Keyboard.Modifiers == ModifierKeys.Control)
            {
                TabControl.SelectedIndex = 2;
            }
            if (e.Key == Key.D4 && Keyboard.Modifiers == ModifierKeys.Control)
            {
                TabControl.SelectedIndex = 3;
            }
        }

        public void RegisterTour()
        {
            TourRegistrationView tourRegistrationView = new TourRegistrationView(_guide);
            tourRegistrationView.ShowDialog();
            UpdateDataGrid();
            UpdateAllTours();
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
            StartTourButtonClick();
        }

        private void StartTourButtonClick()
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
            ViewStartedTourButtonClick();
        }

        private void ViewStartedTourButtonClick()
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
            if (dataGridAllTours.SelectedIndex != -1 && SelectedTour.Status == TourStatus.FINISHED)
            {
                TourStatisticsView tourStatisticsView = new TourStatisticsView(SelectedTour);
                tourStatisticsView.ShowDialog();
            }
        }

        private void mostVisitedButton_Click(object sender, RoutedEventArgs e)
        {
            TourStatisticsView tourStatisticsView = new TourStatisticsView(_tourService.GetMostVisited());
            tourStatisticsView.ShowDialog();
        }

        private void dataGridAllTours_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridAllTours.SelectedIndex == -1)
            {
                cancelTourButton.IsEnabled = false;
                tourRatingButton.IsEnabled = false;
                tourStatisticButton.IsEnabled = false;
            }
            if (SelectedTour.Status == TourStatus.NOT_STARTED && SelectedTour.StartTime.Time >= DateTime.Now.AddDays(2))
            {
                cancelTourButton.IsEnabled = true;
                tourRatingButton.IsEnabled = false;
                tourStatisticButton.IsEnabled = false;
            }
            else if (SelectedTour.Status == TourStatus.FINISHED)
            {
                cancelTourButton.IsEnabled = false;
                tourRatingButton.IsEnabled = true;
                tourStatisticButton.IsEnabled = true;
            }
            else
            {
                cancelTourButton.IsEnabled = false;
                tourRatingButton.IsEnabled = false;
                tourStatisticButton.IsEnabled = false;
            }
        }
    }
}
