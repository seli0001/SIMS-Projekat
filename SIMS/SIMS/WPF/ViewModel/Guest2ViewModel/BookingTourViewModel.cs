using SIMS.Domain.Model;
using SIMS.Service.Services;
using SIMS.WPF.View;
using SIMS.WPF.ViewModel.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SIMS.WPF.ViewModel.Guest2ViewModel
{
    public class BookingTourViewModel : ViewModelBase, IClose, INotifyPropertyChanged
    {
        private readonly BookedTourService _bookedTourService;
        private readonly TourService _tourService;
        public Tour tour { get; set; }
        public int freeSpace;
        public int userId;
        public int tbBox;
        public Action Close { get; set; }

        public BookingTourViewModel (int freespace, Tour tourinput, int userid, int textbox)
        {
            tour = tourinput;
            userId = userid;
            tbBox = textbox;

            _tourService = new TourService();

            _bookedTourService = new BookedTourService();
            freeSpace = freespace;
            FreeSpace = freespace;
           // MessageBox.Show("Ostalo je" + freeSpace.ToString() + "Mesta");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        #region commands

        private int _freeSpace;
        public int FreeSpace
        {
            get => _freeSpace;
            set
            {
                if (value != _freeSpace)
                {
                    _freeSpace = value;
                    OnPropertyChanged();
                }
            }
        }

        private ICommand _bookedClickCommand;
        public ICommand BookedClickCommand
        {
            get
            {
                return _bookedClickCommand ?? (_bookedClickCommand = new CommandBase(() => BookedClick(), true));
            }
        }

        private ICommand _backToNumberFormClickCommand;
        public ICommand BackToNumberFormClickCommand
        {
            get
            {
                return _backToNumberFormClickCommand ?? (_backToNumberFormClickCommand = new CommandBase(() => BackToNumberFormClick(), true));
            }
        }

        
        

            private ICommand _menuClickCommand;
        public ICommand MenuClickCommand
        {
            get
            {
                return _menuClickCommand ?? (_menuClickCommand = new CommandBase(() => MenuClick(), true));
            }
        }

        private ICommand _mainGuest2ViewClickCommand;
        public ICommand MainGuest2ViewClickCommand
        {
            get
            {
                return _mainGuest2ViewClickCommand ?? (_mainGuest2ViewClickCommand = new CommandBase(() => BackToMainClick(), true));
            }
        }


        #endregion

        private void MenuClick()
        {
            MenuGuest2View menuGuest2 = new MenuGuest2View(userId);
            menuGuest2.Show();
            Close();
        }

        private void BackToMainClick()
        {
  
            MainGuest2View mainGuest2View = new MainGuest2View(userId);
            mainGuest2View.Show();

            Close();
        }

        private void BookedClick()
        {
            tour.NumberOfPeople = tour.NumberOfPeople + tbBox;

            _tourService.UpdateNumberOfPeople(tour, tour.Id);
            _bookedTourService.Save(tour, userId,tbBox);
            MessageBox.Show("Uspesno ste rezervisali");
            Close();
        }

        private void BackToNumberFormClick()
        {
            NumberOfTourGuestView numberOfTourGuestView = new NumberOfTourGuestView(tour, userId, 100000);
            numberOfTourGuestView.Show();
            Close();


            
        }
    }



}

