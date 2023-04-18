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
    /// Interaction logic for AlternativeTourView.xaml
    /// </summary>
    public partial class AlternativeTourView : Window
    {
    
        private readonly TourService _tourService;
        public ObservableCollection<Tour> tours { get; set; }

        public Tour selectedTour { get; set; }
        public int userId;

        public AlternativeTourView(Tour tour, int user)
        {
            InitializeComponent();
            DataContext = this;
            userId = user;

            _tourService = new TourService();

            tours = new ObservableCollection<Tour>(_tourService.GetAlternative(tour));
        }

        private void BackToMainForm(object sender, RoutedEventArgs e)
        {
            MainGuest2View mainGuest2View = new MainGuest2View(userId);
            mainGuest2View.Show();

            Close();
        }

        private void NumberOfTourClick(object sender, RoutedEventArgs e)
        {
            NumberOfTourGuestView numberOfTourGuestView = new NumberOfTourGuestView(selectedTour, userId, 100000);
            numberOfTourGuestView.Show();
            Close();
        }
    }
}
