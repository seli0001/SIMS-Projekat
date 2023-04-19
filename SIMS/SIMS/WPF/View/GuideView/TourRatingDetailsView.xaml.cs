using SIMS.Domain.Model;
using SIMS.Service.Services;
using SIMS.WPF.ViewModel;
using SIMS.WPF.ViewModel.GuideViewModel;
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

namespace SIMS.WPF.View
{
    /// <summary>
    /// Interaction logic for TourRatingDetailsView.xaml
    /// </summary>
    public partial class TourRatingDetailsView : Window
    {
        private readonly TourRatingService _tourRatingService;
        private TourRating rating;
        public TourRatingDetailsView(TourRating tourRating)
        {
            InitializeComponent();
            TourRatringDetailsViewModel tourRatringDetailsViewModel = new TourRatringDetailsViewModel(tourRating);
            DataContext = tourRatringDetailsViewModel;

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
