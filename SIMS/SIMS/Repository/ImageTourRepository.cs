using SIMS.Domain.Model;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Repository
{
    class ImageTourRepository
    {
        private const string _filePath = "../../../../SIMS/Resources/Data/Images.csv";
        private readonly Serializer<ImageTour> _serializer;

        public ImageTourRepository()
        {
            _serializer = new Serializer<ImageTour>();
        }
        
        public int GetNextId(List<ImageTour> images)
        {
            if (images.Count < 1)
            {
                return 0;
            }
            return images.Max(image => image.Id) + 1;
        }

        public List<ImageTour> GetAll()
        {
            return _serializer.FromCSV(_filePath);
        }
        
        public List<ImageTour> GetByTourId(int tourId)
        {
            List<ImageTour> images = GetAll();
            return images.Where(image => image.Tour.Id == tourId).ToList();
        }
        
        public void Save(ImageTour image)
        {
            List<ImageTour> images = GetAll();
            image.Id = GetNextId(images);

            images.Add(image);
            _serializer.ToCSV(_filePath, images);
        }
        
        public void SaveAll(List<ImageTour> newImages)
        {
            List<ImageTour> allImages = GetAll();
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
