using SIMS.Domain.Model;
using SIMS.Service.Services;
using SIMS.WPF.View;
using SIMS.WPF.ViewModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SIMS.WPF.ViewModel.Guest2ViewModel
{
    public  class RateTourViewModel : ViewModelBase, IClose, INotifyPropertyChanged
    {
        private readonly BookedTourService _bookedTourService;
        private readonly TourRatingService _tourRatingService;
        public BookedTour bookedTour;
        public ObservableCollection<string> images;
        public int userId;

        public Action Close { get; set; }

        public RateTourViewModel(BookedTour bookedTour, int userId) {
            this.bookedTour = bookedTour;
            this.userId = userId;
            _bookedTourService = new BookedTourService();
            _tourRatingService = new TourRatingService();
            images = new ObservableCollection<string>();

        }

        #region commands

        private string _guideKnown;
        public string GuideKnown
        {
            get => _guideKnown;
            set
            {
                if (value != _guideKnown)
                {
                    _guideKnown = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _guideLanguage;

        public string GuideLanguage
        {
            get => _guideLanguage;
            set
            {
                if (value != _guideLanguage)
                {
                    _guideLanguage = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _tourReview;

        public string TourReview
        {
            get => _tourReview;
            set
            {
                if (value != _tourReview)
                {
                    _tourReview = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _comment;

        public string Commet
        {
            get => _comment;
            set
            {
                if (value != _comment)
                {
                    _comment = value;
                    OnPropertyChanged();
                }
            }
        }

        public string _tbImage;
        public string tbImage
        {
            get => _tbImage;
            set
            {
                if (value != _tbImage)
                {
                    _tbImage = value;
                    OnPropertyChanged();
                }
            }
        }

        private ICommand _rateTourClickCommand;
        public ICommand RateTourClickCommand
        {
            get
            {
                return _rateTourClickCommand ?? (_rateTourClickCommand = new CommandBase(() => RateTourClick(), true));
            }
        }

        private ICommand _backToMainGuest2ViewClickCommand;
        public ICommand BackToMainGuest2ViewClickCommand
        {
            get
            {
                return _backToMainGuest2ViewClickCommand ?? (_backToMainGuest2ViewClickCommand = new CommandBase(() => BackToMainGuest2ViewClick(), true));
            }
        }

        private ICommand _backToMenuClickCommand;
        public ICommand BackToMenuClickCommand
        {
            get
            {
                return _backToMenuClickCommand ?? (_backToMenuClickCommand = new CommandBase(() => BackToMenuClick(), true));
            }
        }

        private ICommand _addImageClickCommand;
        public ICommand AddImageClickCommand
        {
            get
            {
                return _addImageClickCommand ?? (_addImageClickCommand = new CommandBase(() => AddImageClick(), true));
            }
        }

        #endregion


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RateTourClick()
        {
            bookedTour.Review = true;
            _bookedTourService.Update(bookedTour);
            _tourRatingService.Save(bookedTour, userId, int.Parse(GuideKnown), int.Parse(GuideLanguage),int.Parse(TourReview), _comment, images.ToList());
            MessageBox.Show("Uspesno ste ocenili!");
            MainGuest2View mainGuest2View = new MainGuest2View(userId);
            mainGuest2View.Show();
            Close();

        }

        private void BackToMainGuest2ViewClick()
        {
            MainGuest2View mainGuest2View = new MainGuest2View(userId);
            mainGuest2View.Show();
            Close();
        }

        private void BackToMenuClick()
        {
            MenuGuest2 menu = new MenuGuest2(userId);
            menu.Show();
            Close();
        }

        private void AddImageClick()
        {
            images.Add(tbImage);
        }

    }
}
