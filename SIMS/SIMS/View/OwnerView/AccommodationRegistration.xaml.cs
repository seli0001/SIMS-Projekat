using SIMS.Model;
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
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using SIMS.Model.AccommodationModel;
using SIMS.Repository;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Type = SIMS.Model.AccommodationModel.Type;
using System.ComponentModel.DataAnnotations;

namespace SIMS.View.OwnerView
{
    /// <summary>
    /// Interaction logic for AccommondationRegistration.xaml
    /// </summary>
    public partial class AccommondationRegistration : Window, INotifyPropertyChanged
    {
        public User LoggedInUser { get; set; }

        public Accommodation SelectedAccommodation { get; set; }

        private readonly AccommodationRepository _repository;

        private readonly int[] validator;

        public AccommondationRegistration(User user)
        {
            InitializeComponent();
            Title = "Create new accommondation";
            DataContext = this;
            LoggedInUser = user;
            validator = new int[6];
            _repository = new AccommodationRepository();
        }

       
        public AccommondationRegistration(Accommodation selectedAccommodation, User user)
        {
            InitializeComponent();
            DataContext = this;
            Title = "Update comment";
            LoggedInUser = user;
            validator = new int[6];
            SelectedAccommodation = selectedAccommodation;
            _repository = new AccommodationRepository();
        }

        #region data
        private string _name;
        public string AcName
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    if (!Regex.Match(value, @"\p{Lu}\p{Ll}{2,9}").Success)
                    {
                        NameValidator.Content = "Veliko pocetno slovo";
                        NameValidator.Visibility = Visibility.Visible;
                        validator[0] = 0;
                    }
                    else
                    {
                        NameValidator.Visibility = Visibility.Hidden;
                        validator[0] = 1;
                    }
                    BtnSubmit.IsEnabled = true;

                    ValidatorTest();
                    _name = value;
                    OnPropertyChanged();
                }

            }
        }


        private string _location;
        public string Location
        {
            get => _location;
            set
            {
                if (value != _location)
                {
                    if (!Regex.Match(value, @"\p{Lu}\p{Ll}{2,9}").Success)
                    {
                        LocationValidator.Content = "Veliko pocetno slovo";
                        LocationValidator.Visibility = Visibility.Visible;
                        validator[1] = 0;
                    }
                    else
                    {
                        LocationValidator.Visibility = Visibility.Hidden;
                        validator[1] = 1;
                    }
                    BtnSubmit.IsEnabled = true;

                    ValidatorTest();
                    _location = value;
                    OnPropertyChanged();
                }

            }
        }


        private Type AccTypeEnum;

        private int _accommodationType;
        public int AccommodationType
        {
            get
            {
                return _accommodationType;
            }
            set
            {
                if (value != _accommodationType)
                {
                    _accommodationType = value;
                    if (value == 0) AccTypeEnum = Type.apartment;
                    else if (value == 1) AccTypeEnum = Type.house;
                    else AccTypeEnum = Type.hut;
                    OnPropertyChanged();
                }
            }

        }


        private int _maxGuestNum = 1;
        public int MaxGuestNum
        {
            get => _maxGuestNum;
            set
            {

                if (value != _maxGuestNum)
                {
                    if (value < 1)
                    {
                        MaxGuestNumValidator.Content = "Broj gostiju mora biti veci od 0";
                        MaxGuestNumValidator.Visibility = Visibility.Visible;
                        validator[2] = 0;
                    }
                    else
                    {
                        MaxGuestNumValidator.Visibility = Visibility.Hidden;
                        validator[2] = 1;
                    }
                    BtnSubmit.IsEnabled = true;

                    ValidatorTest();
                    _maxGuestNum = value;
                    OnPropertyChanged();
                }

            }
        }


        private int _minReservationDays = 1;
        public int MinReservationDays
        {
            get => _minReservationDays;
            set
            {
                if (value != _minReservationDays)
                {
                    if (value < 1)
                    {
                        MinReservationDaysValidator.Content = "Broj dana mora biti veci od 0";
                        MinReservationDaysValidator.Visibility = Visibility.Visible;
                        validator[3] = 0;
                    }
                    else
                    {
                        MinReservationDaysValidator.Visibility = Visibility.Hidden;
                        validator[3] = 1;
                    }
                    BtnSubmit.IsEnabled = true;

                    ValidatorTest();
                    _minReservationDays = value;
                    OnPropertyChanged();
                }

            }
        }


        private int _cancelDaysNumber = 1;
        public int CancelDaysNumber
        {
            get => _cancelDaysNumber;
            set
            {
                if (value != _cancelDaysNumber)
                {
                    if (value < 1)
                    {
                        CancelDaysNumberValidator.Content = "Broj dana mora biti veci od 0";
                        CancelDaysNumberValidator.Visibility = Visibility.Visible;
                        validator[4] = 0;
                    }
                    else
                    {
                        CancelDaysNumberValidator.Visibility = Visibility.Hidden;
                        validator[4] = 1;
                    }
                    BtnSubmit.IsEnabled = true;

                    ValidatorTest();
                    _cancelDaysNumber = value;
                    OnPropertyChanged();
                }

            }
        }


        private string _picture;
        public string Picture
        {
            get => _picture;
            set
            {
                if (value != _picture)
                {
                    PictureValidator.Visibility = Visibility.Hidden;
                    validator[5] = 1;
                    BtnSubmit.IsEnabled = true;

                    ValidatorTest();
                    _picture = value;
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
                    BtnSubmit.IsEnabled = false;
                }
            }
        }

        private void SaveAccommodation(object sender, RoutedEventArgs e)
        {

            if (SelectedAccommodation != null)
            {
                /*
                SelectedComment.Text = Text;
                SelectedComment.CreationTime = DateTime.Now;
                Comment updatedComment = _repository.Update(SelectedComment);
                if (updatedComment != null)
                {
                    // Update observable collection
                    int index = CommentsOverview.Comments.IndexOf(SelectedComment);
                    CommentsOverview.Comments.Remove(SelectedComment);
                    CommentsOverview.Comments.Insert(index, updatedComment);
                }
                */
            }
            else
            {
                Accommodation newAccommodation = new Accommodation(AcName, Location, AccTypeEnum, MaxGuestNum, MinReservationDays, CancelDaysNumber, Picture, LoggedInUser);
                Accommodation savedAccommodation = _repository.Save(newAccommodation);
                OwnerMainView.Accommodations.Add(savedAccommodation);
            }

            Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
