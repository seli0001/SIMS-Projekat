using SIMS.Model.Guide;
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

namespace SIMS.View.GuideView
{
    /// <summary>
    /// Interaction logic for MainGuideView.xaml
    /// </summary>
    public partial class MainGuideView : Window
    {
        public MainGuideView()
        {
            InitializeComponent();
        }

        private void RegisterTour(object sender, RoutedEventArgs e)
        {
            TourRegistrationView tourRegistrationView = new TourRegistrationView();
            tourRegistrationView.ShowDialog();
        }
    }
}
