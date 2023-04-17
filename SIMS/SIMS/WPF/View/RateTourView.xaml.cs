using SIMS.Domain.Model;
using SIMS.Service.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace SIMS.WPF.View
{
    /// <summary>
    /// Interaction logic for RateTourView.xaml
    /// </summary>
    public partial class RateTourView : Window, INotifyPropertyChanged
    {
        private int znanjeVodica;
        private int jezikVodica;
        private int zanimljvist;

        private string _comment;

        public string Commet
        {
            get => _comment;
            set
            {
                if (value != _comment)
                {
                    _comment = value;
                    OnPropertyChanged();
                }
            }
        }

        private readonly BookedTourService _bookedTourService;
        private readonly TourRatingService _tourRatingService;
        public BookedTour bookedTour;
        public ObservableCollection<string> images;
        public int userId;
        public RateTourView(BookedTour bookedTour, int userId)
        {
            InitializeComponent();
            DataContext = this;
            this.bookedTour = bookedTour;
            this.userId = userId;
            _bookedTourService = new BookedTourService();
            _tourRatingService = new TourRatingService();
            images = new ObservableCollection<string>();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            znanjeVodica = 1;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            znanjeVodica = 2;
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            znanjeVodica = 3;
        }

        private void RadioButton_Checked_3(object sender, RoutedEventArgs e)
        {
            znanjeVodica = 4;
        }

        private void RadioButton_Checked_4(object sender, RoutedEventArgs e)
        {
            znanjeVodica = 5;
        }

        private void RadioButton_Checked_5(object sender, RoutedEventArgs e)
        {
            jezikVodica = 1;
        }

        private void RadioButton_Checked_6(object sender, RoutedEventArgs e)
        {
            jezikVodica = 2;
        }

        private void RadioButton_Checked_7(object sender, RoutedEventArgs e)
        {
            jezikVodica = 4;
        }

        private void RadioButton_Checked_8(object sender, RoutedEventArgs e)
        {
            jezikVodica = 5;
        }

        private void RadioButton_Checked_9(object sender, RoutedEventArgs e)
        {
            jezikVodica = 3;
        }

        private void RadioButton_Checked_10(object sender, RoutedEventArgs e)
        {
            zanimljvist = 1;
        }

        private void RadioButton_Checked_11(object sender, RoutedEventArgs e)
        {
            zanimljvist = 2;
        }

        private void RadioButton_Checked_12(object sender, RoutedEventArgs e)
        {
            zanimljvist = 3;
        }

        private void RadioButton_Checked_13(object sender, RoutedEventArgs e)
        {
            zanimljvist = 4;
        }

        private void RadioButton_Checked_14(object sender, RoutedEventArgs e)
        {
            zanimljvist = 5;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RateTourClick(object sender, RoutedEventArgs e)
        {
            bookedTour.Review = true;
            _bookedTourService.Update(bookedTour);
            _tourRatingService.Save(bookedTour, userId, znanjeVodica, jezikVodica, zanimljvist, _comment, images.ToList());
        }

        private void BackToMainGuest2ViewClick(object sender, RoutedEventArgs e)
        {
            MainGuest2View mainGuest2View = new MainGuest2View(userId);
            mainGuest2View.Show();
            Close();
        }

        private void BackToMenuClick(object sender, RoutedEventArgs e)
        {
            MenuGuest2 menu = new MenuGuest2(userId);
            menu.Show();
            Close();
        }

        private void AddImageClick(object sender, RoutedEventArgs e)
        {
            images.Add(tbImage.Text);
        }
    }
}
