using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Type = SIMS.Domain.Model.Type;
using System.Windows.Input;
using SIMS.Domain.Model;
using SIMS.Service.UseCases;
using SIMS.WPF.ViewModel.ViewModel;

namespace SIMS.WPF.ViewModel.OwnerViewModel
{

    
    public class AccommodationRegistrationViewModel : ViewModelBase, IClose
    {
        public User LoggedInUser { get; set; }
        public Accommodation SelectedAccommodation { get; set; }

        public ObservableCollection<BitmapImage> Images;
        private List<Image> _accommodationImages;

        private readonly AccommodationService _accommodationService;
        private readonly AccommodationImageService _imageService;
        private readonly LocationService _locationService;
        private readonly SuperOwnerService _superOwnerService;


        private readonly int[] validator;

        public AccommodationRegistrationViewModel(User user)
        {
            LoggedInUser = user;
            validator = new int[6];

            _accommodationService = new AccommodationService();
            _imageService = new AccommodationImageService();
            _locationService = new LocationService();
            _superOwnerService = new SuperOwnerService();

            Images = new ObservableCollection<BitmapImage>();
            _accommodationImages = new List<Image>();

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
        private string _name;
        public string AccommodationName
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    if (!Regex.Match(value, @"\p{Lu}\p{Ll}{2,9}").Success)
                    {/*
                        NameValidator.Content = "Veliko pocetno slovo";
                        NameValidator.Visibility = Visibility.Visible;*/
                        validator[0] = 0;
                    }
                    else
                    {
                        //  NameValidator.Visibility = Visibility.Hidden;
                        validator[0] = 1;
                    }
                    IsEnabled = true;

                    ValidatorTest();
                    _name = value;
                    OnPropertyChanged();
                }

            }
        }


        private string _city;
        public string City
        {
            get => _city;
            set
            {
                if (value != _city)
                {
                    if (!Regex.Match(value, @"\p{Lu}\p{Ll}{2,9}").Success)
                    {
                        //CityValidator.Content = "Veliko pocetno slovo";
                        //CityValidator.Visibility = Visibility.Visible;
                        validator[1] = 0;
                    }
                    else
                    {
                        //CityValidator.Visibility = Visibility.Hidden;
                        validator[1] = 1;
                    }
                    IsEnabled = true;

                    ValidatorTest();
                    _city = value;
                    OnPropertyChanged();
                }

            }
        }

        private string _country;
        public string Country
        {
            get => _country;
            set
            {
                if (value != _country)
                {
                    if (!Regex.Match(value, @"\p{Lu}\p{Ll}{2,9}").Success)
                    {
                        //CountryValidator.Content = "Veliko pocetno slovo";
                        //CountryValidator.Visibility = Visibility.Visible;
                        validator[2] = 0;
                    }
                    else
                    {
                        //CountryValidator.Visibility = Visibility.Hidden;
                        validator[2] = 1;
                    }
                    IsEnabled = true;

                    ValidatorTest();
                    _country = value;
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
                        //MaxGuestNumValidator.Content = "Broj gostiju mora biti veci od 0";
                        //MaxGuestNumValidator.Visibility = Visibility.Visible;
                        validator[3] = 0;
                    }
                    else
                    {
                        //MaxGuestNumValidator.Visibility = Visibility.Hidden;
                        validator[3] = 1;
                    }
                    IsEnabled = true;

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
                        //MinReservationDaysValidator.Content = "Broj dana mora biti veci od 0";
                        //MinReservationDaysValidator.Visibility = Visibility.Visible;
                        validator[4] = 0;
                    }
                    else
                    {
                        //MinReservationDaysValidator.Visibility = Visibility.Hidden;
                        validator[4] = 1;
                    }
                    IsEnabled = true;

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
                        //CancelDaysNumberValidator.Content = "Broj dana mora biti veci od 0";
                        //CancelDaysNumberValidator.Visibility = Visibility.Visible;
                        validator[5] = 0;
                    }
                    else
                    {
                        //CancelDaysNumberValidator.Visibility = Visibility.Hidden;
                        validator[5] = 1;
                    }
                    IsEnabled = true;

                    ValidatorTest();
                    _cancelDaysNumber = value;
                    OnPropertyChanged();
                }

            }
        }

        #endregion

        private ICommand _addImageCommand;
        public ICommand AddImageCommand
        {
            get
            {
                return _addImageCommand ?? (_addImageCommand = new CommandBase(() => AddImageFromFileDialog(), true));
            }
        }

        private ICommand _removeImageCommand;
        public ICommand RemoveImageCommand
        {
            get
            {
                return _removeImageCommand ?? (_removeImageCommand = new CommandBase(() => RemoveImage(), true));
            }
        }

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

        public void AddImageFromFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                _accommodationImages.Add(new Image() { Path = openFileDialog.FileName });
                LoadImageFromPath(openFileDialog.FileName);
            }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                OnPropertyChanged();
            }
        }
        private void RemoveImage()
        {
            if (SelectedIndex != -1)
            {
                _accommodationImages.RemoveAt(SelectedIndex);
                Images.RemoveAt(SelectedIndex);
            }
        }

        public void LoadImageFromPath(string path)
        {
            // Convert the path to a URI format
            Uri uri = new Uri(path, UriKind.RelativeOrAbsolute);

            // Set the BitmapImage source to the URI
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.UriSource = uri;
            bitmapImage.EndInit();

            Images.Add(bitmapImage);
        }

        private ICommand _saveAccommodationCommand;
        public ICommand SaveAccommodationCommand
        {
            get
            {
                return _saveAccommodationCommand ?? (_saveAccommodationCommand = new CommandBase(() => SaveAccommodation(), true));
            }
        }

        private ICommand _closeWindowCommand;
        public ICommand CloseWindowCommand
        {
            get
            {
                return _closeWindowCommand ?? (_closeWindowCommand = new CommandBase(() => Close(), true));
            }
        }

       
        private void SaveAccommodation()
        {

            if (SelectedAccommodation != null)
            {
                //Code for previewing accommodation
            }
            else
            {
                StoreAccommodation();
            }

            Close();
        }

        private void StoreAccommodation()
        {
            Location savedLocation = _locationService.Save(Country, City);

            _accommodationImages = _imageService.SetId(_accommodationImages);

            Accommodation savedAccommodation = _accommodationService.Save(AccommodationName, savedLocation, AccTypeEnum, MaxGuestNum,
                MinReservationDays, CancelDaysNumber,
                LoggedInUser, _accommodationImages);
            if (_superOwnerService.CheckById(LoggedInUser.Id)) savedAccommodation = _accommodationService.makeSuper(savedAccommodation);
            OwnerMainViewModel.Accommodations.Add(savedAccommodation);

            _imageService.SaveAll(_accommodationImages);
        }

        public Action Close { 
            get; 
            set;
            }


    }
}
