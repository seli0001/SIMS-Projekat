using SIMS.Domain.Model;
using SIMS.WPF.ViewModel;
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

namespace SIMS.WPF.View.Guest2View
{
    /// <summary>
    /// Interaction logic for ComplexTourView.xaml
    /// </summary>
    public partial class ComplexTourView : Window
    {
        public int userId { get; set; }
        public ObservableCollection<ComplexTour> tours { get; set; }

        public ComplexTourView(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            DataContext = this;
            tours = new ObservableCollection<ComplexTour>();

            ComplexTour complexTour = new ComplexTour("Novi Sad", 4, ComplexTour.Status.Waiting);
            ComplexTour complexTour1 = new ComplexTour("Beograd", 5, ComplexTour.Status.Accepted);
            tours.Add(complexTour);
            tours.Add(complexTour1);
        }


        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            MainGuest2View mainGuest2View = new MainGuest2View(userId);
            mainGuest2View.Show();
            Close();
        }

        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            MenuGuest2View mainGuest2View = new MenuGuest2View(userId);
            mainGuest2View.Show();
            Close();
        }

        #region commands

        private ICommand _selectClickCommand;
        public ICommand SelectClickCommand
        {
            get
            {
                return _selectClickCommand ?? (_selectClickCommand = new CommandBase(() => SelectClick(), true));
            }
        }
        #endregion

        private void SelectClick()
        {
            ComplexTourPartView complexTour = new ComplexTourPartView(userId);
            complexTour.Show();
            Close();
        }

    }
}
