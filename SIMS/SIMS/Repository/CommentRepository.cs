using SIMS.Domain.Model;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Repository
{
    class CommentRepository
    {
        private const string _filePath = "../../../../SIMS/Resources/Data/Comments.csv";
        private readonly Serializer<Comment> _serializer;

        private readonly UserRepository _userRepository;
        private readonly ForumHelperRepository _forumRepository;


        private List<Comment> _comments;

        public CommentRepository()
        {
            _serializer = new Serializer<Comment>();
            _comments = new List<Comment>();
            _userRepository = new UserRepository();
            _forumRepository = new ForumHelperRepository();
        }

        public int NextId()
        {
            _comments = _serializer.FromCSV(_filePath);
            if (_comments.Count < 1)
            {
                return 1;
            }
            return _comments.Max(c => c.Id) + 1;
        }

        public List<Comment> GetAll()
        {
            _comments = _serializer.FromCSV(_filePath);
            foreach (Comment comment in _comments)
            {
                comment.Author = _userRepository.GetById(comment.Author.Id);
                comment.Forum = _forumRepository.GetById(comment.Forum.Id);
            }
            return _comments;
        }

        public Comment GetById(int id)
        {
            _comments = GetAll();
            return _comments.FirstOrDefault(comment => comment.Id == id);
        }

        public List<Comment> GetByForumId(int id)
        {
            _comments = GetAll();
            return _comments.Where(comment => comment.Forum.Id == id).ToList();
        }

        public Comment Update(Comment comment)
        {
            _comments = GetAll();
            Comment current = _comments.Find(c => c.Id == comment.Id);
            int index = _comments.IndexOf(current);
            _comments.Remove(current);
            _comments.Insert(index, comment);
            _serializer.ToCSV(_filePath, _comments);
            return comment;
        }

        public Comment Save(Comment comment)
        {
            comment.Id = NextId();
            _comments = _serializer.FromCSV(_filePath);
            _comments.Add(comment);
            _serializer.ToCSV(_filePath, _comments);
            return comment;
        }
    }
}
