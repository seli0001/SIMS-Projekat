using SIMS.Domain.Model;
using SIMS.Repository;
using SIMS.Service.UseCases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SIMS.WPF.ViewModel.OwnerViewModel
{
    class ForumsViewModel
    {
        private readonly ForumRepository _forumRepository;
        public static ObservableCollection<Forum> Forums { get; set; }
        public Forum SelectedForum { get; set; }
        public User LoggedInUser { get; set; }

        MainViewModel mainViewModel;

        public ForumsViewModel(User user, MainViewModel mvm)
        {
            LoggedInUser = user;
            mainViewModel = mvm;
            _forumRepository = new ForumRepository();
            Forums = new ObservableCollection<Forum>(_forumRepository.GetAll());
        }
        #region commands

        private ICommand _forumCommand;
        public ICommand ForumCommand
        {
            get
            {
                return _forumCommand ?? (_forumCommand = new CommandBase(() => ShowStatisticsView(), true));
            }
        }

        

        #endregion

        private void ShowStatisticsView()
        {
            if (SelectedForum != null)
            {
                mainViewModel.SelectedForum = SelectedForum;
                mainViewModel.ForumCommand.Execute(null);
            }
        }

    }
}
