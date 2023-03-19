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

    public TourRegistrationView(User guide)
    {
        InitializeComponent();
        _tourRepository = new TourRepository();
        Tour = new Tour();
        Checkpoints = new ObservableCollection<Checkpoint>();
        Images = new ObservableCollection<BitmapImage>();
        imageListView.ItemsSource = Images;
        checkpointListView.ItemsSource = Checkpoints;
        _guide = guide;
        DataContext = this;
    }

    public bool CheckStartTime()
    {
        string pattern = @"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$";
        return date.SelectedDate != null && Regex.IsMatch(time.Text, pattern);
    }

    public void AddStartTime()
    {
        string startTime = date.SelectedDate.Value.Date.ToShortDateString() + ' ' + time.Text;
        Tour.StartTime.Time = DateTime.Parse(startTime);
    }

    private void RegisterTour(object sender, RoutedEventArgs e)
    {
        
        if (CheckStartTime())
        {
            AddStartTime();
            Tour.Guide = _guide;
            _tourRepository.Save(Tour);
            Close();
        }
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
    

    private void AddNewCheckpoint(object sender, RoutedEventArgs e)
    {
        if (CheckPointTextBox.Text != "")
        {
            var checkPoint = new Checkpoint() { Name = CheckPointTextBox.Text };
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
}