﻿using SIMS.Domain.Model;
using SIMS.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Service.Services
{
    internal class TourService
    {
        private readonly TourRepository _tourRepository;
        private readonly BookedTourService _bookedTourService;
        private readonly VoucherService _voucherService;

        public TourService() {
            _tourRepository = new TourRepository();
            _bookedTourService = new BookedTourService();
            _voucherService = new VoucherService();
        }

        public List<Tour> GetNotStarted()
        {
            return _tourRepository.GetNotStarted();
        }

        public List<Tour> GetAll()
        {
            return _tourRepository.GetAll();
        }

        public void Update(Tour tour)
        {
            _tourRepository.Update(tour);
        }


        public List<Tour> GetAllByGuideId(int id)
        {
            return GetAll().Where(tour => tour.Guide.Id == id).ToList();

        }

        public List<Tour> GetAlternative(Tour tour)
        {
            List<Tour> tours = GetAll();
            List<Tour> alternativeTours = new List<Tour>();
            return _tourRepository.GetAlternativeTours(tours, alternativeTours, tour);
        }

        public void UpdateNumberOfPeople(Tour tour, int tourid)
        {
         _tourRepository.UpdateNumberOfPeople(tour, tourid);
        }

        public void Delete(Tour tour)
        {
            _tourRepository.Delete(tour);
        }

        public void CancelTour(Tour tour, int valid = 1)
        {
            _bookedTourService.GetAllByTour(tour.Id).ForEach(b => { 
                _voucherService.AddVoucher(new Voucher(DateTime.Now.AddYears(valid), tour.Name, b.User.Id));
                _bookedTourService.Delete(b);
            });
            Delete(tour);
        }

        public Tour GetMostVisited()
        {
            return GetAll().Where(t => t.Status == TourStatus.FINISHED).MaxBy(t => t.NumberOfPeople);
        }

    }
}
