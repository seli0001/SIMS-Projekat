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
using System.Collections.ObjectModel;
using SIMS.Domain.Model;
using SIMS.Repository;
using SIMS.Service.Services;
using SIMS.WPF.View;
using SIMS.WPF.View.GuideView;
using SIMS.WPF.ViewModel.GuideViewModel;
using SIMS.WPF.ViewModel;

namespace SIMS.View.GuideView
{
    /// <summary>
    /// Interaction logic for MainGuideView.xaml
    /// </summary>
    public partial class MainGuideView : Window
    {
        private MainGuideViewModel mainGuideViewModel;
           public MainGuideView(User user) 
           {
                InitializeComponent();
                mainGuideViewModel = new MainGuideViewModel(user);
                DataContext = mainGuideViewModel;
                if (DataContext is IClose vm)
                {
                    vm.Close += () =>
                    {
                        this.Close();
                    };
                }
        }
        public void DisposeTimer()
        {
            // Dispose of the timer when the child window is closed
            mainGuideViewModel.Dispose();
        }
    }
}
