using SIMS.Domain.Model;
using SIMS.Service;
using SIMS.WPF.ViewModel.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SIMS.WPF.ViewModel.OwnerViewModel
{
    class RenovationsViewModel : ViewModelBase
    {
        public User LoggedInUser { get; set; }
        public static ObservableCollection<Renovation> Renovations { get; set; }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }
        private Renovation _selectedRenovation;
        public Renovation SelectedRenovation { 
            get
            {
                return _selectedRenovation;
            }
                
            set
            {
                if (value != null)
                {

                    DateOnly today = DateOnly.FromDateTime(DateTime.Today);

                    if (CompareDates(today, value.EndDate)) IsEnabled = true;
                    else IsEnabled = false;

                    _selectedRenovation = value;
                }
            }
        }

        private bool CompareDates(DateOnly date11, DateOnly date22)
        {
            DateTime date1 = date11.ToDateTime(new TimeOnly());
            DateTime date2 = date22.ToDateTime(new TimeOnly());
            if (date2 < date1) return false;
            TimeSpan difference = date2 - date1;
            int days = difference.Days;
            if (days < 5) return false;
            else return true;
        }

        private readonly RenovationService _renovationService;
        public RenovationsViewModel()
        {


        }

        public RenovationsViewModel(User user, MainViewModel mvm)
        {
            _renovationService = new RenovationService();
            LoggedInUser = user;
            Renovations = new ObservableCollection<Renovation>(_renovationService.GetByOwnerId(user.Id));
        }

        private ICommand _cancelRenovationCommand;
        public ICommand CancelRenovationCommand
        {
            get
            {
                return _cancelRenovationCommand ?? (_cancelRenovationCommand = new CommandBase(() => Cancel(), true));
            }
        }

        private void Cancel()
        {
            if(SelectedRenovation != null)
            {
                _renovationService.Delete(SelectedRenovation);
                Renovations.Remove(SelectedRenovation);
            }
        }
        
    }
}
