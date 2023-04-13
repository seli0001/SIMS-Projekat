using SIMS.Model;
using SIMS.Model.AccommodationModel;
using SIMS.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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

namespace SIMS.View.OwnerView
{
    /// <summary>
    /// Interaction logic for GuestRatingView.xaml
    /// </summary>
    public partial class GuestRatingView : Window, INotifyPropertyChanged
    {
        private readonly int[] validator;
        private readonly GuestRatingRepository _repository;
        public Reservation SelectedReservation { get; set; }
        public User LoggedInUser { get; set; }

        public GuestRatingView(Reservation selectedReservation, User user)
        {
            InitializeComponent();
            Title = "Rate Guest";
            DataContext = this;
            validator = new int[3];
            SelectedReservation = selectedReservation;
            _repository = new GuestRatingRepository();
            LoggedInUser = user;
        }

        #region data

        private int _cleanliness = 1;
        public int Cleanliness
        {
            get => _cleanliness;
            set
            {

                if (value != _cleanliness)
                {
                    if (value < 1 || value > 5)
                    {
                        CleanlinessValidator.Content = "Ocena mora biti (1-5)";
                        CleanlinessValidator.Visibility = Visibility.Visible;
                        validator[0] = 0;
                    }
                    else
                    {
                        CleanlinessValidator.Visibility = Visibility.Hidden;
                        validator[0] = 1;
                    }
                    BtnSubmit.IsEnabled = true;

                    ValidatorTest();
                    _cleanliness = value;
                    OnPropertyChanged();
                }

            }
        }

        private int _rulesRespect = 1;
        public int RulesRespect
        {
            get => _rulesRespect;
            set
            {

                if (value != _rulesRespect)
                {
                    if (value < 1 || value > 5)
                    {
                        RulesValidator.Content = "Ocena mora biti (1-5)";
                        RulesValidator.Visibility = Visibility.Visible;
                        validator[1] = 0;
                    }
                    else
                    {
                        RulesValidator.Visibility = Visibility.Hidden;
                        validator[1] = 1;
                    }
                    BtnSubmit.IsEnabled = true;

                    ValidatorTest();
                    _rulesRespect = value;
                    OnPropertyChanged();
                }

            }
        }

        private string _comment;
        public string Comment
        {
            get => _comment;
            set
            {

                if (value != _comment)
                {
                    if (value.Length == 0)
                    {
                        validator[2] = 0;
                    }
                    else
                    {
                        validator[2] = 1;
                    }
                    BtnSubmit.IsEnabled = true;

                    ValidatorTest();
                    _comment = value;
                    OnPropertyChanged();
                }

            }
        }

        #endregion



        private void SaveRating(object sender, RoutedEventArgs e)
        {
            GuestRating newRating = new GuestRating(Cleanliness, RulesRespect, Comment, LoggedInUser, SelectedReservation);
            GuestRating savedRating = _repository.Save(newRating);
            Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ValidatorTest()
        {
            foreach (int kon in validator)
            {
                if (kon == 0)
                {
                    BtnSubmit.IsEnabled = false;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
