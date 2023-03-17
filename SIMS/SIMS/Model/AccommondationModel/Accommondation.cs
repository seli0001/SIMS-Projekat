using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS.Serializer;

namespace SIMS.Model.AccommondationModel
{
    public enum Type
    {
        apartment,
        house,
        hut
    }
    public class Accommondation : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public Type Type { get; set; }
        public int MaxGuestsNumber { get; set; }
        public int MinBookingDays { get; set; }
        public int CancelDaysNumber { get; set; }
        public string PictureUrl { get; set; }

        public Accommodation()
        {

        }

        public Accommodation(int id, string name, string location, Type acType, int maxGNum, int minDays, int cancelDays, string picture)
        {
            Id = id;
            Name = name;
            Location = location;
            Type = acType;
            MaxGuestsNumber = maxGNum;
            MinBookingDays = minDays;
            CancelDaysNumber = cancelDays;
            PictureUrl = picture;
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
        }
    }
}

