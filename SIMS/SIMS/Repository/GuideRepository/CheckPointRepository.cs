using SIMS.Model.Guide;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Repository.GuideRepository
{
    class CheckPointRepository
    {
        private const string filePath = "../../../../SIMS/Resources/Data/CheckPoints.csv";
        private readonly Serializer<CheckPoint> serializer;

        public CheckPointRepository()
        {
            serializer = new Serializer<CheckPoint>();
        }
        
        public List<CheckPoint> GetAll()
        {
            return serializer.FromCSV(filePath);
        }
        
        public List<CheckPoint> GetByTourId(int tourId)
        {
            List<CheckPoint> checkPoints = GetAll();
            return checkPoints.Where(checkPoint => checkPoint.Tour.Id == tourId).ToList();
        }
        
        public void Save(CheckPoint checkPoint)
        {
            List<CheckPoint> checkPoints = GetAll();
            if (checkPoints == null)
            {
                checkPoints = new List<CheckPoint>();
                checkPoint.Id = 0;
            }
            else
            {
                checkPoint.Id = checkPoints.Max(checkPoint => checkPoint.Id) + 1;
            }

            checkPoints.Add(checkPoint);
            serializer.ToCSV(filePath, checkPoints);
        }
    }
}
