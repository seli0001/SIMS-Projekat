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
        private readonly RenovationSuggestionRepository _renovationSuggestionRepository;
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
            _renovationSuggestionRepository = new RenovationSuggestionRepository();
        }


        #region data

        private int _cleanliness = 1;

        public int Cleanliness
        {
            get { return _cleanliness; }
            set
            {
                _cleanliness = value;
                ValidateCleanliness();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Cleanliness"));
            }
        }

        private void ValidateCleanliness()
        {
            if (Cleanliness < 1 || Cleanliness > 5)
            {
                CleanlinessValidator.Content = "Cleanliness should be between 1 and 5.";
                CleanlinessValidator.Visibility = Visibility.Visible;
            }
            else
            {
                CleanlinessValidator.Visibility = Visibility.Hidden;
            }
        }

        private int _rulesRespect = 1;

        public int RulesRespect
        {
            get { return _rulesRespect; }
            set
            {
                _rulesRespect = value;
                ValidateRulesRespect();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RulesRespect"));
            }
        }

        private void ValidateRulesRespect()
        {
            if (RulesRespect < 1 || RulesRespect > 5)
            {
                RulesValidator.Content = "Rules respect should be between 1 and 5.";
                RulesValidator.Visibility = Visibility.Visible;
            }
            else
            {
                RulesValidator.Visibility = Visibility.Hidden;
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

        private string _suggestion;
        public string Suggestion
        {
            get => _suggestion;
            set
            {

                if (value != _suggestion)
                {
                    if (value.Length == 0)
                    {
                        validator[2] = 1;
                    }
                    else
                    {
                        validator[2] = 1;
                    }
                    BtnSubmitRenovationSuggestion.IsEnabled = true;

                    ValidatorTest();
                    _suggestion = value;
                    OnPropertyChanged();
                }

            }
        }

        private int _urgencyLevel = 1;
        public int UrgencyLevel
        {
            get => _urgencyLevel;
            set
            {

                if (value != _urgencyLevel)
                {
                    _urgencyLevel = value;
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
            
            if (CleanlinessValidator.Visibility == Visibility.Hidden && RulesValidator.Visibility == Visibility.Hidden)
            {
                OwnerRating newRating = new OwnerRating(Cleanliness, RulesRespect, Comment, LoggedInUser, SelectedReservation);
                OwnerRating savedRating = _repository.Save(newRating);
                MessageBox.Show("Rating sent");
                txtBoxCleanliness.Text = "";
                txtBoxComment.Text = "";
                txtBoxImages.Text = "";
                txtBoxRespect.Text = "";
                BtnSubmit.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("Check inputs");
            }

        }

        

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnSubmitRenovationSuggestion_Click(object sender, RoutedEventArgs e)
        {
            if (UrgencyValidator.Visibility == Visibility.Hidden)
            {
                RenovationSuggestion renovationSuggestion = new RenovationSuggestion(UrgencyLevel, Suggestion, LoggedInUser, SelectedReservation);
                RenovationSuggestion savedSuggestion = _renovationSuggestionRepository.Save(renovationSuggestion);
                MessageBox.Show("Renovation suggestion sent");
                txtBoxUrgency.Text = "";
                txtSuggestion.Text = "";
                BtnSubmitRenovationSuggestion.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("Check inputs");
            }
        }
    }
}
