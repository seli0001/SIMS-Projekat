using SIMS.Domain.Model;
using SIMS.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media;
using System.Windows.Input;
using SIMS.WPF.ViewModel.ViewModel;
using SIMS.WPF.View;

namespace SIMS.WPF.ViewModel.GuideViewModel
{
    class TourRatringDetailsViewModel : ViewModelBase, IClose
    {
        private readonly TourRatingService _tourRatingService;
        private TourRating _rating;
        public String GuestName { get; set; }
        public String Knowledge { get; set; }
        public String Language { get; set; }
        public String TourRating { get; set; }
        public String Checkpoint { get; set; }
        public String Comment { get; set; }
        public bool IsEnabled { get; set; }
        public TourRatringDetailsViewModel(TourRating tourRating)
        {
            _tourRatingService = new TourRatingService();
            GuestName = "Ime gosta: " + tourRating.bookedTour.User.Username;
            Knowledge = "Znanje: " + tourRating.GuideKnown.ToString();
            Language = "Jezik: " + tourRating.GuideLanguage.ToString();
            TourRating = "Zanimljivost: " + tourRating.TourReview.ToString();
            Checkpoint = "Tacka kada se gost prikljucio: " + tourRating.bookedTour.Checkpoint.Name;
            Comment = tourRating.Comment;
            IsEnabled = tourRating.Valid;
            _rating = tourRating;
        }

        private ICommand _reportCommand;
        public ICommand ReportCommand
        {
            get
            {
                return _reportCommand ?? (_reportCommand = new CommandBase(() => Report(), true));
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

        public Action Close { get; set; }

        public void Report()
        {
            if (MessageBox.Show("Da li ste sigurni da zelite da prijavite recenziju?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _tourRatingService.ReportRating(_rating.Id);
                Close();
            }
        }

        
    }
}
