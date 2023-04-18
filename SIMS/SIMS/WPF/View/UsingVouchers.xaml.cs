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
    /// Interaction logic for UsingVouchers.xaml
    /// </summary>
    public partial class UsingVouchers : Window
    {
        public Tour tour { get; set; }
        public int userId;
        private readonly VoucherService _voucherService;
        public ObservableCollection<Voucher> vouchers { get; set; }
        public Voucher selectedVoucher { get; set; }
        public UsingVouchers(int userId, Tour tour)
        {
            InitializeComponent();
            DataContext = this;
            this.tour = tour;
            this.userId = userId;
            _voucherService= new VoucherService();
            vouchers = new ObservableCollection<Voucher>(_voucherService
              .GetAvailable(userId));
        }
        
        private void BackClick(object sender, RoutedEventArgs e)
        {
            NumberOfTourGuestView numberOfTour = new NumberOfTourGuestView(tour, userId, 100000);
            numberOfTour.Show();
            Close();
        }

        private void MainGues2ViewClick(object sender, RoutedEventArgs e)
        {
            MainGuest2View mainGuest2View = new MainGuest2View(userId);
            mainGuest2View.Show();
            Close();
        }

        private void UseVoucherClick(object sender, RoutedEventArgs e)
        {
            _voucherService.useVoucher(selectedVoucher.Id);
            NumberOfTourGuestView numberOfTour = new NumberOfTourGuestView(tour, userId, selectedVoucher.Id);
            numberOfTour.Show();
            Close();
        }
    }
}
