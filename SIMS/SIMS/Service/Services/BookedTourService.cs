using SIMS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS.Repository;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;

namespace SIMS.Service.Services
{
    internal class BookedTourService
    {
        private readonly BookedTourRepository _bookedToursRepository;

        public BookedTourService()
        {
            _bookedToursRepository=new BookedTourRepository();
        }
        
        public List<BookedTour> GetAllByTour(int tourId)
        {
            return _bookedToursRepository.GetAll().Where(t => t.Tour.Id == tourId).ToList();
        }

        public List<BookedTour> GetByUser(int userId)
        {
            return _bookedToursRepository.GetAll().Where(u => userId == u.UserId).ToList();
        }

        public void Save(Tour tour,int userId,int peopleNumber)
        {
            _bookedToursRepository.Save(tour, userId,peopleNumber);
        }
        public void Update(BookedTour bookedTour)
        {
            _bookedToursRepository.Update(bookedTour);
        }

        public List<BookedTour> GetUserFinished(int userId)
        {
            return _bookedToursRepository.GetAll().Where(t => t.UserId == userId && 
            t.Tour.Status.ToString().Equals("FINISHED") && 
            t.Review == false && t.Checkpoint != null && 
            t.Notify.ToString().Equals("Accepted")).ToList();
        }

        public List<BookedTour> GetUserActive(int userId)
        {
            return _bookedToursRepository.GetAll().Where(t => t.UserId == userId && 
            t.Tour.Status.ToString().Equals("STARTED") && 
            t.Checkpoint != null && 
            t.Notify.ToString().Equals("Accepted")).ToList();
        }

        public SeriesCollection getDataForChartByAge(Tour tour)
        {
            List<BookedTour> data = GetAllByTour(tour.Id);
            SeriesCollection series = new SeriesCollection();
            int num_under_eighteen = 0;
            int num_eighteen_to_fifty = 0;
            int num_over_fifty = 0;
            foreach (BookedTour t in data)
            {
                if (t.User.Age < 18) num_under_eighteen += t.NumberOfPeople;
                else if (t.User.Age <= 50) num_eighteen_to_fifty += t.NumberOfPeople;
                else num_over_fifty += t.NumberOfPeople;
            }
            series.Add(new PieSeries { Title = "Ispod 18", Values = new ChartValues<ObservableValue> { new ObservableValue(num_under_eighteen) } });
            series.Add(new PieSeries { Title = "Izmedju 18 i 50", Values = new ChartValues<ObservableValue> { new ObservableValue(num_eighteen_to_fifty) } });
            series.Add(new PieSeries { Title = "preko 50", Values = new ChartValues<ObservableValue> { new ObservableValue(num_over_fifty) } });
            return series;
        }

        public SeriesCollection getDataForChartByVoucher(Tour tour)
        {
            List<BookedTour> data = GetAllByTour(tour.Id);
            SeriesCollection series = new SeriesCollection();
            int num_vouchers = 0;
            int num_not_vouchers = 0;
            foreach (BookedTour t in data)
            {
                if (t.UsedVoucher) num_vouchers += t.NumberOfPeople;
                else num_not_vouchers += t.NumberOfPeople;
            }
            series.Add(new PieSeries { Title = "Sa Vaucerom", Values = new ChartValues<ObservableValue> { new ObservableValue(num_vouchers) } });
            series.Add(new PieSeries { Title = "Bez Vaucera", Values = new ChartValues<ObservableValue> { new ObservableValue(num_not_vouchers) } });
            return series;
        }
    }
}
