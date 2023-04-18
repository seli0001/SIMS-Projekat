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
    /// Interaction logic for BookingTourView.xaml
    /// </summary>
    public partial class BookingTourView : Window
    {

        private readonly BookedTourService _bookedTourService;
        private readonly TourService _tourService;
        public Tour tour { get; set; }
        public int freeSpace;
        public int userId;
        public int tbBox;


        public BookingTourView(int freespace, Tour tourinput, int userid, int textbox)
        {
            InitializeComponent();
            tour = tourinput;
            userId = userid;
            tbBox = textbox;

            _tourService = new TourService();

            _bookedTourService = new BookedTourService();
            freeSpace = freespace;
            MessageBox.Show("Ostalo je" + freeSpace.ToString() + "Mesta");
        }

        private void BookedClick(object sender, RoutedEventArgs e)
        {
            tour.NumberOfPeople = tour.NumberOfPeople + tbBox;
            _tourService.UpdateNumberOfPeople(tour, tour.Id);
            _bookedTourService.Save(tour, userId);
            MessageBox.Show("Uspesno ste rezervisali");
            Close();
        }

        private void BackToNumberFormClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
