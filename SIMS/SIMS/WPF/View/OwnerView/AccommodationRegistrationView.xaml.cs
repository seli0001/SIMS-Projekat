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
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using SIMS.Repository;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Type = SIMS.Domain.Model.Type;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using Image = SIMS.Domain.Model.Image;
using SIMS.WPF.ViewModel.OwnerViewModel;
using SIMS.Domain.Model;
using SIMS.WPF.ViewModel;

namespace SIMS.View.OwnerView
{
    /// <summary>
    /// Interaction logic for AccommondationRegistration.xaml
    /// </summary>
    public partial class AccommondationRegistration : Window
    {
      
        public AccommondationRegistration(User user)
        {
            InitializeComponent();
            Title = "Create new accommondation";
            AccommodationRegistrationViewModel accommodationRegistrationViewModel = new AccommodationRegistrationViewModel(user);
            DataContext = accommodationRegistrationViewModel;
            
            if(DataContext is IClose vm)
            {
                vm.Close += () =>
                {
                    this.Close();
                };
            }
        }


    }
}
