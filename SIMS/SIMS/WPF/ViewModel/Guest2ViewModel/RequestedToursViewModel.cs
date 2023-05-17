using LiveCharts;
using SIMS.Domain.Model;
using SIMS.Service.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.WPF.ViewModel.Guest2ViewModel
{
    internal class RequestedToursViewModel
    {
        private readonly TourRequestService _tourRequestService;
        public ObservableCollection<TourRequest> requests { get; set; }

        public SeriesCollection SeriesCollectionTourRequest { get; set; }

        public RequestedToursViewModel(int userId) {

            _tourRequestService = new TourRequestService();
            requests =new ObservableCollection<TourRequest>( _tourRequestService.GetByUser(userId));
            SeriesCollectionTourRequest = _tourRequestService.GetDataForChartByRequest(userId);

        }
    }
}
