using SIMS.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace SIMS.Repository
{
    class ProposalRepository
    {
        private readonly ReservationRepository _reservationRepository;
        private readonly LocationRepository _locationRepository;
        private readonly AccommodationRepository _accommodationRepository;

        public ProposalRepository()
        {
            _reservationRepository = new ReservationRepository();
            _locationRepository = new LocationRepository();
            _accommodationRepository = new AccommodationRepository(); 
        }

        public List<Location> GetTopLocations()
        {
            List<Location> locations = _locationRepository.GetAll();
            Dictionary<Location, int> locationStats = new Dictionary<Location, int>();
            List<Location> topLocations = new List<Location>();
            foreach (Location location in locations)
            {
                locationStats.Add(location, _reservationRepository.CountResForLocation(location));
            }

            return locationStats.OrderByDescending(x => x.Value).Take(3).Select(x => x.Key).ToList();
        }

        public List<Accommodation> GetAccommodationsWorstLocations(User owner)
        {
            List<Location> allLocations = _locationRepository.GetAll();
            List<Accommodation> accommodations = _accommodationRepository.GetAll();
            
            List<Location> locations = getUsefulLocations(allLocations, accommodations);
            List<Location> badLocations = getBadLocations(locations);

            List<Accommodation> accForOwner = _accommodationRepository.GetByUser(owner);

            return getBadAccommodations(badLocations, accForOwner);
        }



        private List<Accommodation> getBadAccommodations(List<Location> locations, List<Accommodation> accommodations)
        {
            List<Accommodation> badAcc = new List<Accommodation>();
            foreach(Accommodation acc in accommodations)
            {
                if (locationExist(acc, locations))
                    badAcc.Add(acc);
            }

            return badAcc;
        }

        private List<Location> getBadLocations(List<Location> locations)
        {
            List<Location> badLocations = new List<Location>();
            foreach (Location location in locations)
            {
                if (_reservationRepository.CountResForLocation(location) < 3)
                {
                    badLocations.Add(location);
                }
            }
            return badLocations;
        }

        private List<Location> getUsefulLocations(List<Location> allLocations, List<Accommodation> accommodations)
        {
            List<Location> locations = new List<Location>();
            foreach (Location location in allLocations)
            {
                if (accommodationExist(location, accommodations) && !isTopLocation(location))
                    locations.Add(location);
            }
            return locations;
        }

        private bool isTopLocation(Location location)
        {
            List<Location> topLocations = GetTopLocations();
            foreach(Location loc in topLocations)
            {
                if (loc.Id == location.Id)
                    return true;
            }

            return false;
        }

        private bool locationExist(Accommodation accommodation, List<Location> locations)
        {
            foreach (Location loc in locations)
            {
                if (loc.Id == accommodation.Location.Id)
                    return true;
            }

            return false;
        }
        private bool accommodationExist(Location location, List<Accommodation> accommodation)
        {
            foreach(Accommodation acc in accommodation)
            {
                if (acc.Location.Id == location.Id)
                    return true;
            }

            return false;
        }
    }
}
