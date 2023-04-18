using System;
using System.Collections.Generic;
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
using SIMS.Domain.Model;
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
            validator = new int[3];
            SelectedReservation = reservation;
            _repository = new OwnerRatingRepository();
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
                        validator[0] = 1;
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
                        validator[1] = 1;
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
                        validator[2] = 1;
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


        private void ValidatorTest()
        {
            foreach (int kon in validator)
            {
                if (kon == 0)
                {
                    //BtnSubmit.IsEnabled = false;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void SaveRating(object sender, RoutedEventArgs e)
        {
            OwnerRating newRating = new OwnerRating(Cleanliness, RulesRespect, Comment, LoggedInUser, SelectedReservation);
            OwnerRating savedRating = _repository.Save(newRating);
            Close();
        }

        

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
