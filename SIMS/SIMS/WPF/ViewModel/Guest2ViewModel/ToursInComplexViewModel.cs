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
    internal class ToursInComplexViewModel
    {
        public ComplexTourRequest ComplexTourRequest { get; set; }
        public ObservableCollection<TourRequest> requests { get; set; }
        private TourRequestService _tourRequestService;
        public ToursInComplexViewModel(int userId,ComplexTourRequest complexTourRequest)
        {

            _tourRequestService = new TourRequestService();

            requests = new ObservableCollection<TourRequest>(_tourRequestService.GetRequestsById(complexTourRequest.TourRequestsId)); 


        }
    }
}
