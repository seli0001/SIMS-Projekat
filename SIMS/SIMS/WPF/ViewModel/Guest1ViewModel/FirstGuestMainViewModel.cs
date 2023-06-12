using SIMS.Domain.Model;
using SIMS.Repository;
using SIMS.Service.UseCases;
using SIMS.View.FirstGuestView;
using SIMS.WPF.ViewModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace SIMS.WPF.ViewModel.Guest1ViewModel
{
    public enum Type
    {
        apartment,
        house,
        hut
    }
    public class FirstGuestMainViewModel : ViewModelBase
    {
        public static Accommodation SelectedAccommodation { get; set; }

        public ObservableCollection<Location> Locations { get; set; }

        private readonly LocationRepository _locationRepository;

        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public User LoggedInUser { get; set; }

        private readonly AccommodationService _service;
        
        public void Update()
        {
            throw new NotImplementedException();
        }

        public FirstGuestMainViewModel(User user)
        {
            _service = new AccommodationService();
            Accommodations = new ObservableCollection<Accommodation>(_service.GetAll());
            LoggedInUser = user;
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
    }
}
