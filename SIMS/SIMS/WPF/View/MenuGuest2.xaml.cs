using SIMS.Domain.Model;
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
    /// Interaction logic for MenuGuest2.xaml
    /// </summary>
    public partial class MenuGuest2 : Window
    {
        private readonly BookedTourService _bookedTourService;
        public List<BookedTour> bookedTours;

        public int userId;
        public MenuGuest2(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            _bookedTourService = new BookedTourService();
            CheckNotify();
        }

        public void CheckNotify()
        {
            bookedTours = _bookedTourService.GetByUser(userId);

            foreach (BookedTour t in bookedTours)
            {
                if (t.Notify == (Notify)2 && t.Checkpoint != null)
                {
                    if (MessageBox.Show("Da li si prisni na" + t.Tour.Name + "?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        t.Notify = (Notify)1;
                        _bookedTourService.Update(t);
                    }
                    else
                    {
                        t.Notify = (Notify)0;
                        _bookedTourService.Update(t);
                    }

                }
            }
        }

        private void MainGuest2ViewClick(object sender, RoutedEventArgs e)
        {
            MainGuest2View mainGuest2View = new MainGuest2View(userId);
            mainGuest2View.Show();
            Close();
        }

        private void AllVouchersClick(object sender, RoutedEventArgs e)
        {
            AllVouchers vauchers = new AllVouchers(userId);
            vauchers.Show();
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void ToursInProgressClick(object sender, RoutedEventArgs e)
        {
            ToursInProgressView toursInProgress = new ToursInProgressView(userId);
            toursInProgress.Show();
            Close();
        }

        private void FinishedToursClick(object sender, RoutedEventArgs e)
        {
            FinishedTours selectFinishedTour = new FinishedTours(userId);
            selectFinishedTour.Show();
            Close();
        }
    }
}
