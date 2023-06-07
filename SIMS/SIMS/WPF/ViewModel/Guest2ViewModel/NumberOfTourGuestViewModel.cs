﻿using SIMS.Domain.Model;
using SIMS.Service.Services;
using SIMS.WPF.View;
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
    public  class NumberOfTourGuestViewModel : ViewModelBase, IClose
    {
        public Action Close { get; set; }
        public Tour tour { get; set; }
        public int userId { get; set; }
        public int voucherId { get; set; }

        private VoucherService _voucherService;

        public NumberOfTourGuestViewModel(Tour selectedtour, int userid, int voucherId)
        {
            tour = selectedtour;
            userId = userid;
            this.voucherId = voucherId;
            _voucherService = new VoucherService();
        }

        #region commands
        private string _tbNumber;
        public string tbNumber
        {
            get => _tbNumber;
            set
            {
                if (value != _tbNumber)
                {
                    _tbNumber = value;
                    OnPropertyChanged();
                }
            }
        }


        private ICommand _backToMainClickCommand;
        public ICommand BackToMainClickCommand
        {
            get
            {
                return _backToMainClickCommand ?? (_backToMainClickCommand = new CommandBase(() => BackToMainClick(), true));
            }
        }

        private ICommand _usingVoucherCommand;
        public ICommand UsingVoucherCommand
        {
            get
            {
                return _usingVoucherCommand ?? (_usingVoucherCommand = new CommandBase(() => UsingVoucher(), true));
            }
        }

        private ICommand _bookingTourViewClickCommand;
        public ICommand BookingTourViewClickCommand
        {
            get
            {
                return _bookingTourViewClickCommand ?? (_bookingTourViewClickCommand = new CommandBase(() => BookingTourViewClick(), true));
            }
        }

       

            private ICommand _backToMenuClickCommand;
        public ICommand BackToMenuClickCommand
        {
            get
            {
                return _backToMenuClickCommand ?? (_backToMenuClickCommand = new CommandBase(() => MenuClick(), true));
            }
        }

        #endregion

        private void MenuClick()
        {
            MenuGuest2View menuGuest2 = new MenuGuest2View(userId);
            menuGuest2.Show();
            Close();
        }



        private void BookingTourViewClick()
        {
            string input = tbNumber;
            int number;

            if (int.TryParse(input, out number))
            {
                if (tour.MaxNumberOfPeople < Convert.ToInt32(tbNumber) || (tour.MaxNumberOfPeople - tour.NumberOfPeople) < Convert.ToInt32(tbNumber))
                {
                    //MessageBox.Show("Nema mesta,evo alternativa!");
                    AlternativeTourView alternativeTourView = new AlternativeTourView(tour, userId);
                    alternativeTourView.Show();
                    Close();
                }
                else
                {
                    int freeSpace = tour.MaxNumberOfPeople - Convert.ToInt32(tbNumber) - tour.NumberOfPeople;
                    BookingTourView bookingTourView = new BookingTourView(freeSpace, tour, userId, Convert.ToInt32(tbNumber));
                    bookingTourView.Show();
                    Close();


                }
            }
            else
            {
                // Vrednost u tbNumber nije validan broj
                MessageBox.Show("Morate  uneti broj!");
            }



           
        }
        private void BackToMainClick()
        {
            _voucherService.DontUseIt(voucherId);
            MainGuest2View mainGuest2View = new MainGuest2View(userId);
            mainGuest2View.Show();

            Close();
        }

        private void UsingVoucher()
        {
            UsingVouchersView usingVaucher = new UsingVouchersView(userId, tour);
            usingVaucher.Show();
            Close();
        }


    }
}
