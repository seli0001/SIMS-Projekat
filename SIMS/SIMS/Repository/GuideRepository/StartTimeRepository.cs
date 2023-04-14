using SIMS.Domain.Model.Guide;
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
        private const string _filePath = "../../../../SIMS/Resources/Data/StartTimes.csv";
        private readonly Serializer<StartTime> _serializer;

        public StartTimeRepository()
        {
            _serializer = new Serializer<StartTime>();
        }
        
        public int GetNextId(List<StartTime> startTimes)
        {
            if (startTimes.Count < 1)
            {
                return 0;
            } 
            return startTimes.Max(startTime => startTime.Id) + 1;
        }

        public List<StartTime> GetAll()
        {
            return _serializer.FromCSV(_filePath);
        }
        
        public StartTime GetById(int id)
        {
            List<StartTime> startTimes = GetAll();
            return startTimes.FirstOrDefault(startTime => startTime.Id == id);
        }
        
        public void Save(StartTime startTime)
        {
            List<StartTime> startTimes = GetAll();
            startTime.Id = GetNextId(startTimes);
            
            startTimes.Add(startTime);
            _serializer.ToCSV( _filePath, startTimes);
        }
    }
}