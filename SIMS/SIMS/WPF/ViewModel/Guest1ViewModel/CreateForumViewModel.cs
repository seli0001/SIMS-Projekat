using SIMS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.WPF.ViewModel.Guest1ViewModel
{
    public class CreateForumViewModel
    {
        public User LoggedInUser { get; set; }
        public CreateForumViewModel(User user)
        {
            LoggedInUser = user;
        }
    }
}
