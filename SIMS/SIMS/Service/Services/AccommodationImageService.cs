using SIMS.Domain.Model;
using SIMS.Repository;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Service.UseCases
{
    public class AccommodationImageService
    {
        private List<Image> _images;
        private readonly ImageRepository _imageRepository;

        public AccommodationImageService()
        {
            _images = new List<Image>();
            _imageRepository = new ImageRepository();
        }

        public List<Image> GetAll()
        {
            return _imageRepository.GetAll();
        }

        public void Save(Image image)
        {
            _imageRepository.Save(image);
        }

        public List<Image> SaveAll(List<Image> images)
        {
            return _imageRepository.SaveAll(images);
        }

        public List<Image> SetId(List<Image> images)
        {
            return _imageRepository.SetId(images);
        }

        public List<Image> GetByAccommodation(Accommodation accommodation)
        {
            List<Image> Allimages = GetAll();
            List<Image> images = new List<Image>();
            foreach (Image image in Allimages)
            {
                if (checkIfAccommodationImage(image, accommodation)) images.Add(image);
            }
            return images;
        }

        //Provera da li je prosledjena slika, slika od prosledjenog smestaja
        private bool checkIfAccommodationImage(Image image, Accommodation accommodation)
        {
            foreach (Image accommodationImage in accommodation.Images)
            {
                if (image.Id == accommodationImage.Id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
