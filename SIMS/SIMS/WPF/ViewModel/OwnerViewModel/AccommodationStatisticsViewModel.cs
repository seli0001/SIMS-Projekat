using SIMS.Domain.Model;
using SIMS.Service.Services;
using SIMS.Service.UseCases;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.Generic;
using LiveCharts.Wpf;
using Axis = LiveChartsCore.SkiaSharpView.Axis;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Windows;
using System.Collections.ObjectModel;
using System.Security.Principal;
using System.ComponentModel;
using SIMS.WPF.ViewModel.ViewModel;

namespace SIMS.WPF.ViewModel.OwnerViewModel
{
    class AccommodationStatisticsViewModel : ViewModelBase
    {
        public ISeries[] YearSeries { get; set; } 
        public Axis[] YearXAxes { get; set; }

        public ISeries[] MonthSeries { get; set; }
        public Axis[] MonthXAxes { get; set; }

        public SolidColorPaint LedgendBackgroundPaint { get; set; } =
        new SolidColorPaint(SKColors.White);

        public ObservableCollection<double> reservationsMonthStats;
        public ObservableCollection<double> canceledReservationsMonthStats;
        public ObservableCollection<double> rescheduledReservationsMonthStats;


        private readonly ReservationService _reservationService;
        private readonly CancelingRequestService _cancelingRequestService;
        private readonly ReschedulingRequestsService _reschedulingRequestsService;

        public Accommodation SelectedAccommodation { get; set; }
        public User LoggedInUser { get; set; }
        public List<int> AvailableYears { get; set; }

        private int _selectedYear;
        public int SelectedYear
        {
            get
            {
                return _selectedYear;
            }
            set
            {
                if(value != _selectedYear)
                {
                    _selectedYear = value;
                    DisplayUIForYear(value);
                    DisplayBestMonth(value);
                }

            }
        }

        private int _bestYear;
        public int BestYear {
            get
            {
                return _bestYear;
            }
            set
            {
                if(value != _bestYear)
                {
                    _bestYear = value;
                    OnPropertyChanged();
                }
            }
                
        }

        private string _bestMonth;
        public string BestMonth
        {
            get => _bestMonth;
            set
            {
                if(_bestMonth != value)
                {
                    _bestMonth = value;
                    OnPropertyChanged();
                }
            }
        }


        public AccommodationStatisticsViewModel()
        {

        }

        public AccommodationStatisticsViewModel(User user, MainViewModel mvm, Accommodation selectedAccommodation)
        {
            SelectedAccommodation = selectedAccommodation;
            LoggedInUser = user;

            _reservationService = new ReservationService();
            _cancelingRequestService = new CancelingRequestService();
            _reschedulingRequestsService = new ReschedulingRequestsService();

            AvailableYears = new List<int>(_reservationService.GetYearsForAccommodation(SelectedAccommodation));
            BestYear = _reservationService.GetBestYear(SelectedAccommodation);

            reservationsMonthStats = new ObservableCollection<double>();
            canceledReservationsMonthStats = new ObservableCollection<double>();
            rescheduledReservationsMonthStats = new ObservableCollection<double>();

            GenerateUI();
            InitializeMonths(AvailableYears[AvailableYears.Count-1]);
            SelectedYear = AvailableYears[AvailableYears.Count - 1];
        }

        private void GenerateUI()
        {
            
            List<string> yearsString = new List<string>();

             List<double> reservationsYearStats = new List<double>();
             List<double> canceledReservationsYearStats = new List<double>();
             List<double> rescheduledReservationsYearStats = new List<double>();

            foreach (int year in AvailableYears)
            {
                reservationsYearStats.Add(_reservationService.GetResNumForYear(year, SelectedAccommodation));
                canceledReservationsYearStats.Add(_cancelingRequestService.GetCanceledResNumForYear(year, SelectedAccommodation));
                rescheduledReservationsYearStats.Add(_reschedulingRequestsService.GetRescheduledResNumForYear(year, SelectedAccommodation));
            }
            //Converting years to string, for displaying on X axes
            foreach (int year in AvailableYears)
            {
                yearsString.Add(year.ToString());
            }

            YearSeries = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Values = reservationsYearStats,
                    Name = "Reservations",
                    Stroke = null,
                    MaxBarWidth = 50,
                    GroupPadding = 30,
                },
                new ColumnSeries<double>
                {
                    Values = canceledReservationsYearStats,
                    Name = "Canceled Reservations",
                    Stroke = null,
                    MaxBarWidth = 50,
                    GroupPadding = 30,
                },
                new ColumnSeries<double>
                {
                    Values = rescheduledReservationsYearStats,
                    Name = "Rescheduled Reservations",
                    Stroke = null,
                    MaxBarWidth = 50,
                    GroupPadding = 30,
                },
            };


