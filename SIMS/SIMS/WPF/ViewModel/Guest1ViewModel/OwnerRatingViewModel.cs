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
        public Reservation SelectedReservation { get; set; }
        public User LoggedInUser { get; set; }


        public OwnerRatingViewModel(Reservation reservation, User user)
        {
            LoggedInUser = user;
            validator = new int[3];
            SelectedReservation = reservation;
            _repository = new OwnerRatingRepository();
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
                        validator[0] = 1;
                    }
                    else
                    {
                        //CleanlinessValidator.Visibility = Visibility.Hidden;
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
                        validator[1] = 1;
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
            OwnerRating newRating = new OwnerRating(Cleanliness, RulesRespect, Comment, LoggedInUser, SelectedReservation);
            OwnerRating savedRating = _repository.Save(newRating);
            Close();
        }

        private void Cancel()
        {
            Close();
        }
    }
}
