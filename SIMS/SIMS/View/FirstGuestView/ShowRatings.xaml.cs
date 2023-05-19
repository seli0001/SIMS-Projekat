using SIMS.Domain.Model;
using SIMS.Model;
using SIMS.Repository;
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

namespace SIMS.View.FirstGuestView
{
    /// <summary>
    /// Interaction logic for ShowRatings.xaml
    /// </summary>
    public partial class ShowRatings : Window
    {
        public User LoggedInUser { get; set; }

        private GuestRatingRepository _guestRatingRepository;
        private UserRepository _userRepository;
        public static ObservableCollection<GuestRating> GuestRatings { get; set; }
        public static ObservableCollection<User> Users { get; set; }
        public GuestRating SelectedGuestRating { get; set; }

        public ShowRatings(User user)
        {
            InitializeComponent();
            DataContext = this;
            _guestRatingRepository = new GuestRatingRepository();
            _userRepository = new UserRepository();
            GuestRatings = new ObservableCollection<GuestRating>(_guestRatingRepository.GetByUserId(user.Id));
            Users = new ObservableCollection<User>(_userRepository.GetAllUsers());
            PopulateUsers();
            LoggedInUser = user;
        }

        private void PopulateUsers()
        {
            foreach (var rating in GuestRatings)
            {
                rating.User = Users.FirstOrDefault(a => a.Id == rating.User.Id);
            }
        }
    }
}
