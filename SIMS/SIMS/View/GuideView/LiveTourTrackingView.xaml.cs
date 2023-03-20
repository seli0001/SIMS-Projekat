using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
using SIMS.Model.Guest2;
using SIMS.Model.Guide;
using SIMS.Repository.Guest2Repository;
using SIMS.Repository.GuideRepository;

namespace SIMS.View.GuideView
{
    /// <summary>
    /// Interaction logic for LiveTourTrackingView.xaml
    /// </summary>
    /// 
    public class NullableToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public partial class LiveTourTrackingView : Window
    {
        public ObservableCollection<BookedTour> BookedTour { get; set; }
        public ObservableCollection<Checkpoint> Checkpoints { get; set; }
        public Tour Tour { get; set; }
        public BookedTour SelectedBookedTour { get; set; }
        private readonly CheckpointRepository _checkpointRepository;
        private readonly BookedTourRepository _bookedTourRepository;
        private readonly TourRepository _tourRepository;
        
        public LiveTourTrackingView(Tour selectedTour)
        {
            InitializeComponent();
            _checkpointRepository = new CheckpointRepository();
            _bookedTourRepository = new BookedTourRepository();
            _tourRepository = new TourRepository();
            SelectedBookedTour = new BookedTour();
            Tour = selectedTour;
            BookedTour = new ObservableCollection<BookedTour>(_bookedTourRepository.GetAllByTourId(selectedTour.Id));
            Checkpoints = new ObservableCollection<Checkpoint>(_checkpointRepository.GetAllByTourId(selectedTour.Id).OrderBy(c => c.Index));
            Checkpoint FirstCheckpoint = Checkpoints.MinBy(c => c.Index);
            FirstCheckpoint.IsChecked = true;
            _checkpointRepository.Update(FirstCheckpoint);
            DataContext = this;
        }
        
        private void CheckCheckpoint_Click(object sender, RoutedEventArgs e)
        {
            Checkpoint NextCheckpoint = Checkpoints.Where(c => c.IsChecked == false).MinBy(c => c.Index);
            if(NextCheckpoint == null) 
            { 
                return;
            }
            NextCheckpoint.IsChecked = true;
            _checkpointRepository.Update(NextCheckpoint);
            if (!Checkpoints.Any(c => c.IsChecked == false))
            {
                Tour.Status = TourStatus.FINISHED;
                CheckpointButton.IsEnabled = false;
                _tourRepository.Update(Tour);
                MessageBox.Show("Tour is finished");
            }
        }

        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            if(UsersDataGrid.SelectedIndex != -1)
            {
                if(SelectedBookedTour.Checkpoint == null)
                {
                    SelectedBookedTour.Checkpoint = Checkpoints.Where(c => c.IsChecked).MaxBy(c => c.Index);
                    _bookedTourRepository.Update(SelectedBookedTour);
                }
                
            }
        }
    }
}
