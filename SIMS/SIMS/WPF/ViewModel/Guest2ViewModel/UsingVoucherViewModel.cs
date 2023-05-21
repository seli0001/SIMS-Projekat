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
    public class UsingVoucherViewModel : ViewModelBase, IClose
    {
        public Action Close { get; set; }
        public Tour tour { get; set; }
        public int userId;
        private readonly VoucherService _voucherService;
        public ObservableCollection<Voucher> vouchers { get; set; }
        public Voucher selectedVoucher { get; set; }

        public UsingVoucherViewModel(int userId, Tour tour) {

            this.tour = tour;
            this.userId = userId;
            _voucherService = new VoucherService();
            vouchers = new ObservableCollection<Voucher>(_voucherService
              .GetAvailable(userId));

        }

        #region commands

        private ICommand _backClickCommand;
        public ICommand BackClickCommand
        {
            get
            {
                return _backClickCommand ?? (_backClickCommand = new CommandBase(() => BackClick(), true));
            }
        }

        private ICommand _useVoucherClick;
        public ICommand UseVoucherClickCommand
        {
            get
            {
                return _useVoucherClick ?? (_useVoucherClick = new CommandBase(() => UseVoucherClick(), true));
            }
        }

        #endregion

        private void BackClick()
        {
            NumberOfTourGuestView numberOfTour = new NumberOfTourGuestView(tour, userId, 100000);
            numberOfTour.Show();
            Close();
        }

        private void UseVoucherClick()
        {
            _voucherService.useVoucher(selectedVoucher.Id);
            NumberOfTourGuestView numberOfTour = new NumberOfTourGuestView(tour, userId, selectedVoucher.Id);
            numberOfTour.Show();
            Close();
        }
    }
}
