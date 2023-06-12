using SIMS.Domain.Model;
using SIMS.Repository;
using SIMS.WPF.ViewModel.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SIMS.WPF.ViewModel.Guest1ViewModel
{
    public class CommentForumViewModel : ViewModelBase
    {
        public Forum SelectedForum { get; set; }
        private ForumRepository _forumRepositoy { get; set; }
        public CommentForumViewModel(User loggedInUser, Forum selectedForum)
        {
            _forumRepositoy = new ForumRepository();
            SelectedForum = selectedForum;
        }

        private ICommand _commentForumCommand;

        public ICommand CommentForumCommand
        {
            get
            {
                return _commentForumCommand ?? (_commentForumCommand = new CommandBase(() => CommentForum(), true));
            }
        }

        private string comment;
        public string Comment
        {
            get { return comment; }
            set
            {
                if (comment != value)
                {
                    comment = value;
                    OnPropertyChanged(nameof(Comment));
                }
            }
        }


        private void CommentForum()
        {
            if(SelectedForum.IsOpen == true)
            {
                MessageBox.Show("tu je");
               // _forumRepositoy.AddComment(Comment, SelectedForum);
               // OnPropertyChanged(nameof(Comments));
            }
            else
            {
                MessageBox.Show("Forum is closed for comments");
            }
        }
    }
}
