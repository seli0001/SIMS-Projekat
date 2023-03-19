using SIMS.Model.Guide;
using SIMS.Repository.GuideRepository;
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

namespace SIMS.View.Guest2View
{
    /// <summary>
    /// Interaction logic for AlternativeTourView.xaml
    /// </summary>
    public partial class AlternativeTourView : Window
    {
        private readonly TourRepository _tourRepository;

        public ObservableCollection<Tour> tours { get; set; }

        public Tour selectedTour { get; set; }
        public int userId;

        public AlternativeTourView(Tour tour, int user)
        {
            InitializeComponent();
            DataContext = this;
            userId = user;
            _tourRepository = new TourRepository();
            tours = new ObservableCollection<Tour>(_tourRepository.GetAlternative(tour));

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainGuest2View mainGuest2View = new MainGuest2View(userId);
            mainGuest2View.Show();

            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NumberOfTourGuestView numberOfTourGuestView = new NumberOfTourGuestView(selectedTour, userId);
            numberOfTourGuestView.Show();
            Close();
        }
    }
}
