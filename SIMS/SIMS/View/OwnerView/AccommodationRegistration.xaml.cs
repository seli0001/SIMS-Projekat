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
using System.Collections.ObjectModel;
using Microsoft.Win32;
using Image = SIMS.Model.Image;

namespace SIMS.View.OwnerView
{
    /// <summary>
    /// Interaction logic for AccommondationRegistration.xaml
    /// </summary>
    public partial class AccommondationRegistration : Window, INotifyPropertyChanged
    {
        public User LoggedInUser { get; set; }

        public Accommodation SelectedAccommodation { get; set; }

        public ObservableCollection<BitmapImage> Images;

        private List<Image> _accommodationImages;

        private readonly AccommodationRepository _repository;
        private readonly LocationRepository _locationRepository;
        private readonly ImageRepository _imageRepository;

        private readonly int[] validator;

        public AccommondationRegistration(User user)
        {
            InitializeComponent();
            Title = "Create new accommondation";
            DataContext = this;
            LoggedInUser = user;
            validator = new int[6];
            _repository = new AccommodationRepository();
            _locationRepository = new LocationRepository();
            _imageRepository = new ImageRepository();
            Images = new ObservableCollection<BitmapImage>();
            _accommodationImages = new List<Image>();
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
            _locationRepository = new LocationRepository();
            _imageRepository = new ImageRepository();
            Images = new ObservableCollection<BitmapImage>();
            _accommodationImages = new List<Image>();
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
                        CityValidator.Content = "Veliko pocetno slovo";
                        CityValidator.Visibility = Visibility.Visible;
                        validator[1] = 0;
                    }
                    else
                    {
                        CityValidator.Visibility = Visibility.Hidden;
                        validator[1] = 1;
                    }
                    BtnSubmit.IsEnabled = true;

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
                        CountryValidator.Content = "Veliko pocetno slovo";
                        CountryValidator.Visibility = Visibility.Visible;
                        validator[2] = 0;
                    }
                    else
                    {
                        CountryValidator.Visibility = Visibility.Hidden;
                        validator[2] = 1;
                    }
                    BtnSubmit.IsEnabled = true;

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
                        MaxGuestNumValidator.Content = "Broj gostiju mora biti veci od 0";
                        MaxGuestNumValidator.Visibility = Visibility.Visible;
                        validator[3] = 0;
                    }
                    else
                    {
                        MaxGuestNumValidator.Visibility = Visibility.Hidden;
                        validator[3] = 1;
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
                        validator[4] = 0;
                    }
                    else
                    {
                        MinReservationDaysValidator.Visibility = Visibility.Hidden;
                        validator[4] = 1;
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
                        validator[5] = 0;
                    }
                    else
                    {
                        CancelDaysNumberValidator.Visibility = Visibility.Hidden;
                        validator[5] = 1;
                    }
                    BtnSubmit.IsEnabled = true;

                    ValidatorTest();
                    _cancelDaysNumber = value;
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


        private void AddImageFromFileDialog(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                _accommodationImages.Add(new Image() { Path = openFileDialog.FileName });
                LoadImageFromPath(openFileDialog.FileName);
            }
        }

        private void RemoveImage(object sender, RoutedEventArgs e)
        {
            if (imageListView.SelectedIndex != -1)
            {
                _accommodationImages.RemoveAt(imageListView.SelectedIndex);
                Images.RemoveAt(imageListView.SelectedIndex);
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
                Location newLocation = new Location(Country, City);
                Location savedLocation = _locationRepository.Save(newLocation);


                _accommodationImages = _imageRepository.SetId(_accommodationImages);

                Accommodation newAccommodation = new Accommodation(AcName, savedLocation, AccTypeEnum, MaxGuestNum, MinReservationDays, CancelDaysNumber, LoggedInUser, _accommodationImages);
                Accommodation savedAccommodation = _repository.Save(newAccommodation);
                _imageRepository.SaveAll(_accommodationImages, savedAccommodation);
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
