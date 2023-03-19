using SIMS.Model;
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
    /// Interaction logic for MainGuest2View.xaml
    /// </summary>
    public partial class MainGuest2View : Window
    {

        private readonly TourRepository _tourRepository;

        public ObservableCollection<Tour> tours { get; set; }

        public Tour selectedTour { get; set; }

        public int userId;

        public MainGuest2View(int userid)
        {
            InitializeComponent();
            DataContext = this;
            userId = userid;
            _tourRepository = new TourRepository();
            tours = new ObservableCollection<Tour>(_tourRepository.GetAll());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string duration = tbDuration.Text;
            string language = tbLanguage.Text;
            int numberOfPeople = 0;
          
            try
            {
                numberOfPeople = Convert.ToInt32(tbNumber.Text);
            }
            catch
            {

            }
                
            string city = tbLocation.Text;

            List<Tour> tours = _tourRepository.GetAll();

            List<Tour> filtratedTours= new List<Tour>();


            filtratedTours = GetFiltratedTours(tours, city, duration, language, numberOfPeople);
            
        
            dgwTours.ItemsSource = filtratedTours;
        }


        public List<Tour> GetFiltratedTours(List <Tour> tours,string city,string duration,string language,int numberOfPeople)
        {


            if (duration != "")
            {
            tours= tours.Where(t => t.Language.StartsWith(language, StringComparison.OrdinalIgnoreCase)
            && t.Duration.ToString().Equals(duration)
            && (t.MaxNumberOfPeople >= numberOfPeople)
            && t.Location.City.StartsWith(city, StringComparison.OrdinalIgnoreCase)

            ).ToList();
            }
            else
            {
            tours= tours.Where(t => t.Language.StartsWith(language, StringComparison.OrdinalIgnoreCase)
            && (t.MaxNumberOfPeople >= numberOfPeople)
            && t.Location.City.StartsWith(city, StringComparison.OrdinalIgnoreCase)

            ).ToList();
            }

            return tours;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NumberOfTourGuestView numberOfTourGuestView = new NumberOfTourGuestView(selectedTour, userId);
            numberOfTourGuestView.Show();
            Close();
        }
    }
}
