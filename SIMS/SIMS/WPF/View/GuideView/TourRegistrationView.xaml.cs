using Microsoft.Win32;
using SIMS.Domain.Model;
using SIMS.Repository;
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
using Image = SIMS.Domain.Model.ImageTour;

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
    public ObservableCollection<DateTime> StartDates { get; set; }
    public TourRegistrationView(User guide)
    {
        InitializeComponent();
        _tourRepository = new TourRepository();
        Tour = new Tour();
        Checkpoints = new ObservableCollection<Checkpoint>();
        Images = new ObservableCollection<BitmapImage>();
        StartDates = new ObservableCollection<DateTime>();
        imageListView.ItemsSource = Images;
        checkpointListView.ItemsSource = Checkpoints;
        datesListView.ItemsSource = StartDates;
        PopulateComboBoxes();
        _guide = guide;
        DataContext = this;
    }

    private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if ( e.Key == Key.F1 )
        {
            AddImageFromFileDialog(sender, e);
        }
        else if (e.Key == Key.F2)
        {
            RemoveImage(sender, e);
        }
        else if (e.Key == Key.F3)
        {
            AddDateButton_Click(sender, e);
        }
        else if (e.Key == Key.F4)
        {
            RemoveDate(sender, e);
        }
        else if (e.Key == Key.F5)
        {
            AddNewCheckpoint(sender, e);
        }
        else if (e.Key == Key.F6)
        {
            RemoveCheckpoint(sender, e);
        }
        else if (e.Key == Key.Escape)
        {
            Close();
        }
        if(e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
        {
            RegisterTour(sender, e);
        }
        if (e.Key == Key.O && Keyboard.Modifiers == ModifierKeys.Control)
        {
            Close();
        }
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
            MessageBox.Show("Morate uneti ime ture!");
            return false;
        }
        if (string.IsNullOrWhiteSpace(Tour.Location.City))
        {
            MessageBox.Show("Morate uneti grad!");
            return false;
        }
        if (string.IsNullOrWhiteSpace(Tour.Location.Country))
        {
            MessageBox.Show("Morate uneti drzavu!");
            return false;
        }
        if (string.IsNullOrWhiteSpace(Tour.Description))
        {
            MessageBox.Show("Morate uneti opis!");
            return false;
        }
        if (string.IsNullOrWhiteSpace(Tour.Language))
        {
            MessageBox.Show("Morate uneti jezik!");
            return false;
        }
        if (string.IsNullOrWhiteSpace(Tour.Description))
        {
            MessageBox.Show("Morate uneti opis!");
            return false;
        }
        if (Checkpoints.Count < 2)
        {
            MessageBox.Show("Morate uneti bar dve tacke!");
            return false;
        }
        if (StartDates.Count < 1)
        {
            MessageBox.Show("Morate uneti bar jedan datum!");
            return false;
        }
        if (!int.TryParse(maxNumberTextBox.Text, out int number))
        {
            MessageBox.Show("Morate uneti maksimalan broj gostijui to mora biti broj!");
            return false;
        }
        if (!int.TryParse(durationTextBox.Text, out int number2))
        {
            MessageBox.Show("Morate uneti trajanje ture i to mora biti broj!");
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
        else
        {
            MessageBox.Show("Morate selektovati sliku koju zelite da obrisete!");
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
            TabIndex = 12;
        }
    }
    
    private void RemoveCheckpoint(object sender, RoutedEventArgs e)
    {
        if (checkpointListView.SelectedIndex != -1)
        {
            Tour.Checkpoints.RemoveAt(checkpointListView.SelectedIndex);
            Checkpoints.RemoveAt(checkpointListView.SelectedIndex);
        }
        else
        {
            MessageBox.Show("Morate selektovati checkpoint!");
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
            TabIndex = 8;
        }
    }

    private void RemoveDate(object sender, RoutedEventArgs e)
    {
        if (datesListView.SelectedIndex != -1)
        {
            StartDates.RemoveAt(datesListView.SelectedIndex);
        }
        else
        {
            MessageBox.Show("Morate selektovati datum!");
        }
    }

    private void date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
        DateTime selectedDate = date.SelectedDate ?? DateTime.Today;

        // Disable selecting dates in the past
        if (selectedDate < DateTime.Today)
        {
            MessageBox.Show("Datum ne sme biti od pre!");
            date.SelectedDate = DateTime.Today;
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}