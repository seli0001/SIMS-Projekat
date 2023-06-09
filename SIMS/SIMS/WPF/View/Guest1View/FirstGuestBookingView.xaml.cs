﻿using SIMS.Domain.Model;
using SIMS.Repository;
using SIMS.Service.Services;
using SIMS.Service.UseCases;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;


namespace SIMS.View.FirstGuestView
{
    /// <summary>
    /// Interaction logic for FirstGuestBookingView.xaml
    /// </summary>
    public partial class FirstGuestBookingView : Window
    {

        public Accommodation SelectedAccommodation { get; set; }

        public User LoggedInUser { get; set; }

        private readonly AccommodationService _repository;
        private readonly ReservationService _reservationService;

        private readonly int[] validator;

        public FirstGuestBookingView(Accommodation selectedAccommodation, User user)
        {
            InitializeComponent();
            Title = "Accommodation reservation";
            DataContext = this;
            validator = new int[2];
            _repository = new AccommodationService();
            _reservationService = new ReservationService();
            SelectedAccommodation = selectedAccommodation;
            LoggedInUser = user;
        }


        private DateTime _fromDate = DateTime.Now;

        public DateTime FromDate
        {
            get => _fromDate;
            set
            {
                _fromDate = value;
            }
        }

        private DateTime _toDate = DateTime.Now;

        public DateTime ToDate
        {
            get => _toDate;
            set
            {
                _toDate = value;
            }
        }

        private int _timeOfStay = 1;
        public int TimeOfStay
        {
            get => _timeOfStay;
            set
            {
                if (value != _timeOfStay)
                {
                    if (value < SelectedAccommodation.MinBookingDays)
                    {
                        TimeOfStayValidator.Content = "Number of days must be at least " + SelectedAccommodation.MinBookingDays;
                        TimeOfStayValidator.Visibility = Visibility.Visible;
                        validator[0] = 0;
                    }
                    else
                    {
                        TimeOfStayValidator.Visibility = Visibility.Hidden;
                        validator[0] = 1;
                    }
                    BtnBook.IsEnabled = true;

                    _timeOfStay = value;
                    OnPropertyChanged();
                }

            }
        }

        private int _numberOfGuests = 1;
        public int NumberOfGuests
        {
            get => _numberOfGuests;
            set
            {
                if (value != _numberOfGuests)
                {
                    if (value > SelectedAccommodation.MaxGuestsNumber)
                    {
                        MaxGuestNumValidator.Content = "Number of days must be at most " + SelectedAccommodation.MaxGuestsNumber;
                        MaxGuestNumValidator.Visibility = Visibility.Visible;
                        validator[1] = 0;
                    }
                    else
                    {
                        MaxGuestNumValidator.Visibility = Visibility.Hidden;
                        validator[1] = 1;
                    }
                    BtnBook.IsEnabled = true;

                    ValidatorTest();
                    _numberOfGuests = value;
                    OnPropertyChanged();
                }

            }
        }

        private void ValidatorTest()
        {
            foreach (int validation in validator)
            {
                if (validation == 0)
                {
                    BtnBook.IsEnabled = false;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void BtnBook_Click(object sender, RoutedEventArgs e)
        {
            Reservation reservation = new Reservation(DateOnly.FromDateTime(FromDate),DateOnly.FromDateTime(FromDate.AddDays(TimeOfStay)), SelectedAccommodation, TimeOfStay, NumberOfGuests, LoggedInUser);
            if(_reservationService.AvailableAccommodation(reservation))
            {
                _reservationService.Save(reservation);
                MessageBox.Show("Accommodation " + SelectedAccommodation.Name + " successfully booked from " + FromDate.ToShortDateString() + " to " + FromDate.AddDays(TimeOfStay).ToShortDateString());
            }
            

        }
    }
}
