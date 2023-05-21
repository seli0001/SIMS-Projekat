using SIMS.Domain.Model;
using SIMS.Service.Services;
using SIMS.WPF.View;
using SIMS.WPF.ViewModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SIMS.WPF.ViewModel.GuideViewModel
{
    class TourReviewViewModel : ViewModelBase, IClose
    {
        private readonly TourRatingService _tourRatingService;
        public TourRating SelectedTourRating { get; set; }
        public ObservableCollection<TourRating> TourRatings { get; set; }
        public TourReviewViewModel(int tourId)
        {
            _tourRatingService = new TourRatingService();
            SelectedTourRating = new TourRating();
            TourRatings = new ObservableCollection<TourRating>(_tourRatingService.GetAllByTourId(tourId));
        }

        private ICommand _detailsCommand;
        public ICommand DetailsCommand
        {
            get
            {
                return _detailsCommand ?? (_detailsCommand = new CommandBase(() => RatingDeatails(), true));
            }
        }

        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new CommandBase(() => Close(), true));
            }
        }

        public void RatingDeatails()
        {
            TourRatingDetailsView tourRatingDetailsView = new TourRatingDetailsView(SelectedTourRating);
            tourRatingDetailsView.ShowDialog();
        }
        public Action Close { get; set; }
    } 
}
