using SIMS.Domain.Model;
using SIMS.Service.Services;
using SIMS.WPF.View.Guest2View;
using SIMS.WPF.ViewModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SIMS.WPF.ViewModel.Guest2ViewModel
{
    internal class ComplexToursViewModel : ViewModelBase, IClose
    {
        public int userId { get; set; }
        public ObservableCollection<ComplexTourRequest> tours { get; set; }
        private ComplexTourService _complexTourService { get; set; }
        public ComplexTourRequest selectedComplexTour { get; set; }
        public Action Close { get; set; }
        public ComplexToursViewModel(int userId)
        {
            _complexTourService = new ComplexTourService();
            this.userId = userId;
            tours = new ObservableCollection<ComplexTourRequest>(_complexTourService.GetByUser(userId));
            
            
        }

        #region commands

        private ICommand _toursClickCommand;
        public ICommand ToursClickCommand
        {
            get
            {
                return _toursClickCommand ?? (_toursClickCommand = new CommandBase(() => ToursClick(), true));
            }
        }



        #endregion


        private void ToursClick()
        {
            ToursInComplexView toursInComplexView = new ToursInComplexView(userId, selectedComplexTour);
            toursInComplexView.Show();
            Close();
        }

       

    }
}
