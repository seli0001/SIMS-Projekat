using SIMS.Domain.Model;
using SIMS.WPF.ViewModel.OwnerViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace SIMS.View.OwnerView
{
    /// <summary>
    /// Interaction logic for GuestRatingView.xaml
    /// </summary>
    public partial class GuestRatingView : Window
    {
       public GuestRatingView(Reservation selectedReservation, User user)
        {
            InitializeComponent();
            Title = "Rate Guest";
            GuestRatingViewModel guestRatingViewModel = new GuestRatingViewModel(selectedReservation, user);
            DataContext = guestRatingViewModel;
        }
    }
}
