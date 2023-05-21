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
    public  class AllVouchersViewModel : ViewModelBase,IClose
    {
        public int userId;
        public ObservableCollection<Voucher> vouchers { get; set; }
        private readonly VoucherService _voucherService;
        public Action Close { get; set; }

        public AllVouchersViewModel(int userId)
        {
            this.userId = userId;
            _voucherService = new VoucherService();
            vouchers = new ObservableCollection<Voucher>(_voucherService.GetAll(userId));
        }

        #region commands

        private ICommand _menuClickCommand;
        public ICommand MenuClickCommand
        {
            get
            {
                return _menuClickCommand ?? (_menuClickCommand = new CommandBase(() => MenuClick(), true));
            }
        }

        private ICommand _mainGuest2ViewClick;
        public ICommand MainGuest2ViewClickCommand
        {
            get
            {
                return _mainGuest2ViewClick ?? (_mainGuest2ViewClick = new CommandBase(() => MainGuest2ViewClick(), true));
            }
        }

        #endregion

        private void MainGuest2ViewClick()
        {
            MainGuest2View mainGuest2View = new MainGuest2View(userId);
            mainGuest2View.Show();
            Close();
        }

        private void MenuClick()
        {
            MenuGuest2View menuGuest2 = new MenuGuest2View(userId);
            menuGuest2.Show();
            Close();
           
        }
    }
}
