using SIMS.Model.Guide;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Repository.GuideRepository
{
    public class StartTimeRepository
    {
        private const string filePath = "../../../../SIMS/Resources/Data/StartTimes.csv";
        private readonly Serializer<StartTime> serializer;

        public StartTimeRepository()
        {
            serializer = new Serializer<StartTime>();
        }

        public List<StartTime> GetAll()
        {
            return serializer.FromCSV(filePath);
        }
        
        public StartTime GetById(int id)
        {
            List<StartTime> startTimes = GetAllStartTimes();
            return startTimes.FirstOrDefault(startTime => startTime.Id == id);
        }
        
        public void Save(StartTime startTime)
        {
            List<StartTime> startTimes = GetAllStartTimes();
            if (startTimes == null)
            {
                startTimes = new List<StartTime>();
                startTime.Id = 0;
            }
            else
            {
                startTime = startTimes.Max(time => time.Id) + 1;
            }
            
            startTimes.Add(startTime);
            serializer.ToCSV( filePath, startTimes);
        }
    }
}