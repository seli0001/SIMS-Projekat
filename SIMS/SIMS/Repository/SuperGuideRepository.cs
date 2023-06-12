using SIMS.Domain.Model;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Repository
{
    internal class SuperGuideRepository
    {
        private const string _filePath = "../../../../SIMS/Resources/Data/SuperGuides.csv";
        private readonly Serializer<SuperGuide> _serializer;
        private List<SuperGuide> _guides;

        public SuperGuideRepository()
        {
            _serializer = new Serializer<SuperGuide>();
        }

        public List<SuperGuide> GetAll()
        {
            return _serializer.FromCSV(_filePath);
        }
        public bool IsSuperOwner(User user)
        {
            _guides = GetAll();
            return _guides.Any(g => g.Id == user.Id);
        }

        public SuperGuide Save(User user, string language)
        {
            SuperGuide superGuide = new SuperGuide();
            superGuide.Id = user.Id;
            superGuide.Language = language;
            _guides = GetAll();
            _guides.Add(superGuide);
            _serializer.ToCSV(_filePath, _guides);
            return superGuide;
        }
        public void Delete(User user)
        {
            _guides = GetAll();
            SuperGuide founded = _guides.Find(c => c.Id == user.Id);
            _guides.Remove(founded);
            _serializer.ToCSV(_filePath, _guides);
        }
        public int NextId()
        {
            _guides = GetAll();
            if (_guides.Count < 1)
            {
                return 1;
            }
            return _guides.Max(c => c.Id) + 1;
        }
    }
}
