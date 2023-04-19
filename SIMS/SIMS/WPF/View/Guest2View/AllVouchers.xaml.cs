﻿using SIMS.Domain.Model;
using SIMS.Repository;
using SIMS.Service.Services;
using SIMS.WPF.ViewModel;
using SIMS.WPF.ViewModel.Guest2ViewModel;
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

namespace SIMS.WPF.View
{
    /// <summary>
    /// Interaction logic for AllVouchers.xaml
    /// </summary>
    public partial class AllVouchers : Window
    {
        
        public AllVouchers(int userId)
        {
            InitializeComponent();
            AllVouchersViewModel allVouchersViewModel = new AllVouchersViewModel(userId);
            DataContext = allVouchersViewModel;

           if (DataContext is IClose vm)
            {
                vm.Close += () =>
                {
                    this.Close();
                };
            }
        

         //   DataContext = this;
          
        }

     
    }
}
