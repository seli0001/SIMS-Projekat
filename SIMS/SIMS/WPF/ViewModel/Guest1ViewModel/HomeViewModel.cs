using Aplikacija.Core;
using SIMS.Domain.Model;
using SIMS.Service;
using SIMS.Service.UseCases;
using SIMS.View.FirstGuestView;
using SIMS.WPF.ViewModel.OwnerViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;
using System.ComponentModel;


namespace SIMS.WPF.ViewModel.Guest1ViewModel
{
    public class HomeViewModel
    {
        private ObservableCollection<Accommodation> _accommodations;
        public ObservableCollection<Accommodation> Accommodations
        {
            get { return _accommodations; }
            set
            {
                _accommodations = value;
            }
        }



        public Accommodation SelectedAccommodation { get; set; }

        private readonly GuestRatingService _guestRatingService;
        private readonly AccommodationService _accommodationService;

        public User LoggedInUser { get; set; }

        Guest1MainViewModel guest1MainViewModel;
        

        public HomeViewModel(User user, Guest1MainViewModel mvm)
        {
            LoggedInUser = user;

            _guestRatingService = new GuestRatingService();
            _accommodationService = new AccommodationService();

            Accommodations = new ObservableCollection<Accommodation>(_accommodationService.GetAll());
            guest1MainViewModel = mvm;
        }

        private ICommand _bookCommand;
        public ICommand BookCommand
        {
            get
            {
                return _bookCommand ?? (_bookCommand = new CommandBase(() => Book(), true));
            }
        }

        private ICommand _whereverCommand;
        public ICommand WhereverCommand
        {
            get
            {
                return _whereverCommand ?? (_whereverCommand = new CommandBase(() => Wherever(), true));
            }
        }


        private string _searchText1;
        public string SearchText1
        {
            get { return _searchText1; }
            set
            {
                _searchText1 = value;
                ValidateSearchText1(); 
            }
        }

        private string _searchText2;
        public string SearchText2
        {
            get { return _searchText2; }
            set
            {
                _searchText2 = value;
                ValidateSearchText2();
            }
        }

        private string _searchText3;
        public string SearchText3
        {
            get { return _searchText3; }
            set
            {
                _searchText3 = value;
                ValidateSearchText1(); 
            }
        }

        private string _searchText4;
        public string SearchText4
        {
            get { return _searchText4; }
            set
            {
                _searchText4 = value;
                ValidateSearchText1(); 
            }
        }


        private bool _isSearchValid;
        public bool IsSearchValid
        {
            get { return _isSearchValid; }
            set
            {
                _isSearchValid = value;
            }
        }



        private ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new CommandBase(() => SearchMethod(), true));
            }
        }


        private void SearchMethod()
        {
            int maxGuests;
            int.TryParse(SearchText3, out maxGuests);
            int minDays;
            int.TryParse(SearchText4, out minDays);
            var searchResults = SearchResults(SearchText1, SearchText2, maxGuests, minDays);
            Accommodations = new ObservableCollection<Accommodation>(searchResults);
        }


        private bool CanExecuteSearchCommand()
        {
            return IsSearchValid;
        }

        private void ExecuteSearchCommand()
        {
            // Perform search logic
        }

        private void ValidateSearchText1()
        {
            if (string.IsNullOrWhiteSpace(SearchText1))
            {
                IsSearchValid = false;
            }
            else
            {
                IsSearchValid = true;
            }
        }

        private void ValidateSearchText2()
        {
            if (string.IsNullOrWhiteSpace(SearchText2))
            {
                IsSearchValid = false;
            }
            else
            {
                IsSearchValid = true;
            }
        }




        private IEnumerable<Accommodation> SearchResults(string name, string location, int maxGuests, int minDays)
        {
            return Accommodations
                    .Where(acc => (string.IsNullOrEmpty(name) || acc.Name.ToLower().Contains(name.ToLower()))
                    && (string.IsNullOrEmpty(location) || acc.Location.Country.ToLower().Contains(location.ToLower()) || acc.Location.City.ToLower().Contains(location.ToLower()))
                    && (maxGuests == 0 || acc.MaxGuestsNumber >= maxGuests)
                    && (minDays == 0 || acc.MinBookingDays <= minDays))
                .Select(acc => acc);
        }

        private void Book()
        {
            if (SelectedAccommodation != null)
            {
                BookingViewModel BookingVM = new BookingViewModel(LoggedInUser, SelectedAccommodation);
                guest1MainViewModel.CurrentView = BookingVM;
            }
        }
        private void Wherever()
        {
            WhereverWheneverViewModel WhereverVM = new WhereverWheneverViewModel(LoggedInUser);
            guest1MainViewModel.CurrentView = WhereverVM;
        }
    }
}
