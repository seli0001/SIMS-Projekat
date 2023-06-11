using SIMS.Domain.Model;
using SIMS.Service.Services;
using SIMS.Service.UseCases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Diagnostics;

namespace SIMS.WPF.ViewModel.Guest1ViewModel
{
    public class BookingViewModel
    {
        public Accommodation SelectedAccommodation { get; set; }
        string filePath = "../../../../SIMS/PDF/pdf.pdf";

        public User LoggedInUser { get; set; }

        private readonly AccommodationService _repository;
        private readonly ReservationService _reservationService;

        public BookingViewModel(User user, Accommodation selectedAccommodation) 
        {
            _repository = new AccommodationService();
            _reservationService = new ReservationService();
            SelectedAccommodation = selectedAccommodation;
            LoggedInUser = user;
        }

        private DateTime _fromDate = DateTime.Now;

        public DateTime FromDate
        {
            get => _fromDate;
            set
            {
                _fromDate = value;
            }
        }

        private DateTime _toDate = DateTime.Now;

        public DateTime ToDate
        {
            get => _toDate;
            set
            {
                _toDate = value;
            }
        }

        private int _timeOfStay = 1;
        public int TimeOfStay
        {
            get => _timeOfStay;
            set
            {
                if (value != _timeOfStay)
                {
                    _timeOfStay = value;
                }

            }
        }

        private int _numberOfGuests = 1;
        public int NumberOfGuests
        {
            get => _numberOfGuests;
            set
            {
                if (value != _numberOfGuests)
                {
                    _numberOfGuests = value;
                }

            }
        }

        private ICommand _saveBookingCommand;
        public ICommand SaveBookingCommand
        {
            get
            {
                return _saveBookingCommand ?? (_saveBookingCommand = new CommandBase(() => SaveBooking(), true));
            }
        }

        private void SaveBooking()
        {
            if(FromDate < DateTime.Today || ToDate < FromDate)
            {
                ValidationDatess();
                MessageBox.Show("Check if the dates are put in correctly ");

                return;
            }
            else if (TimeOfStay <= SelectedAccommodation.MinBookingDays)
            {
                MessageBox.Show("Minimum number of days is " + SelectedAccommodation.MinBookingDays);
                ValidationTime();

                return;
            }
            else if (NumberOfGuests > SelectedAccommodation.MaxGuestsNumber-1)
            {
                MessageBox.Show("Maximum number of guests is " + SelectedAccommodation.MaxGuestsNumber);

                ValidationGuests();

                return;
            }
            Reservation reservation = new Reservation(DateOnly.FromDateTime(FromDate), DateOnly.FromDateTime(FromDate.AddDays(TimeOfStay)), SelectedAccommodation, TimeOfStay, NumberOfGuests, LoggedInUser);
            if (_reservationService.AvailableAccommodation(reservation))
            {
                _reservationService.Save(reservation);

                MakePDF(reservation);
            }
        }

        private bool _validationDates;
        public bool ValidationDates
        {
            get => _validationDates;
            set
            {
                if (value != _validationDates)
                {
                    _validationDates = value;
                }

            }
        }
        private bool _validationTime;
        public bool ValidationTimes
        {
            get => _validationTime;
            set
            {
                if (value != _validationTime)
                {
                    _validationTime = value;
                }

            }
        }
        private bool _validationGuest;
        public bool ValidationGuest
        {
            get => _validationGuest;
            set
            {
                if (value != _validationGuest)
                {
                    _validationGuest = value;
                }

            }
        }

        public void ValidationDatess()
        {
            ValidationDates = false;
        }
        public void ValidationTime()
        {
            ValidationTimes =  false;
        }
        public void ValidationGuests()
        {
            ValidationGuest = false;
        }

        //public void GeneratePdf(string bookingDetails)
        //{
        //    string filePath = "BookingDetails.pdf";

        //    // Create a new PDF document
        //    Document document = new Document();

        //    // Create a PDF writer to write the document to a file
        //    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

        //    // Open the document
        //    document.Open();

        //    // Create a header table with user information
        //    PdfPTable headerTable = new PdfPTable(2);
        //    headerTable.DefaultCell.Border = Rectangle.NO_BORDER;
        //    headerTable.WidthPercentage = 100;

        //    // Add user information to the header table
        //    headerTable.AddCell(new Phrase("User: " + LoggedInUser.Username));
        //    headerTable.AddCell(new Phrase("Age: " + LoggedInUser.Age));
        //    headerTable.AddCell(new Phrase("Date: " + DateTime.Now.ToString()));

        //    // Add the header table to the document
        //    document.Add(headerTable);

        //    // Add a line break
        //    document.Add(new Paragraph("\n"));

        //    // Add booking details
        //    Paragraph bookingParagraph = new Paragraph(bookingDetails);
        //    document.Add(bookingParagraph);

        //    // Close the document
        //    document.Close();

        //    MessageBox.Show("PDF generated successfully. File saved at: " + filePath);
        //}



        public void MakePDF(Reservation reservation)
        {
            // Register code pages encoding provider
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // Create a new PDF document
            PdfSharp.Pdf.PdfDocument document = new PdfSharp.Pdf.PdfDocument();

            // Add a page to the document
            PdfSharp.Pdf.PdfPage page = document.AddPage();

            // Create graphics for drawing on the page
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Set font for titles
            XFont titleFont = new XFont("Helvetica", 18);

            // Set font for data
            XFont dataFont = new XFont("Helvetica", 12);

            // Set initial position for drawing
            XPoint position = new XPoint(50, 50);
            XFont headerFont = new XFont("Arial", 14, XFontStyle.Bold);
            XPoint headerPosition = new XPoint(50, 30);
            // Draw header information
            gfx.DrawString(LoggedInUser.Username, headerFont, XBrushes.Black, headerPosition);
            headerPosition.Y += 20;
            gfx.DrawString(DateTime.Now.ToString(), headerFont, XBrushes.Black, headerPosition);
            headerPosition.Y += 30;
            position.Y = headerPosition.Y;            // Draw title
            gfx.DrawString("Rezervacija" + reservation.Accommodation.Name + " uspesno napravljena,\n i zakazana od " + reservation.FromDate + " do " + reservation.ToDate  , titleFont, XBrushes.Black, position);
            position.Y += 30;


            string fileName = "pdf.pdf";
            // Save the PDF document to a file
            document.Save(fileName);

            // Open the generated PDF file
            Process.Start(new ProcessStartInfo { FileName = fileName, UseShellExecute = true });

        }


    }
}
