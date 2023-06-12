using SIMS.Domain.Model;
using SIMS.Domain.RepositoryInterface;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Repository
{
    class ForumRepository : IForumRepository
    {
        private const string _filePath = "../../../../SIMS/Resources/Data/Forums.csv";
        private readonly Serializer<Forum> _serializer;

        private readonly LocationRepository _locationRepository;
        private readonly UserRepository _userRepository;
        private readonly CommentRepository _commentRepository;
        private readonly AccommodationRepository _accommodationRepository;

        private List<Forum> _forums;

        public ForumRepository()
        {
            _serializer = new Serializer<Forum>();
            _locationRepository = new LocationRepository();
            _userRepository = new UserRepository();
            _commentRepository = new CommentRepository();
            _accommodationRepository = new AccommodationRepository();
            _forums = new List<Forum>();
        }

        public int NextId()
        {
            _forums = _serializer.FromCSV(_filePath);
            if (_forums.Count < 1)
            {
                return 1;
            }
            return _forums.Max(c => c.Id) + 1;
        }

        public Forum Save(Forum forum)
        {
            forum.Id = NextId();
            _forums = _serializer.FromCSV(_filePath);
            _forums.Add(forum);
            _serializer.ToCSV(_filePath, _forums);
            return forum;
        }

        public List<Forum> GetAll()
        {
            _forums = _serializer.FromCSV(_filePath);
            foreach (Forum forum in _forums)
            {
                forum.Location = _locationRepository.GetById(forum.Location.Id);
                forum.ForumOwner = _userRepository.GetById(forum.ForumOwner.Id);
                forum.Comments = _commentRepository.GetByForumId(forum.Id);
            }
            return _forums;
        }

        public Forum GetById(int id)
        {
            _forums = GetAll();
            return _forums.FirstOrDefault(forum => forum.Id == id);
        }

        public void CloseForum(Forum forumm)
        {
            _forums = GetAll();
            Forum forum = _forums.FirstOrDefault(forum => forum.Id == forumm.Id);
            forum.IsOpen = false;
        }

        public void AddComment(string comment, Forum forumm)
        {
            _forums = GetAll();
            Forum forum = _forums.FirstOrDefault(forum => forum.Id == forumm.Id);
            forum.Comments.Add(comment);
        }

        public bool CheckForOwner(User owner)
        {
            _forums = GetAll();
            List<Comment> ownerComments = new List<Comment>(_commentRepository.GetByOwnerId(owner.Id));
            foreach(Forum forum in _forums)
            {
                if (!commentExist(forum, ownerComments) && forumIsOnLocation(forum.Location, owner))
                    return true;
            }

            return false;
        }

        private bool commentExist(Forum forum, List<Comment> ownerComments)
        {
            foreach(Comment comm in ownerComments)
            {
                if (forum.Comments.Any(comment => comment.Id == comm.Id))
                    return true;
            }

            return false;
        }

        private bool forumIsOnLocation(Location location, User owner)
        {
            List<Accommodation> accommodations = new List<Accommodation>(_accommodationRepository.GetByUser(owner));
            foreach(Accommodation acc in accommodations)
            {
                if (acc.Location.Id == location.Id)
                    return true;
            }

            return false;
        }
    }
}
