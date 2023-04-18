using SIMS.Domain.Model.Guide;
using SIMS.Service.Services;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for NumberOfTourGuestView.xaml
    /// </summary>
    public partial class NumberOfTourGuestView : Window
    {
        public Tour tour { get; set; }
        public int userId { get; set; }
        public int voucherId { get; set; }

        private VoucherService _voucherService;

        public NumberOfTourGuestView(Tour selectedtour, int userid, int voucherId)
        {
            InitializeComponent();
            tour = selectedtour;
            userId = userid;
            this.voucherId = voucherId;
            _voucherService = new VoucherService();
        }

        private void BookingTourViewClick(object sender, RoutedEventArgs e)
        {
            if (tour.MaxNumberOfPeople < Convert.ToInt32(tbNumber.Text) || (tour.MaxNumberOfPeople - tour.NumberOfPeople) < Convert.ToInt32(tbNumber.Text))
            {
                MessageBox.Show("Nema mesta,evo alternativa!");
                AlternativeTourView alternativeTourView = new AlternativeTourView(tour, userId);
                alternativeTourView.Show();
                Close();
            }
            else
            {
                int freeSpace = tour.MaxNumberOfPeople - Convert.ToInt32(tbNumber.Text) - tour.NumberOfPeople;
                BookingTourView bookingTourView = new BookingTourView(freeSpace, tour, userId, Convert.ToInt32(tbNumber.Text));
                bookingTourView.ShowDialog();



            }
        }

        private void BackToMainClick(object sender, RoutedEventArgs e)
        {
            //voucherServis -> ponisti Voucher -> false
            _voucherService.DontUseIt(voucherId);
            MainGuest2View mainGuest2View = new MainGuest2View(userId);
            mainGuest2View.Show();

            Close();
        }

        private void UsingVoucher(object sender, RoutedEventArgs e)
        {
            UsingVouchers usingVaucher = new UsingVouchers(userId, tour);
            usingVaucher.ShowDialog();
        }
    }
}
