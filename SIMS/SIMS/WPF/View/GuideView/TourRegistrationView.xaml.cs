using Microsoft.Win32;
using SIMS.Domain.Model;
using SIMS.Repository;
using SIMS.WPF.ViewModel;
using SIMS.WPF.ViewModel.GuideViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
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
using Image = SIMS.Domain.Model.ImageTour;

namespace SIMS.View.GuideView;

/// <summary>
/// Interaction logic for TourRegistrationView.xaml
/// </summary>
public partial class TourRegistrationView : Window
{

    public TourRegistrationView(User guide, Location location = null, string language = "")
    {
        InitializeComponent();
        TourRegistrationViewModel tourRegistrationViewModel = new TourRegistrationViewModel(guide, location, language);
        DataContext = tourRegistrationViewModel;
        if (DataContext is IClose vm)
        {
            vm.Close += () =>
            {
                this.Close();
            };
        }
    }

}