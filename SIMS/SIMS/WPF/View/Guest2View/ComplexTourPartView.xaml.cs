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
    /// Interaction logic for ComplexTourPartView.xaml
    /// </summary>
    public partial class ComplexTourPartView : Window
    {
        public int UserId { get; set; }
        public List<Ethape> Ethapes { get; set; }

        public ComplexTourPartView(int userId)
        {
            InitializeComponent();
            UserId = userId;
            DataContext = this;
            Ethapes = new List<Ethape>();
            Ethape ethape = new Ethape("Novi sad", "petrovaradin", Ethape.Status.Accepted);
            Ethape ethape1 = new Ethape("Novi sad", "liman 1", Ethape.Status.Accepted);
            Ethapes.Add(ethape);
            Ethapes.Add(ethape1);
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            MainGuest2View mainGuest2View = new MainGuest2View(UserId);
            mainGuest2View.Show();
            Close();
        }

        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            MenuGuest2View mainGuest2View = new MenuGuest2View(UserId);
            mainGuest2View.Show();
            Close();
        }


    }
}
