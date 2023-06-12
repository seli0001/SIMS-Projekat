﻿using SIMS.Domain.Model;
using SIMS.Repository;
using SIMS.Service.UseCases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SIMS.WPF.ViewModel.Guest1ViewModel
{
    internal class ForumViewModel
    {
        public User LoggedInUser { get; set; }
        public ForumRepository _forumRepository;
        public UserRepository _userRepository;
        public AccommodationRepository _accommodationRepository;
        public LocationService _locationService;

        public Forum SelectedForum { get; set; }

        private CreateForumViewModel CreateForumVM;

        public static ObservableCollection<Forum> Forums { get; set; }


        Guest1MainViewModel guest1MainViewModel;
        public ForumViewModel(User user, Guest1MainViewModel guest1MainViewModel)
        {
            _forumRepository = new ForumRepository();
            _userRepository = new UserRepository();
            _accommodationRepository = new AccommodationRepository();
            _locationService = new LocationService();
            Forums = new ObservableCollection<Forum>(_forumRepository.GetAll());
            LoggedInUser = user;
            this.guest1MainViewModel = guest1MainViewModel;
            PopulateUsers();
            PopulateLocations();
        }

        private void PopulateUsers()
        {
            foreach (var forum in Forums)
            {
                var user = _userRepository.GetById(forum.ForumOwner.Id);
                forum.ForumOwner = user;
            }
        }

        private void PopulateLocations()
        {
            foreach (var forum in Forums)
            {
                forum.Location = _locationService.GetById(forum.Location.Id);
            }
        }


        private ICommand _createForumCommand;

        public ICommand CreateForumCommand
        {
            get
            {
                return _createForumCommand ?? (_createForumCommand = new CommandBase(
                    () =>
                    {
                        CreateForumVM = new CreateForumViewModel(LoggedInUser);
                        guest1MainViewModel.CurrentView = CreateForumVM;
                    }, true));
            }
        }
    }
}
