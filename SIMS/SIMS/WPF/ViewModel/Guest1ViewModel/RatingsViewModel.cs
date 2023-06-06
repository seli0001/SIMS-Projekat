using SIMS.Domain.Model;
using SIMS.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.WPF.ViewModel.Guest1ViewModel
{
    public class RatingsViewModel
    {
        public User LoggedInUser { get; set; }

        private GuestRatingRepository _guestRatingRepository;
        private OwnerRatingRepository _ownerRatingRepository;
        private UserRepository _userRepository;
        public static ObservableCollection<GuestRating> GuestRatings { get; set; }
        public static ObservableCollection<OwnerRating> OwnerRatings { get; set; }
        public static ObservableCollection<User> Users { get; set; }
        public GuestRating SelectedGuestRating { get; set; }

        public RatingsViewModel(User user)
        {
            _guestRatingRepository = new GuestRatingRepository();
            _ownerRatingRepository = new OwnerRatingRepository();
            _userRepository = new UserRepository();
            OwnerRatings = new ObservableCollection<OwnerRating>(_ownerRatingRepository.GetAll());
            GuestRatings = new ObservableCollection<GuestRating>(_guestRatingRepository.GetByUserId(user.Id));
            Users = new ObservableCollection<User>(_userRepository.GetAllUsers());
            PopulateUsers();
            LoggedInUser = user;
        }

        public RatingsViewModel() { }

        private void PopulateUsers()
        {
            foreach (var rating in GuestRatings)
            {
                OwnerRatings = new ObservableCollection<OwnerRating>(_ownerRatingRepository.GetByUserId(rating.User.Id));
                rating.User = Users.FirstOrDefault(a => a.Id == rating.User.Id);
            }
        }
    }
}
