using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts;
using SIMS.Domain.Model;
using SIMS.Domain.RepositoryInterface;
using SIMS.Injector;
using SIMS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Service.Services
{
    public class TourRequestService
    {
        private readonly ITourRequestRepository _tourRequestRepository;
    
        public TourRequestService()
        {
            _tourRequestRepository = Injector.Injector.CreateInstance<ITourRequestRepository>();

        }

        public void Save(Location location, string description, string language, int maxNumberOfPeople, DateTime startDate, DateTime endDate, RequestStatus status,int userId)
        {
            _tourRequestRepository.Save(location,description,language, maxNumberOfPeople,startDate,endDate, status,userId);
        }
       
        public List<TourRequest> GetByUser(int userId)
        {
           return  _tourRequestRepository.GetByUser(userId);
        }

        public void Update(TourRequest tourRequest)
        {
            _tourRequestRepository.Update(tourRequest);
        }

        public SeriesCollection GetDataForChartByRequest(int userId)
        {
            List<TourRequest> data = GetByUser(userId);
            SeriesCollection series = new SeriesCollection();
            int num_accepted = 0;
            int num_rejected = 0;

            foreach (TourRequest t in data)
            {
                if (t.Status == RequestStatus.Accepted) num_accepted = num_accepted + 1;
                else if (t.Status == RequestStatus.Rejected) num_rejected = num_rejected + 1;
            }



            series.Add(new PieSeries { Title = "Prihvacene", Values = new ChartValues<ObservableValue> { new ObservableValue(num_accepted) } });
            series.Add(new PieSeries { Title = "Odbijene", Values = new ChartValues<ObservableValue> { new ObservableValue(num_rejected) } });

            return series;
        }



    }
}
