using SIMS.Domain.Model;
using SIMS.Model;
using SIMS.Repository;
using SIMS.Service.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.WPF.ViewModel.Guest1ViewModel
{
    public class RequestViewModel
    {
        public User LoggedInUser { get; set; }

        private ReschedulingRequestsService _reschedulingRequestsService;
        public static ObservableCollection<ReschedulingRequests> ReschedulingRequests { get; set; }
        public ReschedulingRequests SelectedRequest { get; set; }


        private ReschedulingRequestsRepository _reschedulingRequestsRepository;
        private CancelingRequestsRepository _cancelingRequestsRepository;
        private ReservationRepository _reservationRepository;
        private AccommodationRepository _accomodationRepository;
        public static ObservableCollection<CancelingRequests> CancelingRequests { get; set; }
        public static ObservableCollection<Accommodation> Accommodations { get; set; }
        public static ObservableCollection<Reservation> Reservations { get; set; }
        public static ObservableCollection<Reservation> ReservationsForRescheduling { get; set; }
        public CancelingRequests SelectedCancelingRequest { get; set; }

        public RequestViewModel(User user)
        {
            _reschedulingRequestsRepository = new ReschedulingRequestsRepository();
            _cancelingRequestsRepository = new CancelingRequestsRepository();
            _reservationRepository = new ReservationRepository();
            _accomodationRepository = new AccommodationRepository();
            ReschedulingRequests = new ObservableCollection<ReschedulingRequests>(_reschedulingRequestsRepository.GetByUserId(user.Id));
            CancelingRequests = new ObservableCollection<CancelingRequests>(_cancelingRequestsRepository.GetByUserId(user.Id));
            Reservations = new ObservableCollection<Reservation>();
            PopulateReservationsForCanceling();
            Accommodations = new ObservableCollection<Accommodation>(_accomodationRepository.GetAll());
            PopulateAccommodationNames();

            ReservationsForRescheduling = new ObservableCollection<Reservation>();
            PopulateReservationsForRescheduling();
            LoggedInUser = user;
        }

        private void PopulateReservationsForRescheduling()
        {
            foreach (var reschedulingRequest in ReschedulingRequests)
            {
                ReservationsForRescheduling.Add(_reservationRepository.GetById(reschedulingRequest.Reservation.Id));
            }
        }

        private void PopulateReservationsForCanceling()
        {
            foreach (var cancelingRequest in CancelingRequests)
            {
                Reservations.Add(_reservationRepository.GetById(cancelingRequest.Reservation.Id));
            }
        }

        private void PopulateAccommodationNames()
        {
            foreach (var reservation in Reservations)
            {
                reservation.Accommodation = Accommodations.FirstOrDefault(a => a.Id == reservation.Accommodation.Id);
            }
        }
        private void PopulateAccommodationNamesForRescheduling()
        {
            foreach (var reservation in ReservationsForRescheduling)
            {
                reservation.Accommodation = Accommodations.FirstOrDefault(a => a.Id == reservation.Accommodation.Id);
            }
        }
    }
}
