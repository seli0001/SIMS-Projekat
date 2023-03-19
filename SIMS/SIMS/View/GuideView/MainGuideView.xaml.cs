using SIMS.Model.Guide;
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
using SIMS.Model;
using System.Collections.ObjectModel;

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
            StartTourButton.IsEnabled = false;
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

        private void dataGridTours_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(SelectedTour.Status == TourStatus.NOT_STARTED)
            {
                StartTourButton.IsEnabled = true;
            }
            else
            {
                StartTourButton.IsEnabled = false;
            }
        }
    }
}
