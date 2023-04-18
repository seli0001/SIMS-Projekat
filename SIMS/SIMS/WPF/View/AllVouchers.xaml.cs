using SIMS.Domain.Model;
using SIMS.Repository;
using SIMS.Service.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SIMS.WPF.View
{
    /// <summary>
    /// Interaction logic for AllVouchers.xaml
    /// </summary>
    public partial class AllVouchers : Window
    {
        public int userId;
        public ObservableCollection<Voucher> vouchers { get; set; }
        private readonly VoucherService _voucherService;
        public AllVouchers(int userId)
        {
            InitializeComponent();
            DataContext = this;
            this.userId = userId;
            _voucherService= new VoucherService();
            vouchers = new ObservableCollection<Voucher>(_voucherService.GetAll(userId));
        }

        private void MainGuest2ViewClick(object sender, RoutedEventArgs e)
        {
            MainGuest2View mainGuest2View = new MainGuest2View(userId);
            mainGuest2View.Show();
            Close();
        }

        private void MenuClick(object sender, RoutedEventArgs e)
        {
            MenuGuest2 menuGuest2 = new MenuGuest2(userId);
            menuGuest2.Show();
            Close();
        }
    }
}
