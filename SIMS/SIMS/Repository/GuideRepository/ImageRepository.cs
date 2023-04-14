using SIMS.Domain.Model.Guide;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Repository.GuideRepository
{
    class ImageRepository
    {
        private const string _filePath = "../../../../SIMS/Resources/Data/Images.csv";
        private readonly Serializer<Image> _serializer;

        public ImageRepository()
        {
            _serializer = new Serializer<Image>();
        }
        
        public int GetNextId(List<Image> images)
        {
            if (images.Count < 1)
            {
                return 0;
            }
            return images.Max(image => image.Id) + 1;
        }

        public List<Image> GetAll()
        {
            return _serializer.FromCSV(_filePath);
        }
        
        public List<Image> GetByTourId(int tourId)
        {
            List<Image> images = GetAll();
            return images.Where(image => image.Tour.Id == tourId).ToList();
        }
        
        public void Save(Image image)
        {
            List<Image> images = GetAll();
            image.Id = GetNextId(images);

            images.Add(image);
            _serializer.ToCSV(_filePath, images);
        }
        
        public void SaveAll(List<Image> newImages)
        {
            List<Image> allImages = GetAll();
            int id = GetNextId(allImages);

            foreach (var image in newImages)
            {
                image.Id = id;
                allImages.Add(image);
                id++;
            }
            _serializer.ToCSV(_filePath, allImages);
        }
    }
}
