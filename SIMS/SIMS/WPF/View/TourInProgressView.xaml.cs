using SIMS.Domain.Model.Guide;
using SIMS.Domain.Model;
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
    /// Interaction logic for TourInProgressView.xaml
    /// </summary>
    public partial class TourInProgressView : Window
    {
        public BookedTour bookedTour { get; set; }
        private CheckpointService _checkpointService { get; set; }

        public ObservableCollection<Checkpoint> checkpoints { get; set; }
        public int userId { get; set; }

        public TourInProgressView(BookedTour bookedTour, int userId)
        {
            InitializeComponent();
            this.bookedTour = bookedTour;
            DataContext = this;
            this.userId = userId;
            _checkpointService = new CheckpointService();
            checkpoints = new ObservableCollection<Checkpoint>(_checkpointService.GetAllByTourId(bookedTour.TourId));

        }

        private void BackToMainGuest2ViewClick(object sender, RoutedEventArgs e)
        {
            MainGuest2View mainGuest2View = new MainGuest2View(userId);
            mainGuest2View.Show();
            Close();
        }

        private void BackToMenuClick(object sender, RoutedEventArgs e)
        {
            MenuGuest2 menu = new MenuGuest2(userId);
            menu.Show();
            Close();
        }
    }
}
