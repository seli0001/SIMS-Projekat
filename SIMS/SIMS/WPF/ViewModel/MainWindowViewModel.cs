using SIMS.Repository;
using SIMS.View.FirstGuestView;
using SIMS.View.Guest2View;
using SIMS.View.GuideView;
using SIMS.View.OwnerView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using SIMS.WPF.ViewModel.OwnerViewModel;
using SIMS.Domain.Model;

namespace SIMS.WPF.ViewModel
{
    public class MainWindowViewModel : OwnerViewModelBase
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
                        OwnerMainView ownerMainView = new OwnerMainView(user);
                        ownerMainView.Show();

                    }
                    else if (user.Role == ROLE.Guest1)
                    {
                        FirstGuestMainView firstGuestMainView = new FirstGuestMainView(user);
                        firstGuestMainView.Show();
                    }
                    else if (user.Role == ROLE.Guest2)
                    {
                        MainGuest2View mainGuest2View = new MainGuest2View(user.Id);
                        mainGuest2View.Show();
                    }
                    else if (user.Role == ROLE.Guide)
                    {
                        MainGuideView mainGuideView = new MainGuideView(user);
                        mainGuideView.Show();
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

        private void ZubaClick()
        {
            User user = _userRepository.GetUserByUsername("Guest1");

            FirstGuestMainView firstGuestMainView = new FirstGuestMainView(user);
            firstGuestMainView.Show();
        }

        private void SlobaClick()
        {
            User user = _userRepository.GetUserByUsername("Sloba");

            OwnerMainView ownerMainView = new OwnerMainView(user);
            ownerMainView.Show();
        }
    }
}
