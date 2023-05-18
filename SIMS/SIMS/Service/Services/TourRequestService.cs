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

        public SeriesCollection GetDataForChartByYear(int userId, int SelectedYear)
        {
            List<TourRequest> data = GetByUser(userId);
            SeriesCollection series = new SeriesCollection();
            int num_accepted = 0;
            int num_rejected = 0;

            foreach (TourRequest t in data)
            {
                if (t.Status == RequestStatus.Accepted && t.StartDate.Year == SelectedYear) num_accepted = num_accepted + 1;
                else if (t.Status == RequestStatus.Rejected && t.StartDate.Year == SelectedYear) num_rejected = num_rejected + 1;
            }



            series.Add(new PieSeries { Title = "Prihvacene", Values = new ChartValues<ObservableValue> { new ObservableValue(num_accepted) } });
            series.Add(new PieSeries { Title = "Odbijene", Values = new ChartValues<ObservableValue> { new ObservableValue(num_rejected) } });

            return series;
        }


        public Dictionary<string, int> GetLanguageGraphData(int userId)
        {
            return _tourRequestRepository.GetLanguageGraphData(userId);
        }

        public List<int> GetYearsOfRequests(int userId)
        {
            return _tourRequestRepository.GetYearsOfRequests(userId);
        }


        public Dictionary<string, int> GetLocationGraphData(int userId)
        {
            return _tourRequestRepository.GetLocationGraphData(userId);
        }

        public int GetPeoplePercentage(int userId)
        {
            List<TourRequest> requests = _tourRequestRepository.GetByUser(userId);
            int numberOfPeople = 0;
            int counter = 0;
            foreach (TourRequest request in requests)
            {
                if (request.Status == RequestStatus.Accepted)
                {
                    numberOfPeople += request.MaxNumberOfPeople;
                    counter++;
                }
            }
            return numberOfPeople / counter;
        }

        public int GetPeoplePercentageByYear(int userId, int SelectedYear)
        {
            List<TourRequest> requests = _tourRequestRepository.GetByUser(userId);
            int numberOfPeople = 0;
            int counter = 0;
            foreach (TourRequest request in requests)
            {
                if (request.StartDate.Year == SelectedYear && request.Status == RequestStatus.Accepted)
                {
                    numberOfPeople += request.MaxNumberOfPeople;
                    counter++;
                }
            }
            return numberOfPeople / counter;
        }

    }
}
