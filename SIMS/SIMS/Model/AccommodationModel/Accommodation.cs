using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS.Serializer;

namespace SIMS.Model.AccommodationModel
{
    public enum Type
    {
        apartment,
        house,
        hut
    }
    public class Accommodation : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public Type Type { get; set; }
        public int MaxGuestsNumber { get; set; }
        public int MinBookingDays { get; set; }
        public int CancelDaysNumber { get; set; }
        public string PictureUrl { get; set; }

        public User User { get; set; }

        public Accommodation()
        {

        }

        public Accommodation(string name, string location, Type acType, int maxGNum, int minDays, int cancelDays, string picture, User user)
        {
            Name = name;
            Location = location;
            Type = acType;
            MaxGuestsNumber = maxGNum;
            MinBookingDays = minDays;
            CancelDaysNumber = cancelDays;
            PictureUrl = picture;
            User = user;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
             Id.ToString(),
             Name,
             Location,
             Type.ToString(),
             MaxGuestsNumber.ToString(),
             MinBookingDays.ToString(),
             CancelDaysNumber.ToString(),
             PictureUrl,
             User.Id.ToString(),
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Location = values[2];
            Type = (Type)Enum.Parse(typeof(Type), values[3]);
            MaxGuestsNumber = Convert.ToInt32(values[4]);
            MinBookingDays = Convert.ToInt32(values[5]);
            CancelDaysNumber = Convert.ToInt32(values[6]);
            PictureUrl = values[7];
            User = new User() { Id = Convert.ToInt32(values[8]) };
        }
    }
}

