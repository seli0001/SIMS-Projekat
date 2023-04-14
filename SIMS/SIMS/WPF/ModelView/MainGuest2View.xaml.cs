using SIMS.Domain.Model.Guide;
using SIMS.Domain.Model;
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
using SIMS.Service.Services;

namespace SIMS.WPF.ModelView
{
    /// <summary>
    /// Interaction logic for MainGuest2View.xaml
    /// </summary>
    public partial class MainGuest2View : Window
    {


        private readonly TourService _tourService;


        public ObservableCollection<Tour> tours { get; set; }

        public Tour selectedTour { get; set; }

        public int userId;

        public MainGuest2View(int userid)
        {
            InitializeComponent();
            DataContext = this;
            userId = userid;
  
            _tourService=new TourService();
             tours = new ObservableCollection<Tour>(_tourService.GetNotStarted());

        }

        private void SearchClick(object sender, RoutedEventArgs e)
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
            string country = tbCountry.Text;

            List<Tour> tours=_tourService.GetAll();
            List<Tour> filtratedTours= new List<Tour>();


            filtratedTours = GetFiltratedTours(tours, city,country, duration, language, numberOfPeople);
            
        
            dgwTours.ItemsSource = filtratedTours;
        }


        public List<Tour> GetFiltratedTours(List <Tour> tours,string city,string country,string duration,string language,int numberOfPeople)
        {


            if (duration != "")
            {
            tours= tours.Where(t => t.Language.StartsWith(language, StringComparison.OrdinalIgnoreCase)
            && t.Duration.ToString().Equals(duration)
            && (t.MaxNumberOfPeople >= numberOfPeople)
            && t.Location.City.StartsWith(city, StringComparison.OrdinalIgnoreCase)
            && t.Location.Country.StartsWith(country, StringComparison.OrdinalIgnoreCase)
            && t.Status.ToString().Equals("NOT_STARTED")
            ).ToList();
            }
            else
            {
            tours= tours.Where(t => t.Language.StartsWith(language, StringComparison.OrdinalIgnoreCase)
            && (t.MaxNumberOfPeople >= numberOfPeople)
            && t.Location.City.StartsWith(city, StringComparison.OrdinalIgnoreCase)
            && t.Location.Country.StartsWith(country, StringComparison.OrdinalIgnoreCase)
            && t.Status.ToString().Equals("NOT_STARTED")
            ).ToList();
            }

            return tours;
        }

        private void NumberOfToursClick(object sender, RoutedEventArgs e)
        {
            NumberOfTourGuestView numberOfTourGuestView = new NumberOfTourGuestView(selectedTour, userId);
            numberOfTourGuestView.Show();
            Close();
        }
    }
}
