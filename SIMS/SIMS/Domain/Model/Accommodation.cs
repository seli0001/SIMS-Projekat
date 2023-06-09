﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using SIMS.Serializer;

namespace SIMS.Domain.Model
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
        public Location Location { get; set; }
        public Type Type { get; set; }
        public int MaxGuestsNumber { get; set; }
        public int MinBookingDays { get; set; }
        public int CancelDaysNumber { get; set; }
        public bool Super { get; set; }
        public List<Image> Images { get; set; }
        public User User { get; set; }
        public DateOnly RenovatedOn { get; set; }
        public bool Renovated { get; set; }

        public Accommodation()
        {
            Images = new List<Image>();
        }

        public Accommodation(string name, Location location, Type acType, int maxGNum, int minDays, int cancelDays, User user, List<Image> images)
        {
            Name = name;
            Location = location;
            Type = acType;
            MaxGuestsNumber = maxGNum;
            MinBookingDays = minDays;
            CancelDaysNumber = cancelDays;
            Super = false;
            User = user;
            Images = images;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
             Id.ToString(),
             Name,
             Type.ToString(),
             MaxGuestsNumber.ToString(),
             MinBookingDays.ToString(),
             CancelDaysNumber.ToString(),
             Super.ToString(),
             Location.Id.ToString(),
             User.Id.ToString(),
             RenovatedOn.ToString(),
             Renovated.ToString(),
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Type = (Type)Enum.Parse(typeof(Type), values[2]);
            MaxGuestsNumber = Convert.ToInt32(values[3]);
            MinBookingDays = Convert.ToInt32(values[4]);
            CancelDaysNumber = Convert.ToInt32(values[5]);
            Super = Convert.ToBoolean(values[6]);
            Location = new Location() { Id = Convert.ToInt32(values[7]) };
            User = new User() { Id = Convert.ToInt32(values[8]) };
            RenovatedOn = DateOnly.FromDateTime(Convert.ToDateTime(values[9]));
            Renovated = Convert.ToBoolean(values[10]);
        }
    }
}

