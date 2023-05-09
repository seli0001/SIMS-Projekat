using Microsoft.VisualBasic;
using SIMS.Domain.Model;
using SIMS.Repository;
using SIMS.WPF.ViewModel.OwnerViewModel;
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

namespace SIMS.View.OwnerView
{
    /// <summary>
    /// Interaction logic for ShowAccommodation.xaml
    /// </summary>
    public partial class ShowAccommodationView : Window
    {
        
        public ShowAccommodationView(Accommodation selectedAccommodation, User user)
        {
            InitializeComponent();
            ShowAccommodationViewModel showAccommodationViewModel = new ShowAccommodationViewModel(user);
            DataContext = showAccommodationViewModel;
        }
       
    }
}
