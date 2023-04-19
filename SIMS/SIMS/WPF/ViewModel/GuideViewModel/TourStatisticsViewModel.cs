using LiveCharts;
using SIMS.Domain.Model;
using SIMS.Service.Services;
using SIMS.WPF.ViewModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SIMS.WPF.ViewModel.GuideViewModel
{
    class TourStatisticsViewModel : ViewModelBase, IClose
    {
        public SeriesCollection SeriesCollectionAge { get; set; }
        public SeriesCollection SeriesCollectionVoucher { get; set; }
        private readonly BookedTourService _bookedTourService;
        public String TourName { get; set; }
        public String City { get; set; }
        public String Country { get; set; }
        public String Date { get; set; }
        public TourStatisticsViewModel(Tour selectedTour)
        {
            _bookedTourService = new BookedTourService();
            SeriesCollectionAge = _bookedTourService.getDataForChartByAge(selectedTour);
            SeriesCollectionVoucher = _bookedTourService.getDataForChartByVoucher(selectedTour);

            TourName = "Naziv ture: " + selectedTour.Name;
            City = "Grad: " + selectedTour.Location.City;
            Country = "Drzava: " + selectedTour.Location.Country;
            Date = "Datum: " + selectedTour.StartTime.Time.ToString();
        }

        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new CommandBase(() => Close(), true));
            }
        }

        public Action Close { get; set; }
    }
}
