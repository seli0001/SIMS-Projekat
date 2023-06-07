using SIMS.Domain.Model;
using SIMS.Service.Services;
using SIMS.WPF.View;
using SIMS.WPF.View.Guest2View;
using SIMS.WPF.ViewModel.ViewModel;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Printing;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Windows.Markup;
using Xceed.Wpf.AvalonDock.Properties;
using System.Diagnostics;
using System.Data;
using Org.BouncyCastle.Asn1.Utilities;
using Org.BouncyCastle.Math;
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Threading;
using System.Globalization;
using System.Resources;


namespace SIMS.WPF.ViewModel.Guest2ViewModel
{
    public class MenuGuest2ViewModel : ViewModelBase, IClose, INotifyPropertyChanged
    {
        public Action Close { get; set; }
        private readonly BookedTourService _bookedTourService;
        public List<BookedTour> bookedTours;

        public int userId;

        public MenuGuest2ViewModel(int userId) {
        this.userId=userId;
            this.userId = userId;
            _bookedTourService = new BookedTourService();
           
            CheckNotify();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region commands

        

            private ICommand _complexTourRequests;
        public ICommand ComplexTourRequests
        {
            get
            {
                return _complexTourRequests ?? (_complexTourRequests = new CommandBase(() => ComplexTourRequestsClick(), true));
            }
        }

        private ICommand _complexTourClickCommand;
        public ICommand ComplexTourClickCommand
        {
            get
            {
                return _complexTourClickCommand ?? (_complexTourClickCommand = new CommandBase(() => ComplexTourClick(), true));
            }
        }


        private ICommand _mainGuest2ViewClickCommand;
        public ICommand MainGuest2ViewClickCommand
        {
            get
            {
                return _mainGuest2ViewClickCommand ?? (_mainGuest2ViewClickCommand = new CommandBase(() => MainGuest2ViewClick(), true));
            }
        }

        private ICommand _allVouchersClickCommand;
        public ICommand AllVouchersClickCommand
        {
            get
            {
                return _allVouchersClickCommand ?? (_allVouchersClickCommand = new CommandBase(() => AllVouchersClick(), true));
            }
        }
  

        private ICommand _signOutClickCommand;
        public ICommand SignOutClickCommand
        {
            get
            {
                return _signOutClickCommand ?? (_signOutClickCommand = new CommandBase(() => SignOutClick(), true));
            }
        }

        private ICommand _toursInProgressClickCommand;
        public ICommand ToursInProgressClickCommand
        {
            get
            {
                return _toursInProgressClickCommand ?? (_toursInProgressClickCommand = new CommandBase(() => ToursInProgressClick(), true));
            }
        }


        private ICommand _finishedToursClickCommand;
        public ICommand FinishedToursClickCommand
        {
            get
            {
                return _finishedToursClickCommand ?? (_finishedToursClickCommand = new CommandBase(() => FinishedToursClick(), true));
            }
        }

        

        private ICommand _tourRequestClickCommand;
        public ICommand TourRequestClickCommand
        {
            get
            {
                return _tourRequestClickCommand ?? (_tourRequestClickCommand = new CommandBase(() => TourRequestClick(), true));
            }
        }
        

        private ICommand _toursRequestAndStatisticClickCommand;
        public ICommand ToursRequestAndStatisticClickCommand
        {
            get
            {
                return _toursRequestAndStatisticClickCommand ?? (_toursRequestAndStatisticClickCommand = new CommandBase(() => ToursRequestAndStatisticClick(), true));
            }
        }

        private ICommand _generateReportClickCommand;
        public ICommand GenerateReportClickCommand
        {
            get
            {
                return _generateReportClickCommand ?? (_generateReportClickCommand = new CommandBase(() => GenerateReportClick(), true));
            }
        }



        private bool _isDarkTheme;
        public bool IsDarkTheme
        {
            get { return _isDarkTheme; }
            set
            {
                _isDarkTheme = value;
                OnPropertyChanged(IsDarkTheme.ToString());
                ChangeTheme();
            }
        }

        private ICommand _themeClickCommand;
        public ICommand ThemeClickCommand
        {
            get
            {
                return _themeClickCommand ?? (_themeClickCommand = new CommandBase(() => ChangeThemeClick(), true));
            }
        }

        

        private ICommand _changeLanguageClickCommand;
        public ICommand ChangeLanguageClickCommand
        {
            get
            {
                return _changeLanguageClickCommand ?? (_changeLanguageClickCommand = new CommandBase(() => ChangeLanguageClick(), true));
            }
        }

        #endregion
        private void ComplexTourRequestsClick()
        {
            ComplexTourView complexTour = new ComplexTourView(userId);
            complexTour.Show();
            Close();
        }
        private void ChangeLanguageClick()
        {
            

        }

        public void CheckNotify()
        {
            bookedTours = _bookedTourService.GetByUser(userId);

            foreach (BookedTour t in bookedTours)
            {
                if (t.Notify != null && t.Notify == Notify.NoAnswer && t.Checkpoint != null && t.Tour != null && t.Tour.Status == TourStatus.STARTED)
                {
                    if (MessageBox.Show("Da li si prisni na " + t.Tour.Name + "?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        t.Notify = Notify.Reject;
                        _bookedTourService.Update(t);
                    }
                    else
                    {

                        t.Notify = Notify.Accepted;
                        _bookedTourService.Update(t);
                    }
                }
            }
        }

        private void ToursRequestAndStatisticClick()
        {
            RequestedToursView requestedToursView = new RequestedToursView(userId);
            requestedToursView.Show();
            Close();
        }
        private void MainGuest2ViewClick()
        {
            MainGuest2View mainGuest2View = new MainGuest2View(userId);
            mainGuest2View.Show();
            Close();
        }

        private void ComplexTourClick()
        {
            ComplexTourRequest complexTourRequest = new ComplexTourRequest(userId);
            complexTourRequest.Show();
            Close();

        }

        private void TourRequestClick()
        {
            TourRequestView tourRequestView = new TourRequestView(userId);
            tourRequestView.Show();
            Close();
        }


        private void AllVouchersClick()
        {
            AllVouchersView vauchers = new AllVouchersView(userId);
            vauchers.Show();
            Close();
        }

        private void SignOutClick()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void ToursInProgressClick()
        {
            ToursInProgressView toursInProgress = new ToursInProgressView(userId);
            toursInProgress.Show();
            Close();
        }

        private void FinishedToursClick()
        {
            FinishedToursView selectFinishedTour = new FinishedToursView(userId);
            selectFinishedTour.Show();
            Close();
        }

        private void ChangeThemeClick()
        {
            IsDarkTheme = !IsDarkTheme;
        }


        private void ChangeTheme()
        {
            if (IsDarkTheme)
            {
                // Promeni na tamnu temu
                Application.Current.Resources["AppBackground"] = new SolidColorBrush(Color.FromRgb(45, 45, 48));
                Application.Current.Resources["AppForeground"] = new SolidColorBrush(Colors.White);
                Application.Current.Resources["ButtonBackground"] = new SolidColorBrush(Colors.DarkBlue);
                Application.Current.Resources["ToolBar"] = new SolidColorBrush(Colors.DarkBlue);
                Application.Current.Resources["List"] = new SolidColorBrush(Colors.DarkGray);
                Application.Current.Resources["MainButton"] = new SolidColorBrush(Colors.DarkMagenta);
                Application.Current.Resources["Text"] = new SolidColorBrush(Colors.White);
            }
            else
            {
                // Promeni na svetlu temu
                Application.Current.Resources["AppBackground"] = new SolidColorBrush(Colors.AliceBlue);
                Application.Current.Resources["AppForeground"] = new SolidColorBrush(Colors.Black);
                Application.Current.Resources["ButtonBackground"] = new SolidColorBrush(Colors.White);
                Application.Current.Resources["ToolBar"] = new SolidColorBrush(Colors.LightBlue);
                Application.Current.Resources["List"] = new SolidColorBrush(Colors.White);
                Application.Current.Resources["MainButton"] = new SolidColorBrush(Colors.DodgerBlue);
                Application.Current.Resources["Text"] = new SolidColorBrush(Colors.Black);
            }
        }


        private void GenerateReportClick()
        {  
            string putanjaIzvestaja = @"../../../../SIMS/Reports/izvestaj"+ DateTime.Now.ToString("hh_mm_ss").ToString() + ".pdf";

            // Kreirajte novi PDF dokument iz iText7 biblioteke
            iText.Kernel.Pdf.PdfDocument pdfDokument = new iText.Kernel.Pdf.PdfDocument(new iText.Kernel.Pdf.PdfWriter(putanjaIzvestaja));
            iText.Layout.Document dokument = new iText.Layout.Document(pdfDokument);


            iText.Layout.Element.Paragraph naslovParagraf = new iText.Layout.Element.Paragraph("Prisustvo na turama")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFontSize(18)
                .SetBold();
            dokument.Add(naslovParagraf);

            iText.Layout.Element.Paragraph informacijeParagraf = new iText.Layout.Element.Paragraph()
                .Add("Novi sad Tura\n")
                .Add(" Star: 23.04.2001, End Date: 15.05.2002\n")
                .Add("Beograd Tura\n")
                .Add(" Star: 23.04.2001, End Date: 15.05.2002");
            dokument.Add(informacijeParagraf);


            dokument.Close();

        }

    }
}
