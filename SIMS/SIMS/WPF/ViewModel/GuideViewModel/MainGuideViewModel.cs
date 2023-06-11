using SIMS.Domain.Model;
using SIMS.Service.Services;
using SIMS.View.GuideView;
using SIMS.WPF.View.GuideView;
using SIMS.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Aplikacija.Core;
using System.ComponentModel;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using System.Diagnostics;

namespace SIMS.WPF.ViewModel.GuideViewModel
{
    public class MainGuideViewModel: INotifyPropertyChanged, IClose
    {
        #region Fields
        private IMessageBoxService _MessageBoxService;
        public User Guide { get; set; }
        private readonly TourService _tourService;
        private readonly BookedTourService _bookedTourService;
        private readonly VoucherService _voucherService;
        public ObservableCollection<Tour> Tours { get; set; }
        public ObservableCollection<Tour> AllTours { get; set; }
        public ObservableCollection<TourRequest> TourRequests { get; set; }
        private readonly TourRequestService _tourRequestService;
        public Tour SelectedTour { get; set; }
        public TourRequest SelectedTourRequest { get; set; }

        private int _selectedTab;
        public int SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                _selectedTab = value;
                OnPropertyChanged("SelectedTab");
            }
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        private int _dataGridSelectedIndex;

        public int DataGridSelectedIndex
        {
            get { return _dataGridSelectedIndex; }
            set 
            {
                _dataGridSelectedIndex = value;
                
            }
        }

        private int _requestIndex;
        public int RequestIndex
        {
            get { return _requestIndex; }
            set
            {
                _requestIndex = value;
                OnPropertyChanged("RequestIndex");
            }
        }

        private int _allDataGridSelectedIndex;
        public int AllDataGridSelectedIndex
        {
            get { return _allDataGridSelectedIndex; }
            set
            {
                _allDataGridSelectedIndex = value;
                
            }
        }

        private bool _tourStatisticButtonEnabled;

        public bool TourStatisticButtonEnabled
        {
            get { return _tourStatisticButtonEnabled; }
            set
            {
                _tourStatisticButtonEnabled = value;
                OnPropertyChanged("TourStatisticButtonEnabled");
            }
        }

        private bool _startTourButtonEnabled;
        public bool StartTourButtonEnabled
        {
            get { return _startTourButtonEnabled; }
            set
            {
                _startTourButtonEnabled = value;
                OnPropertyChanged("StartTourButtonEnabled");
            }
        }

        private bool _cancelTourButtonEnabled;
        public bool CancelTourButtonEnabled
        {
            get { return _cancelTourButtonEnabled; }
            set
            {
                _cancelTourButtonEnabled = value;
                OnPropertyChanged("CancelTourButtonEnabled");
            }
        }

        private bool _tourRatingButtonEnabled;
        public bool TourRatingButtonEnabled
        {
            get { return _tourRatingButtonEnabled; }
            set
            {
                _tourRatingButtonEnabled = value;
                OnPropertyChanged("TourRatingButtonEnabled");
            }
        }

        private bool _viewStartedTourButtonEnabled;
        public bool ViewStartedTourButtonEnabled
        {
            get { return _viewStartedTourButtonEnabled; }
            set
            {
                _viewStartedTourButtonEnabled = value;
                OnPropertyChanged("ViewStartedTourButtonEnabled");
            }
        }

        private bool _mostVisitedTourButtonEnabled;

        public bool MostVisitedTourButtonEnabled
        {
            get { return _mostVisitedTourButtonEnabled; }
            set
            {
                _mostVisitedTourButtonEnabled = value;
                OnPropertyChanged("MostVisitedTourButtonEnabled");
            }
        }

