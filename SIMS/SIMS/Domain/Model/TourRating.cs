using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS.Serializer;

namespace SIMS.Domain.Model
{
    public class TourRating : ISerializable
    {
        public int Id { get; set; }
        public BookedTour bookedTour { get; set; }

        //znanje vodiča (1-5), jezik vodiča (1-5), zanimljivost ture (1-5)
        public int GuideKnown { get; set; }
        public int GuideLanguage { get; set; }
        public int TourReview { get; set; }

        public string Comment { get; set; }

        public bool Valid { get; set; }

        public List<string> Images { get; set; }
        public TourRating()
        {
            bookedTour = new BookedTour();
            Images = new List<String>();
            Valid = true;
        }

        public TourRating(int Id, int bookedTourId, int GuideId, int GuestId, int GuideKnown, int GuideLanguage, int TourReview, string com, List<string> Images)
        {
            this.Id = Id;
            bookedTour = new BookedTour() { Id = bookedTourId };
            this.GuideKnown = GuideKnown;
            this.GuideLanguage = GuideLanguage;
            this.TourReview = TourReview;
            this.Comment = com;
            this.Images = Images;
            this.Valid = true;
        }


        public string[] ToCSV()
        {
            string[] csvValues =
            {
            Id.ToString(),
            bookedTour.Id.ToString(),
            GuideKnown.ToString(),
            GuideLanguage.ToString(),
            TourReview.ToString(),
            Comment,
            string.Join(",", Images),
            Valid.ToString()
        };
            return csvValues;
        }

        public void FromCSV(string[] csvValues)
        {
            Id = int.Parse(csvValues[0]);
            bookedTour = new BookedTour() { Id = int.Parse(csvValues[1]) };
            GuideKnown = int.Parse(csvValues[2]);
            GuideLanguage = int.Parse(csvValues[3]);
            TourReview = int.Parse(csvValues[4]);
            Comment = csvValues[5];
            Images = csvValues[6].Split(',').ToList();
            Valid = bool.Parse(csvValues[7]);
        }



    }
}
