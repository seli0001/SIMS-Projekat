using SIMS.Domain.Model;
using SIMS.Repository;
using SIMS.WPF.ViewModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SIMS.WPF.ViewModel.Guest1ViewModel
{
    public class OwnerRatingViewModel : ViewModelBase, IClose
    {
        private readonly int[] validator;
        private readonly OwnerRatingRepository _repository;
        private readonly RenovationSuggestionRepository _renovationSuggestionRepository;
        public Reservation SelectedReservation { get; set; }
        public User LoggedInUser { get; set; }


        public OwnerRatingViewModel(Reservation reservation, User user)
        {
            LoggedInUser = user;
            validator = new int[3];
            SelectedReservation = reservation;
            _repository = new OwnerRatingRepository();
            _renovationSuggestionRepository = new RenovationSuggestionRepository();
            IsEnabled = false;
        }

        #region data

        private bool _isEnabled;
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

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
                    IsEnabled = true;

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

        public Action Close { get; set; }

        private void ValidatorTest()
        {
            foreach (int kon in validator)
            {
                if (kon == 0)
                {
                    //IsEnabled = false;
                }
            }
        }

        #region commands

        private ICommand _reviewCommand;
        public ICommand ReviewCommand
        {
            get
            {
                return _reviewCommand ?? (_reviewCommand = new CommandBase(() => SaveRating(), true));
            }
        }

        private ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new CommandBase(() => Cancel(), true));
            }
        }

        #endregion

        private void SaveRating()
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

        private void Cancel()
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
