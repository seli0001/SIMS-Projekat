using SIMS.Domain.Model;
using SIMS.Repository;
using SIMS.Service.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SIMS.WPF.ViewModel.Guest1ViewModel
{
    public class CreateForumViewModel
    {

        public ObservableCollection<Location> Locations { get; set; }
        private readonly ProposalRepository _proposalRepository;
        private readonly AccommodationRepository _accommodationRepository;
        private readonly ForumRepository _forumRepository;
        private ForumViewModel ForumVM { get; set; }

        public Location SelectedLocation { get; set; }
        public User LoggedInUser { get; set; }
        Guest1MainViewModel guest1MainViewModel;

        public CreateForumViewModel(User user, Guest1MainViewModel guest1MainViewModel)
        {
            _proposalRepository = new ProposalRepository();
            _accommodationRepository = new AccommodationRepository();
            _forumRepository = new ForumRepository();
            Locations = new ObservableCollection<Location>(_proposalRepository.getLocationsForForum());
            LoggedInUser = user;
            this.guest1MainViewModel = guest1MainViewModel;
        }

        private ICommand _createForumCommand;

        public ICommand CreateForumCommand
        {
            get
            {
                return _createForumCommand ?? (_createForumCommand = new CommandBase(() => CreateForum(), true));
            }
        }

        private string comment;
        public string Comment { get; set; }

        private void CreateForum()
        {
            Forum forum = new Forum(_accommodationRepository.GetByLocation(SelectedLocation), DateOnly.FromDateTime(DateTime.Today), Comment, LoggedInUser);
            if (true)
            {
                _forumRepository.Save(forum);
                MessageBox.Show("Forum made");
                guest1MainViewModel.CurrentView = ForumVM;
            }
        }
    }
}
