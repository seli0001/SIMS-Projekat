using System;
using System.Collections.Generic;
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
using SIMS.Model;
using SIMS.Repository;

namespace SIMS.View.FirstGuestView
{
    /// <summary>
    /// Interaction logic for OwnerRatingView.xaml
    /// </summary>
    public partial class OwnerRatingView : Window
    {
        private readonly int[] validator;
        private readonly OwnerRatingRepository _repository;
        public Reservation SelectedReservation { get; set; }
        public User LoggedInUser { get; set; }

        public OwnerRatingView(Reservation reservation, User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            SelectedReservation = reservation;
            _repository = new OwnerRatingRepository();
        }

        private void SaveRating(object sender, RoutedEventArgs e)
        {
            OwnerRating newRating = new OwnerRating(Cleanliness, RulesRespect, Comment, LoggedInUser, SelectedReservation);
            OwnerRating savedRating = _repository.Save(newRating);
            Close();
        }

        private void OwnerRatingClick(object sender, RoutedEventArgs e)
        {
            
            Close();
        }
    }
}
