using SIMS.Domain.Model;
using SIMS.Repository;
using SIMS.Service.Services;
using SIMS.Service.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SIMS.WPF.ViewModel.OwnerViewModel
{
    class ForumViewModel : MainViewModel
    {
        private readonly ReservationService _reservationService;
        private readonly AccommodationService _accommodationService;

        private readonly CommentRepository _commentRepository;
        private readonly OwnerCommentRepository _ownerCommentRepository;

        private Visibility _isVisible;
        public Visibility IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        private bool _isEnabled;
        public bool IsCommentEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        private string _comment;
        public string OwnerComment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged();
            }
        }

        private Comment _selectedComment;
        public Comment SelectedComment
        {
            get => _selectedComment;

            set
            {
                if (value != null)
                {
                    //Ako gost nije bio na lokaciji mozemo ga prijaviti
                    if(value.Role != "Guest") IsCommentEnabled = false;
                    else
                    {
                        List<Reservation> reservations = new List<Reservation>(_reservationService.GetByUserId(value.Author.Id));
                        if (checkLocations(reservations, value.Forum.Location.Id) && !checkIfReported(value)) IsCommentEnabled = true;
                        else IsCommentEnabled = false;
                    }

                    _selectedComment = value;
                }
            }
        }
        public User LoggedInUser { get; set; }

        public ForumViewModel(User user, Forum selectedForum)
        {
            LoggedInUser = user;
            SelectedForum = selectedForum;
            _reservationService = new ReservationService();
            _accommodationService = new AccommodationService();
            _commentRepository = new CommentRepository();
            _ownerCommentRepository = new OwnerCommentRepository();
            setVisibility();
        }

        private bool checkLocations(List<Reservation> reservations, int locationId)
        {
            foreach(Reservation res in reservations)
            {
                if (res.Accommodation.Location.Id == locationId)
                    return true;
            }

            return false;
        }

        private bool checkIfReported(Comment comment)
        {
            List<OwnerComment> ownerComments = new List<OwnerComment>(_ownerCommentRepository.GetAll());
            foreach(OwnerComment ownerComment in ownerComments)
            {
                if (ownerComment.Comment.Id == comment.Id && ownerComment.Owner.Id == LoggedInUser.Id)
                    return true;
            }
            return false;
        }

        private ICommand _reportCommentCommand;
        public ICommand ReportCommentCommand
        {
            get
            {
                return _reportCommentCommand ?? (_reportCommentCommand = new CommandBase(() => reportComment(), true));
            }
        }

        private ICommand _commentCommand;
        public ICommand CommentCommand
        {
            get
            {
                return _commentCommand ?? (_commentCommand = new CommandBase(() => makeComment(), true));
            }
        }

        private void reportComment()
        {
           
                SelectedComment.ReportNumber++;
                _commentRepository.Update(SelectedComment);
                OwnerComment ownerComment = new OwnerComment(SelectedComment, LoggedInUser);
                _ownerCommentRepository.Save(ownerComment);
            
        }

        private void makeComment()
        {
            Comment newComment = new Comment(LoggedInUser, OwnerComment, "Owner", 0, SelectedForum);
            newComment = _commentRepository.Save(newComment);
            SelectedForum.Comments.Add(newComment);
            OwnerComment = "";
        }

        private void setVisibility()
        {
            List<Accommodation> ownersAcc = new List<Accommodation>(_accommodationService.GetByUser(LoggedInUser));
            if (checkAcc(ownersAcc, SelectedForum.Location.Id))
                IsVisible = Visibility.Visible;
            else IsVisible = Visibility.Hidden;

        }

        private bool checkAcc(List<Accommodation> accommodations, int locationId)
        {
            foreach(Accommodation acc in accommodations)
            {
                if (acc.Location.Id == locationId)
                    return true;
            }
            return false;
        }




    }
}
