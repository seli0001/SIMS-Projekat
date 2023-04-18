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
        public int GuestId { get; set; }
        public User Guest { get; set; }

        public int GuideId { get; set; }

        public BookedTour bookedTour { get; set; }
        public int bookedTourId { get; set; }
        public User Guide { get; set; }

        //znanje vodiča (1-5), jezik vodiča (1-5), zanimljivost ture (1-5)
        public int GuideKnown { get; set; }
        public int GuideLanguage { get; set; }
        public int TourReview { get; set; }

        public string Comment { get; set; }

        public List<string> Images { get; set; }
        public TourRating()
        {
            Guest = new User();
            bookedTour = new BookedTour();
            Guide = new User();
            Images = new List<String>();
        }

        public TourRating(int Id, int bookedTourId, int GuideId, int GuestId, int GuideKnown, int GuideLanguage, int TourReview, string com, List<string> Images)
        {
            this.Id = Id;
            this.bookedTourId = bookedTourId;
            this.GuideId = GuideId;
            this.GuestId = GuestId;
            this.GuideKnown = GuideKnown;
            this.GuideLanguage = GuideLanguage;
            this.TourReview = TourReview;
            this.Comment = com;
            this.Images = Images;
        }


        public string[] ToCSV()
        {
            string[] csvValues =
            {
            Id.ToString(),
            bookedTourId.ToString(),
            GuideId.ToString(),
            GuestId.ToString(),
            GuideKnown.ToString(),
            GuideLanguage.ToString(),
            TourReview.ToString(),
            Comment,
            string.Join(",", Images)
        };
            return csvValues;
        }

        public void FromCSV(string[] csvValues)
        {
            Id = int.Parse(csvValues[0]);
            bookedTourId = int.Parse(csvValues[1]);
            GuideId = int.Parse(csvValues[2]);
            GuestId = int.Parse(csvValues[3]);
            GuideKnown = int.Parse(csvValues[4]);
            GuideLanguage = int.Parse(csvValues[5]);
            TourReview = int.Parse(csvValues[6]);
            Comment = csvValues[7];
            Images = csvValues[8].Split(',').ToList();

        }



    }
}
