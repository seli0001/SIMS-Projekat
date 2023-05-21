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
using SIMS.Model;
using SIMS.Repository;
using SIMS.Domain.Model;
using SIMS.Service.UseCases;
using SIMS.Service;
using SIMS.WPF.ViewModel.OwnerViewModel;
using SIMS.Service.Services;
using System.Threading;

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
        private readonly AccommodationService _accommodationService;

        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public User LoggedInUser { get; set; }

        private readonly AccommodationService _service;
        private readonly GuestMainService _guestMainService;
        private Timer timer;


        public FirstGuestMainView(User user)
        {
            InitializeComponent();
            DataContext = this;
            cbSearch.ItemsSource = Enum.GetValues(typeof(Type));
            _service = new AccommodationService();
            Accommodations = new ObservableCollection<Accommodation>(_service.GetAll());
            LoggedInUser = user;
            _guestMainService = new GuestMainService();
            _accommodationService = new AccommodationService();
            timer = new Timer(CheckCondition, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));
        }

        private void CheckCondition(object? state)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {

                int temp = _guestMainService.checkIsSuperGuest(LoggedInUser);

                if (temp == 1)
                {
                    _accommodationService.makeSuperGuest(LoggedInUser);
                    MessageBox.Show("Congratulations You Have Became a SUPERGUEST!!!!");
                }
                else if (temp == 0)
                {
                    _accommodationService.deleteSuperGuest(LoggedInUser);
                    MessageBox.Show("You have lost superguest title");
                }
            });

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
                FirstGuestBookingView firstGuestBookingView = new FirstGuestBookingView(SelectedAccommodation, LoggedInUser);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ShowReservations showReservations = new ShowReservations(LoggedInUser);
            showReservations.Show();
        }
    }
}
