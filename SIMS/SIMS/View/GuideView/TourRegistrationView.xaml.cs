using Microsoft.Win32;
using SIMS.Model;
using SIMS.Model.Guide;
using SIMS.Repository.GuideRepository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
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
using Image = SIMS.Model.Guide.Image;

namespace SIMS.View.GuideView;

/// <summary>
/// Interaction logic for TourRegistrationView.xaml
/// </summary>
public partial class TourRegistrationView : Window
{
    private readonly TourRepository _tourRepository;
    private readonly User _guide;
    public ObservableCollection<Checkpoint> Checkpoints;
    public ObservableCollection<BitmapImage> Images;
    public Tour Tour { get; set; }
    public List<DateTime> StartDates { get; set; }
    public TourRegistrationView(User guide)
    {
        InitializeComponent();
        _tourRepository = new TourRepository();
        Tour = new Tour();
        Checkpoints = new ObservableCollection<Checkpoint>();
        Images = new ObservableCollection<BitmapImage>();
        StartDates = new List<DateTime>();
        imageListView.ItemsSource = Images;
        checkpointListView.ItemsSource = Checkpoints;
        PopulateComboBoxes();
        _guide = guide;
        DataContext = this;
    }

    public void PopulateComboBoxes()
    {
        List<string> hours = new List<string>();
        List<string> minutes = new List<string>();

        // add the hours to the list
        for (int i = 0; i < 24; i++)
        {
            hours.Add(i.ToString("00"));
        }
        for (int i = 0; i < 60; i += 5)
        {
            minutes.Add(i.ToString("00"));
        }

        
        hourComboBox.ItemsSource = hours;
        minutesComboBox.ItemsSource = minutes;
    }

    public bool CheckStartTime(string time)
    {
        string pattern = @"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$";
        return date.SelectedDate != null && Regex.IsMatch(time, pattern);
    }


    private void RegisterTour(object sender, RoutedEventArgs e)
    {
        
        if (IsValid())
        {
            Tour.Guide = _guide;
            foreach (var date in StartDates)
            {
                Tour.StartTime.Time = date;
                _tourRepository.Save(Tour);
            }
            
            Close();
        }
    }

    public bool IsValid()
    {
        if (string.IsNullOrWhiteSpace(Tour.Name))
        {
            return false;
        }
        if (string.IsNullOrWhiteSpace(Tour.Location.City))
        {
            return false;
        }
        if (string.IsNullOrWhiteSpace(Tour.Location.Country))
        {
            return false;
        }
        if (string.IsNullOrWhiteSpace(Tour.Description))
        {
            return false;
        }
        if (string.IsNullOrWhiteSpace(Tour.Language))
        {
            return false;
        }
        if (string.IsNullOrWhiteSpace(Tour.Description))
        {
            return false;
        }
        if (Checkpoints.Count < 2)
        {
            return false;
        }
        if (StartDates.Count < 1)
        {
            return false;
        }
        if (!int.TryParse(maxNumberTextBox.Text, out int number))
        {
            return false;
        }
        if (!int.TryParse(durationTextBox.Text, out int number2))
        {
            return false;
        }
        return true;
    }

    private void AddImageFromFileDialog(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
        if (openFileDialog.ShowDialog() == true)
        {
            Tour.Images.Add(new Image() { Path = openFileDialog.FileName });
            LoadImageFromPath(openFileDialog.FileName);
        }
    }
    
    
    
    private void RemoveImage(object sender, RoutedEventArgs e)
    {
        if (imageListView.SelectedIndex != -1)
        {
            Tour.Images.RemoveAt(imageListView.SelectedIndex);
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
    
    public int GetNextIndex()
    {
        return Checkpoints.Count == 0 ? 1 : Checkpoints.Max(checkpoint => checkpoint.Index) + 1;
    }
    
    private void AddNewCheckpoint(object sender, RoutedEventArgs e)
    {
        if (CheckPointTextBox.Text != "")
        {
            var checkPoint = new Checkpoint() { Name = CheckPointTextBox.Text };
            checkPoint.Index = GetNextIndex();
            checkPoint.IsChecked = false;
            Tour.Checkpoints.Add(checkPoint);
            Checkpoints.Add(checkPoint);
            CheckPointTextBox.Text = "";
        }
    }
    
    private void RemoveCheckpoint(object sender, RoutedEventArgs e)
    {
        if (checkpointListView.SelectedIndex != -1)
        {
            Tour.Checkpoints.RemoveAt(checkpointListView.SelectedIndex);
            Checkpoints.RemoveAt(checkpointListView.SelectedIndex);
        }
    }

    private void AddDateButton_Click(object sender, RoutedEventArgs e)
    {
        string time = hourComboBox.SelectedItem + ":" + minutesComboBox.SelectedItem;
        if (CheckStartTime(time))
        {
            string startTime = date.SelectedDate.Value.Date.ToShortDateString() + ' ' + time;
            StartDates.Add(DateTime.Parse(startTime));
            hourComboBox.SelectedIndex = -1;
            minutesComboBox.SelectedIndex = -1;
            date.SelectedDate = null;
            MessageBox.Show("Uspesno ste dodali datum");
        }
    }
}