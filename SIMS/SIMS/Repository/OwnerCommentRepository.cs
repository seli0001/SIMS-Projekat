using SIMS.Domain.Model;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Repository
{
    class OwnerCommentRepository
    {
        private const string _filePath = "../../../../SIMS/Resources/Data/OwnerComment.csv";
        private readonly Serializer<OwnerComment> _serializer;

        private readonly UserRepository _userRepository;
        private readonly CommentRepository _commentRepository;

        private List<OwnerComment> _ownerComments;

        public OwnerCommentRepository()
        {
            _serializer = new Serializer<OwnerComment>();

            _userRepository = new UserRepository();
            _commentRepository = new CommentRepository();

            _ownerComments = new List<OwnerComment>();
        }

        public int NextId()
        {
            _ownerComments = _serializer.FromCSV(_filePath);
            if (_ownerComments.Count < 1)
            {
                return 1;
            }
            return _ownerComments.Max(c => c.Id) + 1;
        }

        public List<OwnerComment> GetAll()
        {
            _ownerComments = _serializer.FromCSV(_filePath);
            foreach (OwnerComment ownerComment in _ownerComments)
            {
                ownerComment.Owner = _userRepository.GetById(ownerComment.Owner.Id);
                ownerComment.Comment = _commentRepository.GetById(ownerComment.Comment.Id);
            }
            return _ownerComments;
        }

        public OwnerComment GetById(int id)
        {
            _ownerComments = GetAll();
            return _ownerComments.FirstOrDefault(ownerComment => ownerComment.Id == id);
        }

        public OwnerComment Save(OwnerComment ownerComment)
        {
            ownerComment.Id = NextId();
            _ownerComments = _serializer.FromCSV(_filePath);
            _ownerComments.Add(ownerComment);
            _serializer.ToCSV(_filePath, _ownerComments);
            return ownerComment;
        }
    }
}
