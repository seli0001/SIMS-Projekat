using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using SIMS.Repository;
using SIMS.Domain.Model.AccommodationModel;
using SIMS.Domain.Model;

namespace SIMS.View.OwnerView
{
    /// <summary>
    /// Interaction logic for OwnerMainView.xaml
    /// </summary>
    public partial class OwnerMainView : Window
    {
        public static ObservableCollection<Accommodation> Accommodations { get; set; }

        public Accommodation SelectedAccommodation { get; set; }

        public User LoggedInUser { get; set; }

        private readonly AccommodationRepository _repository;
        public OwnerMainView(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            _repository = new AccommodationRepository();
            Accommodations = new ObservableCollection<Accommodation>(_repository.GetByUser(user));
        }

        private void ShowCreateAccommodationForm(object sender, RoutedEventArgs e)
        {
            AccommondationRegistration createAccommondationForm = new AccommondationRegistration(LoggedInUser);
            createAccommondationForm.Show();
        }

        private void ShowUpdateAccommodationForm(object sender, RoutedEventArgs e)
        {
            ShowAccommodation showAccommodation = new ShowAccommodation(SelectedAccommodation, LoggedInUser);
            showAccommodation.Show();
        }

        private void DeleteAccommodationHandler(object sender, RoutedEventArgs e)
        {
            if (SelectedAccommodation != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure?", "Delete comment",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _repository.Delete(SelectedAccommodation);
                    Accommodations.Remove(SelectedAccommodation);
                }
            }
        }
    }
}