        private void OnPropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }
        #endregion

        #region Constructors
        public MainGuideViewModel(User guide) 
        {
            RequestIndex=-1;
            DataGridSelectedIndex = -1;
            AllDataGridSelectedIndex = -1;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddDays(1);
            _MessageBoxService = new MessageBoxService();
            SelectedTourRequest = new TourRequest();
            _tourRequestService = new TourRequestService();
            Guide = guide;
            _tourService = new TourService();
            _bookedTourService = new BookedTourService();
            _voucherService = new VoucherService();
            Tours = new ObservableCollection<Tour>(GetTours());
            AllTours = new ObservableCollection<Tour>(_tourService.GetAll());
            TourRequests = new ObservableCollection<TourRequest>(_tourRequestService.GetAll().Where(t => t.Status == RequestStatus.Waiting));
            SelectedTour = new Tour();
            StartTourButtonEnabled = !HasTourInProgress() && Tours.Count > 0 && Tours.FirstOrDefault(t => t.Status == TourStatus.NOT_STARTED) != null;
            ViewStartedTourButtonEnabled = HasTourInProgress();
            MostVisitedTourButtonEnabled = AllTours.FirstOrDefault(t => t.Status == TourStatus.FINISHED) != null;
            CancelTourButtonEnabled = false;
            TourRatingButtonEnabled = false;
            TourStatisticButtonEnabled = false;
        }
        #endregion

        #region Commands
        ICommand _generatePDF;

        public ICommand GeneratePDF
        {
            get
            {
                if (_generatePDF == null)
                {
                    _generatePDF = new RelayCommand(param => GeneratePDFFile());
                }
                return _generatePDF;
            }
        }

        ICommand _registerTourCommand;

        public ICommand RegisterTourCommand
        {
            get
            {
                if (_registerTourCommand == null)
                {
                    _registerTourCommand = new RelayCommand(param => RegisterTour());
                }
                return _registerTourCommand;
            }
        }

        ICommand _one;

        public ICommand One
        {
            get
            {
                if (_one == null)
                {
                    _one = new RelayCommand(param => OneKey());
                }
                return _one;
            }
        }

        ICommand _two;

        public ICommand Two
        {
            get
            {
                if (_two == null)
                {
                    _two = new RelayCommand(param => TwoKey());
                }
                return _two;
            }
        }

        ICommand _three;

        public ICommand Three
        {
            get
            {
                if (_three == null)
                {
                    _three = new RelayCommand(param => ThreeKey());
                }
                return _three;
            }
        }

        ICommand _four;

        public ICommand Four
        {
            get
            {
                if (_four == null)
                {
                    _four = new RelayCommand(param => FourKey());
                }
                return _four;
            }
        }


        ICommand _f1Command;

        public ICommand F1Command
        {
            get
            {
                if(_f1Command == null)
                {
                    _f1Command = new RelayCommand(param => F1Key());
                }
                return _f1Command;
            }
        }

        ICommand _f2Command;

        public ICommand F2Command
        {
            get
            {
                if (_f2Command == null)
                {
                    _f2Command = new RelayCommand(param => F2Key());
                }
                return _f2Command;
            }
        }

        ICommand _f3Command;

        public ICommand F3Command
        {
            get
            {
                if (_f3Command == null)
                {
                    _f3Command = new RelayCommand(param => F3Key());
                }
                return _f3Command;
            }
        }

        ICommand _f4Command;

        public ICommand F4Command
        {
            get
            {
                if (_f4Command == null)
                {
                    _f4Command = new RelayCommand(param => F4Key());
                }
                return _f4Command;
            }
        }

        ICommand _f5Command;

        public ICommand F5Command
        {
            get
            {
                if (_f5Command == null)
                {
                    _f5Command = new RelayCommand(param => F5Key());
                }
                return _f5Command;
            }
        }

        ICommand _selectionChangedGrid1;

        public ICommand SelectionChangedGrid1
        {
            get
            {
                if (_selectionChangedGrid1 == null)
                {
                    _selectionChangedGrid1 = new RelayCommand(param => SelectionChangedGrid1Method());
                }
                return _selectionChangedGrid1;
            }
        }

        ICommand _selectionChangedGrid2;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand SelectionChangedGrid2
        {
            get
            {
                if (_selectionChangedGrid2 == null)
                {
                    _selectionChangedGrid2 = new RelayCommand(param => SelectionChangedGrid2Method());
                }
                return _selectionChangedGrid2;
            }
        }

        ICommand StartTourCommand;

        public ICommand StartTour
        {
            get
            {
                if (StartTourCommand == null)
                {
                    StartTourCommand = new RelayCommand(param => StartTourButtonClick());
                }
                return StartTourCommand;
            }
        }

        ICommand _viewStartedTour;


        public ICommand ViewStartedTour
        {
            get
            {
                if (_viewStartedTour == null)
                {
                    _viewStartedTour = new RelayCommand(param => ViewStartedTourButtonClick());
                }
                return _viewStartedTour;
            }
        }

        public ICommand _cancelTour;

        public ICommand CancelTour
        {
            get
            {
                if (_cancelTour == null)
                {
                    _cancelTour = new RelayCommand(param => cancelTourButton_Click(null,null));
                }
                return _cancelTour;
            }
        }

        public ICommand _tourRating;

        public ICommand TourRating
        {
            get
            {
                if (_tourRating == null)
                {
                    _tourRating = new RelayCommand(param => tourRatingButton_Click(null, null));
                }
                return _tourRating;
            }
        }

        public ICommand _tourStatistic;

        public ICommand TourStatistic
        {
            get
            {
                if (_tourStatistic == null)
                {
                    _tourStatistic = new RelayCommand(param => tourStatisticButton_Click(null, null));
                }
                return _tourStatistic;
            }
        }

        public ICommand _mostVisited;

        public ICommand MostVisited
        {
            get
            {
                if (_mostVisited == null)
                {
                    _mostVisited = new RelayCommand(param => mostVisitedButton_Click(null, null));
                }
                return _mostVisited;
            }
        }
        private ICommand _acceptTourRequest;

        public ICommand AcceptTourRequest
        {
            get
            {
                if (_acceptTourRequest == null)
                {
                    _acceptTourRequest = new RelayCommand(param => AcceptTourRequestButton_Click());
                }
                return _acceptTourRequest;
            }
        }

        private ICommand _createByLocation;
        public ICommand CreateByLocation
        {
            get
            {
                if (_createByLocation == null)
                {
                    _createByLocation = new RelayCommand(param => CreateByLocation_Click());
                }
                return _createByLocation;
            }
        }

        private ICommand _createByLanguage;
        public ICommand CreateByLanguage
        {
            get
            {
                if (_createByLanguage == null)
                {
                    _createByLanguage = new RelayCommand(param => CreateByLanguage_Click());
                }
                return _createByLanguage;

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

        private ICommand _openTourRequest;

        public ICommand OpenTourRequest
        {
            get
            {
                if (_openTourRequest == null)
                {
                    _openTourRequest = new RelayCommand(param => OpenTourRequestButton_Click());
                }
                return _openTourRequest;
            }
        }

        public Action Close { get; set; }
        #endregion

        #region Methods

        public void OneKey()
        {
            SelectedTab = 0;
        }

        public void TwoKey()
        {
            SelectedTab = 1;
        }

        public void ThreeKey()
        {
            SelectedTab = 2;
        }

        public void FourKey()
        {
            SelectedTab = 3;
        }

        public void OpenTourRequestButton_Click()
        {
            TourRequestStatisticsView tourRequestStatisticsView = new TourRequestStatisticsView();
            tourRequestStatisticsView.ShowDialog();
        }
        public void GeneratePDFFile()
        {
            if (StartDate.CompareTo(EndDate) < 0)
            {
                List<Tour> tours = _tourService.GetAll().Where(t => t.StartTime.Time.CompareTo(StartDate.Date) > 0 && t.StartTime.Time.CompareTo(EndDate.Date) < 0).ToList();
                if(tours != null && tours.Count > 0)
                {
                    MakePDF(tours);
                }
                else
                {
                    _MessageBoxService.ShowMessage("Nisu pronadjene ture za selektovani period!");
                }
            }
        }

        public void MakePDF(List<Tour> tours)
        {
            // Register code pages encoding provider
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // Create a new PDF document
            PdfDocument document = new PdfDocument();

            // Add a page to the document
            PdfPage page = document.AddPage();

            // Create graphics for drawing on the page
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Set font for titles
            XFont titleFont = new XFont("Helvetica", 18);

            // Set font for data
            XFont dataFont = new XFont("Helvetica", 12);

            // Set initial position for drawing
            XPoint position = new XPoint(50, 50);

            // Draw title
            gfx.DrawString("Lista zakazanih tura", titleFont, XBrushes.Black, position);
            position.Y += 30;

            // Iterate over the list of tours
            foreach (var tour in tours)
            {
                if (position.Y + 20 * 7 > page.Height)
                {
                    // Add a new page to the document
                    page = document.AddPage();

                    // Create graphics for drawing on the new page
                    gfx = XGraphics.FromPdfPage(page);

                    // Reset the position to the top of the new page
                    position = new XPoint(50, 50);
                    position.Y += 30;
                }
                // Draw tour details
                gfx.DrawString($"Naziv ture: {tour.Name}", dataFont, XBrushes.Black, position);
                position.Y += 20;

                gfx.DrawString($"Opis: {tour.Description}", dataFont, XBrushes.Black, position);
                position.Y += 20;

                gfx.DrawString($"Jezik: {tour.Language}", dataFont, XBrushes.Black, position);
                position.Y += 20;

                gfx.DrawString($"Maksimalan broj gostiju: {tour.MaxNumberOfPeople}", dataFont, XBrushes.Black, position);
                position.Y += 20;

                gfx.DrawString($"Vreme pocetka: {tour.StartTime.Time.ToString()}", dataFont, XBrushes.Black, position);
                position.Y += 20;
                gfx.DrawString($"Trajanje: {tour.Duration}h", dataFont, XBrushes.Black, position);
                position.Y += 20;

                gfx.DrawString($"Lokacija: {tour.Location.City}, {tour.Location.Country}", dataFont, XBrushes.Black, position);
                position.Y += 20;



                position.Y += 30;
            }
            string fileName = "IzvestajTura.pdf";
            // Save the PDF document to a file
            document.Save(fileName);

            // Open the generated PDF file
            Process.Start(new ProcessStartInfo { FileName = fileName, UseShellExecute = true });

        }

        public List<Tour> GetTours()
        {
            return _tourService.GetAllByGuideId(Guide.Id).Where(t => DateTime.Now.Date.Equals(t.StartTime.Time.Date)).ToList();
        }

        public void SelectionChangedGrid1Method()
        {
            if (_dataGridSelectedIndex == -1 || SelectedTour == null)
            {
                return;
            }
            if (Tours.FirstOrDefault(t => t.Status == TourStatus.STARTED) != null)
            {
                StartTourButtonEnabled = false;
            }
            else if (SelectedTour.Status == TourStatus.NOT_STARTED)
            {
                StartTourButtonEnabled = true;
            }
            else
            {
                StartTourButtonEnabled = false;
            }
        }

        public void SelectionChangedGrid2Method()
        {
            if (_allDataGridSelectedIndex == -1 || SelectedTour == null)
            {
                CancelTourButtonEnabled = false;
                TourRatingButtonEnabled = false;
                TourStatisticButtonEnabled = false;
            }
            if (SelectedTour.Status == TourStatus.NOT_STARTED && SelectedTour.StartTime.Time >= DateTime.Now.AddDays(2))
            {
                CancelTourButtonEnabled = true;
                TourRatingButtonEnabled = false;
                TourStatisticButtonEnabled = false;
            }
            else if (SelectedTour.Status == TourStatus.FINISHED)
            {
                CancelTourButtonEnabled = false;
                TourRatingButtonEnabled = true;
                TourStatisticButtonEnabled = true;
            }
            else
            {
                CancelTourButtonEnabled = false;
                TourRatingButtonEnabled = false;
                TourStatisticButtonEnabled = false;
            }
        }

        public void F1Key()
        {
            if (SelectedTab == 0)
                RegisterTour();
            else if (SelectedTab == 1)
                if (CancelTourButtonEnabled)
                    cancelTourButton_Click(null, null);
                else
                    _MessageBoxService.ShowMessage("Morate odabrati turu koja moze da se otkaze!");
            else if( SelectedTab == 2)
            {
                if (RequestIndex != -1)
                    AcceptTourRequestButton_Click();
                else
                    _MessageBoxService.ShowMessage("Morate odabrati zahtev koji zelite da prihvatite!");
            }
            else if( SelectedTab == 3)
            {
                Close();
            }
        }

        public void F2Key()
        {
            if (SelectedTab == 0)
            {
                if (StartTourButtonEnabled) StartTourButtonClick();
                else _MessageBoxService.ShowMessage("Morate odabrati turu koju zelite da zapocnete!");
            }
            else if (SelectedTab == 1)
            {
                if (TourRatingButtonEnabled) tourRatingButton_Click(null, null);
                else _MessageBoxService.ShowMessage("Morate odabrati turu koju zelite da ocenite!");
            }
            else if (SelectedTab == 2) {
                CreateByLocation_Click();
            }
            else if( SelectedTab == 3)
            {
                Close();
            }
        }

        public void F3Key()
        {
            if (SelectedTab == 0)
            {
                if (ViewStartedTourButtonEnabled) ViewStartedTourButtonClick();
                else _MessageBoxService.ShowMessage("Nemate zapocetu turu!");
            }
            else if (SelectedTab == 1)
            {
                if (TourStatisticButtonEnabled) tourStatisticButton_Click(null, null);
                else _MessageBoxService.ShowMessage("Morate odabrati turu za koju zelite da vidite statistiku!");
            }
            else if( SelectedTab == 2)
            {
                CreateByLanguage_Click();
            }
        }


        public void F4Key()
        {
            if (SelectedTab == 1)
            {
                    if (MostVisitedTourButtonEnabled) mostVisitedButton_Click(null, null);
                    else _MessageBoxService.ShowMessage("Nemate zavrsenu turu!");
            }
            if( SelectedTab == 2)
            {
                OpenTourRequestButton_Click();
            }
        }

        public void F5Key()
        {
            if (SelectedTab == 1)
            {
                GeneratePDFFile();
            }
        }


        public void RegisterTour()
        {
            TourRegistrationView tourRegistrationView = new TourRegistrationView(Guide);
            tourRegistrationView.ShowDialog();
            UpdateDataGrid();
            UpdateAllTours();
        }


        public void UpdateDataGrid()
        {
            Tours.Clear();
            foreach (var tour in GetTours())
            {
                Tours.Add(tour);
            }
        }

        public void UpdateAllTours()
        {
            AllTours.Clear();
            foreach (var tour in _tourService.GetAll())
            {
                AllTours.Add(tour);
            }
        }

        public bool HasTourInProgress()
        {
            return Tours.FirstOrDefault(t => t.Status == TourStatus.STARTED) != null;
        }


        private void StartTourButton_Click(object sender, RoutedEventArgs e)
        {
            StartTourButtonClick();
        }

        private void StartTourButtonClick()
        {
            if (DataGridSelectedIndex != -1)
            {
                SelectedTour.Status = TourStatus.STARTED;
                StartTourButtonEnabled = false;
                _tourService.Update(SelectedTour);
                LiveTourTrackingView liveTourTrackingView = new LiveTourTrackingView(SelectedTour);
                liveTourTrackingView.ShowDialog();
                ViewStartedTourButtonEnabled = HasTourInProgress();
            }
        }

        private void ViewStartedTourButton_Click(object sender, RoutedEventArgs e)
        {
            ViewStartedTourButtonClick();
        }

        private void ViewStartedTourButtonClick()
        {
            Tour tourInProgress = Tours.FirstOrDefault(t => t.Status == TourStatus.STARTED);
            if (tourInProgress != null)
            {
                ViewStartedTourButtonEnabled = false;
                LiveTourTrackingView liveTourTrackingView = new LiveTourTrackingView(tourInProgress);
                liveTourTrackingView.ShowDialog();

            }
            ViewStartedTourButtonEnabled = HasTourInProgress();
        }

        private void tourRatingButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTour.Status == TourStatus.FINISHED)
            {
                TourReviewView tourReviewView = new TourReviewView(SelectedTour.Id);
                tourReviewView.ShowDialog();
            }
        }

        private void cancelTourButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTour.Status == TourStatus.NOT_STARTED && SelectedTour.StartTime.Time >= DateTime.Now.AddDays(2))
            {
                _tourService.CancelTour(SelectedTour);
                AllTours.Remove(SelectedTour);
                _MessageBoxService.ShowMessage("Uspesno ste otkazali turu");
            }
        }

        private void tourStatisticButton_Click(object sender, RoutedEventArgs e)
        {
            if (AllDataGridSelectedIndex != -1 && SelectedTour.Status == TourStatus.FINISHED)
            {
                TourStatisticsView tourStatisticsView = new TourStatisticsView(SelectedTour);
                tourStatisticsView.ShowDialog();
            }
        }

        private void mostVisitedButton_Click(object sender, RoutedEventArgs e)
        {
            TourStatisticsView tourStatisticsView = new TourStatisticsView(_tourService.GetMostVisited());
            tourStatisticsView.ShowDialog();
        }


        private void UpdateRequest()
        {
            TourRequests.Clear();
            foreach (TourRequest t in _tourRequestService.GetAll().Where(t => t.Status == RequestStatus.Waiting))
            {
                TourRequests.Add(t);
            }
        }

        private void AcceptTourRequestButton_Click()
        {
            if (RequestIndex != -1 && SelectedTourRequest != null)
            {
                AcceptTourRequest acceptTourRequest = new AcceptTourRequest(SelectedTourRequest, Guide);
                acceptTourRequest.ShowDialog();
                UpdateRequest();
            }
        }

        private void CreateByLocation_Click()
        {
            Location location = _tourRequestService.GetLocationMostRequested();
            if (location != null)
            {
                TourRegistrationView tourRegistrationView = new TourRegistrationView(Guide, location);
                tourRegistrationView.ShowDialog();
            }
            else
            {
                _MessageBoxService.ShowMessage("Greska");
            }
        }

        private void CreateByLanguage_Click()
        {
            string language = _tourRequestService.GetLanguageMostVisited();
            if (language != "")
            {
                TourRegistrationView tourRegistrationView = new TourRegistrationView(Guide, null, language);
                tourRegistrationView.ShowDialog();
            }
            else
            {
                _MessageBoxService.ShowMessage("Greska");
            }
        }
        #endregion

    }
}
