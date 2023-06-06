﻿using SIMS.Domain.Model;
using SIMS.WPF.ViewModel.OwnerViewModel;
using SIMS.WPF.ViewModel;
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
using SIMS.WPF.ViewModel.Guest1ViewModel;

namespace SIMS.WPF.View.Guest1View
{
    /// <summary>
    /// Interaction logic for Guest1Main.xaml
    /// </summary>
    public partial class Guest1Main : Window
    {
        public Guest1Main()
        {

        }
        public Guest1Main(User user)
        {
            InitializeComponent();
            Guest1MainViewModel guestMainVM = new Guest1MainViewModel(user);
            DataContext = guestMainVM;
            if (DataContext is IClose vm)
            {
                vm.Close += () =>
                {
                    this.Close();
                };
            }
        }
    }
}
