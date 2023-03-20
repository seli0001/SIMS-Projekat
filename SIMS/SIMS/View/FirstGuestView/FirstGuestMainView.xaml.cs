using SIMS.Model;
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
using System.Xml.Linq;
using SIMS.Model.AccommodationModel;
using SIMS.Repository;

namespace SIMS.View.FirstGuestView
{
    public enum Type
    {
        apartment,
        house,
        hut
    }
    /// <summary>
    /// Interaction logic for FirstGuestMainView.xaml
    /// </summary>
    /// 
    public partial class FirstGuestMainView : Window
    {
        public static Accommodation SelectedAccommodation { get; set; }

        public ObservableCollection<Location> Locations { get; set; }

        private readonly LocationRepository _locationRepository;

        public ObservableCollection<Accommodation> Accommodations { get; set; }

        private readonly AccommodationRepository _repository;

        public FirstGuestMainView()
        {
            InitializeComponent();
            DataContext = this;
            cbSearch.ItemsSource = Enum.GetValues(typeof(Type));
            _repository = new AccommodationRepository();
            Accommodations = new ObservableCollection<Accommodation>(_repository.GetForView());

        }


        public void Update()
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Accommodation> SearchResults(string name, string location, string type, int maxGuests, int minDays)
        {
            return Accommodations
                    .Where(acc => (string.IsNullOrEmpty(name) || acc.Name.ToLower().Contains(name.ToLower()))
                    && (string.IsNullOrEmpty(location) || acc.Location.Country.ToLower().Contains(location.ToLower()) || acc.Location.City.ToLower().Contains(location.ToLower()))
                    && (string.IsNullOrEmpty(type) || acc.Type.ToString().ToLower().Contains(type))
                    && (maxGuests == 0 || acc.MaxGuestsNumber >= maxGuests)
                    && (minDays == 0 || acc.MinBookingDays <= minDays))
                .Select(acc => acc);
        }

        private void Book_Click(object sender, RoutedEventArgs e)
        {
            if (gridGuest.SelectedIndex != -1)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure?", "Booking accomodation", MessageBoxButton.YesNo);
                /*if (MessageBoxResult.Yes == result)
                {
                    MessageBox.Show(SelectedAccommodation.Name.ToString());
                    MessageBox.Show("Accommodation booked");
                }*/
                FirstGuestBookingView firstGuestBookingView = new FirstGuestBookingView(SelectedAccommodation);
                firstGuestBookingView.Show();
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
            txtSearch1.Text = "";
            txtSearch2.Text = "";
            txtSearch3.Text = "";
            cbSearch.SelectedIndex = -1;
        }


        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string name = txtSearch.Text;
            string location = txtSearch1.Text;
            string type = cbSearch.Text.ToLower();
            int maxGuests;
            int.TryParse(txtSearch2.Text, out maxGuests);
            int minDays;
            int.TryParse(txtSearch3.Text, out minDays);
            if (minDays == 0)
                minDays = 15;

            gridGuest.ItemsSource = SearchResults(name, location, type, maxGuests, minDays);
        }

        
    }
}
