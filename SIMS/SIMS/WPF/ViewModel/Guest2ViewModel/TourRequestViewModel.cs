using SIMS.Domain.Model;
using SIMS.Model;
using SIMS.Service.Services;
using SIMS.Service.UseCases;
using SIMS.WPF.View;
using SIMS.WPF.ViewModel.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SIMS.WPF.ViewModel.Guest2ViewModel
{
    public  class TourRequestViewModel : ViewModelBase, IClose, INotifyPropertyChanged
    {

        private readonly TourRequestService _tourRequestService;
        private readonly LocationService _locationService;
        public Action Close { get; set; }
        public int userId;

        public TourRequestViewModel(int userId)
        {   
            _tourRequestService=new TourRequestService();
            _locationService = new LocationService();
            this.userId = userId;
        }

        #region commands



        private string _tbCity;
        public string tbCity
        {
            get => _tbCity;
            set
            {
                if (value != _tbCity)
                {
                    _tbCity = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _tbDescription;
        public string tbDescription
        {
            get => _tbDescription;
            set
            {
                if (value != _tbDescription)
                {
                    _tbDescription = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _tbLanguage;
        public string tbLanguage
        {
            get => _tbLanguage;
            set
            {
                if (value != _tbLanguage)
                {
                    _tbLanguage = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _tbGuestNumber;
        public string tbGuestNumber
        {
            get => _tbGuestNumber;
            set
            {
                if (value != _tbGuestNumber)
                {
                    _tbGuestNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _tbCountry;
        public string tbCountry
        {
            get => _tbCountry;
            set
            {
                if (value != _tbCountry)
                {
                    _tbCountry = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _tbStartDate= new DateTime();
        public DateTime tbStartDate
        {
            get => _tbStartDate;
            set
            {
                if (value != _tbStartDate)
                {
                    _tbStartDate = value;
                    OnPropertyChanged(nameof(tbStartDate));
                }
            }
        }

        private DateTime _tbEndDate = new DateTime();
        public DateTime tbEndDate
        {
            get => _tbEndDate;
            set
            {
                if (value != _tbEndDate)
                {
                    _tbEndDate = value;
                    OnPropertyChanged(nameof(tbEndDate));
                }
            }
        }

        private ICommand _requesTourClickCommand;
        public ICommand RequesTourClickCommand
        {
            get
            {
                return _requesTourClickCommand ?? (_requesTourClickCommand = new CommandBase(() => RequesTourClick(), true));
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

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void BackToMainGuest2ViewClick()
        {
            MainGuest2View mainGuest2View = new MainGuest2View(userId);
            mainGuest2View.Show();
            Close();
        }

        private void BackToMenuClick()
        {
            MenuGuest2View menuGuest2View = new MenuGuest2View(userId);
            menuGuest2View.Show();
            Close();
        }

        private void RequesTourClick()
        {
           

             string input = tbGuestNumber;
            int number;

            if (int.TryParse(input, out number))
            {
                Location location = new Location(tbCountry, tbCity);

                int id = _locationService.IsExists(location);

                if (id == -1)
                {
                    location = _locationService.Save(location);
                    _tourRequestService.Save(location, tbDescription, tbLanguage, int.Parse(tbGuestNumber), tbStartDate, tbEndDate, RequestStatus.Waiting, userId);
                    MessageBox.Show("Uspesno!");
                }
                else
                {
                    location.Id = id;
                    _tourRequestService.Save(location, tbDescription, tbLanguage, int.Parse(tbGuestNumber), tbStartDate, tbEndDate, RequestStatus.Waiting, userId);
                    MessageBox.Show("Uspesno!");
                }
            }
            else if(tbEndDate==null || tbCity==null ||tbCountry==null || tbDescription==null || tbStartDate==null|| tbGuestNumber==null)
            {
                MessageBox.Show("Morate uneti sva polja!");
            }
            else
            {
                MessageBox.Show("Broj gostiju mora biti unet kao broj!");
            }
            
            
        }





    }
}
