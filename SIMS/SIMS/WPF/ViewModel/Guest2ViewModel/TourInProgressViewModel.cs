using SIMS.Domain.Model;
using SIMS.Service.Services;
using SIMS.WPF.View;
using SIMS.WPF.ViewModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SIMS.WPF.ViewModel.Guest2ViewModel
{
    public class TourInProgressViewModel : ViewModelBase, IClose
    {
        public BookedTour bookedTour { get; set; }
        private CheckpointService _checkpointService { get; set; }

        public ObservableCollection<Checkpoint> checkpoints { get; set; }
        public int userId { get; set; }
        public Action Close { get; set; }

        public TourInProgressViewModel(BookedTour bookedTour, int userId)
        {
            this.bookedTour = bookedTour;

            this.userId = userId;
            _checkpointService = new CheckpointService();
            checkpoints = new ObservableCollection<Checkpoint>(_checkpointService.GetAllByTourId(bookedTour.TourId));
        }

        #region commands

        private ICommand _backToMainGuest2ViewClickCommand;
        public ICommand BackToMainGuest2ViewClickCommand
        {
            get
            {
                return _backToMainGuest2ViewClickCommand ?? (_backToMainGuest2ViewClickCommand = new CommandBase(() => BackToMainGuest2ViewClick(), true));
            }
        }

        private ICommand _backToMenuClickCommand;
        public ICommand BackToMenuClickCommand
        {
            get
            {
                return _backToMenuClickCommand ?? (_backToMenuClickCommand = new CommandBase(() => BackToMenuClick(), true));
            }
        }

        #endregion


        private void BackToMainGuest2ViewClick()
        {
            MainGuest2View mainGuest2View = new MainGuest2View(userId);
            mainGuest2View.Show();
            Close();
        }

        private void BackToMenuClick()
        {
            MenuGuest2View menu = new MenuGuest2View(userId);
            menu.Show();
            Close();
        }

    }
}
