﻿using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS.Domain.Model.AccommodationModel;
using SIMS.Domain.Model;

namespace SIMS.Repository
{
    public class ImageRepository
    {
        private const string _filePath = "../../../../SIMS/Resources/Data/AccommodationImages.csv";
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

        public List<Image> SetId(List<Image> images)
        {
            List<Image> allImages = GetAll();
            int id = GetNextId(allImages);

            foreach (var image in images)
            {
                image.Id = id;
                allImages.Add(image);
                id++;
            }

            return images;
        }
        public List<Image> GetByAccommodationId(int accommodationId)
        {
            List<Image> images = GetAll();
            return images.Where(image => image.Accommodation.Id == accommodationId).ToList();
        }

        public void Save(Image image)
        {
            List<Image> images = GetAll();
            image.Id = GetNextId(images);

            images.Add(image);
            _serializer.ToCSV(_filePath, images);
        }

        public List<Image> SaveAll(List<Image> newImages, Accommodation accommodation)
        {
            List<Image> allImages = GetAll();
            int id = GetNextId(allImages);

            foreach (var image in newImages)
            {
                image.Id = id;
                image.Accommodation = accommodation;
                allImages.Add(image);
                id++;
            }
            _serializer.ToCSV(_filePath, allImages);
            return allImages;
        }
    }
}
