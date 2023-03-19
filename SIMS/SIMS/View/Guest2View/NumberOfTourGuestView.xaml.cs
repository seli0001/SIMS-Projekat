using SIMS.Model.Guide;
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

namespace SIMS.View.Guest2View
{
    /// <summary>
    /// Interaction logic for NumberOfTourGuestView.xaml
    /// </summary>
    public partial class NumberOfTourGuestView : Window
    {
        public Tour tour { get; set; }
        public int userId { get; set; }



        public NumberOfTourGuestView(Tour selectedTour, int userid)
        {
            InitializeComponent();
            tour = selectedTour;
            userId = userid;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (tour.MaxNumberOfPeople < Convert.ToInt32(tbNumber.Text) && (tour.MaxNumberOfPeople - tour.NumberOfPeople) < Convert.ToInt32(tbNumber.Text))
            {
                MessageBox.Show("Nema mesta!");
            }
            else
            {
                int freeSpace = tour.MaxNumberOfPeople - Convert.ToInt32(tbNumber.Text);
                BookingTourView bookingTourView = new BookingTourView(freeSpace, tour, userId, Convert.ToInt32(tbNumber.Text));
                bookingTourView.ShowDialog();



            }
        }
    }
}
