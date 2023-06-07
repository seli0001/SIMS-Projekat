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
    /// Interaction logic for ComplexTourRequestAnotherPart.xaml
    /// </summary>
    public partial class ComplexTourRequestAnotherPart : Window
    {
        public int userId;
        public ComplexTourRequestAnotherPart(int userId)
        {
            InitializeComponent();
            this.userId=userId;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
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
        private void Button_Click4(object sender, RoutedEventArgs e)
        {
            ComplexTourRequest complexTourRequest = new ComplexTourRequest(userId);
            complexTourRequest.Show();
            Close();
        }
    }
}
