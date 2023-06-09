﻿using SIMS.Domain.Model;
using SIMS.Service.Services;
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

namespace SIMS.WPF.View.GuideView
{
    /// <summary>
    /// Interaction logic for AcceptTourRequest.xaml
    /// </summary>
    public partial class AcceptTourRequest : Window
    {
        public TourRequest tourRequest { get; set; }
        public User User { get; set; }
        private BookedTourService _bookedTourService;
        private TourRequestService _tourRequestService;
        public AcceptTourRequest(TourRequest tourRequest1, User user)
        {
            InitializeComponent();
            tourRequest = tourRequest1;
            po.Content += tourRequest.StartDate.ToShortDateString();
            kr.Content += tourRequest.EndDate.ToShortDateString();
            dr.Content += tourRequest.Location.Country;
            gr.Content += tourRequest.Location.City;
            User = user;
            _bookedTourService = new BookedTourService();
            _tourRequestService = new TourRequestService();
            List<string> Hours = new List<string>();
            List<string> Minutes = new List<string>();
            for (int i = 0; i < 24; i++)
            {
                Hours.Add(i.ToString("00"));
            }
            for (int i = 0; i < 60; i += 5)
            {
                Minutes.Add(i.ToString("00"));
            }
            hourComboBox.ItemsSource = Hours;
            minutesComboBox.ItemsSource = Minutes;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string startTime = seli.SelectedDate.Value.Date.ToShortDateString() + ' ' + hourComboBox.Text + ":" + minutesComboBox.Text;
            DateTime date = DateTime.Parse(startTime);
            if (IsDateValid(date))
            {
                MessageBox.Show("Morate odabrat datum unutar intervala");
                return;
            }
            List<BookedTour> bts = _bookedTourService.GetByUser(User.Id);

            if (CheckGuideAvailable(date, bts))
            {
                AcceptTour(date);
            }
            else
            {
                MessageBox.Show("Vec imate turu u tom periodu");
            }
        }

        private bool IsDateValid(DateTime date)
        {
            return date.CompareTo(tourRequest.StartDate) < 0 || date.CompareTo(tourRequest.EndDate) > 0;
        }

        private static bool CheckGuideAvailable(DateTime date, List<BookedTour> bts)
        {
            return bts.FirstOrDefault(bt => bt.Tour.StartTime.Time.CompareTo(date) < 0 && bt.Tour.StartTime.Time.AddHours(bt.Tour.Duration).CompareTo(date) > 0) == null;
        }

        private void AcceptTour(DateTime date)
        {
            tourRequest.Status = RequestStatus.Accepted;
            tourRequest.Guide = User;
            tourRequest.TourTime = date;
            _tourRequestService.Update(tourRequest);
            MessageBox.Show("Uspesno ste prihvatili turu");
            Close();
        }
    }
}
