using SIMS.Domain.Model;
using SIMS.Service;
using SIMS.Service.Services;
using SIMS.Service.UseCases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.WPF.ViewModel.OwnerViewModel
{
    class SystemProposalViewModel
    {
        public User LoggedInUser { get; set; }
        public static ObservableCollection<Location> TopLocations { get; set; }
        public static ObservableCollection<Accommodation> WorstAccommodations { get; set; }

        private readonly ProposalService _proposalService;

        public SystemProposalViewModel()
        {

        }

        public SystemProposalViewModel(User user)
        {
            LoggedInUser = user;
            _proposalService = new ProposalService();
            TopLocations = new ObservableCollection<Location>(_proposalService.GetTopLocations());
            WorstAccommodations = new ObservableCollection<Accommodation>(_proposalService.GetAccommodationsWorstLocations(user));
        }
    }
}
