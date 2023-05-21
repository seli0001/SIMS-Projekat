using SIMS.Domain.Model;
using SIMS.Repository;
using SIMS.Service.UseCases;
using SIMS.WPF.ViewModel.ViewModel;
using System;
using System.Windows;
using System.Windows.Input;

namespace SIMS.WPF.ViewModel.OwnerViewModel
{
    public class GuestRatingViewModel : ViewModelBase, IClose
    {
        private readonly int[] validator;
        private readonly GuestRatingRepository _repository;
        private readonly GuestRatingService _guestRatingService;
        public Reservation SelectedReservation { get; set; }
        public User LoggedInUser { get; set; }

        public GuestRatingViewModel(Reservation selectedReservation, User user)
        {
            validator = new int[3];
            SelectedReservation = selectedReservation;
            _repository = new GuestRatingRepository();
            _guestRatingService = new GuestRatingService();
            LoggedInUser = user;
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
            get => _cleanliness;
            set
            {

                if (value != _cleanliness)
                {
                    if (value < 1 || value > 5)
                    {
                        //CleanlinessValidator.Content = "Ocena mora biti (1-5)";
                        //CleanlinessValidator.Visibility = Visibility.Visible;
                        validator[0] = 0;
                    }
                    else
                    {
                      //  CleanlinessValidator.Visibility = Visibility.Hidden;
                        validator[0] = 1;
                    }
                    IsEnabled = true;

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
                        //RulesValidator.Content = "Ocena mora biti (1-5)";
                        //RulesValidator.Visibility = Visibility.Visible;
                        validator[1] = 0;
                    }
                    else
                    {
                        //RulesValidator.Visibility = Visibility.Hidden;
                        validator[1] = 1;
                    }
                    IsEnabled = true;

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
                    IsEnabled = true;

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
                    IsEnabled = false;
                }
            }
        }

        #region commands

        private ICommand _saveRatingCommand;
        public ICommand SaveRatingCommand
        {
            get
            {
                return _saveRatingCommand ?? (_saveRatingCommand = new CommandBase(() => SaveRating(), true));
            }
        }

        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new CommandBase(() => Close(), true));
            }
        }
        #endregion

        private void SaveRating()
        {
            _guestRatingService.Save(new GuestRating(Cleanliness, RulesRespect, 
                Comment, LoggedInUser, SelectedReservation));

            UnratedReservationsViewModel.Reservations.Remove(SelectedReservation);
            Close();
            MessageBox.Show("You have successfully reviewed");
        }

        public Action Close { get; set; }


    }
}
