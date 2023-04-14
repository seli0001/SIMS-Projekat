﻿using SIMS.Model;
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

namespace SIMS.View.OwnerView
{
    /// <summary>
    /// Interaction logic for ShowRatings.xaml
    /// </summary>
    public partial class ShowRatings : Window
    {
        private readonly OwnerRatingRepository _ownerRatingRepository;
        private readonly GuestRatingRepository _guestRatingRepository;
        public static ObservableCollection<OwnerRating> Ratings { get; set; }

        public OwnerRating SelectedRating { get; set; }

        public User Owner { get; set; }
        public ShowRatings(User user)
        {
            InitializeComponent();
            DataContext = this;
            _ownerRatingRepository = new OwnerRatingRepository();
            _guestRatingRepository = new GuestRatingRepository();
            Owner = user;
            Ratings = new ObservableCollection<OwnerRating>();
            UpdateRatings();
        }

        private void UpdateRatings()
        {
            //Prikazemo sve ocene iz OwnerRatings koje su za naseg ownera
            List<OwnerRating> allRatings = new List<OwnerRating>(_ownerRatingRepository.GetByOwnerId(Owner.Id));

            foreach (OwnerRating rating in allRatings)
            {
                if (checkIfRated(rating))
                {
                    Ratings.Add(rating);
                }
            }
        }

        private bool checkIfRated(OwnerRating rating)
        {
            List<GuestRating> allRatings = new List<GuestRating>(_guestRatingRepository.GetAll());
            foreach(GuestRating guestRating in allRatings)
            {
                if(guestRating.Reservation.Id == rating.Reservation.Id)
                {
                    return true;
                }
            }
            return false;

        }

        private void ShowRatingInfo(object sender, RoutedEventArgs e)
        {

        }
    }
}