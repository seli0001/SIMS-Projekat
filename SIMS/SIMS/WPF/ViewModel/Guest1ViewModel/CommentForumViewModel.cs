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
        private CommentRepository _commentRepositoy { get; set; }
        public User LoggedInUser { get; set; }
        public CommentForumViewModel(User loggedInUser, Forum selectedForum)
        {
            _forumRepositoy = new ForumRepository();
            SelectedForum = selectedForum;
            _commentRepositoy = new CommentRepository();
            LoggedInUser = loggedInUser;
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
        public string GuestComment
        {
            get { return comment; }
            set
            {
                if (comment != value)
                {
                    comment = value;
                    OnPropertyChanged();
                }
            }
        }


        private void CommentForum()
        {
            if(SelectedForum.IsOpen == true)
            {
                MessageBox.Show("tu je");
                Comment newComment = new Comment(LoggedInUser, GuestComment, "Guest", 0, SelectedForum);
                newComment = _commentRepositoy.Save(newComment);
                SelectedForum.Comments.Add(newComment);
            }
            else
            {
                MessageBox.Show("Forum is closed for comments");
            }
        }
    }
}
