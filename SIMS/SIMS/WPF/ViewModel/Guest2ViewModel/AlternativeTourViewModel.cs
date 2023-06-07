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
using System.Windows;
using System.Windows.Input;

namespace SIMS.WPF.ViewModel.Guest2ViewModel
{
    public class AlternativeTourViewModel : ViewModelBase, IClose
    {
        private readonly TourService _tourService;
        public ObservableCollection<Tour> tours { get; set; }
        public Tour selectedTour { get; set; }
        public int userId;
        public Action Close { get; set; }

        public AlternativeTourViewModel(Tour tour, int userId)
        {
            this.userId = userId;

            _tourService = new TourService();

            tours = new ObservableCollection<Tour>(_tourService.GetAlternative(tour));
        }

        #region commands

        private ICommand _backToMainFormCommand;
        public ICommand BackToMainFormCommand
        {
            get
            {
                return _backToMainFormCommand ?? (_backToMainFormCommand = new CommandBase(() => BackToMainForm(), true));
            }
        }

        private ICommand _numberOfTourClickCommand;
        public ICommand NumberOfTourClickCommand
        {
            get
            {
                return _numberOfTourClickCommand ?? (_numberOfTourClickCommand = new CommandBase(() => NumberOfTourClick(), true));
            }
        }
        #endregion

        private void BackToMainForm()
        {
            MainGuest2View mainGuest2View = new MainGuest2View(userId);
            mainGuest2View.Show();
            Close();
        }

        private void NumberOfTourClick()
        { if (selectedTour != null)
            {
                NumberOfTourGuestView numberOfTourGuestView = new NumberOfTourGuestView(selectedTour, userId, 100000);
                numberOfTourGuestView.Show();
                Close();
            }
        else
            {
                MessageBox.Show("Morate selektovati turu!");
            }
        }
    }
}
