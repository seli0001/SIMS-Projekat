using SIMS.Domain.Model;
using SIMS.Service.Services;
using SIMS.WPF.View;
using SIMS.WPF.ViewModel.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SIMS.WPF.ViewModel.Guest2ViewModel
{
    internal class ComplexTourRequestViewModel : ViewModelBase, IClose, INotifyPropertyChanged
    { 
        public int userId {get;set;}
        public Action Close { get; set; }

        public List<TourRequest> tourRequests { get; set; }
        public ComplexTourService complexTourService { get; set; }



        public ComplexTourRequestViewModel(int userId)
        {
            this.userId = userId;
            tourRequests= new List<TourRequest>();
            complexTourService=new  ComplexTourService();

        }



        #region commands
       
             private ICommand _mainGuest2ViewClickCommand;
        public ICommand MainGuest2ViewClickCommand
        {
            get
            {
                return _mainGuest2ViewClickCommand ?? (_mainGuest2ViewClickCommand = new CommandBase(() => MainGuest2Click(), true));
            }
        }

        private ICommand _addTourInComplexCommand;
        public ICommand AddTourInComplexCommand
        {
            get
            {
                return _addTourInComplexCommand ?? (_addTourInComplexCommand = new CommandBase(() => AddTourInComplexClick(), true));
            }
        }

        private ICommand _createComplexTourCommand;
        public ICommand CreateComplexTourCommand
        {
            get
            {
                return _createComplexTourCommand ?? (_createComplexTourCommand = new CommandBase(() => CreateComplexTourClick(), true));
            }
        }

        

      


        private string _tbName;
        public string tbName
        {
            get => _tbName;
            set
            {
                if (value != _tbName)
                {
                    _tbName = value;
                    OnPropertyChanged();
                }
            }
        }


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

        private DateTime _tbStartDate = new DateTime();
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



        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        

        private void MainGuest2Click()
        {
            MainGuest2View mainGuest2View = new MainGuest2View(userId);
            mainGuest2View.Show();
            Close();
        }
        //int id, Location location, string description, string language, int maxNumberOfPeople, DateTime startDate, DateTime endDate, RequestStatus status,int userId
        private void AddTourInComplexClick()
        {
            Location location = new Location(tbCountry,tbCity);
            TourRequest tourRequest = new TourRequest(-1,location,tbDescription,tbLanguage,int.Parse(tbGuestNumber),tbStartDate,tbEndDate,RequestStatus.Waiting,userId); 
            tourRequests.Add(tourRequest);
            tbCountry="";
            tbCity = "";
            tbDescription = "";
            tbLanguage = "";
            tbGuestNumber = "";
            tbStartDate = new DateTime();
            tbEndDate = new DateTime();
        }

        private void CreateComplexTourClick()
        {
            complexTourService.add(tbName,tourRequests, userId);
            tourRequests=new List<TourRequest>();
        }
    }
}
