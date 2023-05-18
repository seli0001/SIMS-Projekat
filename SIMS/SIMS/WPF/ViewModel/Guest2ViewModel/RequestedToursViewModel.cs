using LiveCharts;
using SIMS.Domain.Model;
using SIMS.Service.Services;
using SIMS.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SIMS.WPF.ViewModel.Guest2ViewModel
{
    internal class RequestedToursViewModel : INotifyPropertyChanged, IClose
    {
        private readonly TourRequestService _tourRequestService;
        public ObservableCollection<TourRequest> requests { get; set; }


        public Action Close { get; set; }
        

        public int userId { get; set; }
        #region commands

        private SeriesCollection _seriesCollectionTourRequest;
        public SeriesCollection SeriesCollectionTourRequest
        {
            get { return _seriesCollectionTourRequest; }
            set
            {
                _seriesCollectionTourRequest = value;
                OnPropertyChanged(); 
            }
        }

        private int _peoplePercentage;

        public int PeoplePercentage
        {
            get { return _peoplePercentage; }
            set
            {
                _peoplePercentage = value;
                OnPropertyChanged(); 
            }
        }

        private Dictionary<string, int> _languageGraph;
       



        private int _selectedYear;
        public int SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (_selectedYear != value)
                {
                    _selectedYear = value;
                    OnPropertyChanged();
                }
            }
        }




        private ObservableCollection<int> _years;
        public ObservableCollection<int> Years
        {
            get { return _years; }
            set
            {
                _years = value;
                OnPropertyChanged(nameof(Years));
            }
        }

        public Dictionary<string, int> LanguageGraph
        {
            get => _languageGraph;
            set
            {
                if (_languageGraph != value)
                {
                    _languageGraph = value;
                    OnPropertyChanged("LanguageRequests");
                }
            }
        }

        private Dictionary<string, int> _locationGraph;
        public Dictionary<string, int> LocationGraph
        {
            get => _locationGraph;
            set
            {
                if (_locationGraph != value)
                {
                    _locationGraph = value;
                    OnPropertyChanged("LocationRequests");
                }
            }
        }

        private ICommand _backToMainGuest2ViewClickCommand;
        public ICommand BackToMainGuest2ViewClickCommand
        {
            get
            {
                return _backToMainGuest2ViewClickCommand ?? (_backToMainGuest2ViewClickCommand = new CommandBase(() => BackToMainGuest2ViewClick(), true));
            }
        }




        private ICommand _dataByYearCommand;
        public ICommand DataByYearCommand
        {
            get
            {
                return _dataByYearCommand ?? (_dataByYearCommand = new CommandBase(() => DataByYearClick(), true));
            }
        }



        private ICommand _allRequestCommand;
        public ICommand AllRequestCommand
        {
            get
            {
                return _allRequestCommand ?? (_allRequestCommand = new CommandBase(() => DataByAllYearClick(), true));
            }
        }

        #endregion


        private void BackToMainGuest2ViewClick()
        {
            MainGuest2View mainGuest2View = new MainGuest2View(userId);
            mainGuest2View.Show();
            Close();
        }

        private void DataByYearClick()
        {
            SeriesCollectionTourRequest.Clear(); // Briše postojeće podatke
            var newData = _tourRequestService.GetDataForChartByYear(userId, SelectedYear);
            foreach (var series in newData)
            {
                SeriesCollectionTourRequest.Add(series); // Dodaje nove podatke
            }
            PeoplePercentage = _tourRequestService.GetPeoplePercentageByYear(userId, SelectedYear);
        }

        private void DataByAllYearClick()
        {
            SeriesCollectionTourRequest.Clear(); // Briše postojeće podatke
            var newData = _tourRequestService.GetDataForChartByRequest(userId);
            foreach (var series in newData)
            {
                SeriesCollectionTourRequest.Add(series); // Dodaje nove podatke
            }
            PeoplePercentage = _tourRequestService.GetPeoplePercentage(userId);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RequestedToursViewModel(int userId)
        {
            this.userId = userId;
            _tourRequestService = new TourRequestService();
            Years = new ObservableCollection<int>(_tourRequestService.GetYearsOfRequests(userId));
            requests = new ObservableCollection<TourRequest>(_tourRequestService.GetByUser(userId));
            SeriesCollectionTourRequest = _tourRequestService.GetDataForChartByRequest(userId);
            LanguageGraph = _tourRequestService.GetLanguageGraphData(userId);
            LocationGraph = _tourRequestService.GetLocationGraphData(userId);
            PeoplePercentage = _tourRequestService.GetPeoplePercentage(userId);
        }




    }
}
