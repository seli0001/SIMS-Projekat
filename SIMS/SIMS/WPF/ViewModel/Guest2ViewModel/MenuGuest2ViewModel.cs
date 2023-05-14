﻿using SIMS.Domain.Model;
using SIMS.Service.Services;
using SIMS.WPF.View;
using SIMS.WPF.View.Guest2View;
using SIMS.WPF.ViewModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SIMS.WPF.ViewModel.Guest2ViewModel
{
    public class MenuGuest2ViewModel : ViewModelBase, IClose
    {
        public Action Close { get; set; }
        private readonly BookedTourService _bookedTourService;
        public List<BookedTour> bookedTours;

        public int userId;

        public MenuGuest2ViewModel(int userId) {
        this.userId=userId;
            this.userId = userId;
            _bookedTourService = new BookedTourService();
            CheckNotify();
        }

        #region commands

        private ICommand _mainGuest2ViewClickCommand;
        public ICommand MainGuest2ViewClickCommand
        {
            get
            {
                return _mainGuest2ViewClickCommand ?? (_mainGuest2ViewClickCommand = new CommandBase(() => MainGuest2ViewClick(), true));
            }
        }

        private ICommand _allVouchersClickCommand;
        public ICommand AllVouchersClickCommand
        {
            get
            {
                return _allVouchersClickCommand ?? (_allVouchersClickCommand = new CommandBase(() => AllVouchersClick(), true));
            }
        }
  

        private ICommand _signOutClickCommand;
        public ICommand SignOutClickCommand
        {
            get
            {
                return _signOutClickCommand ?? (_signOutClickCommand = new CommandBase(() => SignOutClick(), true));
            }
        }

        private ICommand _toursInProgressClickCommand;
        public ICommand ToursInProgressClickCommand
        {
            get
            {
                return _toursInProgressClickCommand ?? (_toursInProgressClickCommand = new CommandBase(() => ToursInProgressClick(), true));
            }
        }


        private ICommand _finishedToursClickCommand;
        public ICommand FinishedToursClickCommand
        {
            get
            {
                return _finishedToursClickCommand ?? (_finishedToursClickCommand = new CommandBase(() => FinishedToursClick(), true));
            }
        }

        

        private ICommand _tourRequestClickCommand;
        public ICommand TourRequestClickCommand
        {
            get
            {
                return _tourRequestClickCommand ?? (_tourRequestClickCommand = new CommandBase(() => TourRequestClick(), true));
            }
        }
        

        private ICommand _toursRequestAndStatisticClickCommand;
        public ICommand ToursRequestAndStatisticClickCommand
        {
            get
            {
                return _toursRequestAndStatisticClickCommand ?? (_toursRequestAndStatisticClickCommand = new CommandBase(() => ToursRequestAndStatisticClick(), true));
            }
        }


        #endregion

        public void CheckNotify()
        {
            bookedTours = _bookedTourService.GetByUser(userId);

            foreach (BookedTour t in bookedTours)
            {
                if (t.Notify == (Notify)2 && t.Checkpoint != null)
                {
                    if (MessageBox.Show("Da li si prisni na" + t.Tour.Name + "?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        t.Notify = (Notify)1;
                        _bookedTourService.Update(t);
                    }
                    else
                    {
                        t.Notify = (Notify)0;
                        _bookedTourService.Update(t);
                    }

                }
            }
        }
        
        private void ToursRequestAndStatisticClick()
        {
            RequestedToursView requestedToursView = new RequestedToursView(userId);
            requestedToursView.Show();
            Close();
        }
        private void MainGuest2ViewClick()
        {
            MainGuest2View mainGuest2View = new MainGuest2View(userId);
            mainGuest2View.Show();
            Close();
        }

        private void TourRequestClick()
        {
            TourRequestView tourRequestView = new TourRequestView(userId);
            tourRequestView.Show();
            Close();
        }


        private void AllVouchersClick()
        {
            AllVouchersView vauchers = new AllVouchersView(userId);
            vauchers.Show();
            Close();
        }

        private void SignOutClick()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void ToursInProgressClick()
        {
            ToursInProgressView toursInProgress = new ToursInProgressView(userId);
            toursInProgress.Show();
            Close();
        }

        private void FinishedToursClick()
        {
            FinishedToursView selectFinishedTour = new FinishedToursView(userId);
            selectFinishedTour.Show();
            Close();
        }

    }
}
