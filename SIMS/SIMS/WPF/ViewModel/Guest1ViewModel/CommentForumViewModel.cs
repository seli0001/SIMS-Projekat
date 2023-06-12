using SIMS.Domain.Model;
using SIMS.Repository;
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
    public class CommentForumViewModel : INotifyPropertyChanged
    {
        public Forum forum { get; set; }
        public List<string> Comments { get; set; }
        private ForumRepository _forumRepositoy { get; set; }
        public CommentForumViewModel(User loggedInUser, Forum selectedForum)
        {
            _forumRepositoy = new ForumRepository();
            forum = selectedForum;
            Comments = new List<string>();
            Comments = forum.Comments;
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
            if(forum.IsOpen == true)
            {
                MessageBox.Show("tu je");
                _forumRepositoy.AddComment(Comment, forum);
                OnPropertyChanged(nameof(Comments));
            }
            else
            {
                MessageBox.Show("Forum is closed for comments");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
