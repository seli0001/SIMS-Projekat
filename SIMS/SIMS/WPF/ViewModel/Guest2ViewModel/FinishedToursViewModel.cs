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
    public class FinishedToursViewModel : ViewModelBase, IClose
    {
        public ObservableCollection<BookedTour> tours { get; set; }
        private readonly BookedTourService _bookedTourService;
        public BookedTour selectedTour { get; set; }
        public int userId;
        public Action Close { get; set; }

        public FinishedToursViewModel(int userId)
        {
            this.userId = userId;
            selectedTour = new BookedTour();
            _bookedTourService = new BookedTourService();
            tours = new ObservableCollection<BookedTour>(_bookedTourService.GetUserFinished(userId));
        }

        #region commands

        private ICommand _rateTourClickCommand;
        public ICommand RateTourClickCommand
        {
            get
            {
                return _rateTourClickCommand ?? (_rateTourClickCommand = new CommandBase(() => RateTourClick(), true));
            }
        }

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

        private void RateTourClick()
        {
            RateTourView rateTour = new RateTourView(selectedTour, userId);
            rateTour.Show();
            Close();
        }

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
