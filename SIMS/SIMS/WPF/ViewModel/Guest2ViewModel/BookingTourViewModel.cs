using SIMS.Domain.Model;
using SIMS.Service.Services;
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
    public class BookingTourViewModel : ViewModelBase, IClose
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
            MessageBox.Show("Ostalo je" + freeSpace.ToString() + "Mesta");
        }

        #region commands

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

        #endregion

        private void BookedClick()
        {
            tour.NumberOfPeople = tour.NumberOfPeople + tbBox;
            _tourService.UpdateNumberOfPeople(tour, tour.Id);
            _bookedTourService.Save(tour, userId);
            MessageBox.Show("Uspesno ste rezervisali");
            Close();
        }

        private void BackToNumberFormClick()
        {
            Close();
        }
    }



}

