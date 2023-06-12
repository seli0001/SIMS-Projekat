using SIMS.Domain.Model;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Repository
{
    public class ForumRepository
    {
        private const string _filePath = "../../../../SIMS/Resources/Data/Forums.csv";
        private readonly Serializer<Forum> _serializer;

        private readonly AccommodationRepository _accommodationRepository;
        private readonly UserRepository _userRepository;

        private List<Forum> _forums;

        public ForumRepository()
        {
            _serializer = new Serializer<Forum>();
            _accommodationRepository = new AccommodationRepository();
            _userRepository = new UserRepository();
            _forums = new List<Forum>();
        }

        public int GetNextId(List<Forum> forums)
        {
            if (forums.Count < 1)
            {
                return 0;
            }
            return forums.Max(forum => forum.Id) + 1;
        }

        public Forum Save(Forum forum)
        {
            forum.Id = GetNextId(_forums);
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
                forum.Accommodation = _accommodationRepository.GetById(forum.Accommodation.Id);
                forum.ForumOwner = _userRepository.GetById(forum.ForumOwner.Id);
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


    }
}
