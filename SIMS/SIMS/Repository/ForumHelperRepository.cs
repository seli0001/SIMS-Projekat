using SIMS.Domain.Model;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Repository
{
     class ForumHelperRepository
    {
        private const string _filePath = "../../../../SIMS/Resources/Data/Forums.csv";
        private readonly Serializer<Forum> _serializer;

        private readonly LocationRepository _locationRepository;
        private readonly UserRepository _userRepository;

        private List<Forum> _forums;

        public ForumHelperRepository()
        {
            _serializer = new Serializer<Forum>();
            _locationRepository = new LocationRepository();
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

        public List<Forum> GetAll()
        {
            _forums = _serializer.FromCSV(_filePath);
            foreach (Forum forum in _forums)
            {
                forum.Location = _locationRepository.GetById(forum.Location.Id);
                forum.ForumOwner = _userRepository.GetById(forum.ForumOwner.Id);
            }
            return _forums;
        }

        public Forum GetById(int id)
        {
            _forums = GetAll();
            return _forums.FirstOrDefault(forum => forum.Id == id);
        }
    }
}
