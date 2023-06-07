using SIMS.Domain.Model;
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

namespace SIMS.WPF.View.Guest2View
{
    /// <summary>
    /// Interaction logic for SearchHistory.xaml
    /// </summary>
    public partial class SearchHistory : Window
    {
        public List<Tour> tours;
        public int userId;
        public SearchHistory(int userid)
        {
            InitializeComponent();
            DataContext = this;
            userId = userid;
           
            tours = new List<Tour>();
            Tour t = new Tour();
            t.Language = "Srpski";
            t.Location.City = "Uzice";
            t.Location.Country = "Serbia";
            t.NumberOfPeople = 10;

            Tour t1 = new Tour();
            t1.Language = "Srpski";
            t1.Location.City = "Uzice";
            t1.Location.Country = "Serbia";
            t1.NumberOfPeople = 10;

            tours.Add(t);
            tours.Add(t1);

         
            
        }

        private void MainClick(object sender, RoutedEventArgs e)
        {
            MainGuest2View mainGuest2View = new MainGuest2View(userId);
            mainGuest2View.Show();
            Close();
        }

        private void MenuClick(object sender, RoutedEventArgs e)
        {
            MenuGuest2View menuGuest2View = new MenuGuest2View(userId);
            menuGuest2View.Show();
            Close();
        }
    }
}
