using SIMS.Model.Guide;
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
        private const string filePath = "../../../../SIMS/Resources/Data/Images.csv";
        private readonly Serializer<Image> serializer;

        public ImageRepository()
        {
            serializer = new Serializer<Image>();
        }

        public List<Image> GetAll()
        {
            return serializer.FromCSV(filePath);
        }
        
        public List<Image> GetByTourId(int tourId)
        {
            List<Image> images = GetAll();
            return images.Where(image => image.Tour.Id == tourId).ToList();
        }
        
        public void Save(Image image)
        {
            List<Image> images = GetAll();
            if (images == null)
            {
                images = new List<Image>();
                image.Id = 0;
            }
            else
            {
                image.Id = images.Max(img => img.Id) + 1;
            }

            images.Add(image);
            serializer.ToCSV(filePath, images);
        }
    }
}
