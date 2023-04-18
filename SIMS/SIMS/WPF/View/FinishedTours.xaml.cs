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
    /// Interaction logic for FinishedTours.xaml
    /// </summary>
    public partial class FinishedTours : Window
    {
        public ObservableCollection<BookedTour> tours { get; set; }
        private readonly BookedTourService _bookedTourService;
        public BookedTour selectedTour { get; set; }
        public int userId;
        public FinishedTours(int userId)
        {
            InitializeComponent();
            DataContext = this;
            this.userId = userId;
            selectedTour = new BookedTour();
            _bookedTourService = new BookedTourService();
            tours = new ObservableCollection<BookedTour>(_bookedTourService.GetUserFinished(userId));
        }

        private void RateTourClick(object sender, RoutedEventArgs e)
        {
            RateTourView rateTour = new RateTourView(selectedTour, userId);
            rateTour.Show();
            Close();
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
