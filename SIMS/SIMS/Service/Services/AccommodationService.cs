using SIMS.Domain.Model;
using SIMS.Repository;
using System.Collections.Generic;
using Type = SIMS.Domain.Model.Type;

namespace SIMS.Service.UseCases
{
    public class AccommodationService
    {
        private List<Accommodation> _accommodations;
        private readonly AccommodationRepository _accommodationRepository;
        public AccommodationService()
        {
            _accommodations = new List<Accommodation>();
            _accommodationRepository = new AccommodationRepository();
        }

        public List<Accommodation> GetAll()
        {
            _accommodations = _accommodationRepository.GetAll();
            return _accommodations;
        }

        public Accommodation Save(Accommodation accommodation)
        {
            return _accommodationRepository.Save(accommodation);
        }

        public Accommodation Save(string name, Location location, Type acType, int maxGuestNum, int minReservationDays, int cancelDays, User user, List<Image> images)
        {
            Accommodation newAccommodation = new Accommodation(name, location, acType, maxGuestNum, minReservationDays, cancelDays, user, images);
            return Save(newAccommodation);
        }

        public void Delete(Accommodation accommodation)
        {
            _accommodationRepository.Delete(accommodation);
        }

        public Accommodation Update(Accommodation accommodation)
        {
            return _accommodationRepository.Update(accommodation);
        }

        public List<Accommodation> GetByUser(User user)
        {
            _accommodations = GetAll();
            return _accommodations.FindAll(c => c.User.Id == user.Id);
        }

        public Accommodation GetById(int id)
        {
            _accommodations = GetAll();
            return _accommodations.Find(c => c.Id == id);
        }


        public void makeSuperOwner(User user)
        {
            _accommodationRepository.makeSuperOwner(user);
        }

        public void deleteSuperOwner(User user)
        {
            _accommodationRepository.deleteSuperOwner(user);
        }

        public void RegulateRenovations()
        {
            _accommodationRepository.RegulateRenovations();
        }

        public Accommodation makeSuper(Accommodation accommodation)
        {
            accommodation.Super = true;
            Save(accommodation);
            return accommodation;
        }

    }
}
