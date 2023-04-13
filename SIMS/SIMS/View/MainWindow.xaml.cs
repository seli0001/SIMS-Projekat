using SIMS.View.FirstGuestView;
using SIMS.Model;
using SIMS.Repository;
using SIMS.View.Guest2View;
using SIMS.View.GuideView;
using SIMS.View.OwnerView;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SIMS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly UserRepository _userRepository;
        public MainWindow()
        {
            InitializeComponent();
            _userRepository = new UserRepository();
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            User user = _userRepository.GetUserByUsername(usernameTextBox.Text);
            if (user != null)
            {
                if (user.Password == passwordBox.Password)
                {
                    if(user.Role == ROLE.Owner)
                    {
                        OwnerMainView ownerMainView = new OwnerMainView(user);
                        ownerMainView.Show();
                        Close();

                    }
                    else if (user.Role == ROLE.Guest1)
                    {
                        FirstGuestMainView firstGuestMainView = new FirstGuestMainView(user);
                        firstGuestMainView.Show();
                        Close();
                    }
                    else if (user.Role == ROLE.Guest2)
                    {
                        MainGuest2View mainGuest2View = new MainGuest2View(user.Id);
                        mainGuest2View.Show();
                        Close();
                    }
                    else if (user.Role == ROLE.Guide)
                    {
                        MainGuideView mainGuideView = new MainGuideView(user);
                        mainGuideView.Show();
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show("Wrong password!");
                }
            }
            else
            {
                MessageBox.Show("Wrong username!");
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            User user = _userRepository.GetUserByUsername("Guest1");

            FirstGuestMainView firstGuestMainView = new FirstGuestMainView(user);
            firstGuestMainView.Show();
        }
    }
}
