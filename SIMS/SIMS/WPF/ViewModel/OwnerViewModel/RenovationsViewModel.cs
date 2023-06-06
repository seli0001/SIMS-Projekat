using SIMS.Domain.Model;
using SIMS.Service;
using SIMS.WPF.ViewModel.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Diagnostics;
using System.Text;

namespace SIMS.WPF.ViewModel.OwnerViewModel
{
    class RenovationsViewModel : ViewModelBase
    {
        public User LoggedInUser { get; set; }
        public static ObservableCollection<Renovation> Renovations { get; set; }

        private readonly int[] validator;

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
            validator = new int[2];
        }

        private DateTime _fromDate = DateTime.Now;
        public DateTime FromDate
        {
            get => _fromDate;
            set
            {
                if (value < ToDate || ToDate.Date == DateTime.Now.Date)
                {
                    _fromDate = value;
                    validator[0] = 1;
                }
                IsEnabled = true;
                ValidatorTest();
                OnPropertyChanged();
            }
        }

        private DateTime _toDate = DateTime.Now;

        public DateTime ToDate
        {
            get => _toDate;
            set
            {
                if (value > FromDate || FromDate.Date == DateTime.Now.Date)
                {
                    _toDate = value;
                    validator[1] = 1;
                }
                IsEnabled = true;
                ValidatorTest();
                OnPropertyChanged();
            }
        }

        private ICommand _cancelRenovationCommand;
        public ICommand CancelRenovationCommand
        {
            get
            {
                return _cancelRenovationCommand ?? (_cancelRenovationCommand = new CommandBase(() => Cancel(), true));
            }
        }

        private ICommand _generateReportCommand;
        public ICommand GenerateReportCommand
        {
            get
            {
                return _generateReportCommand ?? (_generateReportCommand = new CommandBase(() => GeneratePDFReport(), true));
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

        private void ValidatorTest()
        {
            foreach (int kon in validator)
            {
                if (kon == 0)
                {
                    IsEnabled = false;
                }
            }
        }

        private void GeneratePDFReport()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // Kreirajte novi PDF dokument
            PdfDocument document = new PdfDocument();

            // Dodajte stranicu u dokument
            PdfPage page = document.AddPage();

            // Kreirajte grafiku za crtanje na stranici
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Postavite font za naslove
            XFont titleFont = new XFont("Helvetica", 18);

            // Postavite font za podatke
            XFont dataFont = new XFont("Helvetica", 12);

            // Postavite pocetnu poziciju za crtanje
            XPoint position = new XPoint(50, 50);

            // Ispisite naslov
            gfx.DrawString("Spisak renoviranja", titleFont, XBrushes.Black, position);
            position.Y += 30;

            // Ispisite podatke iz ObservableCollection-a
            ObservableCollection<Renovation> fromToRenovations = new ObservableCollection<Renovation>(_renovationService.GetFromToDate(DateOnly.FromDateTime(FromDate), DateOnly.FromDateTime(ToDate)));
            foreach (var item in fromToRenovations)
            {
                gfx.DrawString($"Smestaj: {item.Accommodation.Name}", dataFont, XBrushes.Black, position);
                position.Y += 20;

                gfx.DrawString($"Pocetak renoviranja: {item.StartDate}", dataFont, XBrushes.Black, position);
                position.Y += 20;

                gfx.DrawString($"Kraj renoviranja: {item.EndDate}", dataFont, XBrushes.Black, position);
                position.Y += 20;

                // Ispisite ostale podatke prema potrebi

                position.Y += 30;
            }

            // Sacuvajte PDF datoteku
            string fileName = "IzvestajRenoviranja.pdf";
            document.Save(fileName);

            // Otvorite PDF datoteku
            Process.Start(new ProcessStartInfo { FileName = fileName, UseShellExecute = true });
        }

    }
}
