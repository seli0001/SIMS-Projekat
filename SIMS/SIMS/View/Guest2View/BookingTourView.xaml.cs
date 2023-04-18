using SIMS.Domain.Model;
using SIMS.Repository.Guest2Repository;
using SIMS.Repository.GuideRepository;
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
    /// Interaction logic for BookingTourView.xaml
    /// </summary>
    public partial class BookingTourView : Window
    {
        private readonly BookedTourRepository _bookedToursRepository;
        private readonly TourRepository _toursRepository;
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
            _toursRepository = new TourRepository();
            _bookedToursRepository = new BookedTourRepository();
            freeSpace = freespace;
            MessageBox.Show("Ostalo je" + freeSpace.ToString() + "Mesta");
        }

        private void BookedClick(object sender, RoutedEventArgs e)
        {
            tour.NumberOfPeople = tour.NumberOfPeople + tbBox;
            _toursRepository.UpdateNumberOfPeople(tour, tour.Id);
            _bookedToursRepository.Save(tour, userId);
            MessageBox.Show("Uspesno ste rezervisali");
            Close();
        }

        private void BackToNumberFormClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