            YearXAxes = new Axis[]
            {
                new Axis
                {
                    Labels = yearsString,
                    LabelsPaint = new SolidColorPaint(SKColors.White),
                }
            };
        }

        private void InitializeMonths(int year)
        {
            List<string> monthsString = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Avg", "Sept", "Oct", "Nov", "Dec" };

            reservationsMonthStats.Clear();
            canceledReservationsMonthStats.Clear();
            rescheduledReservationsMonthStats.Clear();

            for (int i = 1; i < 13; i++)
            {
                reservationsMonthStats.Add(_reservationService.GetResNumForMonth(year, i, SelectedAccommodation));
                canceledReservationsMonthStats.Add(_cancelingRequestService.GetCanceledResNumForMonth(year, i, SelectedAccommodation));
                rescheduledReservationsMonthStats.Add(_reschedulingRequestsService.GetRescheduledResNumForMonth(year, i, SelectedAccommodation));
            }

            MonthSeries = new ISeries[]
           {
                new ColumnSeries<double>
                {
                    Values = reservationsMonthStats,
                    Name = "Reservations",
                    Stroke = null,
                    MaxBarWidth = 50,
                    GroupPadding = 30,
                },
                new ColumnSeries<double>
                {
                    Values = canceledReservationsMonthStats,
                    Name = "Canceled Reservations",
                    Stroke = null,
                    MaxBarWidth = 50,
                    GroupPadding = 30,
                },
                new ColumnSeries<double>
                {
                    Values = rescheduledReservationsMonthStats,
                    Name = "Rescheduled Reservations",
                    Stroke = null,
                    MaxBarWidth = 50,
                    GroupPadding = 30,
                },
           };

            MonthXAxes = new Axis[]
            {
                new Axis
                {
                    Labels = monthsString,
                    LabelsPaint = new SolidColorPaint(SKColors.White),
                }
            };
        }

        private void DisplayUIForYear(int year)
        {
            reservationsMonthStats.Clear();
            canceledReservationsMonthStats.Clear();
            rescheduledReservationsMonthStats.Clear();

            for (int i=1;  i<13; i++)
            {
                reservationsMonthStats.Add(_reservationService.GetResNumForMonth(year, i, SelectedAccommodation));
                canceledReservationsMonthStats.Add(_cancelingRequestService.GetCanceledResNumForMonth(year,i, SelectedAccommodation));
                rescheduledReservationsMonthStats.Add(_reschedulingRequestsService.GetRescheduledResNumForMonth(year,i, SelectedAccommodation));
            }
        }

        private void DisplayBestMonth(int year)
        {
           int bestMonth = _reservationService.GetBestMonth(SelectedAccommodation, year);
            switch(bestMonth)
            {
                case 1:
                    BestMonth = "January";
                    break;
                case 2:
                    BestMonth = "February";
                    break;
                case 3:
                    BestMonth = "March";
                    break;
                case 4:
                    BestMonth = "April";
                    break;
                case 5:
                    BestMonth = "May";
                    break;
                case 6:
                    BestMonth = "Jun";
                    break;
                case 7:
                    BestMonth = "July";
                    break;
                case 8:
                    BestMonth = "Avgust";
                    break;
                case 9:
                    BestMonth = "September";
                    break;
                case 10:
                    BestMonth = "October";
                    break;
                case 11:
                    BestMonth = "November";
                    break;
                case 12:
                    BestMonth = "December";
                    break;

            }
        }
    }
}
