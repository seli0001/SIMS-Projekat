﻿using SIMS.Model;
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
using System.Xml.Linq;
using SIMS.Model.AccommodationModel;
using SIMS.Repository;
using System.Threading;
using System.Windows.Threading;
using SIMS.WPF.ViewModel.OwnerViewModel;

namespace SIMS.View.OwnerView
{
    /// <summary>
    /// Interaction logic for OwnerMainView.xaml
    /// </summary>
    public partial class OwnerMainView : Window
    {
        public OwnerMainView(User user)
        {
            InitializeComponent();
            OwnerMainViewModel ownerMainViewModel = new OwnerMainViewModel(user);
            DataContext = ownerMainViewModel;
        }

    }
}
