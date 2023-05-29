using SIMS.Domain.Model;
using SIMS.Repository;
using System.Collections.Generic;

namespace SIMS.Service.Services
{
    class ProposalService
    {
        private readonly ProposalRepository _proposalRepository;
        public ProposalService()
        {
            _proposalRepository = new ProposalRepository();
        }

        public List<Location> GetTopLocations()
        {
            return _proposalRepository.GetTopLocations();
        }

        public List<Accommodation> GetAccommodationsWorstLocations(User user)
        {
            return _proposalRepository.GetAccommodationsWorstLocations(user);
        }
        
    }
}
