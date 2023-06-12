using SIMS.Domain.Model;
using SIMS.Repository;
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
    internal class ForumViewModel
    {
        public User LoggedInUser { get; set; }
        public ForumRepository _forumRepository;
        public UserRepository _userRepository;
        public AccommodationRepository _accommodationRepository;
        public Forum SelectedForum { get; set; }

        private CreateForumViewModel CreateForumVM;
        private CommentForumViewModel CommentForumVM;

        public static ObservableCollection<Forum> Forums { get; set; }


        Guest1MainViewModel guest1MainViewModel;
        public ForumViewModel(User user, Guest1MainViewModel guest1MainViewModel)
        {
            _forumRepository = new ForumRepository();
            _userRepository = new UserRepository();
            _accommodationRepository = new AccommodationRepository();
            Forums = new ObservableCollection<Forum>(_forumRepository.GetAll());
            LoggedInUser = user;
            this.guest1MainViewModel = guest1MainViewModel;
            PopulateUsers();
            PopulateAccommodations();
        }

        private void PopulateUsers()
        {
            foreach (var forum in Forums)
            {
                var user = _userRepository.GetById(forum.ForumOwner.Id);
                forum.ForumOwner = user;
            }
        }

        private void PopulateAccommodations()
        {
            foreach (var forum in Forums)
            {
                var acc = _accommodationRepository.GetById(forum.Accommodation.Id);
                forum.Accommodation = acc;
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
                        CreateForumVM = new CreateForumViewModel(LoggedInUser, guest1MainViewModel);
                        guest1MainViewModel.CurrentView = CreateForumVM;
                    }, true));
            }
        }

        private ICommand _commentForumCommand;

        public ICommand CommentForumCommand
        {
            get
            {
                return _commentForumCommand ?? (_commentForumCommand = new CommandBase(
                    () =>
                    {
                        CommentForumVM = new CommentForumViewModel(LoggedInUser, SelectedForum);
                        guest1MainViewModel.CurrentView = CommentForumVM;
                    }, true));
            }
        }

        private ICommand _closeForumCommand;

        public ICommand CloseForumCommand
        {
            get
            {
                return _closeForumCommand ?? (_closeForumCommand = new CommandBase(() => CloseForum(), true));
            }
        }

        private void CloseForum()
        {
            _forumRepository.CloseForum(SelectedForum);
        }
    }
}
