using SIMS.Repository;
using SIMS.View.FirstGuestView;
using SIMS.View.GuideView;
using System.Windows;
using System.Windows.Input;
using SIMS.Domain.Model;
using SIMS.WPF.View;
using SIMS.View.OwnerView;
using SIMS.WPF.ViewModel.ViewModel;
using SIMS.WPF.View.OwnerView;
using SIMS.WPF.ViewModel.GuideViewModel;
using SIMS.WPF.View.Guest1View;
using System.ComponentModel;

namespace SIMS.WPF.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly UserRepository _userRepository;

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
            }
        }

        #region commands

        private ICommand _signInCommand;
        public ICommand SignInCommand
        {
            get
            {
                return _signInCommand ?? (_signInCommand = new CommandBase(() => SignIn(), true));
            }
        }

        private ICommand _zubaClickCommand;
        public ICommand ZubaClickCommand
        {
            get
            {
                return _zubaClickCommand ?? (_zubaClickCommand = new CommandBase(() => ZubaClick(), true));
            }
        }

        private ICommand _slobaClickCommand;
        public ICommand SlobaClickCommand
        {
            get
            {
                return _slobaClickCommand ?? (_slobaClickCommand = new CommandBase(() => SlobaClick(), true));
            }
        }

        #endregion
        public MainWindowViewModel()
        {
            _userRepository = new UserRepository();
        }

        private void SignIn()
        {
            User user = _userRepository.GetUserByUsername(Username);
            if (user != null)
            {
                if (user.Password == Password)
                {
                    if (user.Role == ROLE.Owner)
                    {
                        OwnerMain ownerMainView = new OwnerMain(user);
                        ownerMainView.Show();

                    }
                    else if (user.Role == ROLE.Guest1)
                    {
                        Guest1Main guest1Main = new Guest1Main(user);
                        guest1Main.Show();
                    }
                    else if (user.Role == ROLE.Guest2)
                    {
                        MainGuest2View mainGuest2View = new MainGuest2View(user.Id);
                        mainGuest2View.Show();
                    }
                    else if (user.Role == ROLE.Guide)
                    {
                        if (user.Active)
                        {
                            MainGuideView mainGuideView = new MainGuideView(user);
                            mainGuideView.Closing += ChildWindow_Closing;
                            mainGuideView.Show();
                        }
                        else
                        {
                            MessageBox.Show("Korisnik je dao otkaz");
                        }
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

        private void ChildWindow_Closing(object sender, CancelEventArgs e)
        {
            // Dispose of the timer when the child window is closing
            if (sender is MainGuideView mainGuideView)
            {
                mainGuideView.Closing -= ChildWindow_Closing;
                mainGuideView.DisposeTimer();
            }
        }

        private void ZubaClick()
        {
            User user = _userRepository.GetUserByUsername("Guest1");

            //FirstGuestMainView firstGuestMainView = new FirstGuestMainView(user);
            //firstGuestMainView.Show();
            Guest1Main guest1Main = new Guest1Main(user);
            guest1Main.Show();
        }

        private void SlobaClick()
        {
            User user = _userRepository.GetUserByUsername("Sloba");

            OwnerMain ownerMainView = new OwnerMain(user);
            ownerMainView.Show();
        }
    }
}
